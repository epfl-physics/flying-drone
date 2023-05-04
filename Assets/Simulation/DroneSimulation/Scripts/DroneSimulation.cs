using UnityEngine;

public class DroneSimulation : Simulation
{
    public DroneSimulationState simState;
    // public bool ignoreStateOnStart;
    public bool startStationary;

    [Header("Platform")]
    public MovingPlatform platform;
    public PlatformData platformData;

    [Header("Drone")]
    public Drone drone;
    public DroneData droneData;

    [Header("Point Mass")]
    public Transform pointMass;

    private void Awake()
    {
        if (drone)
        {
            drone.autoUpdate = false;
        }

        if (platform)
        {
            platform.autoUpdate = false;
            platform.Position = platformData.position;
        }

        if (startStationary)
        {
            SetDroneVerticalData(0);
            SetDroneHorizontalData(0);
            SetPlatformVerticalData(0);
            SetPlatformRotationData(0);
        }
        else
        {
            ApplyDroneData();
            ApplyPlatformData();
        }
    }

    private void Start()
    {
        if (drone) drone.Time = 0;
        if (platform) platform.Time = 0;

        // if (simState)
        // {
        //     simState.droneVelocityAbsolute = Vector3.zero;
        //     simState.droneVelocityRelative = Vector3.zero;
        //     simState.platformVelocity = Vector3.zero;
        //     simState.tangentialVelocity = Vector3.zero;
        // }
    }

    private void Update()
    {
        Vector3 platformSurfacePosition = Vector3.zero;

        // Determine whether the platform needs to update
        bool platformIsMoving = platformData.motionType != MovingPlatform.MotionType.None;
        platformIsMoving |= platformData.rotationFrequency != 0;
        if (platform && platformIsMoving)
        {
            platform.TakeAStep(Time.deltaTime);
            platformSurfacePosition = platform.GetSurfacePosition();
        }

        // Determine whether the drone needs to update
        bool droneIsMoving = droneData.verticalMotionType != Drone.VerticalMotionType.None;
        droneIsMoving |= droneData.horizontalMotionType != Drone.HorizontalMotionType.None;
        if (drone && droneIsMoving)
        {
            Vector3 relativePosition = platformData.position;
            if (!droneData.isIndependent) relativePosition += platformSurfacePosition;
            drone.TakeAStep(Time.deltaTime, relativePosition);

            // Place the point mass
            if (pointMass) pointMass.localPosition = drone.transform.localPosition;
        }

        UpdateSimState();
    }

    public void ApplyPlatformData(bool synchronizeWithDrone = false)
    {
        if (platform)
        {
            platform.Position = platformData.position;
            platform.SetRestHeight(platformData.restHeight);
            platform.SetAmplitude(platformData.amplitude);
            platform.translationPeriod = platformData.translationPeriod;
            platform.rotationFrequency = platformData.rotationFrequency;
            platform.motionType = platformData.motionType;

            if (drone && synchronizeWithDrone)
            {
                platform.Time = drone.Time;
            }
        }
    }

    public void ApplyDroneData(bool synchronizeWithPlatform = false)
    {
        if (drone)
        {
            drone.SetRestHeight(droneData.restHeight);
            drone.SetVerticalAmplitude(droneData.verticalAmplitude);
            drone.translationPeriod = droneData.translationPeriod;
            drone.verticalMotionType = droneData.verticalMotionType;
            drone.horizontalAmplitude = droneData.horizontalAmplitude;
            drone.circularRadius = droneData.circularRadius;
            drone.orbitalFrequency = droneData.orbitalFrequency;
            drone.horizontalMotionType = droneData.horizontalMotionType;
        }

        // if (resetPosition) SetDroneAtRestPosition();
        if (platform && synchronizeWithPlatform)
        {
            drone.Time = platform.Time;
        }
    }

    public void UpdateSimState()
    {
        if (!simState) return;

        simState.droneTime = drone.Time;
        simState.platformTime = platform.Time;

        simState.droneVelocityAbsolute = drone.GetVelocity();
        simState.droneAccelerationAbsolute = drone.GetAcceleration();

        simState.platformVelocity = platform.GetVelocity();
        simState.platformAcceleration = platform.GetAcceleration();

        simState.omega = platform.Omega;
        simState.origin = transform.localPosition;
        simState.dronePositionAbsolute = drone.transform.localPosition;
        simState.platformPosition = platform.Position + platform.GetSurfacePosition();

        simState.RedrawVectors();
    }

    public void SetDroneAtRestPosition()
    {
        Debug.Log("SetDroneAtRestPosition");
        Vector3 dronePosition = droneData.restHeight * Vector3.up + platformData.position;
        if (!droneData.isIndependent) dronePosition += platform.GetSurfacePosition();

        if (drone) drone.transform.localPosition = dronePosition;
        if (pointMass) pointMass.localPosition = dronePosition;
    }

    public void SetDroneVerticalData(int index)
    {
        if (droneData == null) return;

        droneData.restHeight = 4;
        droneData.verticalAmplitude = 0.8f;
        droneData.translationPeriod = 3;
        if (index == 1 || index == 4)
        {
            droneData.verticalMotionType = Drone.VerticalMotionType.Linear;
        }
        else if (index == 2 || index == 5)
        {
            droneData.verticalMotionType = Drone.VerticalMotionType.Sinusoidal;
        }
        else
        {
            droneData.verticalMotionType = Drone.VerticalMotionType.None;
        }

        ApplyDroneData(true);
    }

    public void SetDroneHorizontalData(int index)
    {
        if (droneData == null) return;

        droneData.horizontalAmplitude = 2;
        droneData.circularRadius = 1.2f;
        if (index > 0)
        {
            droneData.horizontalMotionType = Drone.HorizontalMotionType.Circular;
        }
        else
        {
            droneData.horizontalMotionType = Drone.HorizontalMotionType.None;
        }
        if (index == 1)
        {
            droneData.orbitalFrequency = 0.15f;
        }
        else if (index == 2)
        {
            droneData.orbitalFrequency = -0.15f;
        }
        else
        {
            droneData.orbitalFrequency = 0;
        }

        ApplyDroneData(true);
    }

    public void SetPlatformVerticalData(int index)
    {
        if (platformData == null) return;

        platformData.position = 4 * Vector3.right;
        platformData.restHeight = 1.5f;
        platformData.amplitude = 0.8f;
        platformData.translationPeriod = 3;
        if (index == 1)
        {
            platformData.motionType = MovingPlatform.MotionType.Linear;
        }
        else if (index == 2)
        {
            platformData.motionType = MovingPlatform.MotionType.Sinusoidal;
        }
        else
        {
            platformData.motionType = MovingPlatform.MotionType.None;
        }

        ApplyPlatformData(true);
    }

    public void SetPlatformRotationData(int index)
    {
        if (platformData == null) return;

        if (index == 1)
        {
            platformData.rotationFrequency = 0.15f;
        }
        else if (index == 2)
        {
            platformData.rotationFrequency = -0.15f;
        }
        else
        {
            platformData.rotationFrequency = 0;
        }

        ApplyPlatformData(true);
    }
}

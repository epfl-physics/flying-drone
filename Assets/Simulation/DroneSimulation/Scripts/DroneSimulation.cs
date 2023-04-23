using UnityEngine;

public class DroneSimulation : Simulation
{
    public DroneSimulationState simState;
    public bool ignoreStateOnStart;

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

        ApplyPlatformData();
        ApplyDroneData();
    }

    private void Start()
    {
        if (simState)
        {
            if (!ignoreStateOnStart)
            {
                if (drone) drone.Time = simState.droneTime;
                if (platform) platform.Time = simState.platformTime;
            }

            simState.droneVelocityAbsolute = Vector3.zero;
            simState.droneVelocityRelative = Vector3.zero;
            simState.platformVelocity = Vector3.zero;
            simState.tangentialVelocity = Vector3.zero;
        }
    }

    private void Update()
    {
        Vector3 platformSurfacePosition = Vector3.zero;
        if (platform)
        {
            platform.TakeAStep(Time.deltaTime);
            platformSurfacePosition = platform.GetSurfacePosition();
        }

        if (drone)
        {
            Vector3 relativePosition = platformData.position;
            if (!droneData.isIndependent) relativePosition += platformSurfacePosition;
            drone.TakeAStep(Time.deltaTime, relativePosition);

            // Place the point mass
            if (pointMass) pointMass.localPosition = drone.transform.localPosition;
        }

        UpdateSimState();
    }

    public void ApplyPlatformData()
    {
        if (platform)
        {
            platform.Position = platformData.position;
            platform.SetRestHeight(platformData.restHeight);
            platform.SetAmplitude(platformData.amplitude);
            platform.translationPeriod = platformData.translationPeriod;
            platform.rotationFrequency = platformData.rotationFrequency;
            platform.motionType = platformData.motionType;
        }
    }

    public void ApplyDroneData()
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

        SetDroneAtRestPosition();
    }

    public void UpdateSimState()
    {
        if (!simState) return;

        simState.droneTime = drone.Time;
        simState.platformTime = platform.Time;

        // simState.droneVelocityAbsolute = (drone.transform.localPosition - simState.dronePositionAbsolute) / Time.deltaTime;
        // simState.platformVelocity = (platform.GetSurfacePosition() - simState.platformPosition) / Time.deltaTime;
        simState.droneVelocityAbsolute = drone.GetVelocity();
        simState.platformVelocity = platform.GetVelocity();

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

    public void SetDroneMovement(bool isCircular)
    {
        if (isCircular)
        {
            droneData.horizontalMotionType = Drone.HorizontalMotionType.Circular;
            droneData.orbitalFrequency = 0.3f;
        }
        else
        {
            droneData.horizontalMotionType = Drone.HorizontalMotionType.None;
            droneData.orbitalFrequency = 0f;
        }

        ApplyDroneData();
    }
}

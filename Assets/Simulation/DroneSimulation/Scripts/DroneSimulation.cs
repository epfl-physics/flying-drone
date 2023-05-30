using UnityEngine;

public class DroneSimulation : Simulation
{
    public DroneSimulationState simState;

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
        if (drone) drone.Time = 0;
        if (platform) platform.Time = 0;
    }

    private void Update()
    {
        Vector3 platformSurfacePosition = Vector3.zero;

        // Determine whether the platform needs to update
        bool platformIsMoving = false;
        platformIsMoving |= platformData.motionType != MovingPlatform.MotionType.None;
        platformIsMoving |= platformData.rotationType != MovingPlatform.RotationType.None;

        if (platform && platformIsMoving)
        {
            platform.TakeAStep(Time.deltaTime);
            platformSurfacePosition = platform.GetSurfacePosition();
        }

        // Determine whether the drone needs to update
        bool droneIsMoving = droneData.verticalMotionType != Drone.VerticalMotionType.None;
        droneIsMoving |= droneData.circularMotionType != Drone.CircularMotionType.None;
        if (drone && droneIsMoving)
        {
            drone.TakeAStep(Time.deltaTime);
            SynchronizePointMass();
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
            platform.data.translationPeriod = platformData.translationPeriod;
            platform.data.rotationFrequency = platformData.rotationFrequency;
            platform.data.motionType = platformData.motionType;
            platform.data.rotationType = platformData.rotationType;

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
            drone.data.origin = droneData.origin;
            drone.data.restPosition = droneData.restPosition;
            drone.SetRestHeight(droneData.restPosition.y);
            drone.SetVerticalAmplitude(droneData.verticalAmplitude);
            drone.data.verticalPeriod = droneData.verticalPeriod;
            drone.data.verticalMotionType = droneData.verticalMotionType;
            drone.data.circularRadius = droneData.circularRadius;
            drone.data.circularFrequency = droneData.circularFrequency;
            drone.data.circularMotionType = droneData.circularMotionType;
            drone.ReturnToRestPosition();
        }

        SynchronizePointMass();

        if (platform && synchronizeWithPlatform)
        {
            drone.Time = platform.Time;
        }
    }

    public void UpdateSimState()
    {
        if (!simState) return;

        // Running clocks for drone and platform motions
        simState.droneTime = drone.Time;
        simState.platformTime = platform.Time;

        // Drone absolute quantities
        Vector3 rAbsolute = drone.transform.localPosition;
        Vector3 vAbsolute = drone.GetVelocity();
        Vector3 aAbsolute = drone.GetAcceleration();

        // Platform absolute quantities
        Vector3 rPlatform = platform.Position + platform.GetSurfacePosition();
        Vector3 vPlatform = platform.GetVelocity();
        Vector3 aPlatform = platform.GetAcceleration();

        // Platform rotation quantities
        Vector3 omega = platform.Omega;
        Vector3 omegaDot = platform.OmegaDot;

        // Computed quantities (recall Unity is left-handed)
        Vector3 rRelative = rAbsolute - rPlatform;
        Vector3 vTangential = -Vector3.Cross(omega, rRelative);
        Vector3 vRelative = vAbsolute - vPlatform - vTangential;
        Vector3 aCentrifugal = -Vector3.Cross(omega, vTangential);
        Vector3 aCoriolis = -2 * Vector3.Cross(omega, vRelative);
        Vector3 aEuler = -Vector3.Cross(omegaDot, rRelative);
        Vector3 aRelative = aAbsolute - aPlatform - aCentrifugal - aCoriolis - aEuler;

        simState.omega = omega;
        simState.omegaDot = omegaDot;

        simState.origin = transform.localPosition;
        simState.dronePositionAbsolute = rAbsolute;
        simState.dronePositionRelative = rRelative;
        simState.platformPosition = rPlatform;

        simState.droneVelocityAbsolute = vAbsolute;
        simState.droneVelocityRelative = vRelative;
        simState.platformVelocity = vPlatform;
        simState.tangentialVelocity = vTangential;

        simState.droneAccelerationAbsolute = aAbsolute;
        simState.droneAccelerationRelative = aRelative;
        simState.platformAcceleration = aPlatform;
        simState.centrifugalAcceleration = aCentrifugal;
        simState.coriolisAcceleration = aCoriolis;
        simState.eulerAcceleration = aEuler;

        simState.RedrawVectors();
    }

    public void SetDroneAtRestPosition()
    {
        if (drone)
        {
            drone.ReturnToRestPosition();
            SynchronizePointMass();
        }
    }

    private void SynchronizePointMass()
    {
        // Place the point mass at the drone position
        if (drone && pointMass)
        {
            pointMass.localPosition = drone.transform.localPosition;
        }
    }
}

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

    [Header("Projected Point")]
    public Transform droneProjection;
    public Vector3 droneProjectionOffset;

    private void Awake()
    {
        if (platform) platform.autoUpdate = false;
        if (drone) drone.autoUpdate = false;

        ApplyPlatformData();
        ApplyDroneData();
    }

    private void Update()
    {
        if (IsPaused) return;

        if (platform) platform.TakeAStep(Time.deltaTime);
        if (drone) drone.TakeAStep(Time.deltaTime);

        SynchronizePointMass();
        SynchronizeDroneProjection();
        UpdateSimState();
    }

    public void ApplyPlatformData()
    {
        if (platform)
        {
            platform.Position = platformData.position;
            platform.SetRestHeight(platformData.restHeight);
            platform.data.translationType = platformData.translationType;
            platform.data.translationAmplitude = platformData.translationAmplitude;
            platform.data.translationFrequency = platformData.translationFrequency;
            platform.data.rotationType = platformData.rotationType;
            platform.data.rotationFrequency = platformData.rotationFrequency;
            platform.data.rotationFrequencyVariable = platformData.rotationFrequencyVariable;
        }
    }

    public void ApplyDroneData()
    {
        if (drone)
        {
            drone.data.origin = droneData.origin;
            drone.data.restPosition = droneData.restPosition;
            drone.SetRestHeight(droneData.restPosition.y);
            drone.data.verticalMotionType = droneData.verticalMotionType;
            drone.data.verticalAmplitude = droneData.verticalAmplitude;
            drone.data.verticalFrequency = droneData.verticalFrequency;
            drone.data.circularMotionType = droneData.circularMotionType;
            drone.data.circularRadius = droneData.circularRadius;
            drone.data.circularFrequency = droneData.circularFrequency;
            drone.data.circularFrequencyVariable = droneData.circularFrequencyVariable;
        }

        SynchronizePointMass();
        SynchronizeDroneProjection();
    }

    public void UpdateSimState()
    {
        if (!simState) return;

        // Running clocks and current angles of platform and drone
        simState.platformTranslationTime = platform.translationTime;
        simState.platformRotationTime = platform.rotationTime;
        simState.platformAngle = platform.angle;
        simState.droneVerticalTime = drone.verticalTime;
        simState.droneCircularTime = drone.circularTime;
        simState.droneAngle = drone.angle;

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
        Vector3 aCentripetal = -Vector3.Cross(omega, vTangential);
        Vector3 aCoriolis = -2 * Vector3.Cross(omega, vRelative);
        Vector3 aEuler = -Vector3.Cross(omegaDot, rRelative);
        Vector3 aRelative = aAbsolute - aPlatform - aCentripetal - aCoriolis - aEuler;

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
        simState.centripetalAcceleration = aCentripetal;
        simState.coriolisAcceleration = aCoriolis;
        simState.eulerAcceleration = aEuler;

        simState.RedrawVectors();
    }

    private void SynchronizePointMass()
    {
        // Place the point mass at the drone position
        if (drone && pointMass)
        {
            pointMass.localPosition = drone.transform.localPosition;
        }
    }

    private void SynchronizeDroneProjection()
    {
        if (drone && platform && droneProjection)
        {
            Vector3 position = drone.transform.localPosition;
            position.y = platform.GetSurfacePosition().y;
            position += droneProjectionOffset;
            droneProjection.localPosition = position;
        }
    }

    public void SynchronizePlatformRotationClock()
    {
        if (platform) platform.rotationTime = platform.translationTime;
    }

    public void SynchronizeDroneClocksWithPlatform()
    {
        if (drone && platform)
        {
            drone.verticalTime = platform.translationTime;
            drone.circularTime = platform.rotationTime;
        }
    }
}

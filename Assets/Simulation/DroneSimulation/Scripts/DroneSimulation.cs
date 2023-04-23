using UnityEngine;

public class DroneSimulation : Simulation
{
    public DroneSimulationState simState;
    public bool ignoreStateOnStart;

    [Header("Platform")]
    public MovingPlatform platform;
    public Vector3 platformOffset = Vector3.zero;
    public enum PlatformMotion { None, Linear, Sinusoidal }
    public PlatformMotion platformMotion;
    public float platformRestHeight = 2;
    public float platformAmplitude = 1;
    [Tooltip("Time to execute one full cycle"), Min(0)] public float platformPeriod = 2;
    [Tooltip("Rotations per second"), Range(-0.5f, 0.5f)] public float platformRotationFrequency = 0;

    [Header("Drone")]
    public Transform drone;
    public bool droneIsIndependent = true;
    public enum DroneVerticalMotion { None, Linear, Sinusoidal }
    public DroneVerticalMotion droneVerticalMotion;
    public enum DroneHorizontalMotion { None, Linear, Circular }
    public DroneHorizontalMotion droneHorizontalMotion;
    public float droneRestHeight = 2;
    public float droneVerticalAmplitude = 1;
    public float droneHorizontalAmplitude = 2;
    public float droneCircularRadius = 3;
    [Tooltip("Time to execute one full cycle"), Min(0)] public float dronePeriod = 2;
    [Tooltip("Rotations per second"), Range(-0.5f, 0.5f)] public float droneOrbitalFrequency = 0;

    [Header("Point Mass")]
    public Transform pointMass;

    // [Header("Vectors")]
    // public VectorManager vectors;

    private float droneTime = 0;
    private float minDroneHeight;
    private float maxDroneHeight;
    private float droneAngle = 0;

    private float platformTime = 0;
    private float minPlatformHeight;
    private float maxPlatformHeight;
    private float platformAngle = 0;

    public Vector3 Omega => 2 * Mathf.PI * platformRotationFrequency * Vector3.up;

    private void Awake()
    {
        minDroneHeight = droneRestHeight - droneVerticalAmplitude;
        maxDroneHeight = droneRestHeight + droneVerticalAmplitude;

        minPlatformHeight = platformRestHeight - platformAmplitude;
        maxPlatformHeight = platformRestHeight + platformAmplitude;
    }

    private void Start()
    {
        if (simState && !ignoreStateOnStart)
        {
            droneTime = simState.droneTime;
            platformTime = simState.platformTime;
        }
    }

    private void Update()
    {
        droneTime += Time.deltaTime;
        if (droneTime >= dronePeriod) droneTime = 0;

        platformTime += Time.deltaTime;
        if (platformTime >= platformPeriod) platformTime = 0;

        // Compute the platform's position
        Vector3 platformPosition = platformOffset + platformRestHeight * Vector3.up;

        if (platformMotion != PlatformMotion.None)
        {
            float platformHeight = ComputePlatformHeight(platformTime);
            platformPosition = platformOffset + platformHeight * Vector3.up;
        }

        if (platform)
        {
            // Set the platform at the correct height
            platform.SetSurfacePosition(platformPosition);

            // Set platform rotation angle
            platformAngle += Mathf.Sign(Omega.y) * Omega.magnitude * Time.deltaTime;
            platform.SetSurfaceRotation(-platformAngle * Mathf.Rad2Deg);
        }

        // Compute the drone's position
        Vector3 dronePosition = droneRestHeight * Vector3.up;

        if (droneVerticalMotion != DroneVerticalMotion.None)
        {
            dronePosition.y += ComputeDroneHeight(droneTime);
        }

        if (droneHorizontalMotion == DroneHorizontalMotion.Circular)
        {
            droneAngle += 2 * Mathf.PI * droneOrbitalFrequency * Time.deltaTime;
            dronePosition.x = droneCircularRadius * Mathf.Cos(droneAngle);
            dronePosition.z = droneCircularRadius * Mathf.Sin(droneAngle);
        }

        // Set the drone's position relative to the platform or the ground
        dronePosition += droneIsIndependent ? platformOffset : platformPosition;

        // Place the drone
        if (drone) drone.localPosition = dronePosition;

        // Place the point mass
        if (pointMass) pointMass.localPosition = dronePosition;

        // Update the sim state
        if (simState)
        {
            simState.droneTime = droneTime;
            simState.platformTime = platformTime;

            simState.droneVelocityAbsolute = (dronePosition - simState.dronePositionAbsolute) / Time.deltaTime;
            simState.platformVelocity = (platformPosition - simState.platformPosition) / Time.deltaTime;

            simState.omega = Omega;
            simState.origin = transform.localPosition;
            simState.dronePositionAbsolute = dronePosition;
            simState.platformPosition = platformPosition;

            simState.RedrawVectors();
        }
    }

    private float ComputePlatformHeight(float time)
    {
        float height = 0;
        float t = 2 * time / platformPeriod;

        if (t < 1)
        {
            if (platformMotion == PlatformMotion.Sinusoidal)
            {
                t = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
            }
            // position = Vector3.Lerp(position1, position2, t);
            height = Mathf.Lerp(minPlatformHeight, maxPlatformHeight, t);
        }
        else
        {
            t = t - 1;
            if (platformMotion == PlatformMotion.Sinusoidal)
            {
                t = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
            }
            // position = Vector3.Lerp(position2, position1, t);
            height = Mathf.Lerp(maxPlatformHeight, minPlatformHeight, t);
        }

        return height;
    }

    private float ComputeDroneHeight(float time)
    {
        float height = 0;
        float t = 2 * time / dronePeriod;

        if (t < 1)
        {
            if (droneVerticalMotion == DroneVerticalMotion.Sinusoidal)
            {
                t = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
            }
            height = Mathf.Lerp(minDroneHeight, maxDroneHeight, t);
        }
        else
        {
            t = t - 1;
            if (droneVerticalMotion == DroneVerticalMotion.Sinusoidal)
            {
                t = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
            }
            height = Mathf.Lerp(maxDroneHeight, minDroneHeight, t);
        }

        return height;
    }

    public void SetPositionOffset(Vector3 offset)
    {
        platformOffset = offset;
        SetPlatformAtRestPosition();
    }

    public void SetPlatformAtRestPosition()
    {
        if (platform) platform.SetSurfacePosition(platformOffset + platformRestHeight * Vector3.up);

        SetDroneAtRestPosition();
    }

    public void SetDroneAtRestPosition()
    {
        // Debug.Log("DroneSimulation > SetDroneAtRestPosition");

        Vector3 dronePosition = droneRestHeight * Vector3.up;
        dronePosition += droneIsIndependent ? platformOffset : platform.GetSurfacePosition();

        if (drone) drone.localPosition = dronePosition;
        if (pointMass) pointMass.localPosition = dronePosition;
    }

    public void SetDroneMovement(bool isCircular)
    {
        if (isCircular)
        {
            droneHorizontalMotion = DroneHorizontalMotion.Circular;
            droneOrbitalFrequency = 0.3f;
        }
        else
        {
            droneHorizontalMotion = DroneHorizontalMotion.None;
            droneOrbitalFrequency = 0f;
        }
    }
}

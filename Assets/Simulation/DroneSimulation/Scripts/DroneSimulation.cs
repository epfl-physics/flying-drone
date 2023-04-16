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
    [Tooltip("Rad per second"), Range(-0.2f, 0.2f)] public float platformRotationFrequency = 0;

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
    [Tooltip("Rad per second"), Range(-0.2f, 0.2f)] public float droneOrbitalFrequency = 0;

    [Header("Vectors")]
    public VectorManager vectors;

    private float droneTime = 0;
    private float minDroneHeight;
    private float maxDroneHeight;
    private float droneAngle = 0;

    private float platformTime = 0;
    private float minPlatformHeight;
    private float maxPlatformHeight;
    private float platformAngle = 0;

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

        droneAngle += 2 * Mathf.PI * droneOrbitalFrequency * Time.deltaTime;
        platformAngle += 2 * Mathf.PI * platformRotationFrequency * Time.deltaTime;

        // Vector3 relativePosition = platformOffset;
        Vector3 platformPosition = platformOffset + platformRestHeight * Vector3.up;
        Vector3 dronePosition = droneRestHeight * Vector3.up;
        // if (!droneIsIndependent) dronePosition += platformPosition;

        if (platform)
        {
            // Set platform y position
            if (platformMotion != PlatformMotion.None)
            {
                float platformHeight = ComputePlatformHeight(platformTime);
                platformPosition = platformOffset + platformHeight * Vector3.up;
                platform.SetSurfacePosition(platformPosition);
            }

            // Set platform rotation angle
            platform.SetSurfaceRotation(-platformAngle * Mathf.Rad2Deg);
        }

        // if (!droneIsIndependent)
        //     {
        //         // platform.height refers to the y value of its child Surface transform
        //         relativePosition = platform.GetSurfacePosition();
        //         Debug.Log("Drone relative position " + relativePosition);
        //     }

        if (drone)
        {
            // Vector3 dronePosition = relativePosition + droneRestHeight * Vector3.up;

            if (droneVerticalMotion != DroneVerticalMotion.None)
            {
                dronePosition.y += ComputeDroneHeight(droneTime);
            }

            if (droneHorizontalMotion == DroneHorizontalMotion.Circular)
            {
                dronePosition.x = droneCircularRadius * Mathf.Cos(droneAngle);
                dronePosition.z = droneCircularRadius * Mathf.Sin(droneAngle);
            }

            if (!droneIsIndependent) dronePosition += platformPosition;

            drone.localPosition = dronePosition;
        }

        if (vectors) vectors.Redraw(platformPosition, dronePosition);

        if (simState)
        {
            simState.droneTime = droneTime;
            simState.platformTime = platformTime;
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
        if (drone)
        {
            Debug.Log("DroneSimulation > SetDroneAtRestPosition");
            Vector3 relativePosition = Vector3.zero;
            if (!droneIsIndependent)
            {
                relativePosition = platformOffset + platformRestHeight * Vector3.up;
            }

            drone.localPosition = relativePosition + droneRestHeight * Vector3.up;
        }
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

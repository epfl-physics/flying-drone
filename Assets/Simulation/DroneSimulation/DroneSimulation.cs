using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSimulation : Simulation
{
    public Transform drone;
    public MovingPlatform platform;
    public DroneSimulationState simState;
    public bool ignoreStateOnStart;

    [Header("Parameters")]
    public float minHeight = 0;
    public float maxHeight = 5;
    public float radius = 3;
    public float period = 4;
    public float droneHeightOffset = 1;
    [Range(-1, 1)] public float platformRotationFrequency = 0;

    [Header("Vectors")]
    public Vector platformPositionVector;
    public Vector dronePositionVector;
    public Vector relativePositionVector;

    private float time = 0;

    public enum PlatformMotion { Linear, Sinusoidal }
    public enum DroneMotion { Linear, Sinusoidal }

    [Header("Motion")]
    public PlatformMotion platformMotion;
    public DroneMotion droneMotion;

    private void Start()
    {
        if (simState && !ignoreStateOnStart)
        {
            time = simState.time;
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= period) time = 0;

        if (platform)
        {
            float platformHeight = ComputePlatformHeight(time);

            platform.SetHeight(platformHeight);
            float deltaAngle = -platformRotationFrequency * 360f * Time.deltaTime;
            platform.RotateSurface(Vector3.up, deltaAngle);

            if (drone) drone.position = (platformHeight + droneHeightOffset) * Vector3.up;
        }

        // if (positionVector)
        // {
        //     positionVector.components = drone.position;
        //     positionVector.Redraw();
        // }

        if (simState) simState.time = time;
    }

    private float ComputePlatformHeight(float time)
    {
        float height = 0;
        // Vector3 position1 = minHeight * Vector3.up;
        // Vector3 position2 = maxHeight * Vector3.up;

        float t = 2 * time / period;

        if (t < 1)
        {
            if (platformMotion == PlatformMotion.Sinusoidal)
            {
                t = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
            }
            // position = Vector3.Lerp(position1, position2, t);
            height = Mathf.Lerp(minHeight, maxHeight, t);
        }
        else
        {
            t = t - 1;
            if (platformMotion == PlatformMotion.Sinusoidal)
            {
                t = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
            }
            // position = Vector3.Lerp(position2, position1, t);
            height = Mathf.Lerp(maxHeight, minHeight, t);
        }

        return height;
    }
}

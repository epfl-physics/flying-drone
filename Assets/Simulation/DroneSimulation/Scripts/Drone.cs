using UnityEngine;

public class Drone : MonoBehaviour
{
    public bool autoUpdate;

    public enum VerticalMotionType { Linear, Sinusoidal }
    public enum CircularMotionType { Constant, Sinusoidal }
    public DroneData data;

    public float Omega => ComputeOmega();
    public float OmegaDot => ComputeOmegaDot();

    [Header("Running clocks")]
    public float verticalTime = 0;
    public float circularTime = 0;

    // Orbital angle
    [HideInInspector] public float angle = 0;

    private void Update()
    {
        if (autoUpdate) TakeAStep(UnityEngine.Time.deltaTime);
    }

    public void TakeAStep(float deltaTime)
    {
        // Get the drone's rest position
        Vector3 position = data.restPosition;

        // Compute the drone's current vertical position
        if (data.verticalFrequency > 0)
        {
            verticalTime += deltaTime;

            // Reset the translation clock after a vertical period
            if (verticalTime > data.VerticalPeriod) verticalTime = 0;

            position.y += ComputeHeightOffset();
        }

        // Compute the drone's current position in the horizontal plane
        angle += Omega * deltaTime;
        angle = angle % (2 * Mathf.PI);
        position.x = data.circularRadius * Mathf.Cos(angle);
        position.z = data.circularRadius * Mathf.Sin(angle);

        if (data.circularMotionType == CircularMotionType.Sinusoidal)
        {
            circularTime += deltaTime;
        }

        // Set the drone's position relative to the specified origin
        position += data.origin;

        // Place the drone
        transform.localPosition = position;
    }

    private float ComputeHeightOffset()
    {
        float heightOffset;
        float quarterPeriod = 0.25f * data.VerticalPeriod;
        float time = verticalTime;

        switch (data.verticalMotionType)
        {
            case VerticalMotionType.Linear:
                // Sawtooth function
                float slope = data.verticalAmplitude / quarterPeriod;
                if (time < quarterPeriod)
                {
                    heightOffset = slope * time;
                }
                else if (time < 3 * quarterPeriod)
                {
                    heightOffset = 2 * data.verticalAmplitude - slope * time;
                }
                else
                {
                    heightOffset = slope * (time - data.VerticalPeriod);
                }
                break;
            case VerticalMotionType.Sinusoidal:
                // Sinusoidal function
                float omega = 2 * Mathf.PI * data.verticalFrequency;
                heightOffset = data.verticalAmplitude * Mathf.Sin(omega * time);
                break;
            default:
                heightOffset = 0;
                break;
        }

        return heightOffset;
    }

    public void SetRestHeight(float value)
    {
        data.restPosition.y = value;
    }

    public void ReturnToRestPosition()
    {
        transform.localPosition = data.origin + data.restPosition;
    }

    public Vector3 GetVelocity()
    {
        Vector3 v = Vector3.zero;
        float quarterPeriod = 0.25f * data.VerticalPeriod;
        float time = verticalTime;

        switch (data.verticalMotionType)
        {
            case VerticalMotionType.Linear:
                // Sawtooth function
                float slope = data.verticalAmplitude / quarterPeriod;
                if (time < quarterPeriod || time >= 3 * quarterPeriod)
                {
                    v.y = slope;
                }
                else
                {
                    v.y = -slope;
                }
                break;
            case VerticalMotionType.Sinusoidal:
                // Sinusoidal function
                float omega = 2 * Mathf.PI * data.verticalFrequency;
                v.y = data.verticalAmplitude * omega * Mathf.Cos(omega * time);
                break;
            default:
                break;
        }

        float r = data.circularRadius;
        v.x = -Omega * r * Mathf.Sin(angle);
        v.z = Omega * r * Mathf.Cos(angle);

        return v;
    }

    public Vector3 GetAcceleration()
    {
        Vector3 a = Vector3.zero;
        float quarterPeriod = 0.25f * data.VerticalPeriod;
        float time = verticalTime;

        if (data.verticalMotionType == VerticalMotionType.Sinusoidal)
        {
            float omega = 2 * Mathf.PI * data.verticalFrequency;
            a.y = -data.verticalAmplitude * omega * omega * Mathf.Sin(omega * time);
        }

        float r = data.circularRadius;
        a.x = -r * (Omega * Omega * Mathf.Cos(angle) + OmegaDot * Mathf.Sin(angle));
        a.z = -r * (Omega * Omega * Mathf.Sin(angle) - OmegaDot * Mathf.Cos(angle));

        return a;
    }

    private float ComputeOmega()
    {
        float frequency = 0;

        if (data.circularMotionType == CircularMotionType.Constant)
        {
            frequency = data.circularFrequency;
        }
        else if (data.circularMotionType == CircularMotionType.Sinusoidal)
        {
            float angularSpeed = 2 * Mathf.PI * data.circularFrequencyVariable;
            frequency = data.circularFrequency * Mathf.Cos(angularSpeed * circularTime);
        }

        return 2 * Mathf.PI * frequency;
    }

    private float ComputeOmegaDot()
    {
        float frequencyDot = 0;

        if (data.circularMotionType == CircularMotionType.Sinusoidal)
        {
            float angularSpeed = 2 * Mathf.PI * data.circularFrequencyVariable;
            frequencyDot = -angularSpeed * data.circularFrequency * Mathf.Sin(angularSpeed * circularTime);
        }

        return 2 * Mathf.PI * frequencyDot;
    }
}

[System.Serializable]
public class DroneData
{
    [Tooltip("Position of the drone's origin")]
    public Vector3 origin = Vector3.zero;
    [Tooltip("Default position relative to origin"), Min(0)]
    public Vector3 restPosition = 4 * Vector3.up;

    [Header("Vertical Motion")]
    public Drone.VerticalMotionType verticalMotionType;
    [Tooltip("Vertical displacement")]
    public float verticalAmplitude = 1;
    [Tooltip("Translation cycles per second"), Range(0, 2)]
    public float verticalFrequency = 0.5f;
    public float VerticalPeriod => 1f / verticalFrequency;

    [Header("Circular Motion")]
    public Drone.CircularMotionType circularMotionType;
    [Tooltip("Radius of circular displacement"), Min(0)]
    public float circularRadius = 1;
    [Tooltip("Rotations per second"), Range(-0.5f, 0.5f)]
    public float circularFrequency = 0;
    [Tooltip("Rotation cycles per second when variable"), Range(0, 2)]
    public float circularFrequencyVariable = 0.5f;
}

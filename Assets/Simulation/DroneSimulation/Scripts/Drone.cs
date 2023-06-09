using UnityEngine;

public class Drone : MonoBehaviour
{
    public bool autoUpdate;

    public enum VerticalMotionType { None, Linear, Sinusoidal }
    public enum CircularMotionType { None, Constant, Sinusoidal }
    public DroneData data;

    private float time = 0;
    public float Time { get { return time; } set { time = value; } }

    // Vertical displacement
    private float minHeight;
    private float maxHeight;

    // Orbital angle
    private float angle = 0;

    private void Awake()
    {
        ComputeHeightBounds();
    }

    private void Update()
    {
        if (autoUpdate) TakeAStep(UnityEngine.Time.deltaTime);
    }

    public void TakeAStep(float deltaTime)
    {
        time += deltaTime;
        if (time >= 2 * data.verticalPeriod) time = 0;

        // Compute the drone's local position
        Vector3 position = data.restPosition;

        if (data.verticalMotionType != VerticalMotionType.None)
        {
            position.y = ComputeHeight(time);
        }

        float omega = ComputeOmega(time);
        angle += omega * deltaTime;
        position.x = data.circularRadius * Mathf.Cos(angle);
        position.z = data.circularRadius * Mathf.Sin(angle);

        // Set the drone's position relative to the specified origin
        position += data.origin;

        // Place the drone
        transform.localPosition = position;
    }

    private float ComputeHeight(float time)
    {
        float height = 0;
        float t = time / data.verticalPeriod;
        if (time >= data.verticalPeriod) t -= 1;

        if (t < 0.5f)
        {
            if (data.verticalMotionType == VerticalMotionType.Sinusoidal)
            {
                t = 0.25f * (1 - Mathf.Cos(2 * Mathf.PI * t));
            }
            height = Mathf.Lerp(minHeight, maxHeight, 2 * t);
        }
        else
        {
            t = t - 0.5f;
            if (data.verticalMotionType == VerticalMotionType.Sinusoidal)
            {
                t = 0.25f * (1 - Mathf.Cos(2 * Mathf.PI * t));
            }
            height = Mathf.Lerp(maxHeight, minHeight, 2 * t);
        }

        return height;
    }

    public void ComputeHeightBounds()
    {
        minHeight = data.restPosition.y - data.verticalAmplitude;
        maxHeight = data.restPosition.y + data.verticalAmplitude;
    }

    public void SetRestHeight(float value)
    {
        data.restPosition.y = value;
        ComputeHeightBounds();
    }

    public void SetVerticalAmplitude(float value)
    {
        data.verticalAmplitude = value;
        ComputeHeightBounds();
    }

    public void ReturnToRestPosition()
    {
        // Debug.Log("ReturnToRestPosition");
        transform.localPosition = data.origin + data.restPosition;
    }

    public Vector3 GetVelocity()
    {
        Vector3 velocity = Vector3.zero;

        float t = time / data.verticalPeriod;
        if (time >= data.verticalPeriod) t -= 1;

        if (data.verticalMotionType == VerticalMotionType.Linear)
        {
            if (t < 0.5f)
            {
                velocity.y = 4 * data.verticalAmplitude / data.verticalPeriod;
            }
            else
            {
                velocity.y = -4 * data.verticalAmplitude / data.verticalPeriod;
            }
        }
        else if (data.verticalMotionType == VerticalMotionType.Sinusoidal)
        {
            if (t < 0.5f)
            {
                velocity.y = 2 * data.verticalAmplitude * Mathf.PI * Mathf.Sin(2 * Mathf.PI * t) / data.verticalPeriod;
            }
            else
            {
                velocity.y = -2 * data.verticalAmplitude * Mathf.PI * Mathf.Sin(2 * Mathf.PI * (t - 0.5f)) / data.verticalPeriod;
            }
        }

        float omega = ComputeOmega(time);
        float r = data.circularRadius;
        velocity.x = -omega * r * Mathf.Sin(angle);
        velocity.z = omega * r * Mathf.Cos(angle);

        return velocity;
    }

    public Vector3 GetAcceleration()
    {
        Vector3 acceleration = Vector3.zero;

        float t = time / data.verticalPeriod;
        if (time >= data.verticalPeriod) t -= 1;

        if (data.verticalMotionType == VerticalMotionType.Sinusoidal)
        {
            if (t < 0.5f)
            {
                acceleration.y = 4 * data.verticalAmplitude * Mathf.PI * Mathf.PI * Mathf.Cos(2 * Mathf.PI * t) / data.verticalPeriod / data.verticalPeriod;
            }
            else
            {
                acceleration.y = -4 * data.verticalAmplitude * Mathf.PI * Mathf.PI * Mathf.Cos(2 * Mathf.PI * (t - 0.5f)) / data.verticalPeriod / data.verticalPeriod;
            }
        }

        float omega = ComputeOmega(time);
        float omegaDot = ComputeOmegaDot(time);
        float r = data.circularRadius;
        acceleration.x = -r * (omega * omega * Mathf.Cos(angle) + omegaDot * Mathf.Sin(angle));
        acceleration.z = -r * (omega * omega * Mathf.Sin(angle) - omegaDot * Mathf.Cos(angle));

        return acceleration;
    }

    private float ComputeOmega(float time)
    {
        float frequency = data.circularFrequency;

        if (data.circularMotionType == CircularMotionType.Sinusoidal)
        {
            // Positive for one vertical period, negative for the next, and repeat
            float angularSpeed = 0.5f * (2 * Mathf.PI / data.verticalPeriod);
            // frequency = 0.5f * data.circularFrequency * (1 - Mathf.Cos(2 * Mathf.PI * time / data.verticalPeriod));
            frequency = data.circularFrequency * Mathf.Sin(angularSpeed * time);
        }

        return 2 * Mathf.PI * frequency;
    }

    private float ComputeOmegaDot(float time)
    {
        float frequencyDot = 0;

        if (data.circularMotionType == CircularMotionType.Sinusoidal)
        {
            // float factor = 2 * Mathf.PI * data.circularFrequency / data.verticalPeriod;
            // frequencyDot = 0.5f * factor * Mathf.Sin(2 * Mathf.PI * time / data.verticalPeriod);
            float angularSpeed = 0.5f * (2 * Mathf.PI / data.verticalPeriod);
            frequencyDot = angularSpeed * data.circularFrequency * Mathf.Cos(angularSpeed * time);
        }

        return 2 * Mathf.PI * frequencyDot;
    }
}

[System.Serializable]
public class DroneData
{
    [Tooltip("Origin")] public Vector3 origin = Vector3.zero;
    [Tooltip("Relative to origin"), Min(0)] public Vector3 restPosition = 4 * Vector3.up;
    [Tooltip("Vertical displacement")] public float verticalAmplitude = 1;
    [Tooltip("Time to execute one full cycle"), Min(0)] public float verticalPeriod = 2;
    public Drone.VerticalMotionType verticalMotionType;
    [Tooltip("Radius of circular displacement")] public float circularRadius = 1;
    [Tooltip("Rotations per second"), Range(-0.5f, 0.5f)] public float circularFrequency = 0;
    public Drone.CircularMotionType circularMotionType;
}

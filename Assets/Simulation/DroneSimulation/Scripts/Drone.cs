using UnityEngine;

public class Drone : MonoBehaviour
{
    public bool autoUpdate;

    [Header("Parameters")]
    [Min(0)] public float restHeight = 2;
    [Tooltip("Vertical displacement")] public float verticalAmplitude = 1;
    [Tooltip("Time to execute one full cycle"), Min(0)] public float translationPeriod = 2;
    public enum VerticalMotionType { None, Linear, Sinusoidal }
    public VerticalMotionType verticalMotionType;
    [Tooltip("Horizontal displacement")] public float horizontalAmplitude = 2;
    [Tooltip("Radius of circular displacement")] public float circularRadius = 1;
    [Tooltip("Rotations per second"), Range(-0.5f, 0.5f)] public float orbitalFrequency = 0;
    public enum HorizontalMotionType { None, Linear, Circular }
    public HorizontalMotionType horizontalMotionType;

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
        if (autoUpdate) TakeAStep(UnityEngine.Time.deltaTime, Vector3.zero);
    }

    public void TakeAStep(float deltaTime, Vector3 relativeTo)
    {
        time += deltaTime;
        if (time >= translationPeriod) time = 0;

        // Compute the drone's position
        Vector3 position = restHeight * Vector3.up;

        if (verticalMotionType != VerticalMotionType.None)
        {
            position = ComputeHeight(time) * Vector3.up;
        }

        if (horizontalMotionType == HorizontalMotionType.Circular)
        {
            angle += 2 * Mathf.PI * orbitalFrequency * deltaTime;
            position.x = circularRadius * Mathf.Cos(angle);
            position.z = circularRadius * Mathf.Sin(angle);
        }

        // Set the drone's position relative to the platform or the ground
        position += relativeTo;

        // Place the drone
        transform.localPosition = position;
    }

    private float ComputeHeight(float time)
    {
        float height = 0;
        float t = 2 * time / translationPeriod;

        if (t < 1)
        {
            if (verticalMotionType == VerticalMotionType.Sinusoidal)
            {
                t = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
            }
            height = Mathf.Lerp(minHeight, maxHeight, t);
        }
        else
        {
            t = t - 1;
            if (verticalMotionType == VerticalMotionType.Sinusoidal)
            {
                t = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
            }
            height = Mathf.Lerp(maxHeight, minHeight, t);
        }

        return height;
    }

    public void ComputeHeightBounds()
    {
        minHeight = restHeight - verticalAmplitude;
        maxHeight = restHeight + verticalAmplitude;
    }

    public void SetRestHeight(float value)
    {
        restHeight = value;
        ComputeHeightBounds();
    }

    public void SetVerticalAmplitude(float value)
    {
        verticalAmplitude = value;
        ComputeHeightBounds();
    }

    public void ReturnToRestPosition()
    {
        Debug.Log("ReturnToRestPosition");
        Vector3 restPosition = restHeight * Vector3.up;
        if (horizontalMotionType == HorizontalMotionType.Linear)
        {
            restPosition += horizontalAmplitude * Vector3.right;
        }
        else if (horizontalMotionType == HorizontalMotionType.Circular)
        {
            restPosition += circularRadius * Vector3.right;
        }
        transform.localPosition = restPosition;
    }

    public Vector3 GetVelocity()
    {
        Vector3 velocity = Vector3.zero;

        float t = 2 * time / translationPeriod;

        if (verticalMotionType == VerticalMotionType.Linear)
        {
            if (t < 1)
            {
                velocity.y = 2 * verticalAmplitude / translationPeriod;
            }
            else
            {
                velocity.y = -2 * verticalAmplitude / translationPeriod;
            }
        }
        else if (verticalMotionType == VerticalMotionType.Sinusoidal)
        {
            if (t < 1)
            {
                velocity.y = Mathf.PI * Mathf.Sin(Mathf.PI * t) / translationPeriod;
            }
            else
            {
                velocity.y = -Mathf.PI * Mathf.Sin(Mathf.PI * (t - 1)) / translationPeriod;
            }
        }

        if (horizontalMotionType == HorizontalMotionType.Circular)
        {
            float a = 2 * Mathf.PI * orbitalFrequency;
            velocity.x = -a * circularRadius * Mathf.Sin(angle);
            velocity.z = a * circularRadius * Mathf.Cos(angle);
        }

        return velocity;
    }
}

[System.Serializable]
public class DroneData
{
    public bool isIndependent = true;
    [Min(0)] public float restHeight = 2;
    [Tooltip("Vertical displacement")] public float verticalAmplitude = 1;
    [Tooltip("Time to execute one full cycle"), Min(0)] public float translationPeriod = 2;
    public Drone.VerticalMotionType verticalMotionType;
    [Tooltip("Horizontal displacement")] public float horizontalAmplitude = 2;
    [Tooltip("Radius of circular displacement")] public float circularRadius = 1;
    [Tooltip("Rotations per second"), Range(-0.5f, 0.5f)] public float orbitalFrequency = 0;
    public Drone.HorizontalMotionType horizontalMotionType;
}

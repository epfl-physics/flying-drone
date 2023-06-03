using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool autoUpdate;

    public enum MotionType { None, Linear, Sinusoidal }
    public enum RotationType { None, Constant, Sinusoidal }
    public PlatformData data;

    [Header("Components")]
    [SerializeField] private Transform surface;
    [SerializeField] private Transform piston;
    [SerializeField] private Transform basisTriad;
    [SerializeField] private Transform e3;
    [SerializeField] private Transform originLabel;
    [SerializeField] private Vector3 originLabelOffset;

    // public Vector3 Omega => 2 * Mathf.PI * rotationFrequency * Vector3.up;
    public Vector3 Omega => ComputeOmega();
    public Vector3 OmegaDot => ComputeOmegaDot();

    private float time = 0;
    public float Time { get { return time; } set { time = value; } }

    public Vector3 Position { get { return transform.localPosition; } set { transform.localPosition = value; } }

    private float minHeight;
    private float maxHeight;
    private float angle = 0;

    private void Awake()
    {
        ComputeHeightBounds();
        SetSurfaceAlpha(1);
    }

    private void OnDisable()
    {
        SetSurfaceAlpha(1);
    }

    private void Update()
    {
        if (autoUpdate) TakeAStep(UnityEngine.Time.deltaTime);
    }

    public void TakeAStep(float deltaTime)
    {
        time += deltaTime;
        if (time >= data.translationPeriod) time = 0;

        // Compute the platform's position
        Vector3 position = data.restHeight * Vector3.up;

        if (data.motionType != MotionType.None)
        {
            float height = ComputeHeight(time);
            position = height * Vector3.up;
        }

        // Set the surface at the correct height
        SetSurfacePosition(position);

        // Set the rotation angle
        angle += Mathf.Sign(Omega.y) * Omega.magnitude * deltaTime;
        SetSurfaceRotation(-angle * Mathf.Rad2Deg);
    }

    private float ComputeHeight(float time)
    {
        float height = 0;
        float t = 2 * time / data.translationPeriod;

        if (t < 1)
        {
            if (data.motionType == MotionType.Sinusoidal)
            {
                t = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
            }
            height = Mathf.Lerp(minHeight, maxHeight, t);
        }
        else
        {
            t = t - 1;
            if (data.motionType == MotionType.Sinusoidal)
            {
                t = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
            }
            height = Mathf.Lerp(maxHeight, minHeight, t);
        }

        return height;
    }

    private Vector3 ComputeOmega()
    {
        float frequency = 0;

        if (data.rotationType == RotationType.Constant)
        {
            frequency = data.rotationFrequency;
        }
        else if (data.rotationType == RotationType.Sinusoidal)
        {
            frequency = 0.5f * data.rotationFrequency * (1 - Mathf.Cos(2 * Mathf.PI * time / data.translationPeriod));
        }

        return 2 * Mathf.PI * frequency * Vector3.up;
    }

    private Vector3 ComputeOmegaDot()
    {
        // Vector3 omegaDot = Vector3.zero;
        float frequencyDot = 0;

        if (data.rotationType == RotationType.Sinusoidal)
        {
            float factor = 2 * Mathf.PI * data.rotationFrequency / data.translationPeriod;
            frequencyDot = 0.5f * factor * Mathf.Sin(2 * Mathf.PI * time / data.translationPeriod);
        }

        return 2 * Mathf.PI * frequencyDot * Vector3.up;
    }

    private void ComputeHeightBounds()
    {
        minHeight = data.restHeight - data.amplitude;
        maxHeight = data.restHeight + data.amplitude;
    }

    public void SetAmplitude(float value)
    {
        data.amplitude = value;
        ComputeHeightBounds();
    }

    public void SetRestHeight(float value)
    {
        if (!surface) return;

        Vector3 surfacePosition = surface.localPosition;
        surfacePosition.y = value;
        SetSurfacePosition(surfacePosition);
        ComputeHeightBounds();
        data.restHeight = value;
    }

    private void SetSurfacePosition(Vector3 position)
    {
        // Set the surface at the input position
        if (surface) surface.localPosition = position;

        if (basisTriad) basisTriad.localPosition = position;

        if (e3) e3.localPosition = position;

        if (originLabel) originLabel.position = (surface ? surface.position : Vector3.zero) + originLabelOffset;

        // Scale the piston to connect the surface to the ground
        if (piston)
        {
            float height = position.y;
            float scaleY = 0.5f * height;
            piston.localPosition = position - scaleY * Vector3.up;
            Vector3 scale = piston.localScale;
            scale.y = scaleY;
            piston.localScale = scale;
        }
    }

    private void SetSurfaceRotation(float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        if (surface) surface.localRotation = rotation;

        if (basisTriad) basisTriad.localRotation = rotation;
    }

    public Vector3 GetSurfacePosition()
    {
        Vector3 position = ComputeHeight(0) * Vector3.up;

        if (surface) position = surface.localPosition;

        return position;
    }

    public Vector3 GetVelocity()
    {
        Vector3 velocity = Vector3.zero;

        float t = 2 * time / data.translationPeriod;
        float speed = 0;

        if (data.motionType == MotionType.Linear)
        {
            if (t < 1)
            {
                speed = 2 * data.amplitude / data.translationPeriod;
            }
            else
            {
                speed = -2 * data.amplitude / data.translationPeriod;
            }
        }
        else if (data.motionType == MotionType.Sinusoidal)
        {
            if (t < 1)
            {
                speed = Mathf.PI * Mathf.Sin(Mathf.PI * t) / data.translationPeriod;
            }
            else
            {
                speed = -Mathf.PI * Mathf.Sin(Mathf.PI * (t - 1)) / data.translationPeriod;
            }
        }

        return speed * Vector3.up;
    }

    public Vector3 GetAcceleration()
    {
        Vector3 acceleration = Vector3.zero;

        float t = 2 * time / data.translationPeriod;
        float a = 0;

        if (data.motionType == MotionType.Sinusoidal)
        {
            if (t < 1)
            {
                a = Mathf.PI * Mathf.PI * Mathf.Cos(Mathf.PI * t) / data.translationPeriod / data.translationPeriod;
            }
            else
            {
                a = -Mathf.PI * Mathf.PI * Mathf.Cos(Mathf.PI * (t - 1)) / data.translationPeriod / data.translationPeriod;
            }
        }

        return a * Vector3.up;
    }

    public void SetSurfaceAlpha(float value)
    {
        if (surface)
        {
            surface.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Alpha", value);
        }
    }

    public void SetSurfaceTransparency(bool isTransparent)
    {
        if (surface)
        {
            float alpha = isTransparent ? 0.05f : 1;
            surface.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Alpha", alpha);
        }
    }

}

[System.Serializable]
public class PlatformData
{
    [Tooltip("Of the base")] public Vector3 position;
    [Tooltip("Of the surface above the base"), Min(0)] public float restHeight = 1;
    [Tooltip("Vertical displacement")] public float amplitude = 1;
    [Tooltip("Time to execute one full cycle"), Min(0)] public float translationPeriod = 2;
    [Tooltip("Rotations per second"), Range(-0.5f, 0.5f)] public float rotationFrequency = 0;
    public MovingPlatform.MotionType motionType;
    public MovingPlatform.RotationType rotationType;
}

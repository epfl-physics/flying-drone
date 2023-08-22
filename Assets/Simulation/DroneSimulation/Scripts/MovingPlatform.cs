using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool autoUpdate;

    public enum TranslationType { None, Linear, Sinusoidal }
    public enum RotationType { None, Constant, Sinusoidal }
    public PlatformData data;

    [Header("Components")]
    [SerializeField] private Transform surface;
    [SerializeField] private Transform piston;
    [SerializeField] private Transform basisTriad;
    [SerializeField] private Vector3 basisOffset;
    [SerializeField] private Transform e3;
    [SerializeField] private Transform originLabel;
    [SerializeField] private Vector3 originLabelOffset;

    // public Vector3 Omega => 2 * Mathf.PI * rotationFrequency * Vector3.up;
    public Vector3 Omega => ComputeOmega();
    public Vector3 OmegaDot => ComputeOmegaDot();
    [HideInInspector] public bool tieRotationToTranslation = true;

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
        // Reset the clock after one vertical period
        if (data.rotationType != RotationType.Sinusoidal)
        {
            if (time >= 2 * data.translationPeriod) time = 0;
        }

        // Compute the platform's position
        Vector3 position = data.restHeight * Vector3.up;

        if (data.translationType != TranslationType.None)
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
        float t = time / data.translationPeriod;
        if (time >= data.translationPeriod) t -= 1;

        if (t < 0.5f)
        {
            if (data.translationType == TranslationType.Sinusoidal)
            {
                t = 0.25f * (1 - Mathf.Cos(2 * Mathf.PI * t));
            }
            height = Mathf.Lerp(minHeight, maxHeight, 2 * t);
        }
        else
        {
            t = t - 0.5f;
            if (data.translationType == TranslationType.Sinusoidal)
            {
                t = 0.25f * (1 - Mathf.Cos(2 * Mathf.PI * t));
            }
            height = Mathf.Lerp(maxHeight, minHeight, 2 * t);
        }

        return height;
    }

    public Vector3 GetVelocity()
    {
        Vector3 velocity = Vector3.zero;

        // float t = 2 * time / data.translationPeriod;
        float t = time / data.translationPeriod;
        if (time >= data.translationPeriod) t -= 1;
        float speed = 0;

        if (data.translationType == TranslationType.Linear)
        {
            if (t < 0.5f)
            {
                speed = 4 * data.translationAmplitude / data.translationPeriod;
            }
            else
            {
                speed = -4 * data.translationAmplitude / data.translationPeriod;
            }
        }
        else if (data.translationType == TranslationType.Sinusoidal)
        {
            if (t < 0.5f)
            {
                speed = 2 * data.translationAmplitude * Mathf.PI * Mathf.Sin(2 * Mathf.PI * t) / data.translationPeriod;
            }
            else
            {
                speed = -2 * data.translationAmplitude * Mathf.PI * Mathf.Sin(2 * Mathf.PI * (t - 0.5f)) / data.translationPeriod;
            }
        }

        return speed * Vector3.up;
    }

    public Vector3 GetAcceleration()
    {
        Vector3 acceleration = Vector3.zero;

        // float t = 2 * time / data.translationPeriod;
        float t = time / data.translationPeriod;
        if (time >= data.translationPeriod) t -= 1;
        float a = 0;

        if (data.translationType == TranslationType.Sinusoidal)
        {
            if (t < 0.5f)
            {
                a = 4 * data.translationAmplitude * Mathf.PI * Mathf.PI * Mathf.Cos(2 * Mathf.PI * t) / data.translationPeriod / data.translationPeriod;
            }
            else
            {
                a = -4 * data.translationAmplitude * Mathf.PI * Mathf.PI * Mathf.Cos(2 * Mathf.PI * (t - 0.5f)) / data.translationPeriod / data.translationPeriod;
            }
        }

        return a * Vector3.up;
    }

    private Vector3 ComputeOmega()
    {
        // Default to zero for RotationType.None
        float frequency = 0;

        if (data.rotationType == RotationType.Constant)
        {
            frequency = data.rotationFrequency;
        }
        else if (data.rotationType == RotationType.Sinusoidal)
        {
            // Let it take 2 seconds to go from max to min rotation rate
            float angularSpeed = 0.5f * Mathf.PI;

            if (tieRotationToTranslation)
            {
                // Positive for one vertical period, negative for the next, and repeat
                angularSpeed = 0.5f * (2 * Mathf.PI / data.translationPeriod);
            }

            frequency = data.rotationFrequency * Mathf.Sin(angularSpeed * time);
        }

        return 2 * Mathf.PI * frequency * Vector3.up;
    }

    private Vector3 ComputeOmegaDot()
    {
        float frequencyDot = 0;

        if (data.rotationType == RotationType.Sinusoidal)
        {
            float angularSpeed = 0.5f * Mathf.PI;

            if (tieRotationToTranslation)
            {
                angularSpeed = 0.5f * (2 * Mathf.PI / data.translationPeriod);
            }

            frequencyDot = angularSpeed * data.rotationFrequency * Mathf.Cos(angularSpeed * time);
        }

        return 2 * Mathf.PI * frequencyDot * Vector3.up;
    }

    private void ComputeHeightBounds()
    {
        minHeight = data.restHeight - data.translationAmplitude;
        maxHeight = data.restHeight + data.translationAmplitude;
    }

    public void SetAmplitude(float value)
    {
        data.translationAmplitude = value;
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

        if (basisTriad) basisTriad.localPosition = position + basisOffset;

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

    public void SetSurfaceAlpha(float value)
    {
        if (surface)
        {
            surface.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Alpha", value);
        }
    }

    public void SetSurfaceTransparency(bool isTransparent)
    {
        if (surface)
        {
            float alpha = isTransparent ? 0.05f : 1;
            surface.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Alpha", alpha);
        }
    }

}

[System.Serializable]
public class PlatformData
{
    [Tooltip("Of the base")] public Vector3 position;
    [Tooltip("Of the surface above the base"), Min(0)] public float restHeight = 1;
    [Tooltip("Vertical displacement")] public float translationAmplitude = 1;
    [Tooltip("Time to execute one full cycle"), Min(0)] public float translationPeriod = 2;
    [Tooltip("Rotations per second"), Range(-0.5f, 0.5f)] public float rotationFrequency = 0;
    public MovingPlatform.TranslationType translationType;
    public MovingPlatform.RotationType rotationType;
}

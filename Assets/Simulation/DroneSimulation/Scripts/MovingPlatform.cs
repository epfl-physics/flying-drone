using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool autoUpdate;

    [Header("Components")]
    [SerializeField] private Transform surface;
    [SerializeField] private Transform piston;
    [SerializeField] private Transform basisTriad;
    [SerializeField] private Transform originLabel;
    [SerializeField] private Vector3 originLabelOffset;

    [Header("Parameters")]
    [Min(0)] public float restHeight = 1;
    [Tooltip("Vertical displacement")] public float amplitude = 1;
    [Tooltip("Time to execute one full cycle"), Min(0)] public float translationPeriod = 2;
    [Tooltip("Rotations per second"), Range(-0.5f, 0.5f)] public float rotationFrequency = 0;
    public enum MotionType { None, Linear, Sinusoidal }
    public MotionType motionType;

    public Vector3 Omega => 2 * Mathf.PI * rotationFrequency * Vector3.up;

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

    private void Update()
    {
        if (autoUpdate) TakeAStep(UnityEngine.Time.deltaTime);
    }

    public void TakeAStep(float deltaTime)
    {
        time += deltaTime;
        if (time >= translationPeriod) time = 0;

        // Compute the platform's position
        Vector3 position = restHeight * Vector3.up;

        if (motionType != MotionType.None)
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
        float t = 2 * time / translationPeriod;

        if (t < 1)
        {
            if (motionType == MotionType.Sinusoidal)
            {
                t = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
            }
            height = Mathf.Lerp(minHeight, maxHeight, t);
        }
        else
        {
            t = t - 1;
            if (motionType == MotionType.Sinusoidal)
            {
                t = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
            }
            height = Mathf.Lerp(maxHeight, minHeight, t);
        }

        return height;
    }

    private void ComputeHeightBounds()
    {
        minHeight = restHeight - amplitude;
        maxHeight = restHeight + amplitude;
    }

    public void SetAmplitude(float value)
    {
        amplitude = value;
        ComputeHeightBounds();
    }

    public void SetRestHeight(float value)
    {
        if (!surface) return;

        Vector3 surfacePosition = surface.localPosition;
        surfacePosition.y = value;
        SetSurfacePosition(surfacePosition);
        ComputeHeightBounds();
        restHeight = value;
    }

    private void SetSurfacePosition(Vector3 position)
    {
        // Set the surface at the input position
        if (surface) surface.localPosition = position;

        if (basisTriad) basisTriad.localPosition = position;

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

    // public void RotateSurface(Vector3 axis, float angle)
    // {
    //     if (surface) surface.Rotate(axis, angle);

    //     if (basis) basis.Rotate(axis, angle);
    // }

    public Vector3 GetSurfacePosition()
    {
        Vector3 position = ComputeHeight(0) * Vector3.up;

        if (surface) position = surface.localPosition;

        return position;
    }

    public Vector3 GetVelocity()
    {
        Vector3 velocity = Vector3.zero;

        float t = 2 * time / translationPeriod;
        float speed = 0;

        if (motionType == MotionType.Linear)
        {
            if (t < 1)
            {
                speed = 2 * amplitude / translationPeriod;
            }
            else
            {
                speed = -2 * amplitude / translationPeriod;
            }
        }
        else if (motionType == MotionType.Sinusoidal)
        {
            if (t < 1)
            {
                speed = Mathf.PI * Mathf.Sin(Mathf.PI * t) / translationPeriod;
            }
            else
            {
                speed = -Mathf.PI * Mathf.Sin(Mathf.PI * (t - 1)) / translationPeriod;
            }
        }

        return speed * Vector3.up;
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
    public Vector3 position;
    [Min(0)] public float restHeight = 1;
    [Tooltip("Vertical displacement")] public float amplitude = 1;
    [Tooltip("Time to execute one full cycle"), Min(0)] public float translationPeriod = 2;
    [Tooltip("Rotations per second"), Range(-0.5f, 0.5f)] public float rotationFrequency = 0;
    public MovingPlatform.MotionType motionType;
}

// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool autoUpdate;

    public enum TranslationType { Linear, Sinusoidal }
    public enum RotationType { Constant, Sinusoidal }
    public PlatformData data;

    [Header("Components")]
    [SerializeField] private Transform surface;
    [SerializeField] private Transform piston;
    [SerializeField] private Transform basisTriad;
    [SerializeField] private Vector3 basisOffset;
    [SerializeField] private Transform e3;
    [SerializeField] private Transform originLabel;
    [SerializeField] private Vector3 originLabelOffset;

    public Vector3 Omega => ComputeOmega();
    public Vector3 OmegaDot => ComputeOmegaDot();

    public Vector3 Position
    {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }

    [Header("Running clocks")]
    public float translationTime = 0;
    public float rotationTime = 0;

    // Rotation angle
    [HideInInspector] public float angle = 0;

    // For the Introduction
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
        // Update the platform's vertical position
        if (data.translationFrequency > 0)
        {
            translationTime += deltaTime;

            // Reset the translation clock after a vertical period
            if (translationTime > data.TranslationPeriod) translationTime = 0;

            // Get the current vertical position of the surface
            Vector3 position = ComputeHeight() * Vector3.up;

            // Set the surface at the correct height
            SetSurfacePosition(position);
        }

        // Update the platform's rotation angle
        if (data.rotationFrequency != 0)
        {
            // Rotate the surface to the correct angle
            angle += Mathf.Sign(Omega.y) * Omega.magnitude * deltaTime;
            angle = angle % (2 * Mathf.PI);
            SetSurfaceRotation(-angle * Mathf.Rad2Deg);

            if (data.rotationType == RotationType.Sinusoidal)
            {
                rotationTime += deltaTime;
            }
        }
    }

    private float ComputeHeight()
    {
        float heightOffset;
        float quarterPeriod = 0.25f * data.TranslationPeriod;
        float time = translationTime;

        switch (data.translationType)
        {
            case TranslationType.Linear:
                // Sawtooth function
                float slope = data.translationAmplitude / quarterPeriod;
                if (time < quarterPeriod)
                {
                    heightOffset = slope * time;
                }
                else if (time < 3 * quarterPeriod)
                {
                    heightOffset = 2 * data.translationAmplitude - slope * time;
                }
                else
                {
                    heightOffset = slope * (time - data.TranslationPeriod);
                }
                break;
            case TranslationType.Sinusoidal:
                // Sinusoidal function
                float omega = 2 * Mathf.PI * data.translationFrequency;
                heightOffset = data.translationAmplitude * Mathf.Sin(omega * time);
                break;
            default:
                heightOffset = GetSurfacePosition().y - data.restHeight;
                break;
        }

        return data.restHeight + heightOffset;
    }

    public Vector3 GetVelocity()
    {
        float speed;
        float quarterPeriod = 0.25f * data.TranslationPeriod;
        float time = translationTime;

        switch (data.translationType)
        {
            case TranslationType.Linear:
                // Sawtooth function
                float slope = data.translationAmplitude / quarterPeriod;
                if (time < quarterPeriod || time >= 3 * quarterPeriod)
                {
                    speed = slope;
                }
                else
                {
                    speed = -slope;
                }
                break;
            case TranslationType.Sinusoidal:
                // Sinusoidal function
                float omega = 2 * Mathf.PI * data.translationFrequency;
                speed = data.translationAmplitude * omega * Mathf.Cos(omega * time);
                break;
            default:
                speed = 0;
                break;
        }

        return speed * Vector3.up;
    }

    public Vector3 GetAcceleration()
    {
        float a = 0;
        float quarterPeriod = 0.25f * data.TranslationPeriod;
        float time = translationTime;

        if (data.translationType == TranslationType.Sinusoidal)
        {
            float omega = 2 * Mathf.PI * data.translationFrequency;
            a = -data.translationAmplitude * omega * omega * Mathf.Sin(omega * time);
        }

        return a * Vector3.up;
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
            float angularSpeed = 2 * Mathf.PI * data.rotationFrequencyVariable;
            frequency = data.rotationFrequency * Mathf.Cos(angularSpeed * rotationTime);
        }

        return 2 * Mathf.PI * frequency * Vector3.up;
    }

    private Vector3 ComputeOmegaDot()
    {
        float frequencyDot = 0;

        if (data.rotationType == RotationType.Sinusoidal)
        {
            float angularSpeed = 2 * Mathf.PI * data.rotationFrequencyVariable;
            frequencyDot = -angularSpeed * data.rotationFrequency * Mathf.Sin(angularSpeed * rotationTime);
        }

        return 2 * Mathf.PI * frequencyDot * Vector3.up;
    }

    public void SetRestHeight(float value)
    {
        if (!surface) return;

        Vector3 surfacePosition = surface.localPosition;
        surfacePosition.y = value;
        SetSurfacePosition(surfacePosition);
        data.restHeight = value;
    }

    private void SetSurfacePosition(Vector3 position)
    {
        // Set the surface at the input position
        if (surface) surface.localPosition = position;

        if (basisTriad) basisTriad.localPosition = position + basisOffset;

        if (e3) e3.localPosition = position;

        if (originLabel)
        {
            originLabel.position = (surface ? surface.position : Vector3.zero) + originLabelOffset;
        }

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
        return surface ? surface.localPosition : Vector3.zero;
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
    [Tooltip("Position of the base relative to its parent")]
    public Vector3 position;
    [Tooltip("Height of the surface above the base"), Min(0)]
    public float restHeight = 1;

    [Header("Translation")]
    public MovingPlatform.TranslationType translationType;
    [Tooltip("Vertical displacement")]
    public float translationAmplitude = 1;
    [Tooltip("Translation cycles per second"), Range(0, 2)]
    public float translationFrequency = 0.5f;
    public float TranslationPeriod => 1f / translationFrequency;

    [Header("Rotation")]
    public MovingPlatform.RotationType rotationType;
    [Tooltip("Rotations per second (sign indicates direction)"), Range(-0.5f, 0.5f)]
    public float rotationFrequency = 0;
    [Tooltip("Rotation cycles per second when variable"), Range(0, 2)]
    public float rotationFrequencyVariable = 0.5f;
}

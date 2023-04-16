using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform surface;
    [SerializeField] private Transform piston;
    [SerializeField] private Transform basis;
    [SerializeField] private Transform originLabel;

    [Min(0)] public float height = 1;

    public void SetHeight(float height)
    {
        if (!surface) return;

        Vector3 surfacePosition = surface.localPosition;
        surfacePosition.y = height;
        SetSurfacePosition(surfacePosition);
        this.height = height;
    }

    public void SetSurfacePosition(Vector3 position)
    {
        // Set the surface at the input position
        if (surface) surface.localPosition = position;

        if (basis) basis.localPosition = position;

        if (originLabel) originLabel.localPosition = position + new Vector3(-0.4f, 0.35f, 0);

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

    public void SetSurfaceRotation(float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        if (surface) surface.localRotation = rotation;

        if (basis) basis.localRotation = rotation;
    }

    public void RotateSurface(Vector3 axis, float angle)
    {
        if (surface) surface.Rotate(axis, angle);

        if (basis) basis.Rotate(axis, angle);
    }

    public Vector3 GetSurfacePosition()
    {
        Vector3 position = Vector3.zero;

        if (surface) position = surface.localPosition;

        return position;
    }
}

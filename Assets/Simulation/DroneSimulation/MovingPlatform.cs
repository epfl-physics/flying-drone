using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform surface;
    [SerializeField] private Transform piston;

    [Min(0)] public float height = 1;

    public void SetHeight(float height)
    {
        if (surface) surface.position = height * Vector3.up;
        if (piston)
        {
            piston.position = 0.5f * height * Vector3.up;
            Vector3 scale = piston.localScale;
            scale.y = 0.5f * height;
            piston.localScale = scale;
        }
    }

    public void RotateSurface(Vector3 axis, float angle)
    {
        if (surface) surface.Rotate(axis, angle);
    }
}

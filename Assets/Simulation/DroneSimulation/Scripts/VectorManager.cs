using UnityEngine;

public class VectorManager : MonoBehaviour
{
    public DroneSimulationState simState;

    [Header("Rotation")]
    public Vector omega;
    public bool showRotation;

    [Header("Position")]
    public Vector dronePositionAbsolute;
    public Vector dronePositionRelative;
    public Vector platformPosition;
    public bool showPositions;

    [Header("Velocity")]
    public Vector droneVelocityAbsolute;
    public Vector droneVelocityRelative;
    public Vector platformVelocity;
    public Vector phantomPlatformVelocity;
    public Vector tangentialVelocity;
    public bool showVelocities;
    public enum VelocityAdditionOrder { First, Second }
    public VelocityAdditionOrder velocityAdditionOrder = default;

    private void OnEnable()
    {
        DroneSimulationState.OnRedrawVectors += Redraw;
    }

    private void OnDisable()
    {
        DroneSimulationState.OnRedrawVectors -= Redraw;
    }

    public void Redraw()
    {
        if (!simState) return;

        Vector3 origin = simState.origin;
        Vector3 rAbsolute = simState.dronePositionAbsolute;
        Vector3 rPlatform = simState.platformPosition;

        Vector3 rRelative = rAbsolute - rPlatform;

        Vector3 vAbsolute = simState.droneVelocityAbsolute;
        Vector3 vPlatform = simState.platformVelocity;
        // Negative because Unity is left-handed
        Vector3 vTangential = -Vector3.Cross(simState.omega, rRelative);
        Vector3 vRelative = vAbsolute - vPlatform - vTangential;

        // Rotation
        RedrawVector(omega, origin + rPlatform, -simState.omega, showRotation);

        // Positions
        RedrawVector(dronePositionAbsolute, origin, rAbsolute, showPositions);
        RedrawVector(dronePositionRelative, origin + rPlatform, rRelative, showPositions);
        RedrawVector(this.platformPosition, origin, rPlatform, showPositions);

        // Velocities
        RedrawVector(droneVelocityAbsolute, origin + rAbsolute, vAbsolute, showVelocities);
        RedrawVector(platformVelocity, origin + rPlatform, vPlatform, showVelocities);

        if (velocityAdditionOrder == VelocityAdditionOrder.First)
        {
            RedrawVector(droneVelocityRelative, origin + rAbsolute, vRelative, showVelocities);
            RedrawVector(phantomPlatformVelocity, origin + rAbsolute + vRelative, vPlatform, showVelocities);
            RedrawVector(tangentialVelocity, origin + rAbsolute + vRelative + vPlatform, vTangential, showVelocities);
        }
        else if (velocityAdditionOrder == VelocityAdditionOrder.Second)
        {
            RedrawVector(droneVelocityRelative, origin + rAbsolute, vRelative, showVelocities);
            RedrawVector(tangentialVelocity, origin + rAbsolute + vRelative, vTangential, showVelocities);
            RedrawVector(phantomPlatformVelocity, origin + rAbsolute + vRelative + vTangential, vPlatform, showVelocities);
        }
    }

    public void RedrawVector(Vector vector, Vector3 position, Vector3 components, bool show)
    {
        if (vector)
        {
            vector.transform.position = position;
            vector.components = components;
            vector.Redraw();

            vector.gameObject.SetActive(show);
        }
    }
}

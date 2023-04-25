using UnityEngine;

public class VectorManager : MonoBehaviour
{
    public DroneSimulationState simState;
    public bool hideVectorsOnStart;

    [Header("Rotation")]
    public Vector omega;
    public bool showRotation;

    [Header("Position")]
    public Vector dronePositionAbsolute;
    public Vector dronePositionRelative;
    public Vector platformPosition;
    public bool showPositions;
    // Used by DroneSlideController to turn on and off specific vectors
    [HideInInspector] public bool showDronePositionAbsolute = true;
    [HideInInspector] public bool showDronePositionRelative = true;
    [HideInInspector] public bool showPlatformPosition = true;

    [Header("Velocity")]
    public Vector droneVelocityAbsolute;
    public Vector droneVelocityRelative;
    public Vector platformVelocity;
    public Vector platformVelocityAdditive;
    public Vector tangentialVelocity;
    public bool showVelocities;
    public enum VelocityAdditionOrder { First, Second }
    public VelocityAdditionOrder velocityAdditionOrder = default;

    [Header("Acceleration")]
    public bool showAccelerations;

    private void OnEnable()
    {
        DroneSimulationState.OnRedrawVectors += Redraw;
    }

    private void OnDisable()
    {
        DroneSimulationState.OnRedrawVectors -= Redraw;
    }

    private void Start()
    {
        if (hideVectorsOnStart) SetVectorVisibility(0);
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
        Vector3 omegaPosition = origin + rPlatform - rPlatform.y * Vector3.up - 0.5f * Vector3.right;
        RedrawVector(omega, omegaPosition, simState.omega, showRotation);

        // Positions
        RedrawVector(dronePositionAbsolute, origin, rAbsolute, showPositions && showDronePositionAbsolute);
        RedrawVector(dronePositionRelative, origin + rPlatform, rRelative, showPositions && showDronePositionRelative);
        RedrawVector(this.platformPosition, origin, rPlatform, showPositions && showPlatformPosition);

        // Velocities
        RedrawVector(droneVelocityAbsolute, origin + rAbsolute, vAbsolute, showVelocities);
        RedrawVector(platformVelocity, origin + rPlatform + 1.5f * Vector3.right, vPlatform, showVelocities);

        if (velocityAdditionOrder == VelocityAdditionOrder.First)
        {
            RedrawVector(droneVelocityRelative, origin + rAbsolute, vRelative, showVelocities);
            RedrawVector(platformVelocityAdditive, origin + rAbsolute + vRelative, vPlatform, showVelocities);
            RedrawVector(tangentialVelocity, origin + rAbsolute + vRelative + vPlatform, vTangential, showVelocities);
        }
        else if (velocityAdditionOrder == VelocityAdditionOrder.Second)
        {
            RedrawVector(droneVelocityRelative, origin + rAbsolute, vRelative, showVelocities);
            RedrawVector(tangentialVelocity, origin + rAbsolute + vRelative, vTangential, showVelocities);
            RedrawVector(platformVelocityAdditive, origin + rAbsolute + vRelative + vTangential, vPlatform, showVelocities);
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

    public void SetVectorVisibility(int index)
    {
        switch (index)
        {
            case 1:
                showPositions = true;
                showVelocities = false;
                showAccelerations = false;
                break;
            case 2:
                showPositions = false;
                showVelocities = true;
                showAccelerations = false;
                break;
            case 3:
                showPositions = false;
                showVelocities = false;
                showAccelerations = true;
                break;
            default:
                showPositions = false;
                showVelocities = false;
                showAccelerations = false;
                break;
        }

        Redraw();
    }
}

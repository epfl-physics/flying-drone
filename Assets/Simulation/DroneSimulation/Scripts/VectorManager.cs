using UnityEngine;

public class VectorManager : MonoBehaviour
{
    public DroneSimulationState simState;
    public bool hideVectorsOnStart;

    [Header("Basis Vectors")]
    public Vector y3;

    [Header("Rotation")]
    public Vector omega;
    public GameObject omegaLabel;
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
    public Vector droneAccelerationAbsolute;
    public Vector droneAccelerationRelative;
    public Vector platformAcceleration;
    public Vector platformAccelerationAdditive;
    public Vector centrifugalAcceleration;
    public Vector coriolisAcceleration;
    public Vector eulerAcceleration;
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

        Vector3 aAbsolute = simState.droneAccelerationAbsolute;
        Vector3 aPlatform = simState.platformAcceleration;
        Vector3 aCentrifugal = -Vector3.Cross(simState.omega, vTangential);
        Vector3 aCoriolis = -2 * Vector3.Cross(simState.omega, vRelative);
        Vector3 aEuler = Vector3.zero;
        Vector3 aRelative = aAbsolute - aPlatform - aCentrifugal - aCoriolis - aEuler;

        // Rotation
        // Vector3 omegaPosition = origin + rPlatform - rPlatform.y * Vector3.up - 0.5f * Vector3.right;
        Vector3 omegaPosition = origin + rPlatform;
        RedrawVector(omega, omegaPosition, simState.omega * 1.2f, showRotation);
        if (omegaLabel) omegaLabel.SetActive(simState.omega.y != 0);
        if (y3) y3.gameObject.SetActive(simState.omega.y <= 0);

        // Positions
        RedrawVector(dronePositionAbsolute, origin, rAbsolute, showPositions && showDronePositionAbsolute);
        RedrawVector(dronePositionRelative, origin + rPlatform, rRelative, showPositions && showDronePositionRelative);
        RedrawVector(this.platformPosition, origin, rPlatform, showPositions && showPlatformPosition);

        // Velocities
        RedrawVector(droneVelocityAbsolute, origin + rAbsolute, vAbsolute, showVelocities);
        // RedrawVector(platformVelocity, origin + rPlatform + 1.5f * Vector3.right, vPlatform, showVelocities);
        RedrawVector(platformVelocity, origin + rPlatform, vPlatform, showVelocities);

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

        // Accelerations
        RedrawVector(droneAccelerationAbsolute, origin + rAbsolute, aAbsolute, showAccelerations);
        RedrawVector(platformAcceleration, origin + rPlatform + 1.5f * Vector3.right, aPlatform, showAccelerations);

        RedrawVector(droneAccelerationRelative, origin + rAbsolute, aRelative, showAccelerations);
        RedrawVector(platformAccelerationAdditive, origin + rAbsolute + aRelative, aPlatform, showAccelerations);
        RedrawVector(centrifugalAcceleration, origin + rAbsolute + aRelative + aPlatform, aCentrifugal, showAccelerations);
        RedrawVector(coriolisAcceleration, origin + rAbsolute + aRelative + aPlatform + aCentrifugal, aCoriolis, showAccelerations);
        RedrawVector(eulerAcceleration, origin + rAbsolute + aRelative + aPlatform + aCentrifugal + aCoriolis, aEuler, showAccelerations);
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

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

        // Positions
        Vector3 origin = simState.origin;
        Vector3 rAbsolute = simState.dronePositionAbsolute;
        Vector3 rRelative = simState.dronePositionRelative;
        Vector3 rPlatform = simState.platformPosition;

        // Velocities
        Vector3 vAbsolute = simState.droneVelocityAbsolute;
        Vector3 vRelative = simState.droneVelocityRelative;
        Vector3 vPlatform = simState.platformVelocity;
        Vector3 vTangential = simState.tangentialVelocity;

        // Accelerations
        Vector3 aAbsolute = simState.droneAccelerationAbsolute;
        Vector3 aRelative = simState.droneAccelerationRelative;
        Vector3 aPlatform = simState.platformAcceleration;
        Vector3 aCentrifugal = simState.centrifugalAcceleration;
        Vector3 aCoriolis = simState.coriolisAcceleration;
        Vector3 aEuler = simState.eulerAcceleration;

        // Rotation
        // Vector3 omegaPosition = origin + rPlatform - rPlatform.y * Vector3.up - 0.5f * Vector3.right;
        // Vector3 omegaPosition = origin + rPlatform;
        Vector3 omegaPosition = origin + 6.5f * Vector3.right + 1.5f * Vector3.up;
        RedrawVector(omega, omegaPosition, simState.omega, showRotation);
        if (omegaLabel) omegaLabel.SetActive(simState.omega.y != 0);

        if (y3)
        {
            bool basisVisible = showPositions || showVelocities || showAccelerations;
            y3.gameObject.SetActive(basisVisible && (aPlatform.magnitude == 0 || Vector3.Cross(rRelative, Vector3.up).magnitude != 0));
        }

        // Positions
        RedrawVector(dronePositionAbsolute, origin, rAbsolute, showPositions && showDronePositionAbsolute);
        RedrawVector(dronePositionRelative, origin + rPlatform, rRelative, showPositions && showDronePositionRelative);
        RedrawVector(this.platformPosition, origin, rPlatform, showPositions && showPlatformPosition);

        // Velocities
        RedrawVector(droneVelocityAbsolute, origin + rAbsolute, vAbsolute, showVelocities);
        // RedrawVector(platformVelocity, origin + rPlatform + 1.5f * Vector3.right, vPlatform, showVelocities);
        RedrawVector(platformVelocity, origin + rPlatform, vPlatform, showVelocities);

        // if (velocityAdditionOrder == VelocityAdditionOrder.First)
        // {
        //     RedrawVector(droneVelocityRelative, origin + rAbsolute, vRelative, showVelocities);
        //     RedrawVector(platformVelocityAdditive, origin + rAbsolute + vRelative, vPlatform, showVelocities);
        //     RedrawVector(tangentialVelocity, origin + rAbsolute + vRelative + vPlatform, vTangential, showVelocities);
        // }
        // else if (velocityAdditionOrder == VelocityAdditionOrder.Second)
        // {
        //     RedrawVector(droneVelocityRelative, origin + rAbsolute, vRelative, showVelocities);
        //     RedrawVector(tangentialVelocity, origin + rAbsolute + vRelative, vTangential, showVelocities);
        //     RedrawVector(platformVelocityAdditive, origin + rAbsolute + vRelative + vTangential, vPlatform, showVelocities);
        // }
        RedrawVector(droneVelocityRelative, origin + rAbsolute, vRelative, showVelocities);
        RedrawVector(platformVelocityAdditive, origin + rAbsolute + vRelative, vPlatform, showVelocities);
        RedrawVector(tangentialVelocity, origin + rAbsolute, vTangential, showVelocities);

        // Accelerations
        RedrawVector(droneAccelerationAbsolute, origin + rAbsolute, aAbsolute, showAccelerations);
        // RedrawVector(platformAcceleration, origin + rPlatform + 1.5f * Vector3.right, aPlatform, showAccelerations);
        RedrawVector(platformAcceleration, origin + rPlatform, aPlatform, showAccelerations);

        RedrawVector(droneAccelerationRelative, origin + rAbsolute, aRelative, showAccelerations);
        // RedrawVector(platformAccelerationAdditive, origin + rAbsolute + aRelative, aPlatform, showAccelerations);
        // RedrawVector(centrifugalAcceleration, origin + rAbsolute + aRelative + aPlatform, aCentrifugal, showAccelerations);
        // RedrawVector(coriolisAcceleration, origin + rAbsolute + aRelative + aPlatform + aCentrifugal, aCoriolis, showAccelerations);
        // RedrawVector(eulerAcceleration, origin + rAbsolute + aRelative + aPlatform + aCentrifugal + aCoriolis, aEuler, showAccelerations);
        RedrawVector(platformAccelerationAdditive, origin + rAbsolute + aRelative, aPlatform, showAccelerations);
        RedrawVector(centrifugalAcceleration, origin + rAbsolute, aCentrifugal, showAccelerations);
        RedrawVector(coriolisAcceleration, origin + rAbsolute, aCoriolis, showAccelerations);
        RedrawVector(eulerAcceleration, origin + rAbsolute, aEuler, showAccelerations);
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
                showRotation = true;
                break;
            case 2:
                showPositions = false;
                showVelocities = true;
                showAccelerations = false;
                showRotation = true;
                break;
            case 3:
                showPositions = false;
                showVelocities = false;
                showAccelerations = true;
                showRotation = true;
                break;
            default:
                showPositions = false;
                showVelocities = false;
                showAccelerations = false;
                showRotation = false;
                break;
        }

        Redraw();
    }
}

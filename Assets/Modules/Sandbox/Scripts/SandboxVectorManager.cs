// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

public class SandboxVectorManager : MonoBehaviour
{
    public DroneSimulationState simState;
    public bool hideVectorsOnStart;

    [Header("Basis Vectors")]
    public Vector y3;

    [Header("Platform Piston")]
    public MaterialSelector pistonMaterial;

    [Header("Rotation")]
    public Vector omega;
    public GameObject omegaLabel;
    public float omegaScaleFactor = 1;
    // public bool showRotation;

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

    [Header("Acceleration")]
    public Vector droneAccelerationAbsolute;
    public Vector droneAccelerationRelative;
    public Vector platformAcceleration;
    public Vector platformAccelerationAdditive;
    public Vector centripetalAcceleration;
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

        // Accelerations / forces
        float sign = simState.frameIsInertial ? 1 : -1;
        Vector3 aAbsolute = simState.droneAccelerationAbsolute;
        Vector3 aRelative = simState.droneAccelerationRelative;
        Vector3 aPlatform = sign * simState.platformAcceleration;
        Vector3 aCentripetal = sign * simState.centripetalAcceleration;
        Vector3 aCoriolis = sign * simState.coriolisAcceleration;
        Vector3 aEuler = sign * simState.eulerAcceleration;

        // Rotation
        // Vector3 omegaPosition = origin + rPlatform;
        Vector3 omegaPosition = origin + 6.5f * Vector3.right + 1.5f * Vector3.up;
        RedrawVector(omega, omegaPosition, omegaScaleFactor * simState.omega, true);
        if (omegaLabel) omegaLabel.SetActive(simState.omega.y != 0);

        if (y3)
        {
            bool showY3 = false;
            if (showPositions)
            {
                showY3 = !simState.droneIsOnAxis;
            }
            else if (showVelocities)
            {
                showY3 = simState.translationIsZero;
            }
            else if (showAccelerations)
            {
                showY3 = !simState.translationIsVariable;
            }

            y3.gameObject.SetActive(showY3);
        }

        if (pistonMaterial)
        {
            pistonMaterial.SetMaterial(0);
            if (!simState.translationIsZero && (showVelocities || showAccelerations))
            {
                pistonMaterial.SetMaterial(1);
            }
        }

        // Positions
        RedrawVector(dronePositionAbsolute, origin, rAbsolute, showPositions && showDronePositionAbsolute);
        RedrawVector(dronePositionRelative, origin + rPlatform, rRelative, showPositions && showDronePositionRelative);
        RedrawVector(platformPosition, origin, rPlatform, showPositions && showPlatformPosition);

        // Velocities
        RedrawVector(droneVelocityAbsolute, origin + rAbsolute, vAbsolute, showVelocities);
        RedrawVector(platformVelocity, origin + rPlatform, vPlatform, showVelocities);
        Vector3 vRelativeOffset = vRelative.normalized == vAbsolute.normalized ? 0.2f * Vector3.down : Vector3.zero;
        RedrawVector(droneVelocityRelative, origin + rAbsolute + vRelativeOffset, vRelative, showVelocities);
        bool showPlatformV2 = showVelocities && (vAbsolute != vPlatform);
        RedrawVector(platformVelocityAdditive, origin + rAbsolute, vPlatform, showPlatformV2);
        // bool showTangentialV = showVelocities && (vAbsolute != vTangential);
        Vector3 vTangentialOffset = vTangential.normalized == vAbsolute.normalized ? 0.2f * Vector3.down : Vector3.zero;
        RedrawVector(tangentialVelocity, origin + rAbsolute + vTangentialOffset, vTangential, showVelocities);

        // Accelerations
        RedrawVector(droneAccelerationAbsolute, origin + rAbsolute, aAbsolute, showAccelerations);
        RedrawVector(platformAcceleration, origin + rPlatform, aPlatform, showAccelerations);
        RedrawVector(droneAccelerationRelative, origin + rAbsolute, aRelative, showAccelerations);
        bool showPlatformA2 = showAccelerations && (aAbsolute != aPlatform);
        RedrawVector(platformAccelerationAdditive, origin + rAbsolute, aPlatform, showPlatformA2);
        // bool showCentripetalA = showAccelerations && (aAbsolute != aCentripetal);
        Vector3 centripetalOffset = aRelative == aCentripetal || aAbsolute == aCentripetal ? 0.2f * Vector3.down : Vector3.zero;
        RedrawVector(centripetalAcceleration, origin + rAbsolute + centripetalOffset, aCentripetal, showAccelerations);
        Vector3 coriolisOffset = simState.platformAcceleration.y == 0 && (aRelative.normalized == aCoriolis.normalized) ? 0.2f * Vector3.down : Vector3.zero;
        RedrawVector(coriolisAcceleration, origin + rAbsolute + coriolisOffset, aCoriolis, showAccelerations);
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

// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

public class TheoryEquationTerm : MonoBehaviour
{
    public DroneSimulationState simState;

    public enum EquationTerm
    {
        None,
        AbsoluteVelocity,
        RelativeVelocity,
        PlatformVelocity,
        TangentialVelocity,
        AbsoluteAcceleration,
        RelativeAcceleration,
        PlatformAcceleration,
        CentrifugalAcceleration,
        CoriolisAcceleration,
        EulerAcceleration,
        ExternalForce,
        PlatformForce,
        CentrifugalForce,
        CoriolisForce,
        EulerForce,
        TotalRelativeForce
    }
    public EquationTerm term;
    public GameObject[] displayWhenZero;

    private void OnEnable()
    {
        TheoryController.OnChangeSimulationParameters += HandleSimulationParametersChanged;
    }

    private void OnDisable()
    {
        TheoryController.OnChangeSimulationParameters -= HandleSimulationParametersChanged;
    }

    public void HandleSimulationParametersChanged()
    {
        if (!simState) return;

        bool isZero = false;

        switch (term)
        {
            case EquationTerm.AbsoluteVelocity:
                if (simState.droneIsAtRestInR)
                {
                    isZero = true;
                }
                else if (simState.droneIsAtRestInRPrime)
                {
                    if (simState.droneIsOnAxis)
                    {
                        isZero = simState.translationIsZero;
                    }
                    else
                    {
                        isZero = simState.translationIsZero && simState.rotationIsZero;
                    }
                }
                break;
            case EquationTerm.RelativeVelocity:
                if (simState.droneIsAtRestInRPrime)
                {
                    isZero = true;
                }
                else if (simState.droneIsAtRestInR)
                {
                    if (simState.droneIsOnAxis)
                    {
                        isZero = simState.translationIsZero;
                    }
                    else
                    {
                        isZero = simState.translationIsZero && simState.rotationIsZero;
                    }
                }
                break;
            case EquationTerm.PlatformVelocity:
                isZero = simState.translationIsZero;
                break;
            case EquationTerm.TangentialVelocity:
                if (simState.droneIsOnAxis)
                {
                    isZero = true;
                }
                else if (simState.rotationIsZero)
                {
                    isZero = true;
                }
                break;
            case EquationTerm.AbsoluteAcceleration:
                // isZero = simState.droneIsAtRestInR;
                if (simState.droneIsAtRestInR)
                {
                    isZero = true;
                }
                else if (simState.droneIsAtRestInRPrime)
                {
                    if (simState.droneIsOnAxis)
                    {
                        isZero = !simState.translationIsVariable;
                    }
                    else
                    {
                        isZero = !simState.translationIsVariable && simState.rotationIsZero;
                    }
                }
                break;
            case EquationTerm.RelativeAcceleration:
                if (simState.droneIsAtRestInRPrime)
                {
                    isZero = true;
                }
                else if (simState.droneIsAtRestInR)
                {
                    if (simState.droneIsOnAxis)
                    {
                        isZero = !simState.translationIsVariable;
                    }
                    else
                    {
                        isZero = !simState.translationIsVariable && simState.rotationIsZero;
                    }
                }
                break;
            case EquationTerm.PlatformAcceleration:
                isZero = !simState.translationIsVariable;
                break;
            case EquationTerm.CentrifugalAcceleration:
                if (simState.droneIsOnAxis)
                {
                    isZero = true;
                }
                else if (simState.rotationIsZero)
                {
                    isZero = true;
                }
                break;
            case EquationTerm.CoriolisAcceleration:
                if (simState.droneIsOnAxis)
                {
                    isZero = true;
                }
                else if (simState.rotationIsZero || simState.droneIsAtRestInRPrime)
                {
                    isZero = true;
                }
                break;
            case EquationTerm.EulerAcceleration:
                if (simState.droneIsOnAxis)
                {
                    isZero = true;
                }
                else if (simState.rotationIsZero || simState.rotationIsConstant)
                {
                    isZero = true;
                }
                break;
            case EquationTerm.ExternalForce:
                if (simState.droneIsAtRestInR)
                {
                    isZero = true;
                }
                else if (simState.droneIsAtRestInRPrime)
                {
                    if (simState.droneIsOnAxis)
                    {
                        isZero = !simState.translationIsVariable;
                    }
                    else
                    {
                        isZero = !simState.translationIsVariable && simState.rotationIsZero;
                    }
                }
                break;
            case EquationTerm.PlatformForce:
                isZero = !simState.translationIsVariable;
                break;
            case EquationTerm.CentrifugalForce:
                if (simState.droneIsOnAxis)
                {
                    isZero = true;
                }
                else if (simState.rotationIsZero)
                {
                    isZero = true;
                }
                break;
            case EquationTerm.CoriolisForce:
                if (simState.droneIsOnAxis)
                {
                    isZero = true;
                }
                else if (simState.rotationIsZero || simState.droneIsAtRestInRPrime)
                {
                    isZero = true;
                }
                break;
            case EquationTerm.EulerForce:
                if (simState.droneIsOnAxis)
                {
                    isZero = true;
                }
                else if (simState.rotationIsZero || simState.rotationIsConstant)
                {
                    isZero = true;
                }
                break;
            case EquationTerm.TotalRelativeForce:
                if (simState.droneIsAtRestInRPrime)
                {
                    isZero = true;
                }
                else if (simState.droneIsAtRestInR)
                {
                    if (simState.droneIsOnAxis)
                    {
                        isZero = !simState.translationIsVariable;
                    }
                    else
                    {
                        isZero = !simState.translationIsVariable && simState.rotationIsZero;
                    }
                }
                break;
            default:
                break;
        }

        foreach (var gameObject in displayWhenZero)
        {
            gameObject.SetActive(isZero);
        }
    }
}

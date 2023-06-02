using UnityEngine;
using UnityEngine.UI;

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
        EulerAcceleration
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
                isZero = simState.droneIsAtRestInR;
                break;
            case EquationTerm.RelativeVelocity:
                isZero = simState.droneIsAtRestInRPrime;
                break;
            case EquationTerm.PlatformVelocity:
                isZero = false;
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
                isZero = simState.droneIsAtRestInR;
                break;
            case EquationTerm.RelativeAcceleration:
                isZero = simState.droneIsAtRestInRPrime;
                break;
            case EquationTerm.PlatformAcceleration:
                isZero = false;
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
            default:
                break;
        }

        // if (TryGetComponent(out Image image))
        // {
        //     Color color = image.color;
        //     color.a = isZero ? 0.2f : 1f;
        //     image.color = color;
        // }

        foreach (var gameObject in displayWhenZero)
        {
            gameObject.SetActive(isZero);
        }
    }
}

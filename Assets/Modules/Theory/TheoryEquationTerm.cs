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
            // case EquationTerm.AbsoluteAcceleration:
            //     isZero = simState.droneAccelerationAbsolute.magnitude < threshold;
            //     break;
            // case EquationTerm.RelativeAcceleration:
            //     isZero = simState.droneAccelerationRelative.magnitude < threshold;
            //     break;
            // case EquationTerm.PlatformAcceleration:
            //     isZero = simState.platformAcceleration.magnitude < threshold;
            //     break;
            // case EquationTerm.CentrifugalAcceleration:
            //     isZero = simState.centrifugalAcceleration.magnitude < threshold;
            //     break;
            // case EquationTerm.CoriolisAcceleration:
            //     isZero = simState.coriolisAcceleration.magnitude < threshold;
            //     break;
            // case EquationTerm.EulerAcceleration:
            //     isZero = simState.eulerAcceleration.magnitude < threshold;
            //     break;
            default:
                break;
        }

        if (TryGetComponent(out Image image))
        {
            Color color = image.color;
            color.a = isZero ? 0.2f : 1f;
            image.color = color;
        }
    }
}

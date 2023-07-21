using UnityEngine;

public class ActivitiesVectors : MonoBehaviour
{
    [SerializeField] private VelocityVector velocityVector;
    [SerializeField] private DroneSimulationState simState;

    // private delegate Vector3 GetVelocityComponents();
    // private GetVelocityComponents getVelocityComponents;

    private void OnEnable()
    {
        DroneSimulationState.OnRedrawVectors += HandleRedrawVectors;
    }

    private void OnDisable()
    {
        DroneSimulationState.OnRedrawVectors -= HandleRedrawVectors;
    }

    // private void Start()
    // {
    //     ResetColor();
    // }

    // public void HandleLoadScenario(int answerIndex)
    // {
    //     switch (answerIndex)
    //     {
    //         case 0:
    //             getVelocityComponents = GetAbsoluteComponents;
    //             break;
    //         case 1:
    //             getVelocityComponents = GetPlatformComponents;
    //             break;
    //         case 2:
    //             getVelocityComponents = GetRelativeComponents;
    //             break;
    //         case 3:
    //             getVelocityComponents = GetTangentialComponents;
    //             break;
    //         default:
    //             break;
    //     }
    // }

    public void HandleRedrawVectors()
    {
        if (!velocityVector || !simState) return;

        velocityVector.transform.position = simState.origin + simState.dronePositionAbsolute;
        velocityVector.components = GetAbsoluteComponents();
        velocityVector.Redraw();
    }

    // public void SetVectorColor(Color color)
    // {
    //     if (velocityVector)
    //     {
    //         velocityVector.color = color;
    //         velocityVector.SetColor();
    //     }
    // }

    // public void ResetColor()
    // {
    //     SetVectorColor(Color.black);
    // }

    private Vector3 GetAbsoluteComponents()
    {
        Vector3 components = Vector3.zero;
        if (simState) components = simState.droneVelocityAbsolute;
        return components;
    }

    // private Vector3 GetPlatformComponents()
    // {
    //     Vector3 components = Vector3.zero;
    //     if (simState) components = simState.platformVelocity;
    //     return components;
    // }

    // private Vector3 GetRelativeComponents()
    // {
    //     Vector3 components = Vector3.zero;
    //     if (simState) components = simState.droneVelocityRelative;
    //     return components;
    // }

    // private Vector3 GetTangentialComponents()
    // {
    //     Vector3 components = Vector3.zero;
    //     if (simState) components = simState.tangentialVelocity;
    //     return components;
    // }
}

using UnityEngine;

public class ActivitiesVectors : MonoBehaviour
{
    [SerializeField] private ActivityCustomVector velocityVector;
    [SerializeField] private DroneSimulationState simState;
    [SerializeField] private float scaleFactor = 1;

    private void OnEnable()
    {
        DroneSimulationState.OnRedrawVectors += HandleRedrawVectors;
    }

    private void OnDisable()
    {
        DroneSimulationState.OnRedrawVectors -= HandleRedrawVectors;
    }

    public void HandleRedrawVectors()
    {
        if (!velocityVector || !simState) return;

        velocityVector.transform.position = simState.origin + simState.dronePositionAbsolute;
        velocityVector.components = scaleFactor * GetAbsoluteComponents();
        velocityVector.Redraw();
    }

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

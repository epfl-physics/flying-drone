using UnityEditor;

[CustomEditor(typeof(DroneSimulationState))]
public class DroneSimulationStateEditor : Editor
{
    private DroneSimulationState simState;
    private DroneSimulationState.DisplayMode displayMode;

    public override void OnInspectorGUI()
    {
        simState = (DroneSimulationState)target;

        DrawDefaultInspector();

        // if (displayMode != simState.displayMode)
        // {
        //     simState.SetDisplayMode(simState.displayMode);
        //     displayMode = simState.displayMode;
        // }
    }
}

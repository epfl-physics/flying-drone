using UnityEditor;

[CustomEditor(typeof(DroneSimulationState))]
public class DroneSimulationStateEditor : Editor
{
    private DroneSimulationState simState;

    public override void OnInspectorGUI()
    {
        simState = (DroneSimulationState)target;

        DrawDefaultInspector();
    }
}

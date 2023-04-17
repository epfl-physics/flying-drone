using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DisplayController))]
public class DisplayControllerEditor : Editor
{
    // private DisplayController.CurrentMaterial currentMaterial;

    public override void OnInspectorGUI()
    {
        DisplayController controller = (DisplayController)target;

        DrawDefaultInspector();

        // if (currentMaterial != controller.currentMaterial)
        // {
        //     DroneSimulationState.DisplayMode displayMode;
        //     displayMode = controller.currentMaterial == DisplayController.CurrentMaterial.Opaque ? DroneSimulationState.DisplayMode.Physical : DroneSimulationState.DisplayMode.Mathematical;
        //     controller.HandleDisplayModeChanged(displayMode);
        //     currentMaterial = controller.currentMaterial;
        // }
    }
}

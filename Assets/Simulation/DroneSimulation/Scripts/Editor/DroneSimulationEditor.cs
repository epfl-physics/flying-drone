using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DroneSimulation))]
public class DroneSimulationEditor : Editor
{
    private DroneSimulation sim;

    private void OnEnable()
    {
        sim = (DroneSimulation)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUI.changed)
        {
            sim.ApplyPlatformData();
            sim.ApplyDroneData();
        }
    }
}

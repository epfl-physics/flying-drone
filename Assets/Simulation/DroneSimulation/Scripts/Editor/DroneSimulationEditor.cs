using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DroneSimulation))]
public class DroneSimulationEditor : Editor
{
    private DroneSimulation sim;

    private Vector3 platformOffset;
    private float platformRestHeight;
    private bool droneIsIndependent;
    private float droneRestHeight;
    private DroneSimulationState.Display display;

    private void OnEnable()
    {
        sim = (DroneSimulation)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (platformOffset != sim.platformOffset)
        {
            sim.SetPositionOffset(sim.platformOffset);
            platformOffset = sim.platformOffset;
        }

        if (platformRestHeight != sim.platformRestHeight)
        {
            sim.SetPlatformAtRestPosition();
            platformRestHeight = sim.platformRestHeight;
        }

        if (droneIsIndependent != sim.droneIsIndependent)
        {
            sim.SetDroneAtRestPosition();
            droneIsIndependent = sim.droneIsIndependent;
        }

        if (droneRestHeight != sim.droneRestHeight)
        {
            sim.SetDroneAtRestPosition();
            droneRestHeight = sim.droneRestHeight;
        }

        if (display != sim.display)
        {
            sim.SetDisplay();
            display = sim.display;
        }
    }
}

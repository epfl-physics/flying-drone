using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Drone))]
public class DroneEditor : Editor
{
    private Drone drone;

    private void OnEnable()
    {
        drone = (Drone)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUI.changed)
        {
            drone.ReturnToRestPosition();
            drone.ComputeHeightBounds();
        }
    }
}

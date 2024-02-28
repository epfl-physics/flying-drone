// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
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

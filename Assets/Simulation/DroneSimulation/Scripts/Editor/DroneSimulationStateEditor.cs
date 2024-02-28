// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
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

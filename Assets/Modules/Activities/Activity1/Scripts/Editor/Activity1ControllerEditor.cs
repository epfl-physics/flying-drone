// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Activity1Controller))]
public class Activity1ControllerEditor : Editor
{
    private Activity1Controller controller;

    private void OnEnable()
    {
        controller = (Activity1Controller)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Load Random Question"))
        {
            controller.LoadRandomQuestion();
        }
    }
}

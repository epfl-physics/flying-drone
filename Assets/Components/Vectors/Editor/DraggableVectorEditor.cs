// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DraggableVector))]
public class DraggableVectorEditor : Editor
{
    private DraggableVector vector;

    private bool interactable;
    private Vector3 components;
    private Color color;
    private float lineWidth;

    private void OnEnable()
    {
        vector = target as DraggableVector;
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector fields
        DrawDefaultInspector();

        // Check if properties have been changed in the inspector
        if (interactable != vector.interactable)
        {
            vector.SetInteractable(vector.interactable);
            interactable = vector.interactable;
        }

        if (components != vector.components)
        {
            vector.Redraw();
            components = vector.components;
        }

        if (lineWidth != vector.lineWidth)
        {
            vector.Redraw();
            lineWidth = vector.lineWidth;
        }

        if (color != vector.color)
        {
            vector.SetColor();
            vector.SetClickZoneColors();
            color = vector.color;
        }
    }
}

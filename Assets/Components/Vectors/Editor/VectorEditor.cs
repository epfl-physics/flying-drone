using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vector))]
public class VectorEditor : Editor
{
    private Vector vector;

    private Vector3 components;
    private Color color;
    private float lineWidth;

    private void OnEnable()
    {
        vector = target as Vector;
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector fields
        DrawDefaultInspector();

        // Check if the any fields have been changed
        // if (GUI.changed)
        // {
        //     vector.Redraw();
        //     vector.SetColor();
        // }

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
            color = vector.color;
        }
    }
}

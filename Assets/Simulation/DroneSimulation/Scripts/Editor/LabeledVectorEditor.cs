using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LabeledVector))]
public class LabeledVectorEditor : Editor
{
    private LabeledVector vector;

    private Vector3 components;
    private Color color;
    private float lineWidth;

    private void OnEnable()
    {
        vector = target as LabeledVector;
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector fields
        DrawDefaultInspector();

        // Check if properties have been changed in the inspector
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

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Arrow))]
public class ArrowEditor : Editor
{
    private Arrow arrow;

    private void OnEnable()
    {
        arrow = target as Arrow;
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector fields
        DrawDefaultInspector();

        // Check if the any fields have been changed
        if (GUI.changed)
        {
            arrow.Redraw();
        }
    }
}

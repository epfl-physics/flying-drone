using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConeMesh))]
public class ConeMeshEditor : Editor
{
    private ConeMesh coneMesh;

    private void OnEnable()
    {
        coneMesh = target as ConeMesh;
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector fields
        DrawDefaultInspector();

        // Check if the any fields have been changed
        if (GUI.changed)
        {
            coneMesh.Redraw();
        }
    }
}

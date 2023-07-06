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

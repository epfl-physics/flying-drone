using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    private MovingPlatform platform;

    private void OnEnable()
    {
        platform = (MovingPlatform)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUI.changed)
        {
            platform.SetRestHeight(platform.restHeight);
        }
    }
}

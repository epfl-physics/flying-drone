using UnityEditor;

[CustomEditor(typeof(CameraOrbit))]
public class CameraOrbitEditor : Editor
{
    SerializedProperty orbitTarget;
    SerializedProperty canOrbit;
    SerializedProperty canZoom;
    SerializedProperty orbitSpeed;
    SerializedProperty minPolarAngle;
    SerializedProperty maxPolarAngle;
    SerializedProperty clampAzimuthalAngle;
    SerializedProperty minAzimuthalAngle;
    SerializedProperty maxAzimuthalAngle;
    SerializedProperty zoomSpeed;
    SerializedProperty minDistance;
    SerializedProperty maxDistance;


    private void OnEnable()
    {
        orbitTarget = serializedObject.FindProperty("target");
        canOrbit = serializedObject.FindProperty("canOrbit");
        canZoom = serializedObject.FindProperty("canZoom");
        orbitSpeed = serializedObject.FindProperty("orbitSpeed");
        minPolarAngle = serializedObject.FindProperty("minPolarAngle");
        maxPolarAngle = serializedObject.FindProperty("maxPolarAngle");
        clampAzimuthalAngle = serializedObject.FindProperty("clampAzimuthalAngle");
        minAzimuthalAngle = serializedObject.FindProperty("minAzimuthalAngle");
        maxAzimuthalAngle = serializedObject.FindProperty("maxAzimuthalAngle");
        zoomSpeed = serializedObject.FindProperty("zoomSpeed");
        minDistance = serializedObject.FindProperty("minDistance");
        maxDistance = serializedObject.FindProperty("maxDistance");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(orbitTarget);

        if (orbitTarget.objectReferenceValue != null)
        {
            EditorGUILayout.PropertyField(canOrbit);
            if (canOrbit.boolValue)
            {
                EditorGUILayout.PropertyField(orbitSpeed);
                EditorGUILayout.PropertyField(minPolarAngle);
                EditorGUILayout.PropertyField(maxPolarAngle);
                EditorGUILayout.PropertyField(clampAzimuthalAngle);

                if (clampAzimuthalAngle.boolValue)
                {
                    EditorGUILayout.PropertyField(minAzimuthalAngle);
                    EditorGUILayout.PropertyField(maxAzimuthalAngle);
                }
            }

            EditorGUILayout.PropertyField(canZoom);
            if (canZoom.boolValue)
            {
                EditorGUILayout.PropertyField(zoomSpeed);
                EditorGUILayout.PropertyField(minDistance);
                EditorGUILayout.PropertyField(maxDistance);
            }
        }
        else
        {
            canOrbit.boolValue = false;
            canZoom.boolValue = false;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

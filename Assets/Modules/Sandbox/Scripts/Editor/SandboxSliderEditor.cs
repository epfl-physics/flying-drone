// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(SandboxSlider)), CanEditMultipleObjects]
public class SandboxSliderEditor : SliderEditor
{
    SerializedProperty valueTMP;
    SerializedProperty numDecimalDigits;
    SerializedProperty snapToDecimal;
    SerializedProperty color;
    SerializedProperty applyColorToValue;
    SerializedProperty broadcastActions;

    protected override void OnEnable()
    {
        base.OnEnable();

        valueTMP = serializedObject.FindProperty("valueTMP");
        numDecimalDigits = serializedObject.FindProperty("numDecimalDigits");
        snapToDecimal = serializedObject.FindProperty("snapToDecimal");
        color = serializedObject.FindProperty("color");
        applyColorToValue = serializedObject.FindProperty("applyColorToValue");
        broadcastActions = serializedObject.FindProperty("broadcastActions");
    }

    public override void OnInspectorGUI()
    {
        SandboxSlider component = (SandboxSlider)target;

        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Custom properties");
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.PropertyField(valueTMP);
        EditorGUILayout.PropertyField(numDecimalDigits);
        EditorGUILayout.PropertyField(snapToDecimal);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(color);
        EditorGUILayout.PropertyField(applyColorToValue);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(broadcastActions);
        serializedObject.ApplyModifiedProperties();
    }
}

// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(CustomSlider)), CanEditMultipleObjects]
public class CustomSliderEditor : SliderEditor
{
    CustomSlider component;

    SerializedProperty valueTMP;
    SerializedProperty numDecimalDigits;
    SerializedProperty snapToDecimal;
    SerializedProperty color;

    protected override void OnEnable()
    {
        base.OnEnable();

        component = (CustomSlider)target;

        valueTMP = serializedObject.FindProperty("valueTMP");
        numDecimalDigits = serializedObject.FindProperty("numDecimalDigits");
        snapToDecimal = serializedObject.FindProperty("snapToDecimal");
        color = serializedObject.FindProperty("color");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.PropertyField(valueTMP);
        EditorGUILayout.PropertyField(snapToDecimal);
        EditorGUILayout.PropertyField(numDecimalDigits);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(color);
        EditorGUILayout.Space();
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            component.ApplyColor();
        }
    }
}

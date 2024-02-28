// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LabeledSlider))]
public class LabeledSliderEditor : Editor
{
    private LabeledSlider slider;

    private Color color;

    private void OnEnable()
    {
        slider = (LabeledSlider)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (color != slider.color)
        {
            slider.SetColor(slider.color);
            color = slider.color;
        }
    }
}

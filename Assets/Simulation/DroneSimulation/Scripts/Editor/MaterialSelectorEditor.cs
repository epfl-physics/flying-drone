// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaterialSelector))]
public class MaterialSelectorEditor : Editor
{
    private MaterialSelector materialSelector;

    private void OnEnable()
    {
        materialSelector = (MaterialSelector)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUI.changed)
        {
            materialSelector.SetMaterial(materialSelector.currentIndex);
        }
    }
}

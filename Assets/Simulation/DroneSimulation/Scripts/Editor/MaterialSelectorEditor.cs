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

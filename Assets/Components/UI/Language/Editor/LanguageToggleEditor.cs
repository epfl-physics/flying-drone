using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageToggle))]
public class LanguageToggleEditor : Editor
{
    private LanguageToggle languageToggle;

    private void OnEnable()
    {
        languageToggle = (LanguageToggle)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUI.changed)
        {
            languageToggle.SetTheme(languageToggle.theme);
            languageToggle.SetLanguage(languageToggle.activeLanguage);
        }
    }
}

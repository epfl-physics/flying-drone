using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MultilingualText))]
public class MultilingualTextEditor : Editor
{
    private MultilingualText multilingualText;

    private TextData[] textData;

    private void OnEnable()
    {
        multilingualText = (MultilingualText)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUI.changed)
        {
            if (multilingualText.textData == null) return;

            if (textData == null) textData = new TextData[multilingualText.textData.Length];

            if (textData.Length != multilingualText.textData.Length)
            {
                textData = new TextData[multilingualText.textData.Length];
                SynchronizeTextData();
            }

            for (int i = 0; i < textData.Length; i++)
            {
                if (textData[i].text != multilingualText.textData[i].text)
                {
                    multilingualText.UpdateText((Language)i);
                    textData[i].text = multilingualText.textData[i].text;
                    break;
                }
            }
        }
    }

    private void SynchronizeTextData()
    {
        for (int i = 0; i < textData.Length; i++)
        {
            textData[i].language = multilingualText.textData[i].language;
            textData[i].text = multilingualText.textData[i].text;
        }
    }
}

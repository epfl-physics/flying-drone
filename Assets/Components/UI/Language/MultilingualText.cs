using TMPro;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(TextMeshProUGUI))]
public class MultilingualText : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public TextData[] textData;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        LanguageToggle.OnSetLanguage += UpdateText;
    }

    private void OnDisable()
    {
        LanguageToggle.OnSetLanguage -= UpdateText;
    }

    public void UpdateText(Language activeLanguage)
    {
        if (!tmp) tmp = GetComponent<TextMeshProUGUI>();

        foreach (var item in textData)
        {
            if (item.language == activeLanguage)
            {
                tmp.text = item.text;
                if (item.visibleUI) item.visibleUI.gameObject.SetActive(true);
            }
            else
            {
                if (item.visibleUI) item.visibleUI.gameObject.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public struct TextData
{
    public Language language;
    [TextArea(3, 10)] public string text;
    public RectTransform visibleUI;
}
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultilingualDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public List<DropdownTextItem> textItems;
    public int startIndex = 0;

    public void Awake()
    {
        if (!dropdown) return;

        // Debug.Log("Creating new options");

        dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (DropdownTextItem textItem in textItems)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(textItem.textEN);
            options.Add(option);
        }

        dropdown.options = options;
        dropdown.value = startIndex;
    }

    public void OnEnable()
    {
        LanguageToggle.OnSetLanguage += HandleLanguageChange;
    }

    public void OnDisable()
    {
        LanguageToggle.OnSetLanguage -= HandleLanguageChange;
    }

    public void HandleLanguageChange(Language language)
    {
        if (!dropdown) return;

        // Debug.Log("MultilingualDropdown > " + language);

        for (int i = 0; i < dropdown.options.Count; i++)
        {
            TMP_Dropdown.OptionData option = dropdown.options[i];
            DropdownTextItem textItem = textItems[i];
            switch (language)
            {
                case Language.EN:
                    option.text = textItem.textEN;
                    break;
                case Language.FR:
                    option.text = textItem.textFR;
                    break;
                default:
                    break;
            }

            if (dropdown.value == i)
            {
                dropdown.captionText.text = option.text;
            }
        }
    }
}

[System.Serializable]
public class DropdownTextItem
{
    public string textEN;
    public string textFR;
}

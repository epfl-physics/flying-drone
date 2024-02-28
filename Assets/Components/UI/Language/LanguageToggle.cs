// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
﻿using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LanguageToggle : MonoBehaviour, IPointerClickHandler
{
    // Language options are defined in LanguageState
    public Language activeLanguage = default;

    public enum Theme { Light, Dark }
    public Theme theme = Theme.Light;

    // Needed for retaining the same active language across scenes
    public LanguageState state;

    public static event System.Action<Language> OnSetLanguage;

    [Header("TMPs")]
    [SerializeField] private TextMeshProUGUI labelEN;
    [SerializeField] private TextMeshProUGUI labelFR;

    [Header("Light Theme")]
    [SerializeField] private Color lightActiveColor = Color.black;
    [SerializeField] private Color lightInactiveColor = Color.gray;

    [Header("Dark Theme")]
    [SerializeField] private Color darkActiveColor = Color.gray;
    [SerializeField] private Color darkInactiveColor = Color.black;

    private Color active;
    private Color inactive;

    private void Start()
    {
        SetTheme(theme);

        // Signal to other listening scripts to display the active language
        SetLanguage(state ? state.language : activeLanguage);
    }

    public void SetLanguage(Language language)
    {
        if (state) state.language = language;

        UpdateLanguageDisplay(language);
        OnSetLanguage?.Invoke(language);
        activeLanguage = language;
    }

    public void SetTheme(Theme theme)
    {
        switch (theme)
        {
            case Theme.Light:
                active = lightActiveColor;
                inactive = lightInactiveColor;
                break;
            case Theme.Dark:
                active = darkActiveColor;
                inactive = darkInactiveColor;
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleDisplay();
        OnSetLanguage?.Invoke(activeLanguage);
    }

    private void ToggleDisplay()
    {
        // Switch languages
        activeLanguage = (activeLanguage == Language.EN) ? Language.FR : Language.EN;

        if (state) state.language = activeLanguage;
        UpdateLanguageDisplay(activeLanguage);
    }

    private void UpdateLanguageDisplay(Language language)
    {
        if (!labelEN || !labelFR) { return; }

        switch (language)
        {
            case Language.EN:
                labelEN.color = active;
                labelFR.color = inactive;
                break;
            case Language.FR:
                labelEN.color = inactive;
                labelFR.color = active;
                break;
        }
    }
}


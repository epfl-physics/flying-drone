// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinBanner : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image stroke;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI textCorrect;
    [SerializeField] private GameObject textAlt;
    [SerializeField] private Image imageAlt;
    [SerializeField] private VectorColors colors;
    [SerializeField] private Sprite[] equationTerms;

    public void Hide()
    {
        if (canvasGroup)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

        SetStrokeColor(Color.white);
        SetBackgroundColor(Color.white);
        SetTextCorrectColor(Color.black);

        if (textAlt) textAlt.SetActive(true);
    }

    public void Reveal(int selectedIndex, List<int> answers)
    {
        SetStrokeColor(selectedIndex);
        SetBackgroundColor(selectedIndex);
        SetTextCorrectColor(selectedIndex);

        if (answers.Count > 1)
        {
            for (int i = 0; i < answers.Count; i++)
            {
                if (answers[i] != selectedIndex)
                {
                    SetAltImage(answers[i]);
                }
            }
        }

        int padding = 50;

        if (textAlt)
        {
            bool showAltText = answers.Count > 1;
            if (showAltText) padding = 25;
            textAlt.SetActive(showAltText);
        }

        if (TryGetComponent(out VerticalLayoutGroup layoutGroup))
        {
            layoutGroup.padding.left = padding;
            layoutGroup.padding.right = padding;
        }

        if (canvasGroup)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    private void SetStrokeColor(Color color)
    {
        if (stroke) stroke.color = color;
    }

    private void SetStrokeColor(int index)
    {
        if (!stroke || !colors) return;

        stroke.color = colors.GetColorFromIndex(index);
    }

    private void SetBackgroundColor(Color color)
    {
        if (background) background.color = color;
    }

    private void SetBackgroundColor(int index)
    {
        if (!background || !colors) return;

        // Color color = colors.GetColorFromIndex(index);
        Color color = Color.white;
        color.a = 0.9f;
        background.color = color;
    }

    private void SetTextCorrectColor(Color color)
    {
        if (textCorrect) textCorrect.color = color;
    }

    private void SetTextCorrectColor(int index)
    {
        if (!textCorrect || !colors) return;

        textCorrect.color = colors.GetColorFromIndex(index);
    }

    private void SetAltImage(int index)
    {
        if (!imageAlt) return;

        if (index >= 0 && index < equationTerms.Length)
        {
            imageAlt.sprite = equationTerms[index];
            if (colors) imageAlt.color = colors.GetColorFromIndex(index);
            imageAlt.SetNativeSize();
        }
    }
}


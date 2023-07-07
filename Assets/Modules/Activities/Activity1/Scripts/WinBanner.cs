using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinBanner : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image stroke;
    [SerializeField] private TextMeshProUGUI textCorrect;
    [SerializeField] private GameObject textAlt;
    [SerializeField] private Image imageAlt;
    [SerializeField] private VelocityColors colors;
    [SerializeField] private Sprite[] velocityTerms;

    public void Hide()
    {
        if (canvasGroup)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

        SetStrokeColor(Color.white);
        SetTextCorrectColor(Color.black);

        if (textAlt) textAlt.SetActive(true);
    }

    public void Reveal(int selectedIndex, List<int> answers)
    {
        SetStrokeColor(selectedIndex);
        SetTextCorrectColor(selectedIndex);

        if (answers.Count > 1)
        {
            for (int i = 0; i < answers.Count; i++)
            {
                if (i != selectedIndex)
                {
                    SetAltImage(i);
                }
            }
        }

        if (textAlt) textAlt.SetActive(answers.Count > 1);

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

        if (index >= 0 && index < velocityTerms.Length)
        {
            imageAlt.sprite = velocityTerms[index];
            if (colors) imageAlt.color = colors.GetColorFromIndex(index);
            imageAlt.SetNativeSize();
        }
    }
}


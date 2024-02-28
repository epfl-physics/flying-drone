// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProgressBar : MonoBehaviour
{
    public Image fill;

    private Image background;

    private void Awake()
    {
        background = GetComponent<Image>();
    }

    public void FillProgressBar(int slideIndex, int numSlides)
    {
        if (background && fill)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateFill((slideIndex + 1f) / numSlides));
        }
    }

    private IEnumerator AnimateFill(float targetFraction, float lerpTime = 0.5f)
    {
        RectTransform backgroundRT = background.rectTransform;
        RectTransform fillRT = fill.rectTransform;
        float currentFraction = fillRT.sizeDelta.x / backgroundRT.rect.size.x;

        float time = 0;
        Vector2 sizeDelta = fillRT.sizeDelta;

        while (time < lerpTime)
        {
            time += Time.deltaTime;
            float t = time / lerpTime;
            t = t * t * (3f - 2f * t);
            float fraction = Mathf.Lerp(currentFraction, targetFraction, t);
            sizeDelta.x = fraction * backgroundRT.rect.size.x;
            fillRT.sizeDelta = sizeDelta;
            yield return null;
        }

        sizeDelta.x = targetFraction * backgroundRT.rect.size.x;
        fillRT.sizeDelta = sizeDelta;
    }
}

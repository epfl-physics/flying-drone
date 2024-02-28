// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class PulsateUI : MonoBehaviour, IPointerClickHandler
{
    public float startDelay = 1; // [seconds]
    public float pulseRate = 1; // [seconds]
    public float scalePercentage = 1;

    private float elapsedTime = 0;
    private RectTransform rect;
    private bool doPulsation = false;
    private bool doNotPulsate = false;
    private Vector3 originalScale;

    public void OnPointerClick(PointerEventData eventData)
    {
        doNotPulsate = true;
        rect.localScale = originalScale;
    }

    private void OnEnable()
    {
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;
    }

    private void Update()
    {
        if (doNotPulsate) return;

        if (!doPulsation)
        {
            if (elapsedTime <= startDelay)
            {
                elapsedTime += Time.deltaTime;
                return;
            }
            else
            {
                doPulsation = true;
                elapsedTime = 0;
            }
        }

        if (doPulsation)
        {
            elapsedTime += Time.deltaTime;
            rect.localScale = originalScale * (1 + scalePercentage * Mathf.Sin(2 * Mathf.PI * elapsedTime / pulseRate));
        }
    }
}

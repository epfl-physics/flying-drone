// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class ImageHoverFade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float defaultAlpha = 1;
    public float hoverAlpha = 0.5f;

    private Image image;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!image) image = GetComponent<Image>();
        Color color = image.color;
        color.a = hoverAlpha;
        image.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RestoreDefault();
    }

    private void RestoreDefault()
    {
        if (!image) image = GetComponent<Image>();
        Color color = image.color;
        color.a = defaultAlpha;
        image.color = color;
    }

    private void OnEnable()
    {
        RestoreDefault();
    }

    private void OnDisable()
    {
        RestoreDefault();
    }
}

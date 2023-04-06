using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupHoverFade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float defaultAlpha = 1;
    public float hoverAlpha = 0.5f;

    private CanvasGroup group;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!group) group = GetComponent<CanvasGroup>();
        group.alpha = hoverAlpha;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RestoreDefault();
    }

    private void RestoreDefault()
    {
        if (!group) group = GetComponent<CanvasGroup>();
        group.alpha = defaultAlpha;
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

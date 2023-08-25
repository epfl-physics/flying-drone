using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class SandboxTab : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public void SetActive(bool active)
    {
        if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = active ? 1 : 0;
        canvasGroup.interactable = active;
        canvasGroup.blocksRaycasts = active;
    }
}

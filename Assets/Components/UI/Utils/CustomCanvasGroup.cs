using UnityEngine;

public class CustomCanvasGroup : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;

    public void SetAlpha(bool value)
    {
        if (group)
        {
            group.alpha = value ? 1 : 0;
            group.blocksRaycasts = value;
            group.interactable = value;
        }
    }
}

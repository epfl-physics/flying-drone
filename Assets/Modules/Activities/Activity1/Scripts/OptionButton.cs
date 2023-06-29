using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image background;
    [SerializeField, Range(0, 1)] private float selectedAlpha = 0.8f;

    private bool _isOn;
    public bool IsOn => _isOn;

    // Keep track of whether the mouse is currently over the GameObject
    private bool isHovering;

    public static event System.Action<OptionButton> OnSelect;

    public void OnClick(bool isOn)
    {
        _isOn = isOn;
        SetBackgroundAlpha(isOn);
        OnSelect?.Invoke(this);

        if (isHovering)
        {
            if (TryGetComponent(out CursorHoverUI cursor))
            {
                cursor.OnPointerEnter(null);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    public void SetBackgroundAlpha(bool isOn)
    {
        if (!background) return;

        Color color = background.color;
        color.a = isOn ? selectedAlpha : 1f;
        background.color = color;
    }

    public void SetToOff()
    {
        if (TryGetComponent(out Toggle toggle))
        {
            toggle.isOn = false;
        }
    }

    public void Deactivate(float alpha)
    {
        // Disable interactivity and set overall alpha value
        if (TryGetComponent(out Toggle toggle))
        {
            toggle.interactable = false;
        }

        if (TryGetComponent(out CursorHoverUI cursor))
        {
            cursor.enabled = false;
        }

        if (TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup.alpha = alpha;
        }
    }

    public void Activate()
    {
        // Disable interactivity and set overall alpha value
        if (TryGetComponent(out Toggle toggle))
        {
            toggle.interactable = true;
        }

        if (TryGetComponent(out CursorHoverUI cursor))
        {
            cursor.enabled = true;
        }

        if (TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup.alpha = 1;
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image background;
    [SerializeField] private Image latex;
    [SerializeField] private Color color = Color.black;

    private bool _isOn;
    public bool IsOn => _isOn;

    // Keep track of whether the mouse is currently over the GameObject
    private bool isHovering;

    public static event System.Action<OptionButton> OnSelect;

    private void Awake()
    {
        if (TryGetComponent(out Toggle toggle))
        {
            _isOn = toggle.isOn;
        }

        SetSelected(IsOn);
    }

    public void OnClick(bool isOn)
    {
        _isOn = isOn;
        // SetBackgroundAlpha(isOn);
        SetSelected(isOn);
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

    public void SetSelected(bool isSelected)
    {
        // Debug.Log("Selected ? " + isSelected);

        if (background) background.color = isSelected ? color : Color.white;
        if (latex) latex.color = isSelected ? Color.white : color;
    }

    // public void SetBackgroundAlpha(bool isOn)
    // {
    //     if (!background) return;

    //     Color color = background.color;
    //     color.a = isOn ? selectedAlpha : 1f;
    //     background.color = color;
    // }

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

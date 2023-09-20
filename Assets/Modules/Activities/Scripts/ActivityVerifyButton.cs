using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActivityVerifyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField, Range(0, 1)] private float alphaHover = 0.9f;
    [SerializeField, Range(0, 1)] private float alphaClick = 0.8f;

    [Header("Colors")]
    [SerializeField] private Color backgroundColorDisabled = Color.white;
    [SerializeField] private Color backgroundColorEnabled = Color.black;
    [SerializeField] private Color textColorDisabled = Color.black;
    [SerializeField] private Color textColorEnabled = Color.white;

    [SerializeField] private UnityEvent OnClick;

    private bool cameraIsOrbiting;
    // private bool isEnabled = false;
    private bool isHovering;

    private void OnEnable()
    {
        CameraOrbit.OnStartOrbit += HandleStartCameraOrbit;
        CameraOrbit.OnEndOrbit += HandleEndCameraOrbit;
    }

    private void OnDisable()
    {
        CameraOrbit.OnStartOrbit -= HandleStartCameraOrbit;
        CameraOrbit.OnEndOrbit -= HandleEndCameraOrbit;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // if (isVisible) canvasGroup.alpha = alphaClick;
        if (canvasGroup)
        {
            if (canvasGroup.interactable) canvasGroup.alpha = alphaClick;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // if (isVisible)
        // {
        //     isHovering = true;
        //     if (!cameraIsOrbiting) canvasGroup.alpha = alphaHover;
        // }
        if (canvasGroup)
        {
            if (canvasGroup.interactable)
            {
                isHovering = true;
                if (!cameraIsOrbiting) canvasGroup.alpha = alphaHover;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // if (isVisible)
        // {
        //     isHovering = false;
        //     canvasGroup.alpha = 1;
        // }
        if (canvasGroup)
        {
            if (canvasGroup.interactable)
            {
                isHovering = false;
                canvasGroup.alpha = 1;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // if (isVisible && isHovering)
        // {
        //     canvasGroup.alpha = alphaHover;
        //     OnClick?.Invoke();
        // }
        if (canvasGroup)
        {
            if (canvasGroup.interactable && isHovering)
            {
                canvasGroup.alpha = alphaHover;
                OnClick?.Invoke();
            }
        }
    }

    public void SetVisibility(bool isVisible, float invisibleAlpha = 0)
    {
        if (!canvasGroup) return;

        canvasGroup.alpha = isVisible ? 1 : invisibleAlpha;
        canvasGroup.interactable = isVisible;
        // canvasGroup.blocksRaycasts = isVisible;

        if (!isVisible) isHovering = false;

        if (TryGetComponent(out CursorHoverUI cursor))
        {
            cursor.enabled = isVisible;
        }

        // this.isVisible = isVisible;
    }

    public void Disable()
    {
        if (canvasGroup) canvasGroup.interactable = false;

        if (background) background.color = backgroundColorDisabled;
        if (text) text.color = textColorDisabled;

        if (TryGetComponent(out CursorHoverUI cursor))
        {
            cursor.enabled = false;
        }
    }

    public void Enable()
    {
        if (canvasGroup) canvasGroup.interactable = true;

        if (background) background.color = backgroundColorEnabled;
        if (text) text.color = textColorEnabled;

        if (TryGetComponent(out CursorHoverUI cursor))
        {
            cursor.enabled = true;
        }
    }

    public void HandleStartCameraOrbit()
    {
        cameraIsOrbiting = true;
    }

    public void HandleEndCameraOrbit()
    {
        cameraIsOrbiting = false;
        if (isHovering) canvasGroup.alpha = alphaHover;
    }
}
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class VerifyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField, Range(0, 1)] private float alphaHover = 0.8f;
    [SerializeField, Range(0, 1)] private float alphaClick = 0.6f;

    [SerializeField] private UnityEvent OnClick;

    private bool cameraIsOrbiting;
    private bool isVisible;
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
        if (isVisible) canvasGroup.alpha = alphaClick;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isVisible)
        {
            isHovering = true;
            if (!cameraIsOrbiting) canvasGroup.alpha = alphaHover;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isVisible)
        {
            isHovering = false;
            canvasGroup.alpha = 1;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isVisible && isHovering)
        {
            canvasGroup.alpha = alphaHover;
            Debug.Log("Triggering UnityEvent");
        }
    }

    public void SetVisibility(bool isVisible)
    {
        if (!canvasGroup) return;

        canvasGroup.alpha = isVisible ? 1 : 0;
        canvasGroup.interactable = isVisible;

        if (TryGetComponent(out CursorHoverUI cursor))
        {
            cursor.enabled = isVisible;
        }

        this.isVisible = isVisible;
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

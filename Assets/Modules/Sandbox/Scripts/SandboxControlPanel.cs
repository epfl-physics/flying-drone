using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SandboxControlPanel : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] private float xHidden = -200;
    [SerializeField] private float xShowing = -40;
    [SerializeField] private bool startHidden = false;
    [SerializeField] private CanvasGroup hiddenTitle;
    [SerializeField] private Image icon;
    [SerializeField] private CanvasGroup coverPanel;
    [SerializeField] private CanvasGroup fadePanel;

    private float yPosition;

    protected void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        yPosition = rectTransform.anchoredPosition.y;
    }

    private void Start()
    {
        if (startHidden)
        {
            Hide(0);
        }
        else
        {
            Show(0);
        }
    }

    public void SetVisibility(bool visible)
    {
        if (visible)
        {
            Hide(0.5f);
        }
        else
        {
            Show(1);
        }
    }

    public void Hide(float lerpTime)
    {
        StopAllCoroutines();

        // Vector2 targetPosition = new Vector2(xPosition, -rectTransform.rect.height);
        Vector2 targetPosition = new Vector2(xHidden, yPosition);
        StartCoroutine(LerpPosition(rectTransform, targetPosition, lerpTime, 2));
        if (hiddenTitle) StartCoroutine(LerpCanvasGroupAlpha(hiddenTitle, 1, 0.8f * lerpTime));
        if (icon) StartCoroutine(LerpIconAlpha(1, lerpTime));
        if (coverPanel) StartCoroutine(LerpCanvasGroupAlpha(coverPanel, 1, 0.8f * lerpTime));
        if (fadePanel) StartCoroutine(LerpCanvasGroupAlpha(fadePanel, 0, 0.8f * lerpTime));
    }

    public void Show(float lerpTime)
    {
        StopAllCoroutines();
        Vector2 targetPosition = new Vector2(xShowing, yPosition);
        StartCoroutine(LerpPosition(rectTransform, targetPosition, lerpTime, 2));
        if (hiddenTitle) StartCoroutine(LerpCanvasGroupAlpha(hiddenTitle, 0, 0.6f * lerpTime));
        if (icon) StartCoroutine(LerpIconAlpha(0, 0.6f * lerpTime));
        if (coverPanel) StartCoroutine(LerpCanvasGroupAlpha(coverPanel, 0, 0.6f * lerpTime));
        if (fadePanel) StartCoroutine(LerpCanvasGroupAlpha(fadePanel, 1, 0.6f * lerpTime));
    }

    private IEnumerator LerpPosition(RectTransform rectTransform,
                                     Vector2 targetPosition,
                                     float lerpTime,
                                     float easeFactor = 0)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;

        float time = 0;
        while (time < lerpTime)
        {
            time += Time.unscaledDeltaTime;
            float t = EaseOut(time / lerpTime, 1 + easeFactor);
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }

    private float EaseOut(float t, float a)
    {
        return 1 - Mathf.Pow(1 - t, a);
    }

    private IEnumerator LerpIconAlpha(float targetAlpha, float lerpTime, float easeFactor = 0)
    {
        Color startColor = icon.color;
        Color targetColor = startColor;
        targetColor.a = targetAlpha;

        float time = 0;
        while (time < lerpTime)
        {
            time += Time.unscaledDeltaTime;
            // float t = time / lerpTime;
            float t = EaseOut(time / lerpTime, 1 + easeFactor);
            icon.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        icon.color = targetColor;
    }

    private IEnumerator LerpCanvasGroupAlpha(CanvasGroup cg, float targetAlpha, float lerpTime, float easeFactor = 0)
    {
        float startAlpha = cg.alpha;
        cg.blocksRaycasts = targetAlpha == 1;

        float time = 0;
        while (time < lerpTime)
        {
            time += Time.unscaledDeltaTime;
            float t = EaseOut(time / lerpTime, 1 + easeFactor);
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        cg.alpha = targetAlpha;
    }
}

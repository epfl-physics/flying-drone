using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class TheoryControlDrawer : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] private float xHidden = -200;
    [SerializeField] private float xShowing = -40;
    [SerializeField] private bool startHidden = false;
    [SerializeField] private Image icon;

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
        if (icon) StartCoroutine(LerpIconAlpha(1, lerpTime));
    }

    public void Show(float lerpTime)
    {
        StopAllCoroutines();
        Vector2 targetPosition = new Vector2(xShowing, yPosition);
        StartCoroutine(LerpPosition(rectTransform, targetPosition, lerpTime, 2));
        if (icon) StartCoroutine(LerpIconAlpha(0, 0.6f * lerpTime));
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
}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SandboxVisualisationsPanel : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] private float yHidden = -100;
    [SerializeField] private float yShowing = -20;
    [SerializeField] private bool startHidden = false;
    [SerializeField] private CanvasGroup fadePanel;

    private float xPosition;

    protected void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        xPosition = rectTransform.anchoredPosition.x;
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

        Vector2 targetPosition = new Vector2(xPosition, yHidden);
        StartCoroutine(LerpPosition(rectTransform, targetPosition, lerpTime, 2));
        if (fadePanel) StartCoroutine(LerpFadePanelAlpha(fadePanel, 0, 0.8f * lerpTime));
    }

    public void Show(float lerpTime)
    {
        StopAllCoroutines();
        Vector2 targetPosition = new Vector2(xPosition, yShowing);
        StartCoroutine(LerpPosition(rectTransform, targetPosition, lerpTime, 2));
        if (fadePanel) StartCoroutine(LerpFadePanelAlpha(fadePanel, 1, 0.6f * lerpTime));
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

    private IEnumerator LerpFadePanelAlpha(CanvasGroup cg, float targetAlpha, float lerpTime, float easeFactor = 0)
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

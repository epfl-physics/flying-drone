using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TheoryControlDrawer : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] private float xHidden = -200;
    [SerializeField] private float xShowing = -40;
    [SerializeField] private bool startHidden = false;

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
    }

    public void Show(float lerpTime)
    {
        StopAllCoroutines();
        Vector2 targetPosition = new Vector2(xShowing, yPosition);
        StartCoroutine(LerpPosition(rectTransform, targetPosition, lerpTime, 2));
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
}

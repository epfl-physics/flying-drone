using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class TheoryCanvasGroup : MonoBehaviour
{
    public void SetAlpha(bool isOne)
    {
        GetComponent<CanvasGroup>().alpha = isOne ? 1 : 0;
    }
}

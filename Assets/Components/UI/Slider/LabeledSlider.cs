using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LabeledSlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueTMP;
    [SerializeField, Range(1, 3)] private int numDecimalDigits = 1;
    [SerializeField] private bool snapToDecimal;
    [SerializeField] private bool controlValuePosition = true;

    private Slider slider;

    private void Awake()
    {
        if (controlValuePosition)
        {
            var valueRect = valueTMP.GetComponent<RectTransform>();
            valueRect.sizeDelta = (30 + 10 * numDecimalDigits) * Vector2.right + valueRect.sizeDelta.y * Vector2.up;
            valueRect.anchoredPosition = (45 + 10 * numDecimalDigits) * Vector2.right + valueRect.anchoredPosition.y * Vector2.up;
        }

        if (valueTMP)
        {
            var slider = GetComponent<Slider>();
            SetValue(slider.value);
        }
    }

    public void SetValue(float value)
    {
        if (snapToDecimal)
        {
            if (!slider) slider = GetComponent<Slider>();
            float factor = Mathf.Pow(10f, numDecimalDigits);
            slider.value = Mathf.Round(factor * slider.value) / factor;
        }

        string format = "0.";
        for (int i = 0; i < numDecimalDigits; i++)
        {
            format += "0";
        }
        if (valueTMP) valueTMP.text = value.ToString(format);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CenteredSlider : Slider
{
    public TextMeshProUGUI valueTMP;
    public bool snapToDecimal;
    public enum DecimalDigits { Zero, One, Two, Three }
    public DecimalDigits numDecimalDigits = default;
    public Color color = Color.black;
    public bool applyColorToValue;

    protected override void OnEnable()
    {
        base.OnEnable();
        onValueChanged.AddListener(UpdateValue);
        onValueChanged.AddListener(UpdateFillArea);
        ApplyColor();
    }

    protected override void OnDisable()
    {
        onValueChanged.RemoveListener(UpdateFillArea);
        onValueChanged.RemoveListener(UpdateValue);
        base.OnDisable();
    }

    protected override void Start()
    {
        base.Start();

        // Make sure the slider is rendered properly to start
        UpdateValue(value);
        UpdateFillArea(value);
        ApplyColor();
    }

    private void UpdateFillArea(float value)
    {
        float normalizedValue = (value - minValue) / (maxValue - minValue);
        float zeroPoint = -minValue / (maxValue - minValue);
        float offset = (normalizedValue - zeroPoint) * 2;

        fillRect.anchorMin = new Vector2(offset < 0 ? zeroPoint + offset / 2 : zeroPoint, 0);
        fillRect.anchorMax = new Vector2(offset < 0 ? zeroPoint : zeroPoint + offset / 2, 1);
    }

    public void ApplyColor()
    {
        if (!fillRect)
        {
            Debug.LogWarning("Fill Rect has not been assigned");
            return;
        }

        if (!handleRect)
        {
            Debug.LogWarning("Handle Rect has not been assigned");
            return;
        }

        fillRect.GetComponent<Image>().color = color;
        handleRect.GetChild(0).GetComponent<Image>().color = color;
    }

    public void UpdateValue(float value)
    {
        if (snapToDecimal)
        {
            float factor = Mathf.Pow(10f, (int)numDecimalDigits);
            this.value = Mathf.Round(factor * value) / factor;
        }

        string format = "0.";
        for (int i = 0; i < (int)numDecimalDigits; i++)
        {
            format += "0";
        }

        // Add a minus sign spacer for positive values so the actual digits are always aligned
        float threshold = -0.5f * Mathf.Pow(10f, -(int)numDecimalDigits);
        string spacer = value > threshold ? "<color=#ffffff00>-</color>" : "";

        if (valueTMP) valueTMP.text = spacer + value.ToString(format);
    }
}

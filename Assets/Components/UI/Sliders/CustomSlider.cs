// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CustomSlider : Slider
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
        ApplyColor();
    }

    protected override void OnDisable()
    {
        onValueChanged.RemoveListener(UpdateValue);
        base.OnDisable();
    }

    protected override void Start()
    {
        base.Start();
        onValueChanged.AddListener(UpdateValue);
        UpdateValue(value);
        ApplyColor();
    }

    protected override void OnDestroy()
    {
        onValueChanged.RemoveListener(UpdateValue);
        base.OnDestroy();
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

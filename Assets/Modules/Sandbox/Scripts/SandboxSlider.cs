// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SandboxSlider : Slider
{
    public TextMeshProUGUI valueTMP;
    public enum DecimalDigits { Zero, One, Two, Three }
    public DecimalDigits numDecimalDigits = default;
    public bool snapToDecimal;

    public Color color = Color.black;
    public bool applyColorToValue;

    public bool broadcastActions = true;

    public static event System.Action OnMouseDown;
    public static event System.Action OnMouseUp;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (broadcastActions) OnMouseDown?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (broadcastActions) OnMouseUp?.Invoke();
    }

    protected override void Awake()
    {
        base.Awake();

        ApplyChanges();
    }

    public void ApplyChanges()
    {
        SetDisplayedValue();
        ApplyColor();
    }

    public void SetDisplayedValue()
    {
        if (snapToDecimal)
        {
            float factor = Mathf.Pow(10f, (int)numDecimalDigits);
            value = Mathf.Round(factor * value) / factor;
        }

        string format = "0.";
        for (int i = 0; i < (int)numDecimalDigits; i++)
        {
            format += "0";
        }
        if (valueTMP) valueTMP.text = value.ToString(format);
    }

    public void ApplyColor()
    {
        fillRect.GetComponent<Image>().color = color;
        handleRect.GetChild(0).GetComponent<Image>().color = color;
        if (valueTMP) valueTMP.color = applyColorToValue ? color : Color.black;
    }
}

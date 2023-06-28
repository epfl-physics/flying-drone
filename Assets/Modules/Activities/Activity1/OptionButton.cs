using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField, Range(0, 1)] private float selectedAlpha = 0.8f;

    private bool _isOn;
    public bool IsOn => _isOn;

    public static event System.Action<OptionButton> OnSelect;

    public void OnClick(bool isOn)
    {
        _isOn = isOn;
        SetBackgroundAlpha(isOn);
        OnSelect?.Invoke(this);
    }

    public void SetBackgroundAlpha(bool isOn)
    {
        if (!background) return;

        Color color = background.color;
        color.a = isOn ? selectedAlpha : 1f;
        background.color = color;
    }
}

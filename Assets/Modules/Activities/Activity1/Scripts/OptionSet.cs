using System.Collections.Generic;
using UnityEngine;

public class OptionSet : MonoBehaviour
{
    [SerializeField] private OptionButton[] options;
    [SerializeField] private ActivityVerifyButton verifyButton;
    [SerializeField] private CanvasGroup textIncorrect;

    private int selectedIndex = -1;
    public int SelectedIndex => selectedIndex;

    private void OnEnable()
    {
        OptionButton.OnSelect += HandleOptionSelected;
    }

    private void OnDisable()
    {
        OptionButton.OnSelect -= HandleOptionSelected;
    }

    private void Start()
    {
        if (verifyButton) verifyButton.Disable();
        // SetButtonVisibility(verifyButton, false, 0.2f);
        SetFeedbackTextVisibility(textIncorrect, false);
    }

    public void HandleOptionSelected(OptionButton option)
    {
        selectedIndex = -1;

        if (option.IsOn)
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i] == option)
                {
                    selectedIndex = i;
                    break;
                }
            }
        }

        if (verifyButton)
        {
            if (selectedIndex > -1)
            {
                verifyButton.Enable();
            }
            else
            {
                verifyButton.Disable();
            }
        }

        // SetButtonVisibility(verifyButton, selectedIndex > -1, 0.2f);
        SetFeedbackTextVisibility(textIncorrect, false);
    }

    public void Reset()
    {
        // Note : verify button visibility is automatically handled

        selectedIndex = -1;

        foreach (OptionButton option in options)
        {
            option.Activate();
            option.SetToOff();
        }

        SetFeedbackTextVisibility(textIncorrect, false);
    }

    public void SelectionIsCorrect()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].Deactivate(selectedIndex == i ? 1 : 0.5f);
        }

        // SetButtonVisibility(verifyButton, false, 0.2f);
        if (verifyButton) verifyButton.Disable();
    }

    public void SelectionIsIncorrect(List<int> trueIndex)
    {
        SetFeedbackTextVisibility(textIncorrect, true);
    }

    // private void SetButtonVisibility(Activity1ArrowButton button, bool isVisible, float invisibleAlpha = 0)
    // {
    //     if (button) button.SetVisibility(isVisible, invisibleAlpha);
    // }

    private void SetFeedbackTextVisibility(CanvasGroup feedback, bool isVisible)
    {
        if (feedback) feedback.alpha = isVisible ? 1 : 0;
    }
}

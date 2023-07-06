using System.Collections.Generic;
using UnityEngine;

public class OptionSet : MonoBehaviour
{
    [SerializeField] private OptionButton[] options;
    [SerializeField] private Activity1ArrowButton verifyButton;
    [SerializeField] private Activity1ArrowButton continueButton;
    [SerializeField] private GameObject textCorrect;
    [SerializeField] private GameObject textIncorrect;

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
        SetButtonVisibility(verifyButton, false, 0.2f);
        SetButtonVisibility(continueButton, false);
        SetFeedbackTextVisibility(textCorrect, false);
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

        SetButtonVisibility(verifyButton, selectedIndex > -1, 0.2f);
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

        SetButtonVisibility(continueButton, false);
        SetFeedbackTextVisibility(textCorrect, false);
        SetFeedbackTextVisibility(textIncorrect, false);
    }

    public void SelectionIsCorrect()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].Deactivate(selectedIndex == i ? 1 : 0.5f);
        }

        SetButtonVisibility(verifyButton, false, 0.2f);
        SetButtonVisibility(continueButton, true);

        SetFeedbackTextVisibility(textCorrect, true);
    }

    public void SelectionIsIncorrect(List<int> trueIndex)
    {
        SetFeedbackTextVisibility(textIncorrect, true);
    }

    private void SetButtonVisibility(Activity1ArrowButton button, bool isVisible, float invisibleAlpha = 0)
    {
        if (button) button.SetVisibility(isVisible, invisibleAlpha);
    }

    private void SetFeedbackTextVisibility(GameObject text, bool isVisible)
    {
        if (text) text.SetActive(isVisible);
    }
}

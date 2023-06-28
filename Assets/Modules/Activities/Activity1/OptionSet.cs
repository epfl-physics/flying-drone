using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSet : MonoBehaviour
{
    [SerializeField] private OptionButton[] options;
    [SerializeField] private VerifyButton verifyButton;

    private int selectedIndex = -1;

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
        if (verifyButton) verifyButton.SetVisibility(false);
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

        if (verifyButton) verifyButton.SetVisibility(selectedIndex > -1);
    }
}

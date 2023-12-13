using UnityEngine;

public class SandboxVectorsDropdown : MonoBehaviour
{
    [Header("Dropdowns")]
    [SerializeField] private MultilingualDropdown dropdown;

    [Header("Panels")]
    [SerializeField] private CanvasGroup accelerationsPanel;
    [SerializeField] private CanvasGroup forcesPanel;

    public void SetReferenceFrame(bool frameIsAbsolute)
    {
        if (!dropdown) return;

        // Get reference to the 'accelerations' label
        DropdownTextItem item = dropdown.textItems[3];

        // Set its text values
        item.textEN = frameIsAbsolute ? "Accelerations" : "Fictitious Forces";
        item.textFR = frameIsAbsolute ? "Accélérations" : "Forces d'Inertie";

        dropdown.Apply();

        // Update visible equations
        if (accelerationsPanel)
        {
            accelerationsPanel.alpha = frameIsAbsolute ? 1 : 0;
            accelerationsPanel.interactable = frameIsAbsolute;
            accelerationsPanel.blocksRaycasts = frameIsAbsolute;
        }
        if (forcesPanel)
        {
            forcesPanel.alpha = frameIsAbsolute ? 0 : 1;
            forcesPanel.interactable = !frameIsAbsolute;
            forcesPanel.blocksRaycasts = !frameIsAbsolute;
        }
    }
}

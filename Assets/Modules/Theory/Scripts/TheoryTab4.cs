using UnityEngine;

public class TheoryTab4 : MonoBehaviour
{
    [Header("Tab Labels")]
    [SerializeField] private GameObject accelerationsLabel;
    [SerializeField] private GameObject forcessLabel;

    [Header("Panels")]
    [SerializeField] private CanvasGroup accelerationsPanel;
    [SerializeField] private CanvasGroup forcesPanel;

    public void SetReferenceFrame(bool frameIsTree)
    {
        // Update tab text
        if (accelerationsLabel) accelerationsLabel.SetActive(frameIsTree);
        if (forcessLabel) forcessLabel.SetActive(!frameIsTree);

        // Update visible equations
        if (accelerationsPanel)
        {
            accelerationsPanel.alpha = frameIsTree ? 1 : 0;
            accelerationsPanel.interactable = frameIsTree;
            accelerationsPanel.blocksRaycasts = frameIsTree;
        }
        if (forcesPanel)
        {
            forcesPanel.alpha = frameIsTree ? 0 : 1;
            forcesPanel.interactable = !frameIsTree;
            forcesPanel.blocksRaycasts = !frameIsTree;
        }
    }
}

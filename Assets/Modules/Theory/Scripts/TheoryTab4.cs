using UnityEngine;

public class TheoryTab4 : MonoBehaviour
{
    [Header("Tab Labels")]
    [SerializeField] private CanvasGroup accelerationsLabel;
    [SerializeField] private CanvasGroup forcessLabel;

    [Header("Panels")]
    [SerializeField] private CanvasGroup accelerationsPanel;
    [SerializeField] private CanvasGroup forcesPanel;

    public void SetReferenceFrame(bool frameIsTree)
    {
        // Update tab text
        if (accelerationsLabel)
        {
            accelerationsLabel.alpha = frameIsTree ? 1 : 0;
            accelerationsLabel.interactable = frameIsTree;
            accelerationsLabel.blocksRaycasts = frameIsTree;
        }
        if (forcessLabel)
        {
            forcessLabel.alpha = frameIsTree ? 0 : 1;
            forcessLabel.interactable = !frameIsTree;
            forcessLabel.blocksRaycasts = !frameIsTree;
        }

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

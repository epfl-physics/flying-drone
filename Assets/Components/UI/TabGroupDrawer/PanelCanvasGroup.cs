// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

public class PanelCanvasGroup : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;

    public void SetAlpha(bool value)
    {
        if (group)
        {
            group.alpha = value ? 1 : 0;
            group.blocksRaycasts = value;
            group.interactable = value;
        }
    }
}

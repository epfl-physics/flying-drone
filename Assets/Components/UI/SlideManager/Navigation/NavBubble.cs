// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;

namespace Slides
{
    public class NavBubble : MonoBehaviour, IPointerClickHandler
    {
        public static event System.Action<int> OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(transform.GetSiblingIndex());
        }
    }
}

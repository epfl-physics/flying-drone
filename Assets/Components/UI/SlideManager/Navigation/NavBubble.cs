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

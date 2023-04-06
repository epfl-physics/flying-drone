using UnityEngine;
using UnityEngine.EventSystems;

namespace Slides
{
    public class Bubble : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SendMessageUpwards("HandleBubbleClick", transform.GetSiblingIndex(), SendMessageOptions.DontRequireReceiver);
        }
    }

}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollRectScrollbarOnly : ScrollRect
{
    public override void OnDrag(PointerEventData eventData) {
        // removes drag feature from scrollrect so that you need to use scrollbar or scrollwheel to scroll
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using BrickBuilder.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrickBuilder.UI
{
    public class RepositionableWindow : MonoBehaviour
    {
        public Vector2 minSize;
        public Vector2 maxSize;

        [Space(5)]
        [Header("Resizing")]
        public RectTransform TopZone;
        public RectTransform BottomZone;
        public RectTransform LeftZone;
        public RectTransform RightZone;
        
        // when doing anchor drag thing
        public RectTransform TopSegment;
        public RectTransform BottomSegment;

        private RectTransform window;

        private void Awake()
        {
            window = GetComponent<RectTransform>();
        }

        // resizes window in specified direction based on cursor distance from begin point
        public void ZoneDrag(BaseEventData data)
        {
            PointerEventData ped = (PointerEventData) data;

            Vector2 delta = ped.delta;

            if (TopZone != null && ped.pointerPress == TopZone.gameObject)
            {
                delta.y *= -1; // TODO: Test
                delta.x = 0;
            } else if (BottomZone != null && ped.pointerPress == BottomZone.gameObject)
            {
                delta.x = 0;
            } else if (LeftZone != null && ped.pointerPress == LeftZone.gameObject)
            {
                delta.x *= -1;
                delta.y = 0;
            } else if (RightZone != null && ped.pointerPress == RightZone.gameObject)
            {
                delta.y = 0;
            }
            else
            {
                return;
            }

            Vector2 min = minSize;
            Vector2 max = maxSize;

            if (delta.x == 0)
            {
                min.x = float.MinValue;
                max.x = float.MaxValue;
            }
            if (delta.y == 0)
            {
                min.y = float.MinValue;
                max.y = float.MaxValue;
            }
            
            Vector2 targetSize = (window.sizeDelta + delta).Clamp(min, max);
            window.sizeDelta = targetSize;
        }

        // moves anchor points based on cursor distance from begin point
        public void ZoneDragAnchor(BaseEventData data)
        {
            PointerEventData ped = (PointerEventData) data;
            
            float canvasHeight = EditorUI.instance.CanvasRectTransform.rect.height - EditorUI.SidebarTop;
            float relativeDragAmount = Mathf.Clamp(ped.position.y / canvasHeight, 0.05f, 1f);
            
            TopSegment.anchorMin = new Vector2(0, relativeDragAmount);
            BottomSegment.anchorMax = new Vector2(1, relativeDragAmount);

            RectTransform zoneRect = ped.pointerPress.GetComponent<RectTransform>();
            zoneRect.anchorMin = new Vector2(0, relativeDragAmount);
            zoneRect.anchorMax = new Vector2(1, relativeDragAmount);
        }

        // Repositions window based on cursor movement
        public void PositionDrag(BaseEventData data)
        {
            PointerEventData ped = (PointerEventData) data;

            // Move window
            Vector3 windowPosition = window.position + (Vector3) ped.delta;

            // Clamp position to screen bounds
            // Pivot must be centered
            float canvasWidth = EditorUI.instance.CanvasRectTransform.rect.width;
            float canvasHeight = EditorUI.instance.CanvasRectTransform.rect.height;
            Rect windowRect = window.rect;

            float clampedX = Mathf.Clamp(windowPosition.x, windowRect.width / 2f, canvasWidth - windowRect.width / 2f);
            float clampedY = Mathf.Clamp(windowPosition.y, windowRect.height / 2f, canvasHeight - windowRect.height / 2f);
            
            windowPosition.Set(clampedX, clampedY, 0f);
            
            // Actually move window
            window.position = windowPosition;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(ScrollRect))]
public class ScrollViewElementCuller : MonoBehaviour {
    
    private ScrollRect m_ScrollRect;
    private Transform m_ContentTransform;

    private RectTransform m_UpperDeadSpace;
    private RectTransform m_LowerDeadSpace;
    
    private float m_viewHeight;
    private float m_contentHeight;
    
    private List<GameObject> m_Elements = new List<GameObject>();
    private Dictionary<GameObject, FloatRange> m_ElementsBounds = new Dictionary<GameObject, FloatRange>();

    private void Awake() {
        m_ScrollRect = GetComponent<ScrollRect>();
        m_ContentTransform = m_ScrollRect.content;

        GameObject upperDS = new GameObject("UpperDeadSpace", typeof(RectTransform));
        m_UpperDeadSpace = (RectTransform)upperDS.transform;
        m_UpperDeadSpace.sizeDelta = Vector2.zero;
        m_UpperDeadSpace.SetParent(m_ContentTransform);
        
        GameObject lowerDS = new GameObject("LowerDeadSpace", typeof(RectTransform));
        m_LowerDeadSpace = (RectTransform)lowerDS.transform;
        m_LowerDeadSpace.sizeDelta = Vector2.zero;
        m_LowerDeadSpace.SetParent(m_ContentTransform);
    }

    public void RecalculateView() {
        m_viewHeight = m_ScrollRect.viewport.rect.height;
        VisibilityCheck();
    }
    
    public void ScanForElements() {
        m_Elements.Clear();
        m_contentHeight = 0f;
        
        for (int i = 0; i < m_ContentTransform.childCount; i++) {
            RectTransform element = (RectTransform)m_ContentTransform.GetChild(i);
            
            // ignore deadspace elements
            if (element == m_UpperDeadSpace || element == m_LowerDeadSpace) continue;
            
            // ignore excluded elements
            if (element.CompareTag("ScrollViewCullerExclude")) continue;

            // calculate bounds
            FloatRange elementRange = new FloatRange(Mathf.Abs(element.anchoredPosition.y) - element.sizeDelta.y / 2f, Mathf.Abs(element.anchoredPosition.y) + element.sizeDelta.y / 2f);
            m_contentHeight += elementRange.Size;
            
            m_Elements.Add(element.gameObject);
            m_ElementsBounds.Add(element.gameObject, elementRange);
        }
        
        // ensure that deadspace objects are at each end of the hierarchy
        m_UpperDeadSpace.SetAsFirstSibling();
        m_LowerDeadSpace.SetAsLastSibling();
    }

    public void AddElement(GameObject element) {
        // TODO
    }

    public void RemoveElement(GameObject element) {
        // TODO
    }

    public void ClearElements() {
        // TODO
    }

    private void VisibilityCheck() {
        // calculate visible range
        float viewCenter = m_contentHeight * (1 - m_ScrollRect.verticalScrollbar.value);
        viewCenter += Mathf.Lerp(-m_viewHeight / 2f, m_viewHeight / 2f, m_ScrollRect.verticalScrollbar.value);
        FloatRange viewRange = new FloatRange(viewCenter - m_viewHeight / 2f, viewCenter + m_viewHeight / 2f);
        viewRange.Clamp(0f, m_contentHeight, true);

        //Debug.Log($"Content Height: {m_contentHeight}, View Height: {m_viewHeight}, Scrollbar Value: {1-m_ScrollRect.verticalScrollbar.value}, View Center: {viewCenter}, View Range: {viewRange}");
        
        // calculate deadspace size
        float upperDeadSpace = viewRange.Min;
        float lowerDeadSpace = m_contentHeight - viewRange.Max;

        m_UpperDeadSpace.sizeDelta = new Vector2(0f, upperDeadSpace);
        m_LowerDeadSpace.sizeDelta = new Vector2(0f, lowerDeadSpace);
        
        // set element visibility
        for (int i = 0; i < m_Elements.Count; i++) {
            GameObject element = m_Elements[i];
            FloatRange elementRange = m_ElementsBounds[element];
            if (viewRange.Within(elementRange)) {
                // visible element
                element.SetActive(true);
            }
            else {
                // invisible element
                element.SetActive(false);
            }
        }
    }
}

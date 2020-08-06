using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HierarchyElement : MonoBehaviour
{
    public Image Icon;
    public TMP_Text Label;

    public Map.ElementType Type;
    public new GameObject gameObject;
    public object AssociatedObject;

    public Button SelectionButton;
    public Image SelectionImage;

    private Dictionary<int, HierarchyElement> ChildElements = new Dictionary<int, HierarchyElement>();

    public void Set(Map.ElementType type, Sprite icon, string label, GameObject GO, object associatedObject) {
        Type = type;
        Icon.sprite = icon;
        Label.text = label;
        gameObject = GO;
        AssociatedObject = associatedObject;
    }

    public void Add (HierarchyElement element) {
        if (!ChildElements.ContainsValue(element)) {
            if (element.AssociatedObject is Brick) {
                int id = (element.AssociatedObject as Brick).ID;
                ChildElements.Add(id, element);
            } else if (element.AssociatedObject is BrickGroup) {
                int id = (element.AssociatedObject as BrickGroup).ID;
                ChildElements.Add(id, element);
            }
        }
    }

    public void Remove (int id) {
        if (ChildElements.ContainsKey(id)) {
            ChildElements.Remove(id);
        }
    }
}

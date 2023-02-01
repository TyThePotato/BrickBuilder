using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrickBuilder.UI
{
    public class HierarchyElement : MonoBehaviour
    {
        public Guid ID; // ID of associated element
        public int Position; // Position in hierarchy
        public bool Selected;
        
        public TMP_Text Label;
        public Image Background;
        public Image Icon;
        public Button SelectButton;

        public void SetIcon(Sprite sprite, Color color)
        {
            Icon.sprite = sprite;

            color.a = 1f; // disallow transparent icons
            Icon.color = color;
            
        }
    }
}
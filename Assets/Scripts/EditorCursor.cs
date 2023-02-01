using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrickBuilder.UI
{
    public class EditorCursor : MonoBehaviour
    {
        public Texture2D[] Cursors;
        
        // Cursor Types:
        // 0 : Default
        // 1 : Selected
        // 2 : Resize Horizontal
        // 3 : Resize Vertical
        // 4 : Resize Diagonal
        public void SetCursor(int type)
        {
            Cursor.SetCursor(Cursors[type], Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
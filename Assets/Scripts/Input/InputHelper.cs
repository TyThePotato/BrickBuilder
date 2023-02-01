using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BrickBuilder.Input
{
    // TODO: add helper functions for GetButton equiv (true while pressed)
    public class InputHelper : MonoBehaviour
    {
        public static Controls Controls;

        private void Awake()
        {
            Controls = new Controls();
            SetControlsEnabled(true);
        }

        public static void SetControlsEnabled(bool value)
        {
            if (value)
            {
                Controls.Enable();
            }
            else
            {
                Controls.Disable();
            }
        }
        
        // InputAction Shortcuts

        public static InputAction movement => Controls.Main.Movement;
        public static InputAction vertical => Controls.Main.Vertical;
        public static InputAction look => Controls.Main.Look;
        public static InputAction boost => Controls.Main.Boost;
        public static InputAction select => Controls.Main.Select;
        public static InputAction cancel => Controls.Main.Cancel;
        public static InputAction lookButton => Controls.Main.LookButton;
        
        public static InputAction selectionAdd => Controls.Main.SelectionAdd;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLocker : MonoBehaviour {

    public static bool IsLocked => _locked; // public get-only property

    private static bool _locked;
    private static Vector2 lockedPosition;

    private void LateUpdate() {
        if (_locked) {
            if (Mouse.current.wasUpdatedThisFrame) {
                Mouse.current.WarpCursorPosition(lockedPosition);
            }
        }
    }

    public static void SetLockState(bool locked) {
        if (locked == _locked) return;
        
        if (locked)
            lockedPosition = Mouse.current.position.ReadValue();

        _locked = locked;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BBInputManager : MonoBehaviour
{
    public static BBControls Controls;
    public static bool ControlsEnabled = false;
    public static bool ForceDisable = false;

    public static KeyRebindEvent keyRebinded = new KeyRebindEvent();

    public static List<InputAction> RebindableKeys;

    private static bool rebinding = false;

    private void Awake() {
        Controls = new BBControls();

        RebindableKeys = new List<InputAction>() {
            Controls.EditorKeys.NewBrick,
            Controls.EditorKeys.Copy,
            Controls.EditorKeys.Paste,
            Controls.EditorKeys.Cut,
            Controls.EditorKeys.Duplicate,
            Controls.EditorKeys.SaveFile,
            Controls.EditorKeys.OpenFile,
            Controls.EditorKeys.Translate,
            Controls.EditorKeys.Scale,
            Controls.EditorKeys.Rotate,
            Controls.EditorKeys.SelectAll,
            Controls.EditorKeys.Undo,
            Controls.EditorKeys.Redo,
            Controls.EditorKeys.GroupSelection
        };

        LoadSavedControlsPaths(); // paths can only be set while controls are disabled
        EnableControls(); // enable all controls
        LoadSavedControlToggles(); // disable controls that must be disabled
    }

    public static void LoadSavedControlsPaths () {
        for (int i = 0; i < RebindableKeys.Count; i++) {
            (bool, string) sett = GetSettings(i);
            if (sett.Item2 != null) {
                int bindingIndex = 2;
                if (RebindableKeys[i].name == "Translate" || RebindableKeys[i].name == "Scale" || RebindableKeys[i].name == "Rotate") bindingIndex = 0;
                RebindableKeys[i].ChangeBinding(bindingIndex).WithPath(sett.Item2);
            }
        }
    }

    public static void LoadSavedControlToggles () {
        for (int i = 0; i < RebindableKeys.Count; i++) {
            (bool, string) sett = GetSettings(i);
            if (sett.Item2 != null) {
                if (sett.Item1 == false) {
                    RebindableKeys[i].Disable();
                }
            }
        }
    }

    public static (bool, string) GetSettings (int index) {
        return (SettingsManager.Settings.EnabledHotkeys[index], SettingsManager.Settings.HotkeyPaths[index]);
    }

    public static void SetControls (bool value, bool force = false) {
        if (value) {
            if (ForceDisable && !force) return;
            EnableControls(force);
            LoadSavedControlToggles(); // disable controls that should be disabled
        } else {
            DisableControls(force);
        }
    }

    public static void EnableControls (bool force = false) {
        if (ForceDisable && !force) return;
        Controls.Enable();
        LoadSavedControlToggles(); // disable controls that should be disabled
        ControlsEnabled = true;
        Debug.Log("Enabled Controls");
    }

    public static void DisableControls (bool force = false) {
        if (force) ForceDisable = true;
        Controls.Disable();
        ControlsEnabled = false;
        Debug.Log("Disabled Controls");
    }

    public static void StartRebind (InputAction action) {
        DisableControls();
        rebinding = true;

        int bindingToRebind = 2;
        if (action.name == "Translate" || action.name == "Scale" || action.name == "Rotate") bindingToRebind = 0; // the tool hotkeys dont use a modifier

        var rebindOperation = action.PerformInteractiveRebinding(bindingToRebind).WithControlsExcluding("Mouse").WithControlsExcluding("Keyboard/leftCtrl").WithControlsExcluding("Keyboard/rightCtrl").WithControlsExcluding("Keyboard/ctrl").WithControlsExcluding("Keyboard/anyKey").OnMatchWaitForAnother(0.1f);

        rebindOperation.OnComplete(c => {
            Debug.Log("Successfully rebinded " + c.action.name);

            // trigger event
            keyRebinded.Invoke(c.action);

            // clean up rebind operation
            rebindOperation.Dispose();
            rebindOperation = null;

            // re-enable controls
            EnableControls();
        });

        rebindOperation.Start();
        Debug.Log("Started rebind operation on " + action.name);
    }

    public static bool IsCtrlDown () {
        return Controls.EditorKeys.Control.ReadValue<float>() == 1f;
    }
}

public class KeyRebindEvent : UnityEvent<InputAction> { }
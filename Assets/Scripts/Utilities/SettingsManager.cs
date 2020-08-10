using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class SettingsManager : MonoBehaviour
{
    public static Settings Settings; // is this bad
    public static UnityEvent SettingsChanged;

    private static string SettingsPath;

    private void Awake() {
        if (SettingsChanged == null) SettingsChanged = new UnityEvent();

        SettingsPath = Application.dataPath + "/settings.json";
        LoadSettings();
    }

    public static void LoadSettings () {
        if (File.Exists(SettingsPath)) {
            // load settings
            string jsonData = File.ReadAllText(SettingsPath);
            Settings = JsonUtility.FromJson<Settings>(jsonData);
            SettingsChanged.Invoke();
        } else {
            // create new settings
            Settings = new Settings();
            SaveSettings();
        }
    }

    public static void SaveSettings () {
        if (Settings == null) return;
        string jsonData = JsonUtility.ToJson(Settings, true);
        File.WriteAllText(SettingsPath, jsonData);
        SettingsChanged.Invoke();
    }
}

public class Settings {
    // Editing
    public float StudSnap = 1; // how many studs to snap to when moving and scaling bricks
    public int RotationSnap = 15; // how many degrees to snap to when rotating bricks
    public bool CopySuffix = true; // whether or not to add a "- Copy" suffix to the end of copied bricks
    public bool CopyToWorkshopFormat = false; // when copying bricks, should they be copied in a format thats compatible with workshop? enabling this might make copying slightly slower

    // Personalization
    public int CameraSpeed = 24; // how fast the editor camera moves
    public float CameraSensitivity = 4.0f; // how fast the editor camera looks around
    public int FOV = 70; // field of view of the editor camera
    public int ViewDistance = 1000; // editor camera view distance in studs
    public bool Framelimiter = true; // limit the fps to the framelimit
    public int Framelimit = 60; // framelimit
    public bool Autosave = true; // automatically save the map at a specified interval
    public int AutosaveRate = 10; // how often to autosave the map (in minutes)
    public int ScreenshotSizeMultiplier = 1; // supersizes screenshots
    public bool DiscordRP = false; // Discord Rich Presence

    // Color Picker
    public Color[] SavedColors = new Color[8] { new Color(1.0f, 0.2f, 0.2f), new Color(1.0f, 0.5f, 0.2f), new Color(1.0f, 0.85f, 0.2f), new Color(0.22f, 1.0f, 0.2f), new Color(0.2f, 1.0f, 0.9f), new Color(0.2f, 0.47f, 1.0f), new Color(0.6f, 0.2f, 1.0f), new Color(1.0f, 0.2f, 0.9f) }; // saved colorpicker colors

    // Keybinds
    public bool[] EnabledHotkeys = new bool[] {
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true
    };
    public string[] HotkeyPaths = new string[] {
        "<Keyboard>/n",
        "<Keyboard>/c",
        "<Keyboard>/v",
        "<Keyboard>/x",
        "<Keyboard>/d",
        "<Keyboard>/s",
        "<Keyboard>/o",
        "<Keyboard>/1",
        "<Keyboard>/2",
        "<Keyboard>/3",
        "<Keyboard>/a",
        "<Keyboard>/z",
        "<Keyboard>/y",
        "<Keyboard>/g"
    };

    // Advanced
    public float AmbientLight; // ambient light level, increasing will make unlit faces lighter
    public string BrickHillPlayerPath; // path to the brick-hill player.exe, used for test mode
    public string NodeHillServerPath; // path to your node-hill server folder, used for test mode
}

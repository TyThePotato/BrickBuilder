using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestModeManager : MonoBehaviour
{
    public static TestModeManager instance;

    public static string PlayerPath;
    public static string ServerPath;

    public bool running = false;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }

        // attempt to locate player
        string path = LocatePlayer();
        if (path != "") {
            PlayerPath = path;
            if (SettingsManager.Settings.BrickHillPlayerPath == "") {
                SettingsManager.Settings.BrickHillPlayerPath = path;
                SettingsManager.SaveSettings();
            }
        }

        LoadSettings();
    }

    public void LoadSettings () {
        if (PlayerPath == "" || SettingsManager.Settings.BrickHillPlayerPath != "") PlayerPath = SettingsManager.Settings.BrickHillPlayerPath; // load player if not automatically detected already or a path is already set in the settings
        ServerPath = SettingsManager.Settings.NodeHillServerPath;
    }

    public void StartTest () {
        if (RunServer()) {
            RunClient();
        }
    }

    public void StopTest () {
        // close server and client
        running = false;
    }

    private bool RunServer () {
        // start node hill server
        running = true;
        return true;
    }

    private bool RunClient () {
        // start client and join server
        running = true;
        return true;
    }


    private string LocatePlayer() {
        string AppdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        if (File.Exists(AppdataFolder + @"\Brick Hill\Player.exe")) {
            Debug.Log("Found Brick Hill folder at " + AppdataFolder + @"\Brick Hill\Player.exe");
            return AppdataFolder + @"\Brick Hill\Player.exe";
        }
        return "";
    }
}

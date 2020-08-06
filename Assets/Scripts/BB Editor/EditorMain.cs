using QFSW.RichPresence;
using SFB; // Standalone File Browser
using System;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EditorMain : MonoBehaviour
{
    public static EditorMain instance;

    public Map LoadedMap;
    public bool MapIsLoaded = false;
    public UnityEvent MapLoaded;

    public ExtensionFilter[] MapExtensions = new ExtensionFilter[] {
        new ExtensionFilter("Brick-Hill Map", "brk")
    };

    //public Map.MapVersion ExportVersion;

    // Debug
    private Stopwatch mapStopwatch;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }

        SettingsManager.SettingsChanged.AddListener(ApplySettings);
        mapStopwatch = new Stopwatch();
        ApplySettings();
    }

    private void Start() {
        // check command args
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++) {
            try {
                string extension = Path.GetExtension(args[i]).ToLower();
                if (extension == ".brk") {
                    Map chosenMap = MapParser.Parse(args[i]);
                    LoadMap(chosenMap);
                    break;
                }
            } catch (Exception e) {
                // not epic
                UnityEngine.Debug.LogException(e);
                continue;
            }
        }

        // autosave
        InvokeRepeating("Autosave", SettingsManager.Settings.AutosaveRate * 60, SettingsManager.Settings.AutosaveRate * 60);

        //rp
        if (!MapIsLoaded) {
            SetRichPresence("Staring at the main menu", "icon", "BrickBuilder Icon");
        }
    }

    public void ApplySettings () {
        SetFramelimiter();
    }

    public void SetFramelimiter () {
        if (SettingsManager.Settings.Framelimiter) {
            Application.targetFrameRate = SettingsManager.Settings.Framelimit; //Screen.currentResolution.refreshRate;
        } else {
            Application.targetFrameRate = -1;
        }
    }

    // Map Operations

    public void OpenMap (string path = null) {
        if (path == null) {
            // show a file dialog
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Open Map", "", MapExtensions, false);
            // make sure a file was selected
            if (paths.Length > 0) {
                mapStopwatch.Start();
                Map mapToLoad = MapParser.Parse(paths[0]); // load the selected file
                mapStopwatch.Stop();
                UnityEngine.Debug.Log($"Parsed {mapToLoad.Bricks.Count} bricks in " + mapStopwatch.ElapsedMilliseconds + " ms");

                LoadMap(mapToLoad);
            }
        } else {
            mapStopwatch.Start();
            Map mapToLoad = MapParser.Parse(path); // load the selected file
            mapStopwatch.Stop();
            UnityEngine.Debug.Log($"Parsed {mapToLoad.Bricks.Count} bricks in " + mapStopwatch.ElapsedMilliseconds + " ms");

            LoadMap(mapToLoad);
        }
    }

    public void LoadMap (Map map, bool clearHistory = true) {
        if (MapIsLoaded) {
            UnloadMap();
        }

        LoadedMap = map;

        mapStopwatch.Restart();
        MapBuilder.instance.Build(LoadedMap);
        mapStopwatch.Stop();
        UnityEngine.Debug.Log($"Built {LoadedMap.Bricks.Count} bricks in " + mapStopwatch.ElapsedMilliseconds + " ms");

        MapLoaded.Invoke();
        MapIsLoaded = true;

        SetRichPresence($"Building on '{map.Name}'", "icon", "BrickBuilder Icon");
        if (clearHistory) EditorHistory.ClearHistory();

        mapStopwatch.Reset();
    }

    public void UnloadMap () {
        EditorUI.instance.RemoveAllHierarchyElements();
        //LoadedMap.Bricks.Clear();
        Destroy(GameObject.Find("Baseplate")); // destroy the baseplate
        //LoadedMap = null;
        MapIsLoaded = false;

        SetRichPresence("Staring at the main menu", "icon", "BrickBuilder Icon");
    }

    public void SaveMap (string path = null) {
        if (!MapIsLoaded) return;
        if (path == null) {
            // show a file dialog
            string savePath = StandaloneFileBrowser.SaveFilePanel("Save Map", "", LoadedMap.Name, MapExtensions);
            // make sure a file was selected
            if (savePath != "") {
                mapStopwatch.Start();
                string extension = Path.GetExtension(savePath);
                Map.MapVersion mv = Map.MapVersion.BrickBuilder;
                if (extension == ".brk") mv = Map.MapVersion.v2;
                MapExporter.Export(LoadedMap, savePath, mv);
                mapStopwatch.Stop();
                UnityEngine.Debug.Log($"Saved {LoadedMap.Bricks.Count} bricks in " + mapStopwatch.ElapsedMilliseconds + " ms");
            }
        } else {
            mapStopwatch.Start();
            string extension = Path.GetExtension(path);
            Map.MapVersion mv = Map.MapVersion.BrickBuilder;
            if (extension == ".brk") mv = Map.MapVersion.v2;
            MapExporter.Export(LoadedMap, path, mv);
            mapStopwatch.Stop();
            UnityEngine.Debug.Log($"Saved {LoadedMap.Bricks.Count} bricks in " + mapStopwatch.ElapsedMilliseconds + " ms");
        }
    }

    public void NewMap () {
        if (MapIsLoaded) {
            UnloadMap();
        }
        LoadMap(new Map());
    }

    // Autosave

    public void Autosave () {
        if (!MapIsLoaded || !SettingsManager.Settings.Autosave) return;
        string autosavePath = Application.dataPath + "/autosaves/";
        if (!Directory.Exists(autosavePath)) {
            Directory.CreateDirectory(autosavePath);
        }
        string fileName = $"AUTOSAVE " + LoadedMap.Name + ".brk";
        MapExporter.Export(LoadedMap, autosavePath + fileName, Map.MapVersion.BrickBuilder);
    }

    public void ResetAutosaveRate () {
        CancelInvoke("Autosave");
        InvokeRepeating("Autosave", SettingsManager.Settings.AutosaveRate * 60, SettingsManager.Settings.AutosaveRate * 60);
        UnityEngine.Debug.Log("Autosave rate reset");
    }

    // Misc

    public void SetBrickProperties(BrickData b, int ID) {
        Brick br = LoadedMap.MapElements[ID] as Brick; // unsafe, what if this element isn't a brick?
        br.Name = b.Name;
        br.Position = b.Position;
        br.Scale = b.Scale;
        br.Rotation = b.Rotation;
        br.BrickColor = b.BrickColor;
        br.Transparency = b.Transparency;
        br.CollisionEnabled = b.Collision;
        br.Clickable = b.Clickable;
        br.Shape = (Brick.ShapeType)b.Shape;
        br.Model = b.Model;
        br.ScuffedScale = b.Rotation != 0 && b.Rotation != 180;
    }

    public void SetGroupProperties (GroupData g, int ID) {
        BrickGroup gr = LoadedMap.MapElements[ID] as BrickGroup; // unsafe, what if this element isn't a brickgroup?
        gr.Name = g.Name;
    }

    // RP

    public void SetRichPresence (string primaryText, string primaryImage, string primaryImageTooltip) {
        if (RichPresenceComponent.Instance != null) {
            IRichPresence irp = RichPresenceComponent.Instance.RichPresenceModule;
            irp.SetPrimaryText(primaryText);
            irp.SetPrimaryImage(primaryImage);
            irp.SetPrimaryImageTooltip(primaryImageTooltip);

            irp.SetTimestamps((int)DateTimeOffset.Now.ToUnixTimeSeconds(), 0);
        } 
    }

    // Window Stuff
    void OnApplicationFocus (bool hasFocus) {
        if (hasFocus) {
            SetFramelimiter();
        } else {
            Application.targetFrameRate = 2; // 2 is a nice number
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BrickBuilder.Input;
using BrickBuilder.World;
using BrickBuilder.IO;
using BrickBuilder.UI;
using BrickBuilder.User;
using BrickBuilder.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace BrickBuilder
{
    public class EditorMain : MonoBehaviour
    {
        public static Map OpenedMap;
        public static string FilePath = "";

        public static string CacheFolder;

        // ====
        // MAIN
        // ====
        
        private void Awake()
        {
            // initialize cache folder
            CacheFolder = Application.dataPath + "/.cache/";

            if (!Directory.Exists(CacheFolder))
            {
                Directory.CreateDirectory(CacheFolder);
            }
            
            // debug
            //Site.AssetData ad = Site.BrickHillSiteAPI.RetrieveAssetData("256270");
            //Debug.Log(ad);
        }

        // ===============
        // FILE OPERATIONS
        // ===============

        // Clears current map and creates a new one
        public static void NewFile()
        {
            // clear map prior to creating new file
            ClearMap();

            // create map
            OpenedMap = new Map();
            FilePath = "";
 
            // post-load stuff
            InitializeMap();
        }

        // Opens file from path
        public static void OpenFile(string path)
        {
            if (OpenedMap != null)
                ClearMap();
            
            OpenedMap = FileParser.OpenMap(path);
            FilePath = path;
            Debug.Log("Loaded map with " + OpenedMap.Bricks.Count + " bricks.");
            
            InitializeMap();
        }

        public static void SaveFile(string path)
        {
            FilePath = path;

            FileExporter.ExportMap(OpenedMap, path, FileExporter.FileType.BrkV2);
        }

        // ==============
        // MAP OPERATIONS
        // ==============

        // Call when map is loaded
        // Builds map bricks, loads map UI, etc
        private static void InitializeMap()
        {
            MapBuilder.BuildMap(OpenedMap);
            EditorUI.AddHierarchyElements(OpenedMap);
        }

        // Removes all bricks & other map elements
        public static void ClearMap()
        {
            // Remove all bricks
            MapBuilder.RemoveAllBricks(true);

            // Remove all hierarchy elements
            EditorUI.RemoveAllHierarchyElements();
            
            // Clear Inspector
            EditorUI.SetInspector();
            
            // delete map
            OpenedMap = null;
        }
    }
}
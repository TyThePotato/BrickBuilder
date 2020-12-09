using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class FileHistoryManager : MonoBehaviour
{
    public static List<RecentFile> RecentFiles = new List<RecentFile>();

    private static int RecentsLength = 10;
    private static string recentFilesPath;

    private void Awake () {
        recentFilesPath = Application.dataPath + "/recentfiles.txt";
    }

    public static void LoadRecentFiles () {
        if (!File.Exists(recentFilesPath)) return;
        RecentFiles.Clear();
        string[] data = File.ReadAllLines(recentFilesPath);
        for (int i = 0; i < data.Length-1; i+=2) {
            RecentFile f = new RecentFile();
            f.path = data[i];
            f.timestamp = long.Parse(data[i+1]);
            RecentFiles.Add(f);
        }
    }

    public static void SaveRecentFiles () {
        string output = "";
        int recentCount = RecentFiles.Count > RecentsLength ? RecentsLength : RecentFiles.Count;
        for (int i = 0; i < recentCount; i++) { 
            RecentFile f = RecentFiles[RecentFiles.Count - 1 - i];
            output += f.path + "\n" + f.timestamp.ToString(CultureInfo.InvariantCulture);
            if (i != recentCount-1) output += "\n"; // add newlines unless this is the last line
        }

        File.WriteAllText(recentFilesPath, output);
    }

    public static void AddToRecents (string path, bool save = true) {
        // check for duplicates
        for (int i = 0; i < RecentFiles.Count; i++) {
            if (RecentFiles[i].path == path) {
                RecentFiles.RemoveAt(i);
            }
        }

        RecentFile f = new RecentFile();
        f.path = path;
        f.timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        RecentFiles.Add(f);
        
        if (save) SaveRecentFiles();
    }
}

public class RecentFile {
    public string path;
    public long timestamp;
}

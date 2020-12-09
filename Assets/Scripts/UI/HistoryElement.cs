using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryElement : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Timestamp;
    public TMP_Text Path;
    public Button ElementButton;

    public void SetInfo (string path, long time, string name = null) {
        if (name == null) name = System.IO.Path.GetFileName(path);
        string timeString = DateTimeOffset.FromUnixTimeSeconds(time).ToLocalTime().ToString("MMM d yyyy H:mm:ss");

        Name.SetText(name);
        Timestamp.SetText(timeString);
        Path.SetText(path);
    }
}

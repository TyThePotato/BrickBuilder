using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Utils;

public class MapClipboard : MonoBehaviour
{
    public static MapClipboard instance;
    public BrickList Clipboard;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    public void Start() {
        Clipboard = new BrickList();
    }

    public void Copy (Brick[] bricks) {
        Clipboard.bricks = new BrickData[bricks.Length];
        for (int i = 0; i < bricks.Length; i++) {
            BrickData b = new BrickData(bricks[i]);
            if (SettingsManager.Settings.CopySuffix) {
                b.Name = b.Name + " - Copy";
            }
            Clipboard.bricks[i] = b;
        }

        string bricksToCopy = SettingsManager.Settings.CopyToWorkshopFormat ? MapExporter.ExportBRKFromBricklist(Clipboard) : JsonUtility.ToJson(Clipboard);
        Helper.CopyToClipboard(bricksToCopy);
    }

    public BrickList ParseClipboard () {
        BrickList returnList = new BrickList();
        string text = Helper.ReadClipboard();
        if (text.StartsWith("{\"bricks\":")) { // brickbuilder json clipboard format
            returnList = JsonUtility.FromJson<BrickList>(text);
        } else {
            try {
                returnList = MapParser.ParseBRKBricks(text);
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }
        return returnList;
    }
}

[Serializable]
public struct BrickList {
    public BrickData[] bricks;
}

[Serializable]
public struct BrickData {
    public string Name;
    public Vector3 Position;
    public Vector3 Scale;
    public int Rotation;
    public Color BrickColor;
    public float Transparency;
    public bool Collision;
    public bool Clickable;
    public float ClickDist;
    public int Shape;
    public string Model;
    [NonSerialized]
    public int ID; // there is no reason for ID to be serialized

    public BrickData (Brick brick) {
        Name = brick.Name;
        Position = brick.Position;
        Scale = brick.Scale;
        Rotation = brick.Rotation;
        BrickColor = brick.BrickColor;
        Transparency = brick.Transparency;
        Collision = brick.CollisionEnabled;
        Clickable = brick.Clickable;
        ClickDist = brick.ClickDistance;
        Shape = (int)brick.Shape;
        Model = brick.Model;
        ID = brick.ID;
    }

    public Brick ToBrick (bool includeId = false) {
        Brick b = new Brick();
        b.Name = Name;
        b.Position = Position;
        b.Scale = Scale;
        b.Rotation = Rotation;
        b.BrickColor = BrickColor;
        b.Transparency = Transparency;
        b.CollisionEnabled = Collision;
        b.Clickable = Clickable;
        b.ClickDistance = ClickDist;
        b.Shape = (Brick.ShapeType)Shape;
        b.Model = Model;
        if (includeId) b.ID = ID;

        b.ScuffedScale = b.Rotation != 0 && b.Rotation != 180;
        return b;
    }

    // same as the function in Brick.cs
    public void ConvertTransformToUnity () {
        Vector3 pos = Position.SwapYZ() + Scale.SwapYZ() / 2; // Brick-Hill positions are based around some corner of the brick, while Unity positions are based around the pivot point (usually center) of the transform
        pos.x *= -1; // Flip x axis
        Position = pos;

        Scale = Scale.SwapYZ();
        if (Rotation != 0 && Rotation != 180) Scale = Scale.SwapXZ();

        Rotation = Rotation * -1; // Invert rotation
        Rotation = Rotation.Mod(360); // keep rotation between 0-359
    }
}

[Serializable]
public struct GroupData {
    public string Name;
    [NonSerialized]
    public int ID;

    public GroupData (BrickGroup group) {
        Name = group.Name;
        ID = group.ID;
    }

    public BrickGroup ToGroup (bool includeID = false) {
        BrickGroup g = new BrickGroup();
        g.Name = Name;
        if (includeID) g.ID = ID;
        return g;
    }
}

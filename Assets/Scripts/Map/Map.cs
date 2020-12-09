using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Map
{
    // File Variables
    //public string Version = "B R I C K  W O R K S H O P  V0.2.0.0"; // Map version, this shouldn't be changed to keep compatibility with the legacy workshop.
    public string Name = "New Map";

    public Color AmbientColor = Color.black; // Ambient color
    public Color BaseplateColor = new Color(0.2f, 0.509804f, 0.137255f, 1.0f); // Baseplate color
    public Color SkyColor = new Color(0.4862745f, 0.6980392f, 0.8980392f); // Sky color

    public int BaseplateSize = 100; // Size of the baseplate
    public int SunIntensity = 300; // Intensity of the sun light

    public List<Brick> Bricks = new List<Brick>(); // easy access to all the bricks in the map
    public List<BrickGroup> Groups = new List<BrickGroup>(); // easy access to all the groups in the map
    public List<Team> Teams = new List<Team>();

    public Dictionary<int, object> MapElements = new Dictionary<int, object>(); // all elements in the map, accessed by their id
    public int lastID; // last id in map, because ids can get confusing once bricks get deleted and created

    public Map ShallowCopy () {
        return (Map)this.MemberwiseClone();
    }

    public enum ElementType {
        World,
        Brick,
        Group
    }

    public enum MapVersion {
        BrickBuilder,
        v1,
        v2
    }
}

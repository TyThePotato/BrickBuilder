using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrickBuilder.World {
    [Serializable]
    public struct BrickData {
        public Guid ID;
        public string Name;

        public Vector3 Position;
        public Vector3 Scale;
        public UnityEngine.Vector3Int Rotation;

        public Color Color;
        public BrickShape Shape;
        public int Model;

        public bool Collision;

        public BrickData(Brick source) {
            ID = source.ID;
            Name = source.Name;
            Position = source.Position;
            Scale = source.Scale;
            Rotation = source.Rotation;
            Color = source.Color;
            Shape = source.Shape;
            Model = source.Model;
            Collision = source.Collision;
        }
    }

    [Serializable]
    public struct MapData {
        // Map Properties
        public string Name;
        
        // Environment Properties
        public Color AmbientColor;
        public Color BaseplateColor;
        public Color SkyColor;
        public int BaseplateSize;
        public int SunIntensity;

        public MapData(Map source) {
            Name = source.Name;
            AmbientColor = source.AmbientColor;
            BaseplateColor = source.BaseplateColor;
            SkyColor = source.SkyColor;
            BaseplateSize = source.BaseplateSize;
            SunIntensity = source.SunIntensity;
        }
    }
}
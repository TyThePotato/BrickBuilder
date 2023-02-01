using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrickBuilder.World
{
    /// <summary>
    /// Map object. Houses the environment settings along with the bricks.
    /// </summary>
    public class Map
    {
        // Map Properties
        public string Name;
        
        // Environment Properties
        public Color AmbientColor;
        public Color BaseplateColor;
        public Color SkyColor;
        public int BaseplateSize;
        public int SunIntensity;
        
        // Map Contents
        public List<Brick> Bricks;

        /// <summary>
        /// Creates new map
        /// </summary>
        public Map()
        {
            Name = "New Map";

            AmbientColor = Color.black;
            BaseplateColor = new Color(0.1f,1f,0.1f);
            SkyColor = new Color(0.3f, 0.6f, 1f);
            BaseplateSize = 100;
            SunIntensity = 200; // Is this right?

            Bricks = new List<Brick>();
        }

        /// <summary>
        /// Returns the brick with the associated ID, returns null if brick doesn't exist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Brick GetBrick(Guid id)
        {
            // make sure brick list is initalized
            if (Bricks == null) return null;
            
            // iterate through bricks to find matching id
            for (int i = 0; i < Bricks.Count; i++)
            {
                if (Bricks[i].ID == id)
                {
                    return Bricks[i];
                }
            }

            // there are no bricks with matching id
            return null;
        }
    }
}
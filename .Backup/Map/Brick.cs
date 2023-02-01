using System;
using BrickBuilder.Utilities;
using UnityEngine;

namespace BrickBuilder.World
{
    /// <summary>
    /// Contains the properties of an individual brick, such as Position, Scale, Rotation, and Color.
    /// </summary>
    public class Brick
    {
        public Guid ID; // Is this overkill?
        public string Name;

        public Vector3 Position;
        public Vector3 Scale;
        public UnityEngine.Vector3Int Rotation;

        public Color Color;
        public BrickShape Shape;
        public int Model;

        public bool Collision;

        /// <summary>
        /// Creates a brick with random ID and default values
        /// </summary>
        /// <param name="id"></param>
        public Brick()
        {
            ID = Guid.NewGuid();
            Name = "New Brick";
            
            Position = Vector3.zero;
            Scale = Vector3.one;
            Rotation = UnityEngine.Vector3Int.zero;

            Color = Color.white;

            Collision = true;
        }
        
        /// <summary>
        /// Creates a brick with set ID and default values
        /// </summary>
        /// <param name="id"></param>
        public Brick(Guid id)
        {
            ID = id;
            Name = "New Brick";
            
            Position = Vector3.zero;
            Scale = Vector3.one;
            Rotation = UnityEngine.Vector3Int.zero;

            Color = Color.white;

            Collision = true;
        }
        
        /// <summary>
        /// Creates a brick with random ID and predefined values
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="rotation"></param>
        /// <param name="color"></param>
        /// <param name="collision"></param>
        public Brick(string name, Vector3 position, Vector3 scale, UnityEngine.Vector3Int rotation, Color color, bool collision)
        {
            ID = Guid.NewGuid();
            Name = name;
            
            Position = position;
            Scale = scale;
            Rotation = rotation;

            Color = color;

            Collision = collision;
        }

        /// <summary>
        /// Creates a brick with set ID and predefined values
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="rotation"></param>
        /// <param name="color"></param>
        /// <param name="collision"></param>
        public Brick(Guid id, string name, Vector3 position, Vector3 scale, UnityEngine.Vector3Int rotation, Color color, bool collision)
        {
            ID = id;
            Name = name;
            
            Position = position;
            Scale = scale;
            Rotation = rotation;

            Color = color;

            Collision = collision;
        }

        /// <summary>
        /// Converts transform info from BRKv2 to BB/Unity
        /// This includes swapping Y and Z, flipping X, and adjusting values based on pivot point
        /// It also includes using the correct scale depending on rotation (ugh)
        /// </summary>
        public void ConvertFromBRKv2()
        {
            // Firstly, flip/swap all the transform values
            Position = Position.SwapYZ(); // Swap YZ
            Scale = Scale.SwapYZ(); // ^

            Rotation.y = (-Rotation.y).Modulo(360); // Invert rotation
            
            // Next, adjust position based on scale
            Position += Scale / 2f;
            
            // Flip X axis NOW
            Position.x = -Position.x;
            
            // Finally, adjust scale based on rotation (sheesh)
            if (Rotation.y != 0 && Rotation.y != 180)
            {
                Scale = Scale.SwapXZ();
            }
        }

        /// <summary>
        /// Returns a replica of the brick
        /// </summary>
        /// <returns></returns>
        public Brick Clone()
        {
            Brick returnBrick = new Brick(ID, Name, Position, Scale, Rotation, Color, Collision);
            returnBrick.Shape = Shape;
            return returnBrick;
        }
    }

    public enum BrickShape
    {
        cube,
        slope,
        wedge,
        spawnpoint,
        arch,
        corner, // new
        corner_inv, // new
        dome,
        bars,
        flag,
        pole,
        round, // new
        cylinder,
        round_slope,
        vent,
        cone, // new
        sphere, // new
        invalid // new
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace BrickBuilder.Utilities
{
    /// <summary>
    /// Utility class containing many shared functions to help quickly perform certain tasks
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Converts a string to a color
        /// Each color must be separated by a space, and range from 0 to 1
        /// </summary>
        /// <param name="data"></param>
        /// /// <param name="bgr"></param>
        /// <returns></returns>
        public static Color ToColor(this string data, bool bgr = false)
        {
            string[] words = data.Split(' '); // split by spaces
            
            // make sure array is correct length
            if (words.Length != 3 && words.Length != 4)
            {
                throw new FormatException();
            }

            // parse colors
            float r = words[bgr ? 2 : 0].ToFloat();
            float g = words[1].ToFloat();
            float b = words[bgr ? 0 : 2].ToFloat();
            float a = 1.0f;
            if (data.Length == 4)
                a = words[3].ToFloat();

            return new Color(r,g,b,a);
        }

        /// <summary>
        /// Converts a string to a Vector3
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static Vector3 ToVector3(this string data)
        {
            string[] words = data.Split(' '); // split by spaces
            
            // make sure array is correct length
            if (words.Length != 3)
            {
                throw new FormatException();
            }
            
            // parse vector3
            float x = words[0].ToFloat();
            float y = words[1].ToFloat();
            float z = words[2].ToFloat();

            return new Vector3(x, y, z);
        }
        
        /// <summary>
        /// Converts a string to a Vector3Int
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static UnityEngine.Vector3Int ToVector3Int(this string data)
        {
            string[] words = data.Split(' '); // split by spaces
            
            // make sure array is correct length
            if (words.Length != 3)
            {
                throw new FormatException();
            }
            
            // parse vector3
            int x = words[0].ToInt();
            int y = words[1].ToInt();
            int z = words[2].ToInt();

            return new UnityEngine.Vector3Int(x, y, z);
        }
        

        /// <summary>
        /// Parses string to float with InvariantCulture
        /// Neatens code a little
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static float ToFloat(this string data)
        {
            return float.Parse(data, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses string to int with InvariantCulture
        /// Neatens code a little
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int ToInt(this string data)
        {
            return int.Parse(data, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Creates a Vector3 using strings
        /// Automatically converts strings to numbers
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3 Vector3FromStrings(string x, string y, string z)
        {
            return new Vector3(x.ToFloat(), y.ToFloat(), z.ToFloat());
        }
        
        /// <summary>
        /// Creates a Vector3Int using strings
        /// Automatically converts strings to numbers
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static UnityEngine.Vector3Int Vector3IntFromStrings(string x, string y, string z)
        {
            return new UnityEngine.Vector3Int(x.ToInt(), y.ToInt(), z.ToInt());
        }

        /// <summary>
        /// Creates a Color using strings
        /// Automatically converts strings to numbers
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Color ColorFromStrings(string r, string g, string b, string a)
        {
            return new Color(r.ToFloat(), g.ToFloat(), b.ToFloat(), a.ToFloat());
        }

        public static int Modulo(this int input, int value)
        {
            if (input == 0) return 0;
            return (input % value + value) % value;
        }
        
        public static float Modulo(this float input, int value)
        {
            if (input == 0) return 0;
            return (input % value + value) % value;
        }

        public static Vector3 SwapYZ(this Vector3 input)
        {
            return new Vector3(input.x, input.z, input.y);
        }
        
        public static Vector3 SwapXZ(this Vector3 input)
        {
            return new Vector3(input.z, input.y, input.x);
        }

        public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
        {
            return new Vector3(
                value.x > max.x ? max.x : value.x < min.x ? min.x : value.x,
                value.y > max.y ? max.y : value.y < min.y ? min.y : value.y,
                value.z > max.z ? max.z : value.z < min.z ? min.z : value.z
            );
        }
        
        public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
        {
            return new Vector2(
                value.x > max.x ? max.x : value.x < min.x ? min.x : value.x,
                value.y > max.y ? max.y : value.y < min.y ? min.y : value.y
            );
        }

        /// <summary>
        /// for clamping euler angle components since they are goofy
        /// </summary>
        public static float ClampAngle(float angle, float from, float to)
        {
            float remappedAngle = angle > 180f ? -360 + angle : angle; // yep
            return Mathf.Clamp(remappedAngle, from, to);
        }

        /// <summary>
        /// Checks if a vector3 is inside an imaginary box with it's corners set as from and to
        /// </summary>
        public static bool VectorIsWithinBounds(Vector3 value, Vector3 from, Vector3 to)
        {
            // TODO: Benchmark this vs custom implementation
            
            // get center of bounds with lerp
            Vector3 center = Vector3.Lerp(from, to, 0.5f);
            
            // get size by halving dimensions from corner
            Vector3 size = (to - from) / 2f;
            
            // get absolute vector3
            // might not be necessary?
            size.Set(Mathf.Abs(size.x), Mathf.Abs(size.y),Mathf.Abs(size.z));
            
            // create bounds
            Bounds bounds = new Bounds(center, size);

            return bounds.Contains(value);
        }

        public static Vector3 Round(this Vector3 value)
        {
            return new Vector3(
                Mathf.Round(value.x),
                Mathf.Round(value.y),
                Mathf.Round(value.z)
            );
        }
        
        // rounds each component of a vector3 to the nearest whatever
        public static Vector3 Round(this Vector3 vector, float nearest) {
            return new Vector3(
                Mathf.Round(vector.x * nearest) / nearest,
                Mathf.Round(vector.y * nearest) / nearest,
                Mathf.Round(vector.z * nearest) / nearest
            );
        }
        
        public static Vector3 ToBHPosition(this Vector3 value, Vector3 scale, int rotation) {
            Vector3 tempScale = scale;
            rotation = rotation.Modulo(360);
            if (rotation != 0 && rotation != 180)
                tempScale = tempScale.SwapXZ();
            
            return new Vector3(
                value.x * -1f - tempScale.x / 2f,
                value.z - tempScale.z / 2f,
                value.y - tempScale.y / 2f
            );
        }
    }
}
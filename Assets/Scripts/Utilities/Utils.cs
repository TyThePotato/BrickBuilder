using System;
using System.Globalization;
using UnityEngine;

namespace Utils {

    // BrickBuilder specific functions
    public static class BB {
        /// <summary>
        /// Returns a ShapeType that matches the input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Brick.ShapeType GetShape(string input) {
            string lowerInput = input.ToLowerInvariant(); // convert the input to lowercase just to be safe
            if (Enum.TryParse<Brick.ShapeType>(lowerInput, out Brick.ShapeType returnShape)) {
                return returnShape;
            }
            return Brick.ShapeType.cube;
        } 

        public static Vector3 CorrectScale (Vector3 Scale, int Rotation) {
            if (Rotation == 0 || Rotation == 180) {
                return Scale;
            } else {
                return new Vector3(Scale.y, Scale.x, Scale.z);
            }
        }

        public static Vector3 ToBB (this Vector3 input, Vector3 scale) {
            Vector3 bbVector = input.SwapYZ();
            bbVector.x *= -1;
            bbVector -= scale / 2;
            return bbVector;
        }
    }

    // math related functions
    public static class Math {
        public static Vector3 BigVector3 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue); // probably the biggest possible v3 idk

        // swaps Y and Z components of a vector3
        public static Vector3 SwapYZ(this Vector3 fromVector) {
            return new Vector3(fromVector.x, fromVector.z, fromVector.y);
        }

        // swaps X and Y components of a vector3
        public static Vector3 SwapXY(this Vector3 fromVector) {
            return new Vector3(fromVector.y, fromVector.x, fromVector.z);
        }

        // swaps X and Z components of a vector3
        public static Vector3 SwapXZ(this Vector3 fromVector) {
            return new Vector3(fromVector.z, fromVector.y, fromVector.x);
        }

        // modulo
        public static int Mod(this int a, int b) {
            return (a % b + b) % b; // % operator is actually remainder and not modulo which is fine for positive numbers but not negative numbers, so this function is necessary when potentially working with negative numbers
        }

        // rounds each component of a vector3
        public static Vector3 Round(this Vector3 vector) {
            return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
        }

        // rounds each component of a vector3 to the nearest whatever
        public static Vector3 Round(this Vector3 vector, float nearest) {
            return new Vector3(
                Mathf.Round(vector.x * nearest) / nearest,
                Mathf.Round(vector.y * nearest) / nearest,
                Mathf.Round(vector.z * nearest) / nearest
            );
        }

        // rounds each component of a vector3int
        public static Vector3Int RoundToInt(this Vector3 vector) {
            return new Vector3Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
        }

        // floors each component of a vector3
        public static Vector3 Floor(this Vector3 vector) {
            return new Vector3(Mathf.Floor(vector.x), Mathf.Floor(vector.y), Mathf.Floor(vector.z));
        }

        // floors each component of a vector3int
        public static Vector3Int FloorToInt(this Vector3 vector) {
            return new Vector3Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), Mathf.FloorToInt(vector.z));
        }

        // ceils each component of a vector3
        public static Vector3 Ceil(this Vector3 vector) {
            return new Vector3(Mathf.Ceil(vector.x), Mathf.Ceil(vector.y), Mathf.Ceil(vector.z));
        }

        // rounds vector3 components to nearest tenth
        public static Vector3 NearestTenth(this Vector3 vector) {
            return new Vector3(Mathf.Round(vector.x * 10f) / 10.0f, Mathf.Round(vector.y * 10f) / 10.0f, Mathf.Round(vector.z * 10f) / 10.0f);
        }

        // rounds vector3 components to nearest hundredth
        public static Vector3 NearestHundredth(this Vector3 vector) {
            return new Vector3(Mathf.Round(vector.x * 100f) / 100.0f, Mathf.Round(vector.y * 100f) / 100.0f, Mathf.Round(vector.z * 100f) / 100.0f);
        }

        // rounds vector3 components to nearest whatever
        public static float Round(this float num, float nearest) {
            return Mathf.Round(num * nearest) / nearest;
        }

        // clamps vector3 components to respective components of min and max vector3
        public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max) {
            return new Vector3(
                value.x > max.x ? max.x : value.x < min.x ? min.x : value.x,
                value.y > max.y ? max.y : value.y < min.y ? min.y : value.y,
                value.z > max.z ? max.z : value.z < min.z ? min.z : value.z
            );
        }

        public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max) {
            return new Vector2(
                value.x > max.x ? max.x : value.x < min.x ? min.x : value.x,
                value.y > max.y ? max.y : value.y < min.y ? min.y : value.y
            );
        }

        public static Vector3 TransformPointUnscaled(this Transform transform, Vector3 position) {
            Matrix4x4 localToWorldMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            return localToWorldMatrix.MultiplyPoint3x4(position);
        }

        public static Vector3 InverseTransformPointUnscaled(this Transform transform, Vector3 position) {
            Matrix4x4 worldToLocalMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).inverse;
            return worldToLocalMatrix.MultiplyPoint3x4(position);
        }

        public static bool GreaterThan(this Vector3 thisVector, Vector3 otherVector) {
            return thisVector.x > otherVector.x || thisVector.y > otherVector.y || thisVector.z > otherVector.z;
        }

        public static bool LessThan(this Vector3 thisVector, Vector3 otherVector) {
            return thisVector.x < otherVector.x || thisVector.y < otherVector.y || thisVector.z < otherVector.z;
        }
    }

    // functions to help quickly do simple things
    public static class Helper {
        public static Vector3 EightWayFromAngle (int angle) {
            switch (angle) {
                case 0:
                    return new Vector3(0, 0, 1);
                    break;
                case 45:
                    return new Vector3(1, 0, 1);
                    break;
                case 90:
                    return new Vector3(1, 0, 0);
                    break;
                case 135:
                    return new Vector3(1, 0, -1);
                    break;
                case 180:
                    return new Vector3(0, 0, -1);
                    break;
                case 225:
                    return new Vector3(-1, 0, -1);
                    break;
                case 270:
                    return new Vector3(-1, 0, 0);
                    break;
                case 315:
                    return new Vector3(-1, 0, 1);
                    break;
                case 360:
                    return new Vector3(0, 0, 1);
                    break;
            }
            return Vector3.zero;
        }

        public static float[] StringToFloatArray(string input, int amount = 0) {
            string[] separated = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (amount <= 0) { 
                return Array.ConvertAll(separated, i => float.Parse(i, CultureInfo.InvariantCulture));
            } else {
                float[] floatArray = new float[amount];
                for (int i = 0; i < amount; i++) {
                    floatArray[i] = float.Parse(separated[i], CultureInfo.InvariantCulture);
                }
                return floatArray;
            }
        }

        public static float[] StringArrayToFloatArray(string[] input, int amount = 0) {
            if (amount <= 0) {
                return Array.ConvertAll(input, i => float.Parse(i, CultureInfo.InvariantCulture));
            } else {
                float[] floatArray = new float[amount];
                for (int i = 0; i < amount; i++) {
                    floatArray[i] = float.Parse(input[i], CultureInfo.InvariantCulture);
                }
                return floatArray;
            }
        }

        public static Color StringToColor(string input, bool reverse = false) {
            string[] separated = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            float[] converted = Array.ConvertAll(separated, i => float.Parse(i, CultureInfo.InvariantCulture));
            // the reason im not using StringToFloatArray is because its probably faster not to because less function calls, but idk
            if (converted.Length < 3) {
                throw new Exception($"Failed to convert string to color! Expected 3+ arguments, got {converted.Length}.");
            }
            return new Color(converted[reverse ? 2 : 0], converted[1], converted[reverse ? 0 : 2], converted.Length > 3 ? converted[3] : 1f); // looks funky but it's actually fine, just changing which array elements are chosen depending on the "reverse" argument
        }

        public static string JoinSection (this string[] input, int start) {
            string[] sectionedArray = new string[input.Length - start];
            for (int i = 0; i < input.Length; i++) {
                if (i >= start) {
                    sectionedArray[i-start] = input[i];
                }
            }
            return string.Join(" ", sectionedArray);
        }

        public static string RemoveNewlines(this string input) {
            return input.Replace("\r", "").Replace("\n", "");
        }

        public static int ToDecimal(this Color input) {
            // convert color from 0-1 to 0-255
            int r = (int)(input.r * 255);
            int g = (int)(input.g * 255);
            int b = (int)(input.b * 255);
            return (r << 16) + (g << 8) + b;
        }

        public static Color FromDecimal (int input) {
            int r = (input >> 16) & 0xff;
            int g = (input >> 8) & 0xff;
            int b = input & 0xff;
            return new Color(r/255f, g/255f, b/255f); // divide by 255 to convert 0-255 values to 0-1
        }

        public static void CopyToClipboard(this string str) {
            TextEditor textEditor = new TextEditor();
            textEditor.text = str;
            textEditor.SelectAll();
            textEditor.Copy();
        }

        public static string ReadClipboard() {
            TextEditor textEditor = new TextEditor();
            textEditor.multiline = true;
            textEditor.Paste();
            return textEditor.text;
        }
    }

    public static class ExtDebug {
        //Draws just the box at where it is currently hitting.
        public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color) {
            origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
            DrawBox(origin, halfExtents, orientation, color);
        }

        //Draws the full box from start of cast to its end distance. Can also pass in hitInfoDistance instead of full distance
        public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color) {
            direction.Normalize();
            Box bottomBox = new Box(origin, halfExtents, orientation);
            Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

            Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color);
            Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color);
            Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color);
            Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color);
            Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color);
            Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color);
            Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color);
            Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color);

            DrawBox(bottomBox, color);
            DrawBox(topBox, color);
        }

        public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color) {
            DrawBox(new Box(origin, halfExtents, orientation), color);
        }
        public static void DrawBox(Box box, Color color) {
            Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
            Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
            Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
            Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);

            Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
            Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
            Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
            Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);

            Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
            Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
            Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
            Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
        }

        public struct Box {
            public Vector3 localFrontTopLeft { get; private set; }
            public Vector3 localFrontTopRight { get; private set; }
            public Vector3 localFrontBottomLeft { get; private set; }
            public Vector3 localFrontBottomRight { get; private set; }
            public Vector3 localBackTopLeft { get { return -localFrontBottomRight; } }
            public Vector3 localBackTopRight { get { return -localFrontBottomLeft; } }
            public Vector3 localBackBottomLeft { get { return -localFrontTopRight; } }
            public Vector3 localBackBottomRight { get { return -localFrontTopLeft; } }

            public Vector3 frontTopLeft { get { return localFrontTopLeft + origin; } }
            public Vector3 frontTopRight { get { return localFrontTopRight + origin; } }
            public Vector3 frontBottomLeft { get { return localFrontBottomLeft + origin; } }
            public Vector3 frontBottomRight { get { return localFrontBottomRight + origin; } }
            public Vector3 backTopLeft { get { return localBackTopLeft + origin; } }
            public Vector3 backTopRight { get { return localBackTopRight + origin; } }
            public Vector3 backBottomLeft { get { return localBackBottomLeft + origin; } }
            public Vector3 backBottomRight { get { return localBackBottomRight + origin; } }

            public Vector3 origin { get; private set; }

            public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents) {
                Rotate(orientation);
            }
            public Box(Vector3 origin, Vector3 halfExtents) {
                this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
                this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
                this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
                this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

                this.origin = origin;
            }


            public void Rotate(Quaternion orientation) {
                localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
                localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
                localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
                localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
            }
        }

        //This should work for all cast types
        static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance) {
            return origin + (direction.normalized * hitInfoDistance);
        }

        static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation) {
            Vector3 direction = point - pivot;
            return pivot + rotation * direction;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

using BrickBuilder.World;
using BrickBuilder.Utilities;

namespace BrickBuilder.IO {
    public class FileExporter {
        public static void ExportMap(Map map, string path, FileType format) {
            byte[] fileData;
            
            switch (format) {
                case FileType.BrkV2:
                    fileData = ToBrkV2(map);
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            if (fileData == null) {
                return;
            }
            
            // write file data
            File.WriteAllBytes(path, fileData);
        }

        private static byte[] ToBrkV2 (Map map) {
            StringBuilder mapStringBuilder = new StringBuilder();
            
            // write file version
            mapStringBuilder.AppendLine("B R I C K  W O R K S H O P  V0.2.0.0");
            
            mapStringBuilder.AppendLine();
            
            // world info
            mapStringBuilder.AppendLine(ColorToString(map.AmbientColor, false));
            mapStringBuilder.AppendLine(ColorToString(map.BaseplateColor, true));
            mapStringBuilder.AppendLine(ColorToString(map.SkyColor, false));
            mapStringBuilder.AppendLine(map.BaseplateSize.ToString(CultureInfo.InvariantCulture));
            mapStringBuilder.AppendLine(map.SunIntensity.ToString(CultureInfo.InvariantCulture));

            mapStringBuilder.AppendLine();
            
            // bricks
            for (int i = 0; i < map.Bricks.Count; i++) {
                AppendBrick(map.Bricks[i]);
            }
            
            void AppendBrick (Brick brick) {
                // initialize brick
                int adjustedRot = brick.Rotation.y;
                adjustedRot *= -1; // invert
                adjustedRot = adjustedRot.Modulo(360);

                Vector3 adjustedPos = brick.Position.ToBHPosition(brick.Scale, adjustedRot);
                Vector3 adjustedScale = brick.Scale.ToBHScale(adjustedRot);

                string brickInitLine = $"{VectorToString(adjustedPos)} {VectorToString(adjustedScale)} {ColorToString(brick.Color, true)}";
                mapStringBuilder.AppendLine(brickInitLine);
                
                // brick properties
                mapStringBuilder.AppendLine($"\t+NAME {brick.Name}");
                
                if (adjustedRot != 0) 
                    mapStringBuilder.AppendLine($"\t+ROT {adjustedRot}");

                if (brick.Shape != BrickShape.cube)
                    mapStringBuilder.AppendLine($"\t+SHAPE {brick.Shape.ToString()}");

                if (!brick.Collision)
                    mapStringBuilder.AppendLine("\t+NOCOLLISION");

                if (brick.Model != 0)
                    mapStringBuilder.AppendLine($"\t+MODEL {brick.Model}");

            }

            // return bytes
            return Encoding.ASCII.GetBytes(mapStringBuilder.ToString());
        }

        private static string ColorToString(Color color, bool includeAlpha, bool bgr = false) {
            string colorString = bgr ? $"{color.b} {color.g} {color.r}" : $"{color.r} {color.g} {color.b}";
            if (includeAlpha) colorString += $" {color.a}";
            return colorString;
        }

        private static string VectorToString(Vector3 vector) {
            return $"{vector.x} {vector.y} {vector.z}";
        }

        public enum FileType {
            BrkV2,
            BrkV1,
            BrickBuilder
        }
    }
}
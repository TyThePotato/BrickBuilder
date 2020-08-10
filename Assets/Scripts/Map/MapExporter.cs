using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;
using Utils;

public class MapExporter : MonoBehaviour
{
    
    public static void Export (Map map, string path, Map.MapVersion version, bool bbData = true) {
        switch (version) {
            case Map.MapVersion.BrickBuilder:
                //Export02BRK(map, path, true);
                ExportBB(map, path);
                break;
            case Map.MapVersion.v1:
                Export01BRK(map, path);
                break;
            case Map.MapVersion.v2:
                Export02BRK(map, path, bbData);
                break;
        }
    }

    private static void Export02BRK (Map input, string path, bool BrickBuilder = false) {
        string Version = "B R I C K  W O R K S H O P  V0.2.0.0"; // default version
        //if (BrickBuilder) Version = "BBv1";

        using (StreamWriter s = new StreamWriter(path)) {
            s.WriteLine(Version); // write version
            s.WriteLine(); // blank line
            s.WriteLine($"{input.AmbientColor.b.ToString(CultureInfo.InvariantCulture)} {input.AmbientColor.g.ToString(CultureInfo.InvariantCulture)} {input.AmbientColor.r.ToString(CultureInfo.InvariantCulture)}"); // write ambient color
            s.WriteLine($"{input.BaseplateColor.b.ToString(CultureInfo.InvariantCulture)} {input.BaseplateColor.g.ToString(CultureInfo.InvariantCulture)} {input.BaseplateColor.r.ToString(CultureInfo.InvariantCulture)} 1"); // write baseplate color
            s.WriteLine($"{input.SkyColor.r.ToString(CultureInfo.InvariantCulture)} {input.SkyColor.g.ToString(CultureInfo.InvariantCulture)} {input.SkyColor.b.ToString(CultureInfo.InvariantCulture)}"); // write sky color
            s.WriteLine(input.BaseplateSize.ToString(CultureInfo.InvariantCulture)); // baseplate size
            s.WriteLine(input.SunIntensity.ToString(CultureInfo.InvariantCulture)); // sun intensity
            s.WriteLine(); // another blank line

            // now export bricks
            if (BrickBuilder) {
                // export groups and brick
                List<BrickGroup> groupHistory = new List<BrickGroup>();
                for (int i = 0; i < input.Bricks.Count; i++) {
                    if (groupHistory.Count > 0) {
                        // last brick was in a group
                        if (input.Bricks[i].Parent == groupHistory[groupHistory.Count-1]) {
                            // this brick is in the same group
                            s.WriteLine(ExportBrick(input.Bricks[i])); // export brick
                        } else if (input.Bricks[i].Parent == null) {
                            // this brick is not in a group, so we need to end all groups in the history
                            for (int j = 0; j < groupHistory.Count; j++) {
                                s.WriteLine(">ENDGROUP");
                            }
                            groupHistory.Clear();
                            s.WriteLine(ExportBrick(input.Bricks[i])); // export brick
                        } else {
                            // this brick is part of a different group
                            if (groupHistory.Contains(input.Bricks[i].Parent)) {
                                // brick is in a previous group, end all groups until we reach it
                                for (int j = groupHistory.Count-1; j > 0; j--) {
                                    if (groupHistory[j] != input.Bricks[i].Parent) {
                                        s.WriteLine(">ENDGROUP");
                                        groupHistory.RemoveAt(j);
                                    } else {
                                        // we have ended enough groups
                                        break;
                                    }
                                }
                                s.WriteLine(ExportBrick(input.Bricks[i])); // export brick
                            } else {
                                // brick is part of a new group
                                s.WriteLine($">GROUP {input.Bricks[i].Parent.Name}"); // define group
                                groupHistory.Add(input.Bricks[i].Parent); // add group to history
                                s.WriteLine(ExportBrick(input.Bricks[i])); // export brick
                            }
                        }
                    } else {
                        // last brick was not in a group
                        if (input.Bricks[i].Parent != null) {
                            // this brick is in a new group
                            s.WriteLine($">GROUP {input.Bricks[i].Parent.Name}"); // define group
                            groupHistory.Add(input.Bricks[i].Parent); // add group to history
                            s.WriteLine(ExportBrick(input.Bricks[i])); // export brick
                        } else {
                            // this brick isn't in a group either
                            s.WriteLine(ExportBrick(input.Bricks[i])); // export brick
                        }
                    }
                }
            } else {
                // export bricks
                for (int i = 0; i < input.Bricks.Count; i++) {
                    s.WriteLine(ExportBrick(input.Bricks[i]));
                }
            }
        }
    }

    private static void ExportBB(Map input, string path, bool compress = true) {
        string output = "";
        output = $"{ColorUtility.ToHtmlStringRGB(input.AmbientColor)} {ColorUtility.ToHtmlStringRGB(input.BaseplateColor)} {ColorUtility.ToHtmlStringRGB(input.SkyColor)} {input.BaseplateSize.ToString(CultureInfo.InvariantCulture)} {input.SunIntensity.ToString(CultureInfo.InvariantCulture)}";

        // convert groups and brick to string
        List<BrickGroup> groupHistory = new List<BrickGroup>();
        for (int i = 0; i < input.Bricks.Count; i++) {
            if (groupHistory.Count > 0) {
                // last brick was in a group
                if (input.Bricks[i].Parent == groupHistory[groupHistory.Count - 1]) {
                    // this brick is in the same group
                    output += "\n" + ExportBBBrick(input.Bricks[i]); // export brick
                } else if (input.Bricks[i].Parent == null) {
                    // this brick is not in a group, so we need to end all groups in the history
                    for (int j = 0; j < groupHistory.Count; j++) {
                        output += "\n#E";
                    }
                    groupHistory.Clear();
                    output += "\n" + ExportBBBrick(input.Bricks[i]); // export brick
                } else {
                    // this brick is part of a different group
                    if (groupHistory.Contains(input.Bricks[i].Parent)) {
                        // brick is in a previous group, end all groups until we reach it
                        for (int j = groupHistory.Count - 1; j > 0; j--) {
                            if (groupHistory[j] != input.Bricks[i].Parent) {
                                output += "\n#E";
                                groupHistory.RemoveAt(j);
                            } else {
                                // we have ended enough groups
                                break;
                            }
                        }
                        output += "\n" + ExportBBBrick(input.Bricks[i]); // export brick
                    } else {
                        // brick is part of a new group
                        output += $"\n#S {input.Bricks[i].Parent.Name}"; // define group
                        groupHistory.Add(input.Bricks[i].Parent); // add group to history
                        output += "\n" + ExportBBBrick(input.Bricks[i]); // export brick
                    }
                }
            } else {
                // last brick was not in a group
                if (input.Bricks[i].Parent != null) {
                    // this brick is in a new group
                    output += $"\n#S {input.Bricks[i].Parent.Name}"; // define group
                    groupHistory.Add(input.Bricks[i].Parent); // add group to history
                    output += "\n" + ExportBBBrick(input.Bricks[i]); // export brick
                } else {
                    // this brick isn't in a group either
                    output += "\n" + ExportBBBrick(input.Bricks[i]); // export brick
                }
            }
        }

        // compress string and export
        if (compress) {
            byte[] compressed = CLZF2.Compress(Encoding.UTF8.GetBytes(output));
            File.WriteAllBytes(path, compressed);
        } else {
            File.WriteAllText(path, output);
        }
    }

    private static void Export01BRK (Map input, string path) {
        using (StreamWriter s = new StreamWriter(path)) {
            // export environment
            s.WriteLine("[environment]");
            s.WriteLine($"ambient={input.AmbientColor.ToDecimal()}");
            s.WriteLine($"sky={input.SkyColor.ToDecimal()}");
            s.WriteLine($"ground={input.BaseplateColor.ToDecimal()}");

            // export bricks
            s.WriteLine("[bricks]");
            for (int i = 0; i < input.Bricks.Count; i++) {
                Brick target = input.Bricks[i];

                // convert position
                Vector3 convertedPos = new Vector3(target.Position.x + (int)(target.Scale.x / 2), target.Position.y + (int)(target.Scale.y / 2), target.Position.z);

                // write brick
                s.WriteLine($"{target.Name.Replace(" ","")}=\" {convertedPos.x} {convertedPos.y} {convertedPos.z} {target.Scale.x} {target.Scale.y} {target.Scale.z} {target.BrickColor.ToDecimal()} {target.Transparency} 1");
            }

            // it is done
            s.WriteLine("[end]");
        }
    }

    public static string ExportBRKFromBricklist (BrickList bl) {
        string export = "";

        for (int i = 0; i < bl.bricks.Length; i++) {
            export += ExportBrick(bl.bricks[i].ToBrick());
            if (i < bl.bricks.Length - 1) export += "\n";
        }

        return export;
    }

    private static string ExportBrick (Brick b) {
        // this is the long line that defines bricks
        string export = $"{b.Position.x.ToString(CultureInfo.InvariantCulture)} {b.Position.y.ToString(CultureInfo.InvariantCulture)} {b.Position.z.ToString(CultureInfo.InvariantCulture)} {b.Scale.x.ToString(CultureInfo.InvariantCulture)} {b.Scale.y.ToString(CultureInfo.InvariantCulture)} {b.Scale.z.ToString(CultureInfo.InvariantCulture)} {b.BrickColor.r.ToString(CultureInfo.InvariantCulture)} {b.BrickColor.g.ToString(CultureInfo.InvariantCulture)} {b.BrickColor.b.ToString(CultureInfo.InvariantCulture)} {b.Transparency.ToString(CultureInfo.InvariantCulture)}";

        export += $"\n\t+NAME {b.Name.RemoveNewlines()}"; // brick name
        if (b.Rotation != 0) export += $"\n\t+ROT {b.Rotation.ToString(CultureInfo.InvariantCulture)}"; // rotation
        if (b.Shape != Brick.ShapeType.cube) export += $"\n\t+SHAPE {b.Shape.ToString()}"; // shape
        if (!b.CollisionEnabled) export += $"\n\t+NOCOLLISION"; // collision
        if (!string.IsNullOrEmpty(b.Model)) export += $"\n\t+MODEL {b.Model.RemoveNewlines()}"; // model
        if (b.Clickable) export += $"\n\t+CLICKABLE"; // clickable

        return export;
    }

    private static string ExportBBBrick (Brick b) {
        string export = $"!{b.Position.x.ToString(CultureInfo.InvariantCulture)} {b.Position.y.ToString(CultureInfo.InvariantCulture)} {b.Position.z.ToString(CultureInfo.InvariantCulture)} {b.Scale.x.ToString(CultureInfo.InvariantCulture)} {b.Scale.y.ToString(CultureInfo.InvariantCulture)} {b.Scale.z.ToString(CultureInfo.InvariantCulture)} {b.Rotation.ToString(CultureInfo.InvariantCulture)} {ColorUtility.ToHtmlStringRGBA(b.BrickColor)} {((int)b.Shape).ToString("x", CultureInfo.InvariantCulture)} {b.Name}";
        if (!b.CollisionEnabled) export += $"\nNOCOLLISION"; // collision
        if (!string.IsNullOrEmpty(b.Model)) export += $"\nMODEL {b.Model.RemoveNewlines()}"; // model
        if (b.Clickable) export += $"\nCLICKABLE"; // clickable
        return export;
    }
}
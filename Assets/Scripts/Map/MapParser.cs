using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;
using Utils;

public class MapParser : MonoBehaviour
{
    
    public static Map Parse (string path) {
        string extension = Path.GetExtension(path);
        if (extension.ToLower() == ".brk") {
            string version = File.ReadAllLines(path)[0];
            if (version.StartsWith("B")) {
                return Parse02BRK(path);
            } else {
                return Parse01BRK(path);
            }
        } else {
            return ParseBB(path);
        }
    }

    private static Map Parse02BRK (string path) {
        Map map = new Map(); // map that will be outputted
        string name = Path.GetFileNameWithoutExtension(path);
        string input = File.ReadAllText(path);
        string[] lines = input.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries); // split all the lines of the input into separate strings, and remove empty lines

        map.Name = name;

        // the first 7 lines (including whitespace) should be the map info.
        //map.Version = lines[0]; // this is the line that says which version of workshop the map was created with
        map.AmbientColor = Helper.StringToColor(lines[1], true); // for some reason, the ambient color is in BGR format. why?
        map.BaseplateColor = Helper.StringToColor(lines[2], true); // same as above
        map.SkyColor = Helper.StringToColor(lines[3]); // this however, is RGB. wack
        map.BaseplateSize = int.Parse(lines[4], CultureInfo.InvariantCulture);
        map.SunIntensity = int.Parse(lines[5], CultureInfo.InvariantCulture);

        int currentID = 0; // this will be incremented when a brick or group is defined
        List<BrickGroup> startedGroups = new List<BrickGroup>(); // when groups are defined, they are added to this list, and removed when they are ended

        // iterate through lines
        for (int i = 6; i < lines.Length; i++) {
            string line = lines[i].Trim(); // get the line without any leading or trailing whitepsaces for easy parsing
            if (line[0] != '+') {
                // this line is either defining a new brick or something else that isn't a brick property
                if (line.StartsWith(">TEAM")) {
                    // this line is clearly defining a team
                    // TODO: teams
                } else if (line.StartsWith(">SLOT")) {
                    // this line is defining an item, but those are history so ignore them
                    continue;
                } else if (line.StartsWith(">CAMPOS")) {
                    // this line is defining the camera position/rotation. this is created by brickbuilder
                    // TODO
                } else if (line.StartsWith(">SUNROT")) {
                    // this line is defining the sun rotation. this is created by brickbuilder.
                    // TODO
                } else if (line.StartsWith(">GROUP")) {
                    // this line is defining a new group. this is created by brickbuilder.
                    BrickGroup newGroup = new BrickGroup();
                    newGroup.Name = line.Substring(7);
                    newGroup.ID = currentID;

                    if (startedGroups.Count > 0) {
                        startedGroups[startedGroups.Count - 1].Children.Add(currentID); // there is already an opened group, which means this group is a child of that group
                        newGroup.Parent = startedGroups[startedGroups.Count - 1];
                    }

                    startedGroups.Add(newGroup);
                    map.Groups.Add(newGroup);
                    map.MapElements.Add(currentID, newGroup);
                    map.lastID = currentID;
                    currentID++;
                } else if (line.StartsWith(">ENDGROUP")) {
                    // this line is ending the last group. this is created by brickbuilder.
                    // make sure there even is an opened group
                    if (startedGroups.Count > 0) {
                        startedGroups.RemoveAt(startedGroups.Count - 1); // we have reached the end of that group
                    } else {
                        throw new Exception("Nonexistent group ended!");
                    }
                } else if (line[0] != '>') {
                    // this line is most likely defining a new brick
                    Brick brick = new Brick();
                    float[] brickInfo = Helper.StringToFloatArray(line);
                    brick.Position = new Vector3(brickInfo[0], brickInfo[1], brickInfo[2]); // first 3 numbers are position
                    brick.Scale = new Vector3(brickInfo[3], brickInfo[4], brickInfo[5]); // next 3 numbers are scale
                    brick.BrickColor = new Color(brickInfo[6], brickInfo[7], brickInfo[8]); // next 3 numbers are color
                    brick.Transparency = brickInfo[9]; // last number is transparency
                    brick.ID = currentID;

                    if (startedGroups.Count > 0) {
                        startedGroups[startedGroups.Count - 1].Children.Add(currentID); // there is already an opened group, which means this group is a child of that group
                        brick.Parent = startedGroups[startedGroups.Count - 1];
                    }

                    map.Bricks.Add(brick); // adds brick to map object
                    map.MapElements.Add(currentID, brick);
                    map.lastID = currentID;
                    currentID++;
                }
            } else {
                // this line is most likely defining a brick property
                Brick brick = map.Bricks[map.Bricks.Count - 1]; // get the last added brick
                if (line.StartsWith("+NAME")) {
                    // this line is defining the brick name
                    if (line.Length == 5) {
                        // the brick doesn't have a name for some reason
                        brick.Name = "";
                    } else {
                        brick.Name = line.Substring(6);
                    }
                } else if (line.StartsWith("+ROT")) {
                    // this line is defining the brick rotation
                    brick.Rotation = int.Parse(line.Substring(5), CultureInfo.InvariantCulture).Mod(360);
                    if (brick.Rotation == 90 || brick.Rotation == 270) brick.ScuffedScale = true;
                } else if (line.StartsWith("+SHAPE")) {
                    // this line is defining the brick shape
                    brick.Shape = BB.GetShape(line.Substring(7));
                } else if (line.StartsWith("+NOCOLLISION")) {
                    // this line is defining whether or not the brick has collision
                    brick.CollisionEnabled = false;
                } else if (line.StartsWith("+MODEL")) {
                    // this line is defining the custom asset the brick uses
                    brick.Model = line.Substring(7);
                } else if (line.StartsWith("+CLICKABLE")) {
                    // this line is defining whether or not the brick is clickable
                    // i dont even know if this property is used anymore but eh
                    brick.Clickable = true;
                }
            }
        }

        return map;
    }

    private static Map ParseBB (string path) {
        Map map = new Map();
        string name = Path.GetFileNameWithoutExtension(path);
        map.Name = name;
        // first we must decompress the file
        byte[] bytes = CLZF2.Decompress(File.ReadAllBytes(path));
        string decompressed = Encoding.UTF8.GetString(bytes);
        // then split into lines
        string[] lines = decompressed.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        // then parse
        // first environment data
        string[] envData = lines[0].Split(' '); // line 0 is environment data
        ColorUtility.TryParseHtmlString("#" + envData[0], out Color ambientColor);
        ColorUtility.TryParseHtmlString("#" + envData[1], out Color baseplateColor);
        ColorUtility.TryParseHtmlString("#" + envData[2], out Color skyColor);
        map.AmbientColor = ambientColor;
        map.BaseplateColor = baseplateColor;
        map.SkyColor = skyColor;
        map.BaseplateSize = int.Parse(envData[3], CultureInfo.InvariantCulture);
        map.SunIntensity = int.Parse(envData[4], CultureInfo.InvariantCulture);
        // then everything else
        int currentID = 0;
        List<BrickGroup> startedGroups = new List<BrickGroup>();
        for (int i = 1; i < lines.Length; i++) {
            if (lines[i][0] == '!') { // Brick
                Brick b = new Brick();
                string[] data = lines[i].Substring(1).Split(' ');
                float[] fd = Helper.StringArrayToFloatArray(data, 6);
                b.Position = new Vector3(fd[0], fd[1], fd[2]);
                b.Scale = new Vector3(fd[3], fd[4], fd[5]);
                b.Rotation = int.Parse(data[6], CultureInfo.InvariantCulture);
                if (b.Rotation != 0 && b.Rotation != 180) b.ScuffedScale = true;
                ColorUtility.TryParseHtmlString("#" + data[7], out Color brickColor);
                b.BrickColor = brickColor;
                b.Transparency = brickColor.a;
                b.Shape = (Brick.ShapeType)int.Parse(data[8], NumberStyles.HexNumber);
                b.Name = data.JoinSection(9);
                b.ID = currentID;

                if  (startedGroups.Count > 0) {
                    startedGroups[startedGroups.Count - 1].Children.Add(currentID);
                    b.Parent = startedGroups[startedGroups.Count - 1];
                }
                 
                map.Bricks.Add(b);
                map.MapElements.Add(currentID, b);
                map.lastID = currentID;
                currentID++;
            } else if (lines[i][0] == '#') { // Group
                if (lines[i][1] == 'S') { // group start
                    BrickGroup g = new BrickGroup();
                    g.Name = lines[i].Substring(3);
                    g.ID = currentID;
                    if (startedGroups.Count > 0) {
                        startedGroups[startedGroups.Count - 1].Children.Add(currentID); // there is already an opened group, which means this group is a child of that group
                        g.Parent = startedGroups[startedGroups.Count - 1];
                    }
                    startedGroups.Add(g);
                    map.Groups.Add(g);
                    map.MapElements.Add(currentID, g);
                    map.lastID = currentID;
                    currentID++;
                } else if (lines[i][1] == 'E') { // group end
                    if(startedGroups.Count > 0) {
                        startedGroups.RemoveAt(startedGroups.Count - 1);
                    }
                }
            } else if (lines[i][0] == '@') { // Team
                //TODO
            } else if (lines[i][0] == '%') { // BB Info
                if (lines[i][1] == 'C') { // camera transform
                    //TODO
                } else if (lines[i][1] == 'S') { // sun transform
                    //TODO
                }
            } else { // Probably extra brick data
                Brick b = map.Bricks[map.Bricks.Count - 1]; // get last brick
                if (lines[i].StartsWith("NOCOLLISION")) {
                    b.CollisionEnabled = false;
                } else if (lines[i].StartsWith("MODEL")) {
                    b.Model = lines[i].Substring(6);
                } else if (lines[i].StartsWith("CLICKABLE")) {
                    b.Clickable = true;
                }
            }
        }

        return map;
    }

    private static Map Parse01BRK (string path) {
        Map map = new Map(); // map that will be outputted
        string name = Path.GetFileNameWithoutExtension(path);
        string input = File.ReadAllText(path);
        string[] lines = input.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries); // split all the lines of the input into separate strings, and remove empty lines
        int id = 0;

        map.Name = name;

        Header01 currentHeader = Header01.unassigned;

        for (int i = 0; i < lines.Length; i++) {
            if (lines[i] == "[bricks]") {
                // defining bricks
                currentHeader = Header01.bricks;
            } else if (lines[i] == "[environment]") {
                // defining environment
                currentHeader = Header01.environment;
            } else if (lines[i] == "[scripts]") {
                // defining scripts
                currentHeader = Header01.scripts;
            } else if (lines[i] == "[slots]") {
                // defining item
                currentHeader = Header01.slots;
            } else if (lines[i] == "[end]") {
                // end of file
                break;
            } else {
                if (currentHeader == Header01.bricks) {
                    Brick brick = new Brick();

                    bool valid = false;
                    for (int j = 0; j < lines[i].Length - 1; j++) {
                        string cur = lines[i].Substring(j, 2);
                        if (cur == "=\"") {
                            brick.Name = lines[i].Substring(0, j);
                            valid = true;
                            break;
                        }
                    }
                    if (!valid) throw new Exception("Invalid File");

                    string extra = lines[i].Substring(brick.Name.Length + 2); // get everything that isn't the name, so all the numbers
                    float[] properties = Helper.StringToFloatArray(extra); // converts it to an array of floats

                    brick.Position = new Vector3(properties[0] - (int)(properties[3] / 2), properties[1] - (int)(properties[4] / 2), properties[2]);
                    brick.Scale = new Vector3(properties[3], properties[4], properties[5]);

                    int decColor = (int)properties[6];
                    brick.BrickColor = Helper.FromDecimal(decColor);
                    brick.BrickColor.a = properties[7]; //alpha
                    brick.Transparency = properties[7]; // also alpha
                    brick.ID = id;

                    map.Bricks.Add(brick);
                    map.MapElements.Add(id, brick);
                    map.lastID = id;
                    id++;
                } else if (currentHeader == Header01.environment) {
                    if (lines[i].StartsWith("ambient=")) {
                        int color = int.Parse(lines[i].Substring(8));
                        map.AmbientColor = Helper.FromDecimal(color);
                    } else if (lines[i].StartsWith("sky=")) {
                        int color = int.Parse(lines[i].Substring(4));
                        map.SkyColor = Helper.FromDecimal(color);
                    } else if (lines[i].StartsWith("ground=")) {
                        int color = int.Parse(lines[i].Substring(7));
                        map.BaseplateColor = Helper.FromDecimal(color);
                    }
                }
            }
        }

        return map;
    }

    private enum Header01 {
        bricks,
        environment,
        scripts,
        slots,
        unassigned
    }

    // this is designed for pasting stuff copied in the workshop - no group support, no environment settings, only bricks
    public static BrickList ParseBRKBricks (string text) {
        List<BrickData> bricks = new List<BrickData>();
        string[] lines = text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++) {
            string line = lines[i].Trim();
            if (line[0] != '+') {
                // defining brick
                BrickData brick = new BrickData();
                float[] brickInfo = Helper.StringToFloatArray(line);
                brick.Position = new Vector3(brickInfo[0], brickInfo[1], brickInfo[2]);
                brick.Scale = new Vector3(brickInfo[3], brickInfo[4], brickInfo[5]);
                brick.BrickColor = new Color(brickInfo[6], brickInfo[7], brickInfo[8]);
                brick.Transparency = brickInfo[9];

                bricks.Add(brick);
            } else {
                // defining brick properties
                BrickData brick = bricks[bricks.Count-1];
                if (line.StartsWith("+NAME")) {
                    if (line.Length == 5) {
                        // the brick doesn't have a name for some reason
                        brick.Name = "";
                    } else {
                        brick.Name = line.Substring(6);
                    }
                } else if (line.StartsWith("+ROT")) {
                    brick.Rotation = int.Parse(line.Substring(5), CultureInfo.InvariantCulture).Mod(360);
                } else if (line.StartsWith("+SHAPE")) {
                    brick.Shape = (int)BB.GetShape(line.Substring(7));
                } else if (line.StartsWith("+NOCOLLISION")) {
                    brick.Collision = false;
                } else if (line.StartsWith("+MODEL")) {
                    brick.Model = line.Substring(7);
                } else if (line.StartsWith("+CLICKABLE")) {
                    brick.Clickable = true;
                }
                bricks[bricks.Count - 1] = brick; // i guess i gotta do this
            }
        }

        BrickList returnBL = new BrickList();
        returnBL.bricks = bricks.ToArray();
        return returnBL;
    }
}

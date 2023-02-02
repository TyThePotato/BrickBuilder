using System;
using BrickBuilder.World;
using BrickBuilder.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BrickBuilder.IO
{
    public class FileParser
    {

        /// <summary>
        /// Loads and parses a map from the specified path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Map OpenMap(string path)
        {
            // Load file
            string mapData = File.ReadAllText(path);

            Map map;

            // Determine correct parser
            if (path.EndsWith(".bb"))
            {
                // BrickBuilder map file
                map = ParseBB(mapData);
            }
            else
            {
                // Assuming BRK file
                // Determine BRK version
                if (mapData.Trim().StartsWith("B R I C K  W O R K S H O P"))
                {
                    // BRK v2
                    map = ParseBRKv2(mapData);
                }
                else
                {
                    // Assuming BRK v1
                    map = ParseBRKv1(mapData);
                }
            }
            
            return map;
        }
        
        /// <summary>
        /// Parses a string containing BB map data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Map ParseBB(string data)
        {
            Map map = new Map();

            // TODO: parse BB data
            
            return map;
        }
        
        /// <summary>
        /// Parses a string containing BRK v2 map data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Map ParseBRKv2(string data)
        {
            Map map = new Map();

            // split data to lines so we can parse line by line
            string[] lines = data.Split(new string[] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);

            // parse environment data
            map.AmbientColor = lines[1].ToColor();
            map.BaseplateColor = lines[2].ToColor();
            map.SkyColor = lines[3].ToColor();
            map.BaseplateSize = lines[4].ToInt();
            map.SunIntensity = lines[5].ToInt();
            
            // iterate through lines
            for (int i = 6; i < lines.Length; i++)
            {
                string line = lines[i].Trim(); // line without leading/trailing whitespace
                
                // check first character
                if (line[0] == '>')
                {
                    // Extra map data
                    
                    if (line.StartsWith(">TEAM"))
                    {
                        // Defining team
                        // TODO: Teams
                    } else if (line.StartsWith(">CAMPOS"))
                    {
                        // BrickBuilder camera position
                        // TODO: CamPos
                    } else if (line.StartsWith(">SUNPOS"))
                    {
                        // BrickBuilder sun position
                        // TODO: SunPos
                    } else if (line.StartsWith(">SLOT"))
                    {
                        // Item slot, unused.
                        continue;
                    } else if (line.StartsWith(">GROUP")) 
                    {
                        // Defining new group (BrickBuilder data)
                        continue;
                    }
                }
                else
                {
                    // Brick, probably.
                    Brick brick = new Brick();
                    
                    // Brick properties on current line
                    string[] properties = line.Split(' '); // split by spaces
                    
                    brick.Position = Utils.Vector3FromStrings(properties[0], properties[1], properties[2]);
                    brick.Scale = Utils.Vector3FromStrings(properties[3], properties[4], properties[5]);
                    brick.Color =
                        Utils.ColorFromStrings(properties[6], properties[7], properties[8], properties[9]);
                    
                    // Check next lines for extra brick data
                    int iteration = 0;
                    while (true) // !!!
                    {
                        // make sure line exists
                        if (i + iteration + 1 >= lines.Length)
                            break;
                        
                        string nextLine = lines[i + iteration + 1].Trim();

                        if (nextLine[0] == '+')
                        {
                            // Brick property
                            if (nextLine.StartsWith("+NAME"))
                            {
                                // Name
                                brick.Name = nextLine.Substring(6);

                                // Disallow empty names because wack
                                if (string.IsNullOrWhiteSpace(brick.Name))
                                {
                                    brick.Name = "New Brick";
                                }
                            }
                            else if (nextLine.StartsWith("+ROT"))
                            {
                                // Rotation
                                brick.Rotation = Utils.Vector3IntFromStrings("0", nextLine.Substring(5), "0");
                                brick.Rotation.y = brick.Rotation.y.Modulo(360); // keep between 0 and 360
                            }
                            else if (nextLine.StartsWith("+SHAPE"))
                            {
                                // Brick Shape
                                try
                                {
                                    brick.Shape = (BrickShape) Enum.Parse(typeof(BrickShape),
                                        nextLine.Substring(7));
                                }
                                catch (ArgumentException e)
                                {
                                    if (nextLine.Substring(7) == "plate") {
                                        brick.Shape = BrickShape.cube;
                                    }
                                    else {
                                        brick.Shape = BrickShape.invalid;
                                    }
                                }
                            }
                            else if (nextLine.StartsWith("+NOCOLLISION"))
                            {
                                // Brick has no collision
                                brick.Collision = false;
                            }
                            else if (nextLine.StartsWith("+MODEL"))
                            {
                                // Brick has a custom model
                                brick.Model = nextLine.Substring(7).ToInt();
                            }

                            iteration++;
                        }
                        else break; // no more brick properties
                    }
                    
                    // convert brick values from BRKv2 to BB/Unity
                    brick.ConvertFromBRKv2();
                    
                    // add brick to map
                    map.Bricks.Add(brick);
                    
                    // finally increment i by iteration
                    i += iteration;
                }
            }
        
            return map;
        }

        /// <summary>
        /// Parses a string containing BRK v1 map data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Map ParseBRKv1(string data)
        {
            Map map = new Map();
            
            // TODO: parse BRK1 data
            
            return map;
        }
    }
}
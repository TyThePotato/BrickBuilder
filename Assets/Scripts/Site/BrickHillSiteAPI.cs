using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using BrickBuilder.Exceptions;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace BrickBuilder.Site
{
    public class BrickHillSiteAPI
    {
        private static string assetDataAPI = "https://api.brick-hill.com/v1/assets/getPoly/1/";
        private static string assetAPI = "https://api.brick-hill.com/v1/assets/get/";
        
        /// <summary>
        /// Retrieves the type, texture id, and mesh id (if applicable) of the specified asset.
        /// </summary>
        /// <param name="assetID"></param>
        /// <returns></returns>
        /// <exception cref="InvalidAssetException"></exception>
        public static AssetData RetrieveAssetData(string assetID)
        {
            string result = getTextFromURL(assetDataAPI + assetID);
            if (result != "")
            {
                if (result.StartsWith("["))
                {
                    // root element is an array, json.net doesnt like that
                    // enclose array in an object
                    result = "{\"data\":" + result + "}";
                }
                
                // parse json
                JObject o = JObject.Parse(result);
                
                // check if enclosed
                if (o.ContainsKey("data"))
                {
                    o = (JObject)o["data"][0]; // so we can use containskey
                    
                    string type = o["type"].ToString(); // type is pretty much guaranteed to be a key

                    string mesh = "";
                    if (o.ContainsKey("mesh")) mesh = o["mesh"].ToString().Substring(8); // trim "asset://"

                    string texture = "";
                    if (o.ContainsKey("texture")) texture = o["texture"].ToString().Substring(8); // see above

                    return new AssetData(type, texture, mesh);
                }
                // check if error
                else if (o.ContainsKey("error"))
                {
                    throw new InvalidAssetException();
                }
                
            }
            else
            {
                // Failed to get text. Connection error, invalid id?
                throw new InvalidAssetException();
            }

            return new AssetData();
        }
        
        /// <summary>
        /// Retrieves the bytes of the specified asset file. Must be a texture or mesh asset.
        /// </summary>
        /// <param name="assetID"></param>
        /// <returns></returns>
        /// <exception cref="InvalidAssetException"></exception>
        public static byte[] RetrieveAsset(string assetID)
        {
            byte[] returnBytes = getBytesFromURL(assetAPI + assetID);

            if (returnBytes.Length == 0)
            {
                // failed
                throw new InvalidAssetException();
            }

            return returnBytes;
        }
        
        private static string getTextFromURL(string url)
        {
            string returnText = "";

            try
            {
                using (WebClient client = new WebClient())
                {
                    returnText = client.DownloadString(url);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return returnText;
        }

        private static byte[] getBytesFromURL(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    return client.DownloadData(url);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return new byte[0];
        }
    }

    public struct AssetData
    {
        public string Type;
        public string Mesh;
        public string Texture;

        public AssetData(string type = "", string texture = "", string mesh = "")
        {
            Type = type;
            Texture = texture;
            Mesh = mesh;
        }

        public override string ToString()
        {
            return $"{Type}, {Texture}, {Mesh}";
        }
    }
}
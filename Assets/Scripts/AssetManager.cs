using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using BrickBuilder.Site;
using BrickBuilder.Utilities;
using UnityEngine;

namespace BrickBuilder.Assets
{
    public class AssetManager
    {
        private static string[] validModelTypes = new string[]
        {
            "hat",
            "tool"
        };
        
        public static Mesh GetAssetMesh(string assetID)
        {
            string filename = assetID + ".obj";
            
            // check if asset needs to be downloaded
            if (!File.Exists(EditorMain.CacheFolder + filename))
            {
                AssetData asset = BrickHillSiteAPI.RetrieveAssetData(assetID);
                // make sure asset is correct type
                if (validModelTypes.Contains(asset.Type))
                {
                    // real
                    string meshID = asset.Mesh;

                    if (!downloadAsset(meshID, filename))
                    {
                        // failed to download for some reason
                        return null;
                    }
                }
                else
                {
                    // not real
                    return null;
                }
            }
            
            // by this point, asset should be downloaded
            Mesh returnMesh = FastObjImporter.Instance.ImportFile(EditorMain.CacheFolder + filename);
            return returnMesh;
        }

        public static Texture2D GetAssetTexture(string assetID)
        {
            string filename = assetID + ".png";
            
            // check if asset needs to be downloaded
            if (!File.Exists(EditorMain.CacheFolder + filename))
            {
                AssetData asset = BrickHillSiteAPI.RetrieveAssetData(assetID);
                // make sure asset is correct type
                if (validModelTypes.Contains(asset.Type))
                {
                    // real
                    string texID = asset.Mesh;

                    if (!downloadAsset(texID, filename))
                    {
                        // failed to download for some reason
                        return null;
                    }
                }
                else
                {
                    // not real
                    return null;
                }
            }
            
            // by this point, asset should be downloaded
            // load file
            byte[] fileBytes = File.ReadAllBytes(EditorMain.CacheFolder + filename);
            
            // create texture
            Texture2D returnTex = new Texture2D(256, 256);
            returnTex.LoadImage(fileBytes);
            
            return returnTex;
        }

        private static bool downloadAsset(string id, string filename)
        {
            bool result = false;

            try
            {
                byte[] bytes = BrickHillSiteAPI.RetrieveAsset(id);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return result;
        }
    }
}
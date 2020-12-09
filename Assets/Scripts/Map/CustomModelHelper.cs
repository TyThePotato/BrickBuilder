using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class CustomModelHelper : MonoBehaviour
{
    public static CustomModelHelper instance;
    private static string cacheFolder;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }

        cacheFolder = Application.dataPath + "/cache/";
        Directory.CreateDirectory(cacheFolder); // create cache folder if it doesn't exist
    }

    public static bool IsValidID (string id) {
        if (String.IsNullOrWhiteSpace(id) || !Regex.IsMatch(id, "^[0-9]+$")) return false; // immediately return false if the id is empty or not a number
        string textureURL = GetAssetURL(id, true);
        string content = ReadURL(textureURL);
        return !string.IsNullOrEmpty(content) && content != "{\"error\":\"Record not found\"}" && !content.Contains("404 Not Found");
    }

    public static void SetCustomModel(BrickShape bs, string ModelID) {
        instance.StartCoroutine(GetModel(ModelID, (mesh) => {
            if (mesh != null) {
                instance.StartCoroutine(GetTexture(ModelID, (texture) => {
                    if (texture != null) {
                        bs.SetAssetGameobject(mesh, texture);
                    }
                }));
            }
        }));
    }

    static IEnumerator GetModel(string id, System.Action<Mesh> callback) {
        Mesh returnMesh = null;
        if (File.Exists(cacheFolder + id + ".obj")) {
            // load model from cache
            returnMesh = FastObjImporter.Instance.ImportFile(cacheFolder + id + ".obj");
        } else {
            // download model
            using (UnityWebRequest w = UnityWebRequest.Get(GetAssetURL(id, true))) {
                yield return w.SendWebRequest();
                if (!w.isNetworkError && !w.isHttpError) {
                    // loaded page, write obj file
                    File.WriteAllBytes(cacheFolder + id + ".obj", w.downloadHandler.data);
                    returnMesh = FastObjImporter.Instance.ImportFile(cacheFolder + id + ".obj");
                }
            }
        }
        callback(returnMesh);
    }

    static IEnumerator GetTexture(string id, Action<Texture2D> callback) {
        Texture2D returnTexture = null;
        if (File.Exists(cacheFolder + id + ".png")) {
            // load texture from cache
            returnTexture = new Texture2D(256, 256);
            returnTexture.LoadImage(File.ReadAllBytes(cacheFolder + id + ".png"));
        } else {
            // download textyure
            using (UnityWebRequest w = UnityWebRequest.Get(GetAssetURL(id, false))) {
                yield return w.SendWebRequest();
                if (!w.isNetworkError && !w.isHttpError) {
                    // loaded page, write png file
                    File.WriteAllBytes(cacheFolder + id + ".png", w.downloadHandler.data);
                    returnTexture = new Texture2D(256, 256);
                    returnTexture.LoadImage(File.ReadAllBytes(cacheFolder + id + ".png"));
                }
            }
        }
        callback(returnTexture);
    }

    public static string GetAssetURL (string id, bool model) {
        string assetType = model ? "obj" : "png";
        return $"https://api.brick-hill.com/v1/games/retrieveAsset?id={id}&type={assetType}";
    }

    // the reason i use webclient instead of unitywebrequest is that im too lazy to rework my code to support coroutines or whatever 
    public static string ReadURL (string url) {
        try {
            using (WebClient w = new WebClient()) {
                string s = w.DownloadString(url);
                return s;
            }
        } catch (Exception e) {
            Debug.LogException(e);
            return "";
        }
    }
}

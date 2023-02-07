using System;
using System.Collections;
using System.Collections.Generic;
using BrickBuilder.Rendering;
using BrickBuilder.User;
using UnityEngine;
using UnityEngine.Rendering;

namespace BrickBuilder.World
{
    /// <summary>
    /// Handles the building/rendering of bricks and other map content
    /// </summary>
    public class MapBuilder : MonoBehaviour
    {
        public static MapBuilder instance;

        public Mesh BaseplateMesh;
        
        public static GameObject BaseplateGO;
        public static Dictionary<Guid, GameObject> BrickGOs = new Dictionary<Guid, GameObject>();

        //public static int BricksPerChunk = 100;
        //public static List<Chunk> Chunks = new List<Chunk>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public static void BuildMap(Map map)
        {
            SetEnvironment(map);
            
            // Build baseplate
            BaseplateGO = new GameObject();
            BaseplateGO.name = "Baseplate";
            BaseplateGO.layer = 6;
            
            Mesh baseplateMesh = BrickMeshGenerator.CreatePlane(map.BaseplateSize, map.BaseplateColor);
            BaseplateGO.AddComponent<MeshFilter>().mesh = baseplateMesh;
            BaseplateGO.AddComponent<MeshCollider>().sharedMesh = baseplateMesh;
            
            BaseplateGO.AddComponent<MeshRenderer>().material = MaterialCache.GetMaterial(
                MaterialCache.FaceType.Stud, false);
            
            // Build bricks
            foreach (Brick b in map.Bricks) {
                BuildBrick(b);
            }
        }

        public static GameObject BuildBrick(Brick brick) {
            GameObject go = new GameObject(brick.Name);
            go.transform.position = brick.Position;
            go.layer = 6;

            // mesh
            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.mesh = BrickMeshGenerator.CreateMesh(brick);
            go.AddComponent<MeshCollider>().sharedMesh = mf.sharedMesh;
            
            // rendering
            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            mr.materials = GetBrickMaterials(brick);
            
            BrickGOs.Add(brick.ID, go);
            return go;
        }

        public static void UpdateBrick(Guid id) {
            // just wipe and rebuild lol
            Brick targetBrick = EditorMain.OpenedMap.GetBrick(id);
            if (targetBrick == null) return; // TODO: Exception?
            
            RemoveBrick(id);
            BuildBrick(targetBrick);
        }

        public static void SetBrickGlow(Brick brick, bool glow) {
            if (!BrickGOs.ContainsKey(brick.ID)) return; // TODO: Exception?
            
            MeshRenderer mr = BrickGOs[brick.ID].GetComponent<MeshRenderer>();
            mr.materials = GetBrickMaterials(brick, glow);
        }

        public static void RemoveBrick(Guid id) {
            if (!BrickGOs.ContainsKey(id)) {
                Debug.Log("Tried to delete invalid brick??");
                return;
            }
            
            GameObject go = BrickGOs[id];
            Destroy(go);
            BrickGOs.Remove(id);

            // Does mesh need to be disposed?
        }

        public static void RemoveAllBricks(bool includeBaseplate = false) {
            foreach (GameObject go in BrickGOs.Values) {
                Destroy(go);
                // Do meshes need to be disposed?
            }
            BrickGOs.Clear();

            if (includeBaseplate) {
                Destroy(BaseplateGO);
            }
        }

        public static void SetEnvironment(Map map)
        {
            // Ambient
            // TODO
            
            // Baseplate
            // TODO
            
            // Sky
            EditorCamera.Camera.backgroundColor = map.SkyColor;
        }

        public static Material[] GetBrickMaterials(Brick brick, bool glow = false) {
            int materialCount = 3;
            
            // determine face types
            MaterialCache.FaceType slot1 = MaterialCache.FaceType.Smooth;
            MaterialCache.FaceType slot2 = MaterialCache.FaceType.Stud;
            MaterialCache.FaceType slot3 = MaterialCache.FaceType.Inlet;
            MaterialCache.FaceType slot4 = MaterialCache.FaceType.Grain;

            switch (brick.Shape) {
                case BrickShape.slope:
                    materialCount = 4;
                    break;
                case BrickShape.dome:
                    materialCount = 1;
                    break;
                case BrickShape.flag:
                    materialCount = 1;
                    break;
                case BrickShape.pole:
                    materialCount = 1;
                    break;
                case BrickShape.cylinder:
                    materialCount = 1;
                    break;
                case BrickShape.round_slope:
                    materialCount = 2;
                    slot2 = MaterialCache.FaceType.Inlet;
                    break;
                case BrickShape.vent:
                    materialCount = 2;
                    slot2 = MaterialCache.FaceType.Inlet;
                    break;
                case BrickShape.cone:
                    materialCount = 1;
                    break;
                case BrickShape.spawnpoint:
                    slot2 = MaterialCache.FaceType.Spawnpoint;
                    break;
                case BrickShape.sphere:
                    materialCount = 1;
                    break;
                case BrickShape.invalid:
                    materialCount = 3;
                    slot1 = MaterialCache.FaceType.Grid;
                    slot2 = MaterialCache.FaceType.Grid;
                    slot3 = MaterialCache.FaceType.Grid;
                    break;
            }
            
            Material[] materials = new Material[materialCount];
            bool transparent = brick.isTransparent;
            
            // populate array
            materials[0] = MaterialCache.GetMaterial(slot1, transparent, glow);
            if (materialCount > 1) materials[1] = MaterialCache.GetMaterial(slot2, transparent, glow);
            if (materialCount > 2) materials[2] = MaterialCache.GetMaterial(slot3, transparent, glow);
            if (materialCount > 3) materials[3] = MaterialCache.GetMaterial(slot4, transparent, glow);
            
            return materials;
        }
    }
}
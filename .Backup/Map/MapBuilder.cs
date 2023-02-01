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
        //public static Dictionary<Guid, GameObject> BrickGOs = new Dictionary<Guid, GameObject>();

        public static int BricksPerChunk = 100;
        public static List<Chunk> Chunks = new List<Chunk>();

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

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F))
            {
                foreach (Chunk ch in Chunks)
                {
                    RegenerateChunk(ch, EditorMain.OpenedMap.Bricks);
                }
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
                MaterialCache.FaceType.Stud,
                new Vector2(map.BaseplateSize, map.BaseplateSize),
                false,
                false);
            
            // Build bricks
            Chunks.AddRange(MeshBricks(map.Bricks, Chunks, true));
        }

        public static List<Chunk> MeshBricks(List<Brick> bricks, List<Chunk> chunks, bool onlyReturnNewChunks = false, bool onlyUseNewChunks = false)
        {
            // separate opaque and transparent bricks
            List<Brick> opaqueBricks = new List<Brick>();
            List<Brick> transparentBricks = new List<Brick>();

            for (int i = 0; i < bricks.Count; i++)
            {
                if (bricks[i].Color.a == 1f)
                {
                    // opaque
                    opaqueBricks.Add(bricks[i]);
                }
                else
                {
                    // opaquen't
                    transparentBricks.Add(bricks[i]);
                }
            }

            // run all this code twice; once for opaque bricks and again for transparent bricks
            List<Chunk> modifiedChunks = new List<Chunk>();
            for (int i = 0; i < 2; i++)
            {
                bool alpha = i == 1;
                Brick[] iterationBricks = alpha ? transparentBricks.ToArray() : opaqueBricks.ToArray();
                
                // loop to mesh bricks into separate chunks when necessary
                int meshedBricks = 0;
                while (meshedBricks < iterationBricks.Length)
                {
                    // determine number of bricks to mesh
                    int bricksToMesh = iterationBricks.Length - meshedBricks;
                    if (bricksToMesh > BricksPerChunk)
                    {
                        bricksToMesh = BricksPerChunk;
                    }

                    // determine chunk to use
                    Chunk chunkToUse = null;
                    if (!onlyUseNewChunks)
                    {
                        for (int j = 0; j < chunks.Count; j++)
                        {
                            // first make sure chunk is compatible
                            if (chunks[j].Alpha != alpha) continue;

                            // then make sure chunk isn't locked
                            if (chunks[i].Locked) continue;

                            int availableSpace = BricksPerChunk - chunks[j].Bricks.Count;

                            // make sure chunk has space
                            if (availableSpace > 0)
                            {
                                // only mesh enough bricks to not overflow chunk
                                if (bricksToMesh > availableSpace)
                                {
                                    bricksToMesh = availableSpace;
                                }

                                chunkToUse = chunks[j];
                                if (!onlyReturnNewChunks && !modifiedChunks.Contains(chunkToUse))
                                    modifiedChunks.Add(chunkToUse);
                                
                                break;
                            }
                        }
                    }

                    // create new chunk if necessary
                    if (chunkToUse == null)
                    {
                        chunkToUse = new Chunk();
                        chunkToUse.Alpha = alpha;

                        // create chunk mesh
                        Mesh mesh = new Mesh();
                        mesh.indexFormat = IndexFormat.UInt32;
                        mesh.subMeshCount = 4;
                        chunkToUse.Mesh = mesh;

                        // create chunk gameobject
                        string namePrefix = alpha ? "Alpha " : "Opaque ";
                        GameObject go = new GameObject(namePrefix + "Chunk");
                        go.layer = 6;
                        go.AddComponent<MeshFilter>().sharedMesh = mesh;
                        go.AddComponent<MeshCollider>().sharedMesh = mesh;

                        Material[] materials = new[]
                        {
                            MaterialCache.GetMaterial(MaterialCache.FaceType.Smooth, Vector2.one, alpha, false),
                            MaterialCache.GetMaterial(MaterialCache.FaceType.Stud, Vector2.one, alpha, false),
                            MaterialCache.GetMaterial(MaterialCache.FaceType.Inlet, Vector2.one, alpha, false),
                            MaterialCache.GetMaterial(MaterialCache.FaceType.Spawnpoint, Vector2.one, alpha, false)
                        };

                        go.AddComponent<MeshRenderer>().materials = materials;
                        chunkToUse.GameObject = go;
                        
                        modifiedChunks.Add(chunkToUse);
                    }

                    // get array of bricks to mesh
                    Brick[] brickSubArray = new Brick[bricksToMesh];
                    Array.Copy(iterationBricks, meshedBricks, brickSubArray, 0, bricksToMesh);

                    // add bricks to chunk
                    for (int j = 0; j < brickSubArray.Length; j++)
                    {
                        chunkToUse.Bricks.Add(brickSubArray[j].ID);
                    }

                    // mesh chunk
                    BrickMeshGenerator.AppendBricks(chunkToUse.Mesh, brickSubArray);
                    
                    // reset chunk mesh collider
                    chunkToUse.GameObject.GetComponent<MeshCollider>().sharedMesh = chunkToUse.Mesh;

                    // increment meshedBricks
                    meshedBricks += bricksToMesh;
                }
            }
            
            return modifiedChunks;
        }

        // "separates" bricks from their chunks and adds them to new chunks
        // the bricks technically remain in their original chunks, they just get removed from the mesh
        public static List<Chunk> SeparateBricks(List<Brick> bricks, List<Chunk> existingChunks = null)
        {
            List<Brick> bricksToMesh = new List<Brick>();

            // regen chunk meshes without the specified bricks
            for (int i = 0; i < Chunks.Count; i++)
            {
                List<Guid> removedBricks = new List<Guid>();
                for (int j = 0; j < bricks.Count; j++)
                {
                    // if this is the first chunk iteration, might as well also use this brick loop
                    // to determine which bricks have not already been meshed (if existingChunks != null)
                    if (i == 0)
                    {
                        if (existingChunks != null)
                        {
                            bool alreadyMeshed = false;
                            for (int k = 0; k < existingChunks.Count; k++)
                            {
                                if (existingChunks[k].Bricks.Contains(bricks[j].ID))
                                {
                                    alreadyMeshed = true;
                                    break;
                                }
                            }

                            if (!alreadyMeshed)
                            {
                                bricksToMesh.Add(bricks[j]);
                            }
                        }
                    }
                    
                    // check if chunk contains a specified brick
                    if (Chunks[i].Bricks.Contains(bricks[j].ID))
                    {
                        // temporarily remove brick from chunk
                        Chunks[i].Bricks.Remove(bricks[j].ID);
                        removedBricks.Add(bricks[j].ID);
                    }
                }
                
                // regenerate chunk if necessary
                if (removedBricks.Count > 0)
                {
                    RegenerateChunk(Chunks[i], EditorMain.OpenedMap.Bricks);
                    Chunks[i].Bricks.AddRange(removedBricks);
                }
            }

            // then generate separate chunks containing specified bricks
            List<Chunk> separatedChunks = MeshBricks(bricksToMesh, existingChunks, true, existingChunks == null);
            return separatedChunks;
        }

        public static void RegenerateChunk(Chunk chunk, List<Brick> bricks)
        {
            // delete chunk if empty
            if (chunk.Bricks.Count == 0) {
                // remove chunk from chunk list if it exists there (non selected chunks)
                if (Chunks.Contains(chunk)) {
                    Chunks.Remove(chunk);
                }
                
                // delete chunk data
                Destroy(chunk.GameObject);
                Destroy(chunk.Mesh);
                
                // does anything else need to be disposed?
                return;
            }
            
            // determine bricks that belong in chunk
            Brick[] chunkBricks = new Brick[chunk.Bricks.Count];
            int addedBricks = 0;
            
            for (int i = 0; i < bricks.Count; i++)
            {
                if (chunk.Bricks.Contains(bricks[i].ID))
                {
                    chunkBricks[addedBricks] = bricks[i];
                    addedBricks++;
                }
            }

            // warn if chunk brick mismatch or whatever
            if (addedBricks != chunkBricks.Length)
            {
                Debug.LogWarning("WARNING: Couldn't find one or more bricks for chunk.");
            }
            
            // overwrite chunk mesh
            chunk.Mesh.Clear();
            chunk.Mesh.indexFormat = IndexFormat.UInt32;
            chunk.Mesh.subMeshCount = 4;
            BrickMeshGenerator.AppendBricks(chunk.Mesh, chunkBricks);
            
            // reset chunk collider
            chunk.GameObject.GetComponent<MeshCollider>().sharedMesh = chunk.Mesh;
        }

        public static void RemoveBrick(Brick brick)
        {
            RemoveBricks(new Brick[] { brick });
        }

        public static void RemoveBricks(Brick[] bricks)
        {
            // keep track of modified chunks so they can be regenerated afterwards
            List<Chunk> modifiedChunks = new List<Chunk>();
            
            // first remove bricks from their chunks
            for (int i = 0; i < Chunks.Count; i++)
            {
                for (int j = 0; j < bricks.Length; j++)
                {
                    if (Chunks[i].Bricks.Contains(bricks[j].ID))
                    {
                        // remove brick from chunk
                        Chunks[i].Bricks.Remove(bricks[j].ID);
                        
                        // mark for regeneration
                        modifiedChunks.Add(Chunks[i]);
                    }
                }
            }

            // next regenerate or delete modified chunks as necessary
            for (int i = 0; i < modifiedChunks.Count; i++)
            {
                if (modifiedChunks.Count > 0)
                {
                    // regenerate chunk
                    RegenerateChunk(modifiedChunks[i], EditorMain.OpenedMap.Bricks);
                }
                else
                {
                    // delete empty chunk
                    Destroy(modifiedChunks[i].GameObject);
                    
                    if (Chunks.Contains(modifiedChunks[i]))
                        Chunks.Remove(modifiedChunks[i]);
                    
                    // Do we need to dispose of any other chunk stuff?
                }
            }
        }

        public static void RemoveAllBricks()
        {
            for (int i = 0; i < Chunks.Count; i++)
            {
                Destroy(Chunks[i].GameObject);
                Chunks[i].Mesh.Clear();
            }
            
            Chunks.Clear();
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
    }
}
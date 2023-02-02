using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrickBuilder.Rendering
{
    public class MaterialCache : MonoBehaviour
    {
        static MaterialCache instance;
        
        public Texture StudTexture;
        public Texture InletTexture;
        public Texture SpawnpointTexture;
        public Texture GridTexture;
        public Texture GrainTexture;

        public Shader OpaqueShader;
        public Shader TransparentShader;

        // bertha
        private static readonly Dictionary<(FaceType, bool), Material> MaterialDictionary =
            new Dictionary<(FaceType, bool), Material>();

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

        /// <summary>
        /// Checks material cache for matching material. If it doesn't exist, it makes it.
        /// </summary>
        /// <param name="face"></param>
        /// <param name="transparent"></param>
        /// <param name="glow"></param>
        /// <returns></returns>
        public static Material GetMaterial(FaceType face, bool transparent, bool glow = false)
        {
            // Check cache (if not glow material)
            if (!glow && MaterialDictionary.TryGetValue((face, transparent), out Material match))
            {
                return match;
            }

            // No match, create material
            Material mat = new Material(transparent ? instance.TransparentShader : instance.OpaqueShader);

            // Set Texture
            Texture materialTexture = face switch {
                FaceType.Stud => instance.StudTexture,
                FaceType.Inlet => instance.InletTexture,
                FaceType.Spawnpoint => instance.SpawnpointTexture,
                FaceType.Grid => instance.GridTexture,
                FaceType.Grain => instance.GrainTexture,
                _ => null
            };

            mat.SetTexture("_Texture", materialTexture);
            
            // Set Tile
            //mat.SetVector("_Tiling", tile);
            
            // Make glow
            mat.SetFloat("_Glow", glow ? 1f : 0f);
            
            // Add to cache then return
            mat.name = (materialTexture ? materialTexture.name : "Smooth") + "_" + transparent.ToString() + "_" + glow.ToString();
            
            if (!glow)
                MaterialDictionary.Add((face, transparent), mat);
            return mat;
        }

        public enum FaceType
        {
            Smooth,
            Stud,
            Inlet,
            Spawnpoint,
            Grid,
            Grain
        }
    }
}
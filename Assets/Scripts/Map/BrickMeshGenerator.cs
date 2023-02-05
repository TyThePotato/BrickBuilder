using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace BrickBuilder.World
{
    public class BrickMeshGenerator : MonoBehaviour
    {
        public static BrickMeshGenerator instance;

        // TODO: more cube combinations
        // TODO: adjust shape uvs
        
        public Mesh Arch; // Archway interior
        public Mesh BarBase; // Bottom segment of bars
        public Mesh Cone;
        public Mesh CornerBack;
        public Mesh CornerBackInverted;
        public Mesh CornerSlope;
        public Mesh CornerSlopeInverted;
        public Mesh CornerSide;
        public Mesh CornerSideInverted;
        public Mesh Cube; // Full 6-Sided Cube
        public Mesh CubeFbk; // Cube WITH ONLY front and back faces
        public Mesh CubeLR; // Cube WITH ONLY left and right faces
        public Mesh CubeT; // Cube without top face
        public Mesh CubeTB; // Cube WITH ONLY top and bottom faces
        public Mesh CubeB; // Cube without bottom face
        public Mesh Cylinder; // Cylinder with all faces
        public Mesh CylinderT; // Cylinder without top face
        public Mesh CylinderTB; // Cylinder without top or bottom face
        public Mesh Dome;
        public Mesh DomeTop;
        public Mesh Flag;
        public Mesh Plane;
        public Mesh RoundedSlope;
        public Mesh RoundedWedge;
        public Mesh Slope;
        public Mesh Sphere;
        public Mesh Vent; // Sus!
        public Mesh Wedge;
        
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

        // Creates a mesh using brick values (shape, color, scale, etc)
        public static Mesh CreateMesh(Brick source) {
            Mesh returnMesh = source.Shape switch {
                BrickShape.arch => ArchMesh(source),
                BrickShape.bars => BarsMesh(source),
                BrickShape.cone => ConeMesh(source),
                BrickShape.corner => CornerMesh(source),
                BrickShape.corner_inv => InvertedCornerMesh(source),
                BrickShape.cube => CubeMesh(source),
                BrickShape.cylinder => CylinderMesh(source),
                BrickShape.dome => DomeMesh(source),
                BrickShape.flag => FlagMesh(source),
                BrickShape.invalid => GridMesh(source),
                BrickShape.pole => PoleMesh(source),
                BrickShape.round => RoundWedgeMesh(source),
                BrickShape.round_slope => RoundSlopeMesh(source),
                BrickShape.slope => SlopeMesh(source),
                BrickShape.spawnpoint => SpawnpointMesh(source),
                BrickShape.sphere => SphereMesh(source),
                BrickShape.vent => VentMesh(source),
                BrickShape.wedge => WedgeMesh(source),
                _ => GridMesh(source)
            };

            ColorMesh(returnMesh, source.Color);
            returnMesh.RecalculateBounds(); // Should this be done during mesh generation?
            
            return returnMesh;
        }
        
        private static Mesh CubeMesh(Brick source, bool fullUVTop = false, bool fullUVSides = false)
        {
            Mesh mesh = Instantiate(instance.Cube);

            TransformMesh(mesh, Vector3.zero, source.Scale, source.Rotation);
            ScaleUVs(mesh, source.Scale.x, source.Scale.z);

            return mesh;
        }

        private static Mesh SlopeMesh(Brick source) {
            Mesh slopeBase = Instantiate(instance.CubeT);
            Mesh slopeBack = Instantiate(instance.CubeB);
            Mesh slope = Instantiate(instance.Slope);
            
            TransformMesh(slopeBase, 
                new Vector3(0f, -source.Scale.y / 2f + 0.15f, 0f),
                new Vector3(source.Scale.x, 0.3f, source.Scale.z),
                source.Rotation
            );
            ScaleUVs(slopeBase, source.Scale.x, source.Scale.z);
            
            TransformMesh(slopeBack, 
                new Vector3(-source.Scale.x / 2f + 0.5f, 0f, 0f),
                new Vector3(1f, source.Scale.y, source.Scale.z),
                source.Rotation
            );
            ScaleUVs(slopeBack, 1f, source.Scale.z);
            
            TransformMesh(slope, 
                new Vector3(0.5f, 0.15f, 0f),
                new Vector3(source.Scale.x - 1f, source.Scale.y - 0.3f, source.Scale.z),
                source.Rotation
            );

            return CombineMeshes(
                new[] { slopeBase, slopeBack, slope },
                new[] { 0, 2, 0, 1, 0, 3}
            );
        }

        private static Mesh WedgeMesh(Brick source) {
            Mesh madgeSide = Instantiate(instance.Cube); // accidentally typed madge and thought it was funny
            Mesh madge = Instantiate(instance.Wedge); 

            TransformMesh(madgeSide,
                new Vector3(source.Scale.x / 2f - 0.5f, 0f, 0f),
                new Vector3(1f, source.Scale.y, source.Scale.z),
                source.Rotation
            );
            ScaleUVs(madgeSide, 1f, source.Scale.z);
            
            TransformMesh(madge,
                new Vector3(-0.5f, 0f, 0f),
                new Vector3(source.Scale.x - 1f, source.Scale.y, source.Scale.z),
                source.Rotation
            );

            return CombineMeshes(
                new[] { madgeSide, madge },
                new[] { 0, 1, 2, 0, 0, 0 }
            );
        }

        private static Mesh SpawnpointMesh(Brick source) {
            Mesh spawnpointBase = Instantiate(instance.CubeT);
            Mesh spawnpointTop = Instantiate(instance.Plane);
            
            TransformMesh(spawnpointBase, Vector3.zero, source.Scale, source.Rotation);
            ScaleUVs(spawnpointBase, source.Scale.x, source.Scale.z);
            
            TransformMesh(spawnpointTop, new Vector3(0f, source.Scale.y / 2f, 0f), source.Scale, source.Rotation);
            ScaleUVs(spawnpointTop, 1f, 1f);
            
            return CombineMeshes(
                new [] { spawnpointBase, spawnpointTop },
                new [] { 0, 2, 1 }
            );
        }
        
        private static Mesh ArchMesh(Brick source) {
            Mesh archMain = Instantiate(instance.Arch);
            Mesh archLeft = Instantiate(instance.CubeT);
            Mesh archRight = Instantiate(instance.CubeT);
            Mesh archTop = Instantiate(instance.CubeB);

            TransformMesh(archMain,
                new Vector3(0f, -0.15f, 0f),
                new Vector3(source.Scale.x, source.Scale.y - 0.3f, source.Scale.z - 2f),
                source.Rotation
            );
            
            TransformMesh(archLeft,
                new Vector3(0f, 0f, -source.Scale.z / 2f + 0.5f),
                new Vector3(source.Scale.x, source.Scale.y, 1f),
                source.Rotation
            );       
            ScaleUVs(archLeft, source.Scale.x, 1f);
            
            TransformMesh(archRight,
                new Vector3(0f, 0f, source.Scale.z / 2f - 0.5f),
                new Vector3(source.Scale.x, source.Scale.y, 1f),
                source.Rotation
            );
            ScaleUVs(archRight, source.Scale.x, 1f);

            TransformMesh(archTop,
                new Vector3(0f, source.Scale.y / 2f - 0.15f, 0f),
                new Vector3(source.Scale.x, 0.3f, source.Scale.z),
                source.Rotation
            );
            ScaleUVs(archTop, source.Scale.x, source.Scale.z);
            
            return CombineMeshes(
                new[] { archMain, archLeft, archRight, archTop },
                new[] { 0, 0, 2, 0, 2, 0, 1 }
            );
        }

        private static Mesh CornerMesh(Brick source) {
            Mesh cornerBase = Instantiate(instance.CubeT);
            Mesh cornerSide = Instantiate(instance.CornerSide);
            Mesh cornerBack = Instantiate(instance.CornerBack);
            Mesh cornerCorner = Instantiate(instance.CubeB);
            Mesh cornerSlope = Instantiate(instance.CornerSlope);
            
            TransformMesh(cornerBase,
                new Vector3(0f, -source.Scale.y / 2f + 0.15f, 0f),
                new Vector3(source.Scale.x, 0.3f, source.Scale.z),
                source.Rotation
            );
            ScaleUVs(cornerBase, source.Scale.x, source.Scale.z);
            
            TransformMesh(cornerSide,
                new Vector3(-0.5f, 0.15f, -source.Scale.z / 2f + 0.5f),
                new Vector3(source.Scale.x - 1f, source.Scale.y - 0.3f, 1f),
                source.Rotation
            );
            
            TransformMesh(cornerBack,
                new Vector3(source.Scale.x / 2f - 0.5f, 0.15f, 0.5f),
                new Vector3(1f, source.Scale.y - 0.3f, source.Scale.z - 1f),
                source.Rotation
            );
            
            TransformMesh(cornerCorner,
                new Vector3(source.Scale.x / 2f - 0.5f, 0.15f, -source.Scale.z / 2f + 0.5f),
                new Vector3(1f, source.Scale.y - 0.3f, 1f),
                source.Rotation
            );
            
            TransformMesh(cornerSlope,
                new Vector3(-0.5f, 0.15f, 0.5f),
                new Vector3(source.Scale.x - 1f, source.Scale.y - 0.3f, source.Scale.z - 1f),
                source.Rotation
            );

            return CombineMeshes(
                new[] { cornerBase, cornerSide, cornerBack, cornerCorner, cornerSlope },
                new[] { 0, 2, 0, 0, 0, 1, 0 }
            );
        }
        
        private static Mesh InvertedCornerMesh(Brick source) {
            Mesh cornerBase = Instantiate(instance.CubeB);
            Mesh cornerSide = Instantiate(instance.CornerSideInverted);
            Mesh cornerBack = Instantiate(instance.CornerBackInverted);
            Mesh cornerCorner = Instantiate(instance.CubeT);
            Mesh cornerSlope = Instantiate(instance.CornerSlopeInverted);
            
            TransformMesh(cornerBase,
                new Vector3(0f, source.Scale.y / 2f - 0.15f, 0f),
                new Vector3(source.Scale.x, 0.3f, source.Scale.z),
                source.Rotation
            );
            
            ScaleUVs(cornerBase, source.Scale.x, source.Scale.z);
            
            TransformMesh(cornerSide,
                new Vector3(-0.5f, -0.15f, -source.Scale.z / 2f + 0.5f),
                new Vector3(source.Scale.x - 1f, source.Scale.y - 0.3f, 1f),
                source.Rotation
            );
            
            TransformMesh(cornerBack,
                new Vector3(source.Scale.x / 2f - 0.5f, -0.15f, 0.5f),
                new Vector3(1f, source.Scale.y - 0.3f, source.Scale.z - 1f),
                source.Rotation
            );
            
            TransformMesh(cornerCorner,
                new Vector3(source.Scale.x / 2f - 0.5f, -0.15f, -source.Scale.z / 2f + 0.5f),
                new Vector3(1f, source.Scale.y - 0.3f, 1f),
                source.Rotation
            );
            
            TransformMesh(cornerSlope,
                new Vector3(-0.5f, -0.15f, 0.5f),
                new Vector3(source.Scale.x - 1f, source.Scale.y - 0.3f, source.Scale.z - 1f),
                source.Rotation
            );

            return CombineMeshes(
                new[] { cornerBase, cornerSide, cornerBack, cornerCorner, cornerSlope },
                new[] { 0, 1, 0, 0, 0, 2, 0 }
                );
        }
        
        private static Mesh DomeMesh(Brick source) {
            Mesh dome = Instantiate(instance.Dome);
            TransformMesh(dome, Vector3.zero, source.Scale, source.Rotation);
            return dome;
        }
        
        private static Mesh BarsMesh(Brick source) {
            int depth = Mathf.CeilToInt(source.Scale.z);
            
            float startingX = source.Scale.x / 2f - 0.5f; // should we use ceiled x?
            float startingZ = -depth / 2f + 0.5f;
            if (source.Rotation.y != 0 && source.Rotation.y != 180) startingX *= -1f;
            
            Mesh[] barsMeshes = new Mesh[depth * 3]; // 3 segments per bar
            int[] submeshIndexes = new int[depth * 6]; // 6 submesh index remaps per bar
            for (int i = 0; i < depth; i++) {
                Mesh barBase = Instantiate(instance.BarBase);
                Mesh barPole = Instantiate(instance.CylinderTB);
                Mesh barTop = Instantiate(instance.Cube);
                barsMeshes[i * 3] = barBase;
                barsMeshes[i * 3 + 1] = barPole;
                barsMeshes[i * 3 + 2] = barTop;
                
                TransformMesh(barBase,
                    new Vector3(startingX, -source.Scale.y / 2f + 0.3f, startingZ + i),
                    new Vector3(1f, 0.6f, 1f),
                    source.Rotation
                );
                
                TransformMesh(barPole,
                    new Vector3(startingX, 0.15f, startingZ + i),
                    new Vector3(0.5f, source.Scale.y - 0.9f, 0.5f),
                    source.Rotation
                );
                
                TransformMesh(barTop,
                    new Vector3(startingX, source.Scale.y / 2f - 0.15f, startingZ + i),
                    new Vector3(1f, 0.3f, 1f),
                    source.Rotation
                );

                // remap submeshes (to fix material order)
                submeshIndexes[i * 6 + 0] = 0;
                submeshIndexes[i * 6 + 1] = 2;
                submeshIndexes[i * 6 + 2] = 0;
                submeshIndexes[i * 6 + 3] = 0;
                submeshIndexes[i * 6 + 4] = 1;
                submeshIndexes[i * 6 + 5] = 2;
            }
            
            return CombineMeshes(barsMeshes, submeshIndexes);
        }
        
        private static Mesh FlagMesh(Brick source) {
            Mesh flag = Instantiate(instance.Flag);
            TransformMesh(flag, Vector3.zero, source.Scale, source.Rotation);
            return flag;
        }

        private static Mesh PoleMesh(Brick source) {
            Mesh poleBase = Instantiate(instance.Cylinder);
            Mesh pole = Instantiate(instance.CylinderTB);
            Mesh poleTip = Instantiate(instance.DomeTop);

            TransformMesh(poleBase,
                new Vector3(0f, -source.Scale.y / 2f + 0.15f, 0f),
                new Vector3(1f, 0.3f, 1f),
                source.Rotation
            );
            
            TransformMesh(pole,
                new Vector3(0f, 0f, 0f),
                new Vector3(0.6f, source.Scale.y - 0.6f, 0.6f),
                source.Rotation
            );
            
            TransformMesh(poleTip,
                new Vector3(0f, source.Scale.y / 2f - 0.15f, 0f),
                new Vector3(0.6f, 0.3f, 0.6f),
                source.Rotation
            );
            
            return CombineMeshes(new[] { poleBase, pole, poleTip });
        }
        
        private static Mesh RoundWedgeMesh(Brick source) {
            Mesh roundWedgeSide = Instantiate(instance.Cube);
            Mesh roundWedgeFront = Instantiate(instance.Cube);
            Mesh roundWedgeMain = Instantiate(instance.RoundedWedge);

            TransformMesh(roundWedgeSide,
                new Vector3(0f, 0f, source.Scale.z / 2f - 0.5f),
                new Vector3(source.Scale.x, source.Scale.y, 1f),
                source.Rotation
            );
            ScaleUVs(roundWedgeSide, source.Scale.x, 1f);
            
            TransformMesh(roundWedgeFront,
                new Vector3(-source.Scale.x / 2f + 0.5f, 0f, 0f),
                new Vector3(1f, source.Scale.y, source.Scale.z),
                source.Rotation
            );
            ScaleUVs(roundWedgeFront, 1f, source.Scale.z);
            
            TransformMesh(roundWedgeMain,
                new Vector3(0.5f, 0f, -0.5f),
                new Vector3(source.Scale.x - 1f, source.Scale.y, source.Scale.z - 1f),
                source.Rotation
            );
            
            return CombineMeshes(new[] { roundWedgeSide, roundWedgeFront, roundWedgeMain });
        }
        
        private static Mesh CylinderMesh(Brick source) {
            Mesh cylBase = Instantiate(instance.CylinderT);
            Mesh cylMain = Instantiate(instance.Cylinder);

            TransformMesh(cylBase,
                new Vector3(0f, -source.Scale.y / 2f + 0.15f, 0f),
                new Vector3(source.Scale.x - 0.2f, 0.3f, source.Scale.z - 0.2f),
                source.Rotation
            );
            
            TransformMesh(cylMain,
                new Vector3(0f, 0.15f, 0f),
                new Vector3(source.Scale.x, source.Scale.y - 0.3f, source.Scale.z),
                source.Rotation
            );
            
            return CombineMeshes(new[] { cylBase, cylMain });
        }
        
        private static Mesh RoundSlopeMesh(Brick source) {
            Mesh roundSlope = Instantiate(instance.RoundedSlope);
            TransformMesh(roundSlope, Vector3.zero, source.Scale, source.Rotation);
            ScaleUVs(roundSlope, source.Scale.x, source.Scale.z);
            return roundSlope;
        }
        
        private static Mesh VentMesh(Brick source) {
            int width = Mathf.CeilToInt(source.Scale.x);
            int depth = Mathf.CeilToInt(source.Scale.z);
            float startingX = -source.Scale.x / 2f + 0.5f;

            Mesh[] ventMeshes = new Mesh[width];

            for (int i = 0; i < width; i++) {
                Mesh vent = Instantiate(instance.Vent);
                ventMeshes[i] = vent;

                TransformMesh(vent,
                    new Vector3(startingX + i, 0f, 0f),
                    new Vector3(1f, 0.3f, depth),
                    source.Rotation
                );
                
                ScaleUVs(vent, width, 1f);
            }
            
            return CombineMeshes(ventMeshes);
        }
        
        private static Mesh ConeMesh(Brick source) {
            Mesh cone = Instantiate(instance.Cone);
            TransformMesh(cone, Vector3.zero, source.Scale, source.Rotation);
            return cone;
        }
        
        private static Mesh SphereMesh(Brick source) {
            Mesh sphere = Instantiate(instance.Sphere);
            TransformMesh(sphere, Vector3.zero, source.Scale, source.Rotation);
            return sphere;
        }

        private static Mesh GridMesh(Brick source) {
            Mesh tb = Instantiate(instance.CubeTB);
            Mesh lr = Instantiate(instance.CubeLR);
            Mesh fbk = Instantiate(instance.CubeFbk);
            
            TransformMesh(tb, Vector3.zero, source.Scale, source.Rotation);
            ScaleUVs(tb, source.Scale.x / 2f, source.Scale.z / 2f);
            TransformMesh(lr, Vector3.zero, source.Scale, source.Rotation);
            ScaleUVs(lr, source.Scale.x / 2f, source.Scale.y / 2f);
            TransformMesh(fbk, Vector3.zero, source.Scale, source.Rotation);
            ScaleUVs(fbk, source.Scale.z / 2f, source.Scale.y / 2f);
            
            return CombineMeshes(
                new [] { tb, lr, fbk },
                new [] { 1, 1, 0, 0, 2, 2 }
            );
        }        
        
        // hmm, i wonder what this does
        private static Mesh CombineMeshes(Mesh[] meshes, int[] remapSubMeshes = null)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<List<int>> triangles = new List<List<int>>();
            List<Vector2> uvs = new List<Vector2>();
            List<Color> colors = new List<Color>();
            List<Vector3> normals = new List<Vector3>();

            int totalSubMeshIndex = 0;
            for (int i = 0; i < meshes.Length; i++)
            {
                if (meshes[i] == null) continue;
                
                // get vertex count prior to appending vertices
                // this is so that we can correct the triangle data later on
                int startingVertex = vertices.Count;
                
                // append vertices as-is
                vertices.AddRange(meshes[i].vertices);

                // append colors as-is
                colors.AddRange(meshes[i].colors);
                
                // append normals as-is
                normals.AddRange(meshes[i].normals);
                
                // append modified triangle data so that triangles reference the correct vertices
                for (int j = 0; j < meshes[i].subMeshCount; j++)
                {
                    List<int> submeshTriangles = new List<int>();

                    meshes[i].GetTriangles(submeshTriangles, j);

                    for (int k = 0; k < submeshTriangles.Count; k++)
                    {
                        submeshTriangles[k] += startingVertex;
                    }

                    int targetSubMesh = j;
                    if (remapSubMeshes != null) {
                        targetSubMesh = remapSubMeshes[totalSubMeshIndex + j];
                        if (targetSubMesh == -1) targetSubMesh = j;
                    }
                    
                    if (triangles.Count > targetSubMesh)
                    {
                        triangles[targetSubMesh].AddRange(submeshTriangles);
                    }
                    else {
                        // populate list with empty lists so we are not out of range
                        while (triangles.Count <= targetSubMesh)
                            triangles.Add(new List<int>());
                        
                        triangles[targetSubMesh] = submeshTriangles;
                    }
                }
                
                // append uvs as-is
                uvs.AddRange(meshes[i].uv);

                totalSubMeshIndex += meshes[i].subMeshCount;
            }

            Mesh combinedMesh = new Mesh();
            if (vertices.Count > 65565)
                combinedMesh.indexFormat = IndexFormat.UInt32;
            
            combinedMesh.SetVertices(vertices);

            // Set submesh data
            combinedMesh.subMeshCount = triangles.Count;
            for (int i = 0; i < triangles.Count; i++)
            {
                combinedMesh.SetTriangles(triangles[i], i);
            }
            
            combinedMesh.SetUVs(0, uvs);
            combinedMesh.SetColors(colors);
            combinedMesh.SetNormals(normals);

            return combinedMesh;
        }

        private static void TransformMesh(Mesh mesh, Vector3 position, Vector3 scale, Vector3 rotation) {
            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++) {
                // scale
                verts[i].Scale(scale);
                
                // position
                verts[i] += position;
                
                // rotation
                verts[i] = RotatePointAroundPivot(verts[i], Vector3.zero, rotation);
            }
            mesh.SetVertices(verts);
        }

        private static void ScaleUVs(Mesh mesh, float x, float y) {
            Vector2[] uvs = mesh.uv;

            for (int i = 0; i < uvs.Length; i++) {
                uvs[i].x *= x;
                uvs[i].y *= y;
            }
            
            mesh.SetUVs(0, uvs);
        }
        
        // sets all vertexcolors of a mesh to specified color
        private static void ColorMesh(Mesh mesh, Color color)
        {
            Color[] colors = new Color[mesh.vertexCount];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }
            mesh.SetColors(colors);
        }

        private static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 eulerRotation)
        {
            return Quaternion.Euler(eulerRotation) * (point - pivot) + pivot;
        }
        
        // Creates a quad plane with the specified properties
        // Really only used for the baseplate...
        public static Mesh CreatePlane(float size, Color color)
        {
            // for neater vertex assignment
            float halfSize = size / 2;
            
            // Quad vertices
            Vector3[] vertices = 
            {
                new Vector3(-halfSize, 0, -halfSize),
                new Vector3(halfSize, 0, -halfSize),
                new Vector3(halfSize, 0, halfSize),
                new Vector3(-halfSize, 0, halfSize)
            };

            // Vertex indexes to create triangles
            int[] triangles =
            {
                2, 1, 0,
                3, 2, 0
            };

            // UV texture coordinates
            Vector2[] uvs =
            {
                new Vector2(0, 0),
                new Vector2(size, 0),
                new Vector2(size, size),
                new Vector2(0, size)
            };
            
            // Vertex colors - 1 color per vertex
            Color[] colors =
            {
                color,
                color,
                color,
                color
            };
            
            // create mesh using calculated values
            Mesh planeMesh = new Mesh()
            {
                vertices = vertices,
                triangles = triangles,
                uv = uvs,
                colors = colors
            };
            
            // extra mesh stuff + return
            planeMesh.RecalculateNormals(); // is this necessary?
            planeMesh.RecalculateBounds();
            
            return planeMesh;
        }
    }
}
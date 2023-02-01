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

        public Mesh Arch; // Archway interior
        public Mesh BarBase; // Bottom segment of bars
        public Mesh Cone;
        public Mesh CornerSlope;
        public Mesh CornerSlopeInverted;
        public Mesh Cube; // Full 6-Sided Cube
        public Mesh CubeT; // Cube without top face - e.g bottom of slope
        public Mesh CubeB; // Cube without bottom face - e.g top of arch
        public Mesh CubeR; // Cube without right face - e.g left side of arch
        public Mesh CubeL; // Cube without left face - e.g right side of arch
        public Mesh CubeF; // Cube without front face - e.g back side of slope
        public Mesh Cylinder; // Cylinder with all faces
        public Mesh CylinderT; // Cylinder without top face - e.g base of cylinder shape
        public Mesh CylinderTB; // Cylinder without top or bottom face - e.g pole, NOT tuberculosis.
        public Mesh Dome;
        public Mesh Flag; 
        public Mesh PoleTip;
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

        // Adds bricks to a mesh
        public static void AppendBricks(Mesh mesh, Brick[] bricks)
        {
            List<Mesh> brickMeshes = new List<Mesh>();
            for (int i = 0; i < bricks.Length; i++)
            {
                brickMeshes.Add(CreateMesh(bricks[i]));
            }

            if (brickMeshes.Count > 0)
            {
                brickMeshes.Insert(0, mesh);
                Mesh combinedMesh = CombineMeshes(brickMeshes.ToArray());
                mesh.SetVertices(combinedMesh.vertices); // transfer verts
                for (int i = 0; i < combinedMesh.subMeshCount; i++)
                {
                    // transfer triangles
                    List<int> submeshTriangles = new List<int>();
                    combinedMesh.GetTriangles(submeshTriangles, i);
                    mesh.SetTriangles(submeshTriangles, i);
                    
                    // transfer uvs
                    List<Vector2> submeshUVs = new List<Vector2>();
                    combinedMesh.GetUVs(i, submeshUVs);
                    mesh.SetUVs(i, submeshUVs);
                }
                mesh.SetColors(combinedMesh.colors); // transfer colors
                mesh.SetNormals(combinedMesh.normals); // transfer normals
            }
        }
        
        // Creates a mesh using brick values (shape, color, scale, etc)
        public static Mesh CreateMesh(Brick source)
        {
            switch (source.Shape)
            {
                case BrickShape.arch:
                    break;
                case BrickShape.bars:
                    break;
                case BrickShape.cube:
                    return CubeMesh(source);
                case BrickShape.cylinder:
                    break;
                case BrickShape.dome:
                    break;
                case BrickShape.flag:
                    break;
                case BrickShape.pole:
                    break;
                case BrickShape.round_slope:
                    break;
                case BrickShape.slope:
                    break;
                case BrickShape.spawnpoint:
                    break;
                case BrickShape.vent:
                    break;
                case BrickShape.wedge:
                    break;
            }
            
            // if for some reason we get here, fallback to cube
            return CubeMesh(source);
        }
        
        private static Mesh CubeMesh(Brick source)
        {
            Mesh mesh = Instantiate(instance.Cube);
            
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                // Scale vertex
                vertices[i].Scale(source.Scale);
                
                // Reposition vertex
                vertices[i] += source.Position;

                // Rotate vertex
                vertices[i] = RotatePointAroundPivot(vertices[i], source.Position, source.Rotation);
            }
            
            mesh.SetVertices(vertices);
            
            List<Vector2> uvs = new List<Vector2>();
            mesh.GetUVs(0, uvs);

            for (int i = 0; i < uvs.Count; i++)
            {
                uvs[i] = new Vector2(uvs[i].x * source.Scale.x, uvs[i].y * source.Scale.z);
            }
            
            mesh.SetUVs(0, uvs);

            ColorMesh(mesh, source.Color);
            
            return mesh;
        }

        // hmm, i wonder what this does
        private static Mesh CombineMeshes(Mesh[] meshes)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<List<int>> triangles = new List<List<int>>();
            List<List<Vector2>> uvs = new List<List<Vector2>>();
            List<Color> colors = new List<Color>();
            List<Vector3> normals = new List<Vector3>();
            
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
                    List<Vector2> submeshUVs = new List<Vector2>();

                    meshes[i].GetTriangles(submeshTriangles, j);
                    meshes[i].GetUVs(j, submeshUVs);

                    for (int k = 0; k < submeshTriangles.Count; k++)
                    {
                        // WOW 3 NESTED LOOPS 
                        // NOT GOOD

                        submeshTriangles[k] += startingVertex;
                    }

                    if (triangles.Count > j)
                    {
                        triangles[j].AddRange(submeshTriangles);
                    }
                    else
                    {
                        triangles.Add(submeshTriangles);
                    }
                    
                    if (uvs.Count > j)
                    {
                        uvs[j].AddRange(submeshUVs);
                    }
                    else
                    {
                        uvs.Add(submeshUVs);
                    }
                }
            }

            Mesh combinedMesh = new Mesh();
            if (vertices.Count > 65565)
                combinedMesh.indexFormat = IndexFormat.UInt32;
            
            combinedMesh.SetVertices(vertices);

            combinedMesh.subMeshCount = triangles.Count;
            for (int i = 0; i < triangles.Count; i++)
            {
                combinedMesh.SetTriangles(triangles[i], i);
                combinedMesh.SetUVs(i, uvs[i]);
            }
            
            combinedMesh.SetColors(colors);
            combinedMesh.SetNormals(normals);

            return combinedMesh;
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
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
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
            
            return planeMesh;
        }
    }
}
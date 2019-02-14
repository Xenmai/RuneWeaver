using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionShapes;
using FreneticGameCore.CoreSystems;
using FreneticGameCore.EntitySystem.PhysicsHelpers;
using FreneticGameCore.UtilitySystems;
using FreneticGameGraphics;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using FreneticGameGraphics.GraphicsHelpers;
using OpenTK;
using RuneWeaver.GameRenderables;
using RuneWeaver.MainGame;
using RuneWeaver.Utilities;
using System;
using System.Collections.Generic;

namespace RuneWeaver.TriangularGrid
{
    /// <summary>
    /// Represents a an entity that rotates.
    /// </summary>
    public class TerrainGridProperty : BasicMeshRenderableProperty
    {
        /// <summary>
        /// The terrain grid size.
        /// </summary>
        public int Size;

        /// <summary>
        /// The terrain grid height map.
        /// </summary>
        public double[,] HeightMap;

        /// <summary>
        /// The amount of grid layers that will be applied.
        /// </summary>
        public int Layers = 5;

        // <summary>
        /// The X and Y seeds used for applying grid layers.
        /// </summary>
        public Vector2[] Seeds;

        /// <summary>
        /// The terrain grid materials.
        /// </summary>
        public GridMaterial[,] Materials;

        /// <summary>
        /// The terrain's physics body.
        /// </summary>
        public Terrain Body;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Game game = Engine3D.Source as Game;
            // Generate the layer seeds, height map and materials
            int s2 = Size * 2;
            Seeds = new Vector2[Layers];
            for (int i = 0; i < Layers; i++)
            {
                Seeds[i] = new Vector2((float)game.Random.NextDouble(), (float)game.Random.NextDouble());
            }
            HeightMap = new double[s2 + 2, Size + 1];
            ApplyClampedHeightMap(5, 0.025f, 0.75f, Seeds[0]);
            ApplyHeightMap(0.5f, 0.25f, Seeds[1]);
            GenerateMaterialLayer(GridMaterial.Grass);
            ApplyMaterialLayer(GridMaterial.Dirt, 0.1f, Seeds[2], 0.3f);
            ApplyHighMaterialLayer(GridMaterial.Rock, 0.05f, Seeds[3], 0.05f);
            // Generate the terrain grid mesh: Vertices, Normals and Indices
            Renderable.ListBuilder builder = new Renderable.ListBuilder();
            int n = s2 * Size * 3;
            builder.Prepare(n);
            for (int i = 1; i < s2; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    GridFace face = new GridFace(i, j);
                    List<Vector3> vertices = new List<Vector3>();
                    foreach (GridVertex vert in face.Corners())
                    {
                        vertices.Add(vert.ToCartesianCoords3D(HeightMap));
                        builder.AddEmptyBoneInfo();
                    }
                    builder.Vertices.AddRange(vertices);
                    Vector3[] vArray = vertices.ToArray();
                    Vector3 normal = Vector3.Cross(vArray[2] - vArray[0], vArray[1] - vArray[0]);
                    normal.Normalize();
                    builder.Normals.Add(normal);
                    builder.Normals.Add(normal);
                    builder.Normals.Add(normal);
                    Vector4 c = Materials[i, j].Color.ToOpenTK();
                    builder.Colors.Add(c);
                    builder.Colors.Add(c);
                    builder.Colors.Add(c);
                    if (face.PointsUp())
                    {
                        builder.TexCoords.Add(new Vector3(0, 0, 0));
                        builder.TexCoords.Add(new Vector3(0.5f, 1, 0));
                        builder.TexCoords.Add(new Vector3(1, 0, 0));
                    }
                    else
                    {
                        builder.TexCoords.Add(new Vector3(1, 1, 0));
                        builder.TexCoords.Add(new Vector3(0.5f, 0, 0));
                        builder.TexCoords.Add(new Vector3(0, 1, 0));
                    }
                }
            }
            int count = builder.Vertices.Count;
            for (uint k = 0; k < count; k++)
            {
                builder.Indices.Add(k);
            }
            Rend = builder.Generate();
            BEPUutilities.Vector3 scaling = new BEPUutilities.Vector3(0.5, 1, -0.866);
            BEPUutilities.Quaternion rotation = BEPUutilities.Quaternion.CreateFromAxisAngle(BEPUutilities.Vector3.UnitX, MathHelper.PiOver2);
            BEPUutilities.Vector3 translation = BEPUutilities.Vector3.Zero;
            Body = new Terrain(new TerrainShape(HeightMap), new BEPUutilities.AffineTransform(ref scaling, ref rotation, ref translation))
            {
                ImproveBoundaryBehavior = true
            };
        }

        /// <summary>
        /// Applies a random height distribution to the terrain grid.
        /// </summary>
        public void ApplyHeightMap(double h, float scale, Vector2 seed)
        {
            for (int j = 0; j <= Size; j++)
            {
                int x = j % 2;
                Vector2 pos = new GridVertex(x, j).ToCartesianCoords2D();
                HeightMap[x, j] += SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale) * h;
            }
            for (int i = 1; i <= Size; i++)
            {
                for (int j = 0; j <= Size; j++)
                {
                    int x = i * 2 + (j % 2);
                    Vector2 pos = new GridVertex(x, j).ToCartesianCoords2D();
                    HeightMap[x, j] += SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale) * h;
                    HeightMap[x - 1, j] = (HeightMap[x, j] + HeightMap[x - 2, j]) / 2;
                }
            }
        }

        /// <summary>
        /// Applies a random height distribution to the terrain grid.
        /// </summary>
        public void ApplyClampedHeightMap(float h, float scale, float clamp, Vector2 seed)
        {
            for (int j = 0; j <= Size; j++)
            {
                int x = j % 2;
                Vector2 pos = new GridVertex(x, j).ToCartesianCoords2D();
                HeightMap[x, j] += Math.Min(SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale), clamp) * h;
            }
            for (int i = 1; i <= Size; i++)
            {
                for (int j = 0; j <= Size; j++)
                {
                    int x = i * 2 + (j % 2);
                    Vector2 pos = new GridVertex(x, j).ToCartesianCoords2D();
                    HeightMap[x, j] += Math.Min(SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale), clamp) * h;
                    HeightMap[x - 1, j] = (HeightMap[x, j] + HeightMap[x - 2, j]) / 2;
                }
            }
        }

        /// <summary>
        /// Generates a base material distribution for the terrain grid.
        /// </summary>
        public void GenerateMaterialLayer(GridMaterial mat)
        {
            int s2 = Size * 2;
            Materials = new GridMaterial[s2, Size];
            for (int i = 1; i < s2; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Materials[i, j] = mat;
                }
            }
        }

        /// <summary>
        /// Applies a random material distribution to the terrain grid.
        /// </summary>
        public void ApplyMaterialLayer(GridMaterial mat, float scale, Vector2 seed, float chance)
        {
            int s2 = Size * 2;
            for (int i = 1; i < s2; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    GridFace face = new GridFace(i, j);
                    Vector2 pos = face.ToCartesianCoords2D();
                    if (face.PointsUp())
                    {
                        pos += new Vector2(0f, 0.333f);
                    }
                    else
                    {
                        pos += new Vector2(0f, 0.667f);
                    }
                    if (SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale) < chance)
                    {
                        Materials[i, j] = mat;
                    }
                }
            }
        }

        /// <summary>
        /// Applies a weighted material distribution to the terrain grid, based on height.
        /// </summary>
        public void ApplyHighMaterialLayer(GridMaterial mat, float scale, Vector2 seed, float chance)
        {
            int s2 = Size * 2;
            for (int i = 1; i < s2; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    GridFace face = new GridFace(i, j);
                    double h = 0;
                    foreach (GridVertex vert in face.Corners())
                    {
                        h += HeightMap[vert.U, vert.V];
                    }
                    h *= 0.333f;
                    Vector2 pos = face.ToCartesianCoords2D();
                    if (face.PointsUp())
                    {
                        pos += new Vector2(0f, 0.333f);
                    }
                    else
                    {
                        pos += new Vector2(0f, 0.667f);
                    }
                    if (SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale) / h < chance)
                    {
                        Materials[i, j] = mat;
                    }
                }
            }
        }
    }
}

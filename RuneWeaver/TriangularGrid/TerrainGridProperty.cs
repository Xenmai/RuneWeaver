using FreneticGameCore.CoreSystems;
using FreneticGameCore.UtilitySystems;
using FreneticGameGraphics;
using FreneticGameGraphics.GraphicsHelpers;
using OpenTK;
using RuneWeaver.GameRenderables;
using RuneWeaver.MainGame;
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
        public float[,] HeightMap;

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
            HeightMap = new float[s2 + 1, Size + 1];
            ApplyClampedHeightMap(5, 0.025f, 0.75f, Seeds[0]);
            ApplyHeightMap(0.5f, 0.25f, Seeds[1]);
            GenerateMaterialLayer(GridMaterial.Grass);
            ApplyMaterialLayer(GridMaterial.Dirt, 0.1f, Seeds[2], 0.3f);
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
                        vertices.Add(vert.ToCartesianCoords3D(HeightMap[vert.U, vert.V]));
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
        }

        /// <summary>
        /// Applies a random height distribution to the terrain grid.
        /// </summary>
        public void ApplyHeightMap(float h, float scale, Vector2 seed)
        {
            int s2 = Size * 2;
            for (int i = 0; i <= s2; i++)
            {
                for (int j = 0; j <= Size; j++)
                {
                    Vector2 pos = new GridVertex(i, j).ToCartesianCoords2D();
                    HeightMap[i, j] += (float)SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale) * h;
                }
            }
        }

        /// <summary>
        /// Applies a random height distribution to the terrain grid.
        /// </summary>
        public void ApplyClampedHeightMap(float h, float scale, float clamp, Vector2 seed)
        {
            int s2 = Size * 2;
            for (int i = 0; i <= s2; i++)
            {
                for (int j = 0; j <= Size; j++)
                {
                    Vector2 pos = new GridVertex(i, j).ToCartesianCoords2D();
                    HeightMap[i, j] += (float)Math.Min(SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale), clamp) * h;
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
            for (int i = 0; i < s2; i++)
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
            for (int i = 0; i < s2; i++)
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
    }
}

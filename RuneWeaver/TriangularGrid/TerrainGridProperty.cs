using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionShapes;
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
        public double[,] HeightMap;

        /// <summary>
        /// The amount of grid layers that will be applied.
        /// </summary>
        public int Layers = 10;

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
        /// The terrain mesh renderable builder.
        /// </summary>
        public Renderable.ListBuilder Builder;

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
            GenerateHeightMap(2.0f);
            ApplyClampedHeightMap(8.0f, 0.025f, 0.0f, 0.75f, Seeds[0]);
            ApplyHeightMap(0.5f, 0.25f, 0.5f, Seeds[1]);
            ApplyHighHeightMap(2.0f, 0.15f, 0.5f, 7.0f, Seeds[2]);
            GenerateMaterialLayer(GridMaterial.Grass);
            ApplyHighMaterialLayer(GridMaterial.Dirt, 0.05f, Seeds[3], 7.0f, 3.0f);
            ApplyHighMaterialLayer(GridMaterial.Rock, 0.025f, Seeds[4], 9.0f, 1.0f);
            ApplyLowMaterialLayer(GridMaterial.Water, 0.1f, Seeds[5], 3.0f, 0.5f);
            // Generate the terrain grid mesh: Vertices, Normals and Indices
            Builder = new Renderable.ListBuilder();
            int n = s2 * Size * 3;
            Builder.Prepare(n);
            for (int i = 1; i < s2; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    GridFace face = new GridFace(i, j);
                    List<Vector3> vertices = new List<Vector3>();
                    foreach (GridVertex vert in face.Corners())
                    {
                        vertices.Add(vert.ToCartesianCoords3D(HeightMap));
                        Builder.AddEmptyBoneInfo();
                    }
                    Builder.Vertices.AddRange(vertices);
                    Vector3[] vArray = vertices.ToArray();
                    Vector3 normal = Vector3.Cross(vArray[2] - vArray[0], vArray[1] - vArray[0]);
                    normal.Normalize();
                    Builder.Normals.Add(normal);
                    Builder.Normals.Add(normal);
                    Builder.Normals.Add(normal);
                    Vector4 c = Materials[i, j].Color.ToOpenTK();
                    Builder.Colors.Add(c);
                    Builder.Colors.Add(c);
                    Builder.Colors.Add(c);
                    if (face.PointsUp())
                    {
                        Builder.TexCoords.Add(new Vector3(0, 0, 0));
                        Builder.TexCoords.Add(new Vector3(0.5f, 1, 0));
                        Builder.TexCoords.Add(new Vector3(1, 0, 0));
                    }
                    else
                    {
                        Builder.TexCoords.Add(new Vector3(1, 1, 0));
                        Builder.TexCoords.Add(new Vector3(0.5f, 0, 0));
                        Builder.TexCoords.Add(new Vector3(0, 1, 0));
                    }
                }
            }
            int count = Builder.Vertices.Count;
            for (uint k = 0; k < count; k++)
            {
                Builder.Indices.Add(k);
            }
            Rend = Builder.Generate();
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
        public void GenerateHeightMap(double h)
        {
            HeightMap = new double[Size * 2 + 2, Size + 1];
            for (int j = 0; j <= Size; j++)
            {
                int x = j % 2;
                Vector2 pos = new GridVertex(x, j).ToCartesianCoords2D();
                HeightMap[x, j] = h;
            }
            for (int i = 1; i <= Size; i++)
            {
                for (int j = 0; j <= Size; j++)
                {
                    int x = i * 2 + (j % 2);
                    Vector2 pos = new GridVertex(x, j).ToCartesianCoords2D();
                    HeightMap[x, j] = h;
                    HeightMap[x - 1, j] = h;
                }
            }
        }

        /// <summary>
        /// Applies a random height distribution to the terrain grid.
        /// </summary>
        public void ApplyHeightMap(double h, float scale, float offset, Vector2 seed)
        {
            for (int j = 0; j <= Size; j++)
            {
                int x = j % 2;
                Vector2 pos = new GridVertex(x, j).ToCartesianCoords2D();
                HeightMap[x, j] += (SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale) - offset) * h;
            }
            for (int i = 1; i <= Size; i++)
            {
                for (int j = 0; j <= Size; j++)
                {
                    int x = i * 2 + (j % 2);
                    Vector2 pos = new GridVertex(x, j).ToCartesianCoords2D();
                    HeightMap[x, j] += (SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale) - offset) * h;
                    HeightMap[x - 1, j] = (HeightMap[x, j] + HeightMap[x - 2, j]) / 2;
                }
            }
        }

        /// <summary>
        /// Applies a random height distribution to the terrain grid.
        /// </summary>
        public void ApplyClampedHeightMap(float h, float scale, float offset, float clamp, Vector2 seed)
        {
            for (int j = 0; j <= Size; j++)
            {
                int x = j % 2;
                Vector2 pos = new GridVertex(x, j).ToCartesianCoords2D();
                HeightMap[x, j] += (Math.Min(SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale), clamp) - offset) * h;
            }
            for (int i = 1; i <= Size; i++)
            {
                for (int j = 0; j <= Size; j++)
                {
                    int x = i * 2 + (j % 2);
                    Vector2 pos = new GridVertex(x, j).ToCartesianCoords2D();
                    HeightMap[x, j] += (Math.Min(SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale), clamp) - offset) * h;
                    HeightMap[x - 1, j] = (HeightMap[x, j] + HeightMap[x - 2, j]) / 2;
                }
            }
        }

        /// <summary>
        /// Applies a random height distribution to the terrain grid.
        /// </summary>
        public void ApplyHighHeightMap(float h, float scale, float offset, float limit, Vector2 seed)
        {
            for (int j = 0; j <= Size; j++)
            {
                int x = j % 2;
                GridVertex vert = new GridVertex(x, j);
                if (HeightMap[vert.U, vert.V] > limit)
                {
                    Vector2 pos = vert.ToCartesianCoords2D();
                    HeightMap[x, j] += (SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale) - offset) * h;
                }
            }
            for (int i = 1; i <= Size; i++)
            {
                for (int j = 0; j <= Size; j++)
                {
                    int x = i * 2 + (j % 2);
                    GridVertex vert = new GridVertex(x, j);
                    if (HeightMap[vert.U, vert.V] > limit)
                    {
                        Vector2 pos = vert.ToCartesianCoords2D();
                        HeightMap[x, j] += (SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale) - offset) * h;
                        HeightMap[x - 1, j] = (HeightMap[x, j] + HeightMap[x - 2, j]) / 2;
                    }
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
                    // pos += face.PointsUp() ? new Vector2(0f, 0.333f * 0.866f) : pos += new Vector2(0f, 0.667f * 0.866f);
                    if (SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale) < chance)
                    {
                        Materials[i, j] = mat;
                    }
                }
            }
        }

        /// <summary>
        /// Applies a material distribution to the terrain grid, based on height.
        /// </summary>
        public void ApplyHighMaterialLayer(GridMaterial mat, float scale, Vector2 seed, float clamp, float mul)
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
                    // pos += face.PointsUp() ? new Vector2(0f, 0.333f * 0.866f) : pos += new Vector2(0f, 0.667f * 0.866f);
                    if ((SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale) - 0.5) * mul + h > clamp)
                    {
                        Materials[i, j] = mat;
                    }
                }
            }
        }

        /// <summary>
        /// Applies a material distribution to the terrain grid, based on height.
        /// </summary>
        public void ApplyLowMaterialLayer(GridMaterial mat, float scale, Vector2 seed, float clamp, float mul)
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
                    // pos += face.PointsUp() ? new Vector2(0f, 0.333f * 0.866f) : pos += new Vector2(0f, 0.667f * 0.866f);
                    if ((SimplexNoise.Generate(seed.X + pos.X * scale, seed.Y + pos.Y * scale) - 0.5) * mul + h < clamp)
                    {
                        Materials[i, j] = mat;
                    }
                }
            }
        }

        /// <summary>
        /// Adjusts the height of the specified vertex, updating the terrain.
        /// </summary>
        /// <param name="vert">The adjusted vertex.</param>
        /// <param name="h">The height that will be added.</param>
        public void AdjustVertexHeight(GridVertex vert, float h)
        {
            HeightMap[vert.U, vert.V] += h;
            Body.Shape.Heights[vert.U, vert.V] += h;
            int index = ((vert.U - 1) * Size + vert.V) * 3;
            int s3 = Size * 3;
            Vector3 point = vert.ToCartesianCoords3D(HeightMap);
            Builder.Vertices[index - s3 - 1] = point;
            Builder.Vertices[index - 3] = point;
            Builder.Vertices[index + s3 - 2] = point;
            Builder.Vertices[index - s3 + 1] = point;
            Builder.Vertices[index] = point;
            Builder.Vertices[index + s3 + 2] = point;
            Rend.GenerateVBO(Builder);
        }
    }
}

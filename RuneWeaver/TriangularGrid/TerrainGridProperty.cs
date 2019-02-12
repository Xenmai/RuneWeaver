using FreneticGameCore.MathHelpers;
using FreneticGameCore.UtilitySystems;
using FreneticGameGraphics;
using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using FreneticGameGraphics.GraphicsHelpers;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RuneWeaver.MainGame;
using System;

namespace RuneWeaver.TriangularGrid
{
    /// <summary>
    /// Represents a an entity that rotates.
    /// </summary>
    public class TerrainGridProperty : EntityRenderableProperty
    {
        /// <summary>
        /// The terrain grid size.
        /// </summary>
        public int Size = 30;

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
        public GridMaterial[,,] Materials;

        /// <summary>
        /// The terrain grid renderable.
        /// </summary>
        public Renderable Rend;

        /// <summary>
        /// The terrain grid physics body.
        /// </summary>
        public ClientEntityPhysicsProperty Body;

        /// <summary>
        /// The diffuse color texture.
        /// </summary>
        public Texture DiffuseTexture;

        /// <summary>
        /// The render scale.
        /// </summary>
        public Location Scale = new Location(1, 1, 1);

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Game game = Engine3D.Source as Game;
            // Generate the layer seeds, height map and materials
            Seeds = new Vector2[Layers];
            for (int i = 0; i < Layers; i++)
            {
                Seeds[i] = new Vector2((float)game.Random.NextDouble(), (float)game.Random.NextDouble());
            }
            HeightMap = new float[Size + 1, Size + 1];
            ApplyClampedHeightMap(5, 0.025f, 0.75f, Seeds[0]);
            ApplyHeightMap(0.5f, 0.25f, Seeds[1]);
            GenerateMaterialLayer(GridMaterial.Grass);
            ApplyMaterialLayer(GridMaterial.Dirt, 0.1f, Seeds[2], 0.3f);
            // Generate the terrain grid mesh: Vertices, Normals and Indices
            Renderable.ArrayBuilder builder = new Renderable.ArrayBuilder();
            int n = Size * Size * 6;
            builder.Prepare(n, n);
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    int index = ((i * Size) + j) * 6;
                    Vector3 vertex1 = new GridVertex(i, j).ToCartesianCoords3D(HeightMap[i, j]);
                    Vector3 vertex2 = new GridVertex(i, j + 1).ToCartesianCoords3D(HeightMap[i, j + 1]);
                    Vector3 vertex3 = new GridVertex(i + 1, j).ToCartesianCoords3D(HeightMap[i + 1, j]);
                    Vector3 vertex4 = new GridVertex(i + 1, j + 1).ToCartesianCoords3D(HeightMap[i + 1, j + 1]);
                    builder.Vertices[index] = vertex1;
                    builder.Vertices[index + 1] = vertex2;
                    builder.Vertices[index + 2] = vertex3;
                    builder.Vertices[index + 3] = vertex4;
                    builder.Vertices[index + 4] = vertex3;
                    builder.Vertices[index + 5] = vertex2;
                    Vector3 normal1 = Vector3.Cross(vertex3 - vertex1, vertex2 - vertex1);
                    normal1.Normalize();
                    builder.Normals[index] = normal1;
                    builder.Normals[index + 1] = normal1;
                    builder.Normals[index + 2] = normal1;
                    Vector4 c1 = Materials[i, j, 0].Color.ToOpenTK();
                    builder.Colors[index] = c1;
                    builder.Colors[index + 1] = c1;
                    builder.Colors[index + 2] = c1;
                    builder.TexCoords[index] = new Vector3(0, 0, 0);
                    builder.TexCoords[index + 1] = new Vector3(0.5f, 1, 0);
                    builder.TexCoords[index + 2] = new Vector3(1, 0, 0);
                    Vector3 normal2 = Vector3.Cross(vertex4 - vertex2, vertex4 - vertex3);
                    normal2.Normalize();
                    builder.Normals[index + 3] = normal2;
                    builder.Normals[index + 4] = normal2;
                    builder.Normals[index + 5] = normal2;
                    Vector4 c2 = Materials[i, j, 1].Color.ToOpenTK();
                    builder.Colors[index + 3] = c2;
                    builder.Colors[index + 4] = c2;
                    builder.Colors[index + 5] = c2;
                    builder.TexCoords[index + 3] = new Vector3(1, 1, 0);
                    builder.TexCoords[index + 4] = new Vector3(0.5f, 0, 0);
                    builder.TexCoords[index + 5] = new Vector3(0, 1, 0);
                    for (uint k = 0; k < 6; k++)
                    {
                        builder.Indices[index + k] = (uint) index + k;
                    }
                }
            }
            Rend = builder.Generate();
        }

        /// <summary>
        /// Applies a random height distribution to the terrain grid.
        /// </summary>
        public void ApplyHeightMap(float h, float scale, Vector2 seed)
        {
            for (int i = 0; i <= Size; i++)
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
            for (int i = 0; i <= Size; i++)
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
            Materials = new GridMaterial[Size, Size, 2];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Materials[i, j, 0] = mat;
                    Materials[i, j, 1] = mat;
                }
            }
        }

        /// <summary>
        /// Applies a random material distribution to the terrain grid.
        /// </summary>
        public void ApplyMaterialLayer(GridMaterial mat, float scale, Vector2 seed, float chance)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    int index = ((i * Size) + j) * 6;
                    Vector2 pos = new GridVertex(i, j).ToCartesianCoords2D();
                    Vector2 pos1 = pos + new Vector2(0.5f, 0.333f);
                    if (SimplexNoise.Generate(seed.X + pos1.X * scale, seed.Y + pos1.Y * scale) < chance)
                    {
                        Materials[i, j, 0] = mat;
                    }
                    Vector2 pos2 = pos + new Vector2(1, 0.667f);
                    if (SimplexNoise.Generate(seed.X + pos2.X * scale, seed.Y + pos2.Y * scale) < chance)
                    {
                        Materials[i, j, 1] = mat;
                    }
                }
            }
        }

        /// <summary>
        /// Render the entity as seen normally, in 3D.
        /// </summary>
        /// <param name="context">The render context.</param>
        public override void RenderStandard(RenderContext context)
        {
            if (DiffuseTexture != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                DiffuseTexture.Bind();
            }
            context.Engine.Rendering.SetColor(Color4F.White, context.Engine.MainView);
            Matrix4d mat = Matrix4d.Scale(Scale.ToOpenTK3D()) * Matrix4d.CreateFromQuaternion(RenderOrientation.ToOpenTKDoubles()) * Matrix4d.CreateTranslation(RenderAt.ToOpenTK3D());
            context.Engine.MainView.SetMatrix(ShaderLocations.Common.WORLD, mat);
            Rend.Render(true);
        }
        
        /// <summary>
        /// Render the entity as seen by a top-down map.
        /// </summary>
        /// <param name="context">The render context.</param>
        public override void RenderForTopMap(RenderContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Non-implemented 2D option.
        /// </summary>
        /// <param name="context">The 2D render context.</param>
        public override void RenderStandard2D(RenderContext2D context)
        {
            throw new NotImplementedException();
        }
    }
}

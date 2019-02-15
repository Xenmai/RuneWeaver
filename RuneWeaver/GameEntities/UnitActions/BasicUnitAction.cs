using FreneticGameGraphics.GraphicsHelpers;
using OpenTK;
using RuneWeaver.GameRenderables;
using RuneWeaver.MainGame;
using RuneWeaver.TriangularGrid;
using System.Collections.Generic;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions
{
    /// <summary>
    /// The basic action class.
    /// </summary>
    public abstract class BasicUnitAction
    {
        /// <summary>
        /// The owner of this action.
        /// </summary>
        public BasicUnitProperty Unit;

        /// <summary>
        /// The energy cost of this action.
        /// </summary>
        public int Cost;

        /// <summary>
        /// The icon that will be used for this action.
        /// </summary>
        public string Icon;

        /// <summary>
        /// The name of this unit action.
        /// </summary>
        public string Name;

        /// <summary>
        /// The affected zone vertices of this action.
        /// </summary>
        public HashSet<GridVertex> AffectedVertices;

        /// <summary>
        /// The renderable property.
        /// </summary>
        public BasicMeshRenderableProperty Prop;

        /// <summary>
        /// Constructs a new basic unit action.
        /// </summary>
        /// <param name="unit">The unit that has this action.</param>
        /// <param name="cost">The energy cost of this action.</param>
        public BasicUnitAction(BasicUnitProperty unit, int cost)
        {
            this.Unit = unit;
            this.Cost = cost;
        }

        /// <summary>
        /// Adjusts the unit's current energy.
        /// </summary>
        /// <param name="amount">The new energy amount.</param>
        public bool CheckEnergy()
        {
            return Unit.Energy < Cost ? false : true;
        }

        public void GenerateRenderable(HashSet<GridFace> faces)
        {
            Game game = Unit.Engine3D.Source as Game;
            Renderable.ListBuilder builder = new Renderable.ListBuilder();
            int n = faces.Count * 3;
            builder.Prepare(n);
            foreach (GridFace face in faces)
            {
                List<Vector3> vertices = new List<Vector3>();
                foreach (GridVertex vert in face.Corners())
                {
                    vertices.Add(vert.ToCartesianCoords3D(game.Terrain.HeightMap) + new Vector3(0, 0, 0.1f));
                    builder.AddEmptyBoneInfo();
                }
                builder.Vertices.AddRange(vertices);
                Vector3[] vArray = vertices.ToArray();
                Vector3 normal = Vector3.Cross(vArray[2] - vArray[0], vArray[1] - vArray[0]);
                normal.Normalize();
                builder.Normals.Add(normal);
                builder.Normals.Add(normal);
                builder.Normals.Add(normal);
                builder.Colors.Add(new Vector4(1, 0, 0, 0.4f));
                builder.Colors.Add(new Vector4(1, 0, 0, 0.4f));
                builder.Colors.Add(new Vector4(1, 0, 0, 0.4f));
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
                uint count = (uint)builder.Normals.Count;
                builder.Indices.Add(count - 3);
                builder.Indices.Add(count - 2);
                builder.Indices.Add(count - 1);
            }
            Renderable rend = builder.Generate();
            Prop = new BasicMeshRenderableProperty()
            {
                DiffuseTexture = game.Client.Textures.White,
                Rend = rend
            };
            game.UnitController.Entity.AddProperty(Prop);
        }

        public void Select()
        {
            Game game = Unit.Engine3D.Source as Game;
            game.UnitController.SelectedAction = this;
        }

        public abstract void Prepare();

        public abstract void Update();

        public abstract void Cancel();

        public abstract void Execute();
    }
}

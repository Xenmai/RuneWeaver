using FreneticGameCore.MathHelpers;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using FreneticGameGraphics.GraphicsHelpers;
using OpenTK;
using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes;
using RuneWeaver.GameRenderables;
using RuneWeaver.MainGame;
using RuneWeaver.TriangularGrid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions
{
    /// <summary>
    /// The move unit action class.
    /// </summary>
    public class AttackUnitAction : BasicUnitAction
    {
        /// <summary>
        /// The damage amount of this attack action.
        /// </summary>
        public int Damage;

        /// <summary>
        /// The hitbox of this attack action.
        /// </summary>
        public LineHitbox Hitbox;

        /// <summary>
        /// Constructs a new attack unit action.
        /// </summary>
        /// <param name="unit">The unit that owns this action.</param>
        /// <param name="cost">The energy cost of this action.</param>
        /// <param name="damage">The damage amount of this action.</param>
        /// <param name="hitbox">The damage hitbox of this action.</param>
        public AttackUnitAction(BasicUnitProperty unit, int cost, int damage, LineHitbox hitbox) : base(unit, cost)
        {
            this.Damage = damage;
            this.Hitbox = hitbox;
        }

        /// <summary>
        /// The renderable property.
        /// </summary>
        public BasicMeshRenderableProperty Prop;

        /// <summary>
        /// Prepares and renders the action's affected zone. This usually happens when the action is selected.
        /// </summary>
        public override void Prepare()
        {
            Game game = Unit.Engine3D.Source as Game;
            AffectedVertices = Hitbox.Area(Unit.Coords, GridVertex.Directions[3]);
            HashSet<GridFace> faces = new HashSet<GridFace>();
            foreach (GridVertex vert in AffectedVertices)
            {
                faces.UnionWith(vert.Touches());
            }
            faces.ExceptWith(game.UnitController.OccupiedFaces(Unit.Size, Unit.Coords));
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

        public override void Update()
        {
            
        }

        public override void Cancel()
        {
            Game game = Unit.Engine3D.Source as Game;
            game.UnitController.Entity.RemoveProperty<BasicMeshRenderableProperty>();
            game.UnitController.SelectedAction = null;
        }

        public override void Execute()
        {
            
        }
    }
}

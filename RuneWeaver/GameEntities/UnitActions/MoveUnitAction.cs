using FreneticGameCore.CoreSystems;
using FreneticGameGraphics.GraphicsHelpers;
using OpenTK;
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
    public class MoveUnitAction : BasicUnitAction
    {
        /// <summary>
        /// The range of this move action.
        /// </summary>
        public int Range;

        /// <summary>
        /// Constructs a new move unit action.
        /// </summary>
        /// <param name="unit">The unit that owns this action.</param>
        /// <param name="cost">The energy cost of this action.</param>
        /// <param name="range">The movement range of this action.</param>
        public MoveUnitAction(BasicUnitProperty unit, int cost, int range) : base(unit, cost)
        {
            this.Range = range;
        }

        /// <summary>
        /// The flood filled affected zone of this movement action.
        /// </summary>
        public Dictionary<GridVertex, GridVertex> AffectedVertices = new Dictionary<GridVertex, GridVertex>();

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
            Flood(Unit.Coords, Range);
            List<GridFace> faces = new List<GridFace>();
            foreach (GridVertex vert in AffectedVertices.Keys)
            {
                faces = new List<GridFace>(faces.Union(Utilities.GridHelper.Expand(vert.Touches(), (Unit.Size - 1) * 2)));
            }
            Renderable.ListBuilder builder = new Renderable.ListBuilder();
            int n = faces.Count * 3;
            builder.Prepare(n);
            foreach (GridFace face in faces)
            {
                List<Vector3> vertices = new List<Vector3>();
                foreach (GridVertex vert in face.Corners())
                {
                    vertices.Add(vert.ToCartesianCoords3D(game.Terrain.HeightMap[vert.U, vert.V] + 0.25f));
                }
                Vector3[] vArray = vertices.ToArray();
                builder.Vertices.AddRange(vArray);
                builder.AddEmptyBoneInfo();
                builder.AddEmptyBoneInfo();
                builder.AddEmptyBoneInfo();
                Vector3 normal = Vector3.Cross(vArray[2] - vArray[0], vArray[1] - vArray[0]);
                normal.Normalize();
                builder.Normals.Add(normal);
                builder.Normals.Add(normal);
                builder.Normals.Add(normal);
                builder.Colors.Add(new Vector4(1, 0, 0, 0.4f));
                builder.Colors.Add(new Vector4(1, 0, 0, 0.4f));
                builder.Colors.Add(new Vector4(1, 0, 0, 0.4f));
                if (face.Side == 0)
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

        /// <summary>
        /// Calculates the next affected grid vertices.
        /// </summary>
        /// <param name="source">The current flooded grid vertex.</param>
        public void Flood(GridVertex source, int rangeLeft)
        {
            SysConsole.OutputCustom("info", "Range: " + rangeLeft);
            if (rangeLeft <= 0)
            {
                return;
            }
            Game game = Unit.Engine3D.Source as Game;
            foreach (GridVertex target in source.Adjacent())
            {
                float h = Math.Abs(game.Terrain.HeightMap[target.U, target.V] - game.Terrain.HeightMap[source.U, source.V]);
                if (h <= Unit.Stability && !AffectedVertices.ContainsKey(target))
                {
                    AffectedVertices.Add(target, source);
                    Flood(target, rangeLeft - 1);
                }
            }
        }

        /// <summary>
        /// Updates the action's affected zone. This usually happens when the action is selected and the cursor is moved.
        /// </summary>
        public override void Update()
        {

        }

        /// <summary>
        /// Cancels and deselects the action, without doing anything else.
        /// </summary>
        public override void Cancel()
        {
            
        }

        /// <summary>
        /// Executes and then deselects the action. This usually happens when right clicking after an action has been selected.
        /// </summary>
        public override void Execute()
        {
            
        }
    }
}

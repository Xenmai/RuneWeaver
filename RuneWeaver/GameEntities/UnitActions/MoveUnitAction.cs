using BEPUphysics;
using FreneticGameCore.CoreSystems;
using FreneticGameCore.MathHelpers;
using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
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
            Flood(Unit.Coords, 0);
            HashSet<GridFace> faces = new HashSet<GridFace>();
            foreach (GridVertex vert in AffectedVertices.Keys)
            {
                faces.UnionWith(game.UnitController.OccupiedFaces(Unit.Size, vert));
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

        /// <summary>
        /// Calculates the next affected grid vertices.
        /// </summary>
        /// <param name="source">The current flooded grid vertex.</param>
        public void Flood(GridVertex source, int currentSteps)
        {
            if (Range - currentSteps <= 0)
            {
                return;
            }
            Game game = Unit.Engine3D.Source as Game;
            foreach (GridVertex target in source.Adjacent())
            {
                double h = Math.Abs(game.Terrain.HeightMap[target.U, target.V] - game.Terrain.HeightMap[source.U, source.V]);
                if (h <= Unit.Stability)
                {
                    if (!AffectedVertices.ContainsKey(target))
                    {
                        AffectedVertices.Add(target, source);
                        Flood(target, currentSteps + 1);
                    }
                    else if (currentSteps < TraceSteps(target))
                    {
                        AffectedVertices.Remove(target);
                        AffectedVertices.Add(target, source);
                        Flood(target, currentSteps + 1);
                    }
                }
            }
        }

        /// <summary>
        /// Traces a grid vertex step back to the source.
        /// </summary>
        /// <param name="vert">The final vertex.</param>
        /// <returns>The number of steps taken to reach the start.</returns>
        public int TraceSteps(GridVertex vert)
        {
            int steps = 0;
            while (!vert.Equals(Unit.Coords))
            {
                steps++;
                AffectedVertices.TryGetValue(vert, out vert);
            }
            return steps;
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
            Game game = Unit.Engine3D.Source as Game;
            game.UnitController.Entity.RemoveProperty<BasicMeshRenderableProperty>();
            game.UnitController.SelectedAction = null;
        }

        /// <summary>
        /// Executes and then deselects the action. This usually happens when right clicking after an action has been selected.
        /// </summary>
        public override void Execute()
        {
            Game game = Unit.Engine3D.Source as Game;
            GameEngine3D engine = Unit.Engine3D;
            Matrix4 m = engine.MainView.PrimaryMatrix.Inverted();
            m.Transpose();
            float x = 2.0f * engine.Client.MouseX / engine.Window.Width - 1.0f;
            float y = 1.0f - 2.0f * engine.Client.MouseY / engine.Window.Height;
            Vector4 vIn = new Vector4(x, y, 1, 1);
            Vector4 vOut = Vector4.Transform(m, vIn);
            float mul = 1.0f / vOut.W;
            BEPUutilities.Vector3 dir = new BEPUutilities.Vector3(vOut.X * mul, vOut.Y * mul, vOut.Z * mul);
            if (game.Terrain.Body.RayCast(new BEPUutilities.Ray(engine.MainCamera.Position.ToBVector(), dir), 100.0, out BEPUutilities.RayHit hit))
            {
                BEPUutilities.Vector3 loc = hit.Location;
                GridVertex target = GridVertex.FromXY(loc.X, loc.Y);
                game.Terrain.Body.GetPosition(target.U, target.V, out BEPUutilities.Vector3 v);
                if (AffectedVertices.ContainsKey(target))
                {
                    List<GridVertex> steps = new List<GridVertex>();
                    while (!target.Equals(Unit.Coords))
                    {
                        steps.Add(target);
                        AffectedVertices.TryGetValue(target, out target);
                    }
                    Unit.MoveSteps = steps.Reverse<GridVertex>().ToList().GetEnumerator();
                    Unit.MoveSteps.MoveNext();
                    Unit.IsMoving = true;
                    game.UnitController.Entity.RemoveProperty<BasicMeshRenderableProperty>();
                    game.UnitController.SelectedAction = null;
                }
            }
        }
    }
}

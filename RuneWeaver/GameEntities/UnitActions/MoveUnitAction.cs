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
        /// The flood filled affected zone paths of this movement action.
        /// </summary>
        public Dictionary<GridVertex, GridVertex> AffectedPaths;
        
        /// <summary>
        /// Prepares and renders the action's affected zone. This usually happens when the action is selected.
        /// </summary>
        public override void Prepare()
        {
            if (!CheckEnergy())
            {
                return;
            }
            Game game = Unit.Engine3D.Source as Game;
            AffectedPaths = new Dictionary<GridVertex, GridVertex>();
            Flood(Unit.Coords, 0);
            AffectedVertices = new HashSet<GridVertex>(AffectedPaths.Keys);
            AffectedVertices.Remove(Unit.Coords);
            HashSet<GridFace> faces = new HashSet<GridFace>();
            foreach (GridVertex vert in AffectedVertices)
            {
                faces.UnionWith(game.UnitController.OccupiedFaces(Unit.Size, vert));
            }
            GenerateRenderable(faces);
            Select();
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
                if (target.IsInsideBoundaries(game.Terrain.Size))
                {
                    double h = Math.Abs(game.Terrain.HeightMap[target.U, target.V] - game.Terrain.HeightMap[source.U, source.V]);
                    if (h <= Unit.Stability)
                    {
                        if (!AffectedPaths.ContainsKey(target))
                        {
                            AffectedPaths.Add(target, source);
                            Flood(target, currentSteps + 1);
                        }
                        else if (currentSteps < TraceSteps(target))
                        {
                            AffectedPaths.Remove(target);
                            AffectedPaths.Add(target, source);
                            Flood(target, currentSteps + 1);
                        }
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
                AffectedPaths.TryGetValue(vert, out vert);
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
            Unit.Energy -= Cost;
            Game game = Unit.Engine3D.Source as Game;
            GridVertex target = game.CursorController.Target;
            if (AffectedVertices.Contains(target))
            {
                List<GridVertex> steps = new List<GridVertex>();
                while (!target.Equals(Unit.Coords))
                {
                    steps.Add(target);
                    AffectedPaths.TryGetValue(target, out target);
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

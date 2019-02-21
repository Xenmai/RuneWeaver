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
    public class DigUnitAction : BasicUnitAction
    {
        /// <summary>
        /// The height amount of this dig action.
        /// </summary>
        public int Amount;

        /// <summary>
        /// Constructs a new move unit action.
        /// </summary>
        /// <param name="unit">The unit that owns this action.</param>
        /// <param name="cost">The energy cost of this action.</param>
        /// <param name="range">The movement range of this action.</param>
        public DigUnitAction(BasicUnitProperty unit, int cost, int amount) : base(unit, cost)
        {
            this.Amount = amount;
        }
        
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
            AffectedVertices = new HashSet<GridVertex>();
            foreach (GridVertex t1 in Unit.Coords.Adjacent())
            {
                foreach (GridVertex t2 in t1.Adjacent())
                {
                    AffectedVertices.UnionWith(t2.Adjacent());
                }
            }
            AffectedVertices.ExceptWith(Unit.Coords.Adjacent());
            AffectedVertices.Remove(Unit.Coords);
            HashSet<GridFace> faces = new HashSet<GridFace>();
            foreach (GridVertex vert in AffectedVertices)
            {
                faces.UnionWith(vert.Touches());
            }
            GenerateRenderable(faces);
            Select();
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
            SubtractEnergy();
            Game game = Unit.Engine3D.Source as Game;
            GridVertex target = game.CursorController.Target;
            if (AffectedVertices.Contains(target))
            {
                game.Terrain.AdjustVertexHeight(target, -Amount * 0.25f);
                game.UnitController.Entity.RemoveProperty<BasicMeshRenderableProperty>();
                game.UnitController.SelectedAction = null;
            }
        }
    }
}

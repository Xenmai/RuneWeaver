using FreneticGameCore.MathHelpers;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes;
using RuneWeaver.GameRenderables;
using RuneWeaver.MainGame;
using RuneWeaver.TriangularGrid;
using System;
using System.Collections.Generic;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions
{
    /// <summary>
    /// The move unit action class.
    /// </summary>
    public class AttackEachUnitAction : BasicUnitAction
    {
        /// <summary>
        /// The damage amount of this attack action.
        /// </summary>
        public int Damage;

        /// <summary>
        /// The hitbox of this attack action.
        /// </summary>
        public BasicHitbox Hitbox;

        /// <summary>
        /// The affected zone faces of this action.
        /// </summary>
        public HashSet<GridFace> AffectedFaces;

        /// <summary>
        /// Constructs a new attack unit action.
        /// </summary>
        /// <param name="unit">The unit that owns this action.</param>
        /// <param name="cost">The energy cost of this action.</param>
        /// <param name="damage">The damage amount of this action.</param>
        /// <param name="hitbox">The damage hitbox of this action.</param>
        public AttackEachUnitAction(BasicUnitProperty unit, int cost, int damage, BasicHitbox hitbox) : base(unit, cost)
        {
            this.Damage = damage;
            this.Hitbox = hitbox;
        }

        /// <summary>
        /// The attack direction.
        /// </summary>
        public GridVertex Direction = GridVertex.Directions[0];

        /// <summary>
        /// Prepares and renders the action's affected zone. This usually happens when the action is selected.
        /// </summary>
        public override void Prepare()
        {
            if (!CheckEnergy())
            {
                return;
            }
            Generate();
            Select();
        }

        /// <summary>
        /// Generates the afftected vertices set and the action renderable.
        /// </summary>
        public void Generate()
        {
            Game game = Unit.Engine3D.Source as Game;
            AffectedVertices = Hitbox.Area(Unit.Coords, Direction);
            AffectedFaces = new HashSet<GridFace>();
            foreach (GridVertex vert in AffectedVertices)
            {
                AffectedFaces.UnionWith(vert.Touches());
            }
            AffectedFaces.ExceptWith(game.UnitController.OccupiedFaces(Unit.Size, Unit.Coords));
            GenerateRenderable(AffectedFaces);
        }

        /// <summary>
        /// Updates the action's affected zone. This usually happens when the action is selected and the cursor is moved.
        /// </summary>
        public override void Update()
        {
            Game game = Unit.Engine3D.Source as Game;
            Vector2 distance = game.CursorController.Target.ToCartesianCoords2D() - Unit.Coords.ToCartesianCoords2D();
            float degrees = (float)(Math.Atan2(distance.Y, distance.X) * 180 / Math.PI);
            int Angle = (int)(((degrees + 390) % 360) / 60);
            Direction = GridVertex.Directions[Angle];
            game.UnitController.Entity.RemoveProperty<BasicMeshRenderableProperty>();
            Generate();
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
            foreach (GridFace face in AffectedFaces)
            {
                game.UnitFaces[face.U, face.V]?.Hurt(Damage);
            }
            game.UnitController.Entity.RemoveProperty<BasicMeshRenderableProperty>();
            game.UnitController.SelectedAction = null;
        }
    }
}

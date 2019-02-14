using FreneticGameCore.CoreSystems;
using FreneticGameGraphics.GraphicsHelpers;
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
        /// The attack direction.
        /// </summary>
        public GridVertex Direction = GridVertex.Directions[0];

        /// <summary>
        /// Prepares and renders the action's affected zone. This usually happens when the action is selected.
        /// </summary>
        public override void Prepare()
        {
            if(!CheckEnergy())
            {
                return;
            }
            Generate();
            Game game = Unit.Engine3D.Source as Game;
            Select();
        }

        /// <summary>
        /// Generates the afftected vertices set and the action renderable.
        /// </summary>
        public void Generate()
        {
            Game game = Unit.Engine3D.Source as Game;
            AffectedVertices = Hitbox.Area(Unit.Coords, Direction);
            HashSet<GridFace> faces = new HashSet<GridFace>();
            foreach (GridVertex vert in AffectedVertices)
            {
                faces.UnionWith(vert.Touches());
            }
            faces.ExceptWith(game.UnitController.OccupiedFaces(Unit.Size, Unit.Coords));
            GenerateRenderable(faces);
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
            Game game = Unit.Engine3D.Source as Game;
            HashSet<BasicUnitProperty> targets = new HashSet<BasicUnitProperty>();
            foreach (GridVertex vert in Hitbox.Area(Unit.Coords, Direction))
            {
                foreach (GridFace face in vert.Touches())
                {
                    if (game.UnitFaces[face.U, face.V] != null)
                    {
                        targets.UnionWith(new BasicUnitProperty[] { game.UnitFaces[face.U, face.V] });
                    }
                }
            }
            foreach (BasicUnitProperty t in targets)
            {
                t.Hurt(Damage);
            }
            game.UnitController.Entity.RemoveProperty<BasicMeshRenderableProperty>();
            game.UnitController.SelectedAction = null;
        }
    }
}

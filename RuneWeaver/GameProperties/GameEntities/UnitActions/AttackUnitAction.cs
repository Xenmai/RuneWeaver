using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes;
using RuneWeaver.TriangularGrid;

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
        public BasicHitbox Hitbox;

        public override void Prepare(UnitControllerProperty c)
        {
            c.SelectBorders(TriangularGrid.Utilities.ExternalBorders(Hitbox.Faces(c.SelectedUnit.Coords, c.ActionDirection)));
        }

        public override void Cancel(UnitControllerProperty c)
        {
            c.DeselectBorders(TriangularGrid.Utilities.ExternalBorders(Hitbox.Faces(c.SelectedUnit.Coords, c.ActionDirection)));
        }

        public override void Execute(UnitControllerProperty c)
        {
            c.DeselectBorders(TriangularGrid.Utilities.ExternalBorders(Hitbox.Faces(c.SelectedUnit.Coords, c.ActionDirection)));
        }
    }
}

﻿using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes;

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

        public override void Prepare(UnitControllerProperty controller)
        {
            controller.SelectBorders(Hitbox.Borders(controller.SelectedUnit.Coords, 0));
            controller.ExecutingAction = true;
        }

        public override void Cancel(UnitControllerProperty controller)
        {
            controller.DeselectBorders(Hitbox.Borders(controller.SelectedUnit.Coords, 0));
            controller.ExecutingAction = false;
        }

        public override void Execute(UnitControllerProperty controller)
        {
            controller.DeselectBorders(Hitbox.Borders(controller.SelectedUnit.Coords, 0));
            controller.ExecutingAction = false;
        }
    }
}

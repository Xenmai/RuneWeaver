using FreneticGameCore;
using OpenTK;
using RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes;
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
        public BasicHitbox Hitbox = new LineHitbox();
    }
}

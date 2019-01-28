using RuneWeaver.GameProperties.GameEntities.UnitActions;
using RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes;
using System.Collections.Generic;

namespace RuneWeaver.GameProperties.GameEntities
{
    public class GoblinUnitProperty : BasicUnitProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Name = "Goblin";
            Size = 1;
            Vision = 5;
            MaxHealth = 25;
            Resistance = 12;
            Stability = 8;
            Actions.Add(new AttackUnitAction()
            {
                Hitbox = new LineHitbox(2)
            });
            Actions.Add(new MoveUnitAction()
            {
                Range = 6
            });
            base.OnSpawn();
        }
    }
}

using RuneWeaver.GameProperties.GameEntities.UnitActions;
using RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes;

namespace RuneWeaver.GameProperties.GameEntities
{
    public class TrollUnitProperty : BasicUnitProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Name = "Troll";
            Size = 3;
            Vision = 5;
            MaxHealth = 150;
            MaxEnergy = 4;
            Resistance = 30;
            Actions.Add(new AttackUnitAction(this, 4, 15, new LineHitbox(4, 2, 3))
            {
                Name = "Smash",
                Icon = "Sword_Icon"
            });
            Actions.Add(new MoveUnitAction(this, 5, 5));
            base.OnSpawn();
        }
    }
}

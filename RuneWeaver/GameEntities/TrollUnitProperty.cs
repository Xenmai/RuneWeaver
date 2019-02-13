using RuneWeaver.GameProperties.GameEntities.UnitActions;
using RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes;

namespace RuneWeaver.GameProperties.GameEntities
{
    public class TrollUnitProperty : BasicUnitProperty
    {
        /// <summary>
        /// Constructs a new goblin unit property, setting its values.
        /// </summary>
        public TrollUnitProperty()
        {
            Name = "Troll";
            Size = 2;
            Vision = 5;
            MaxHealth = 150;
            MaxEnergy = 4;
            Resistance = 30;
            Stability = 1.0f;
        }

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Actions.Add(new AttackUnitAction(this, 4, 15, new LineHitbox(4, 2, Size))
            {
                Name = "Smash",
                Icon = "Sword_Icon"
            });
            Actions.Add(new MoveUnitAction(this, 5, 5));
            base.OnSpawn();
        }
    }
}

using RuneWeaver.GameProperties.GameEntities.UnitActions;
using RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes;

namespace RuneWeaver.GameProperties.GameEntities
{
    public class GoblinUnitProperty : BasicUnitProperty
    {
        /// <summary>
        /// Constructs a new goblin unit property, setting its values.
        /// </summary>
        public GoblinUnitProperty()
        {
            Name = "Goblin";
            Size = 1;
            Vision = 5;
            MaxHealth = 25;
            MaxEnergy = 5;
            Resistance = 12;
            Stability = 0.8f;
        }
        
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Actions.Add(new AttackUnitAction(this, 3, 5, new LineHitbox(1, 1, Size))
            {
                Name = "Stab",
                Icon = "Sword_Icon"
            });
            Actions.Add(new MoveUnitAction(this, 2, 6));
            base.OnSpawn();
        }
    }
}

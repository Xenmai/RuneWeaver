using RuneWeaver.GameProperties.GameEntities.UnitActions;

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
            Size = 48;
            MaxHealth = 75;
            Resistance = 30;
            Stability = 20;
            MaxEnergy = 3;
            base.OnSpawn();
            AddActions();
        }

        /// <summary>
        /// Adds the default actions for this entity.
        /// </summary>
        private void AddActions()
        {
            Entity.AddProperties(new MoveActionProperty()
            {
                Name = "Charge",
                Cost = 1,
                MaxDistance = 3,
                Speed = 1.2f
            });
        }
    }
}

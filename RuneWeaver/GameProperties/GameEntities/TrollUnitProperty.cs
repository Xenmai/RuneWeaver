using RuneWeaver.GameProperties.GameEntities.UnitActions;

namespace RuneWeaver.GameProperties.GameEntities
{
    class TrollUnitProperty : BasicUnitProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Size = 48;
            MaxHealth = 75;
            Resistance = 30;
            Stability = 20;
            MaxEnergy = 5;
            base.OnSpawn();
            AddActions();
        }

        /// <summary>
        /// Adds the default actions for this entity.
        /// </summary>
        private void AddActions()
        {
            Entity.AddProperty(new ChargeActionProperty()
            {
                Cost = 1,
                MaxDistance = 3,
                Force = 1.2f
            });
        }
    }
}

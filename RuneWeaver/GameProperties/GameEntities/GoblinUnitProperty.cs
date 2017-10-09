using RuneWeaver.GameProperties.GameEntities.UnitActions;

namespace RuneWeaver.GameProperties.GameEntities
{
    class GoblinUnitProperty : BasicUnitProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            MaxHealth = 25;
            Resistance = 12;
            Size = 8;
            MaxEnergy = 5;
            base.OnSpawn();
            AddActions();
        }

        /// <summary>
        /// Adds the default actions for this entity.
        /// </summary>
        private void AddActions()
        {
            Entity.AddProperty(new MoveActionProperty()
            {
                MaxDistance = 30
            });
        }
    }
}

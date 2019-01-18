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
            Vision = 8;
            MaxHealth = 75;
            Resistance = 30;
            Stability = 20;
            base.OnSpawn();
        }
    }
}

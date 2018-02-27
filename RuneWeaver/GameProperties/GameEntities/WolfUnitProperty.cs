namespace RuneWeaver.GameProperties.GameEntities
{
    public class WolfUnitProperty : BasicUnitProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Name = "Wolf";
            Size = 32;
            Vision = 150;
            MaxHealth = 40;
            Resistance = 18;
            Stability = 10;
            MaxEnergy = 4;
            base.OnSpawn();
        }
    }
}

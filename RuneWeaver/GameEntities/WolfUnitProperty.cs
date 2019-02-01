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
            Size = 2;
            Vision = 10;
            MaxHealth = 40;
            Resistance = 18;
            base.OnSpawn();
        }
    }
}

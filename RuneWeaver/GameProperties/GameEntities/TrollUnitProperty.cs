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
            Resistance = 30;
            Actions.Add(new AttackUnitAction()
            {
                Hitbox = new LineHitbox(3, 3, 3)
            });
            Actions.Add(new MoveUnitAction()
            {
                Range = 5
            });
            base.OnSpawn();
        }
    }
}

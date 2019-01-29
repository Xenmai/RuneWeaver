using RuneWeaver.GameProperties.GameEntities.UnitActions;
using RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes;

namespace RuneWeaver.GameProperties.GameEntities
{
    public class GoblinUnitProperty : BasicUnitProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Name = "Goblin";
            Size = 1;
            Vision = 5;
            MaxHealth = 25;
            Resistance = 12;
            Actions.Add(new AttackUnitAction()
            {
                Hitbox = new LineHitbox(1, 1, 1)
            });
            Actions.Add(new MoveUnitAction()
            {
                Range = 6
            });
            base.OnSpawn();
        }
    }
}

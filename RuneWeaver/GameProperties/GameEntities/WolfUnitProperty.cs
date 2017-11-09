using RuneWeaver.GameProperties.GameEntities.UnitActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneWeaver.GameProperties.GameEntities
{
    public class WolfUnitProperty : BasicUnitProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Size = 32;
            MaxHealth = 40;
            Resistance = 18;
            Stability = 10;
            MaxEnergy = 4;
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
                Cost = 2,
                MaxDistance = 5.5f,
                Force = 1
            });
        }
    }
}

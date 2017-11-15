using System;
using RuneWeaver.GameProperties.GameInterfaces;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions
{
    public class BasicActionProperty : CustomClientEntityProperty, IExecutable
    {
        /// <summary>
        /// The action's name.
        /// </summary>
        public string Name;

        /// <summary>
        /// The action's cost.
        /// </summary>
        public int Cost;

        /// <summary>
        /// The unit property.
        /// </summary>
        public BasicUnitProperty Unit;

        /// <summary>
        /// Whether the action is being executed.
        /// </summary>
        public Boolean Executing = false;

        /// <summary>
        /// Whether the action is being prepared.
        /// </summary>
        public Boolean Preparing = false;

        /// <summary>
        /// When the entity that owns the property spawns.
        /// </summary>
        public override void OnSpawn()
        {
        }

        /// <summary>
        /// Prepares the action.
        /// </summary>
        public virtual void Prepare()
        {
            Preparing = true;
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        public virtual void Execute()
        {
            Unit.Energy -= Cost;
            Executing = true;
            Preparing = false;
        }

        /// <summary>
        /// Cancels the action.
        /// </summary>
        public virtual void Cancel()
        {
            Preparing = false;
        }

        /// <summary>
        /// Checks if the unit has enough energy to perform this action.
        /// </summary>
        public bool CheckEnergy()
        {
            return Unit.Energy >= Cost;
        }
    }
}

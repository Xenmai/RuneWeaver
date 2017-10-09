using System;
using RuneWeaver.GameProperties.GameInterfaces;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions
{
    class BasicActionProperty : CustomClientEntityProperty, IExecutable
    {
        /// <summary>
        /// The action's cost.
        /// </summary>
        public int Cost;

        /// <summary>
        /// Whether the action is being executed.
        /// </summary>
        public Boolean Executing = false;

        /// <summary>
        /// Whether the action is being prepared.
        /// </summary>
        public Boolean Preparing = false;

        /// <summary>
        /// Executes the action.
        /// </summary>
        public override void OnSpawn()
        {

        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        public virtual void Execute()
        {
            Executing = true;
            Preparing = false;
        }

        /// <summary>
        /// Prepares the action.
        /// </summary>
        public virtual void Prepare()
        {
            Preparing = true;
        }

        /// <summary>
        /// Cancels the action.
        /// </summary>
        public virtual void Cancel()
        {
            Preparing = false;
        }
    }
}

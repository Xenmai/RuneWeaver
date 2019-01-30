using RuneWeaver.GameProperties.GameControllers;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions
{
    /// <summary>
    /// The basic action class.
    /// </summary>
    public abstract class BasicUnitAction
    {
        /// <summary>
        /// The owner of this action.
        /// </summary>
        public BasicUnitProperty Unit;

        /// <summary>
        /// The energy cost of this action.
        /// </summary>
        public int Cost;

        /// <summary>
        /// The icon that will be used for this action.
        /// </summary>
        public string Icon;

        public BasicUnitAction(BasicUnitProperty unit)
        {
            this.Unit = unit;
        }

        public abstract void Prepare();

        public abstract void Update();

        public abstract void Cancel();

        public abstract void Execute();
    }
}

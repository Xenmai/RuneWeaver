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

        public abstract void Prepare(UnitControllerProperty controller);

        public abstract void Cancel(UnitControllerProperty controller);

        public abstract void Execute(UnitControllerProperty controller);
    }
}

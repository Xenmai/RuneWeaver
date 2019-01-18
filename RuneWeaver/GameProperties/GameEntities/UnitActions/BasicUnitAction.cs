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
        /// The cost of this action.
        /// </summary>
        public int Cost;
    }
}

using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.TriangularGrid;
using System.Collections.Generic;

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

        /// <summary>
        /// The name of this unit action.
        /// </summary>
        public string Name;

        /// <summary>
        /// The affected zone vertices of this action.
        /// </summary>
        public HashSet<GridVertex> AffectedVertices;

        /// <summary>
        /// Constructs a new basic unit action.
        /// </summary>
        /// <param name="unit">The unit that has this action.</param>
        /// <param name="cost">The energy cost of this action.</param>
        public BasicUnitAction(BasicUnitProperty unit, int cost)
        {
            this.Unit = unit;
            this.Cost = cost;
        }

        public abstract void Prepare();

        public abstract void Update();

        public abstract void Cancel();

        public abstract void Execute();
    }
}

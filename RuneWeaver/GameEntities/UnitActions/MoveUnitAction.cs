using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.TriangularGrid;
using System.Collections.Generic;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions
{
    /// <summary>
    /// The move unit action class.
    /// </summary>
    public class MoveUnitAction : BasicUnitAction
    {
        /// <summary>
        /// The range of this move action.
        /// </summary>
        public int Range;

        public MoveUnitAction(BasicUnitProperty unit, int cost, int range) : base(unit, cost)
        {
            this.Range = range;
        }

        public override void Prepare()
        {
            
        }

        public override void Update()
        {

        }

        public override void Cancel()
        {
            
        }

        public override void Execute()
        {
            
        }
    }
}

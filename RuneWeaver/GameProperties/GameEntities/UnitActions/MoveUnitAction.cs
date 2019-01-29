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

        public override void Prepare(UnitControllerProperty controller)
        {
            controller.SelectBorders(Borders(controller.SelectedUnit.Coords));
            controller.SelectedAction = this;
        }

        public override void Cancel(UnitControllerProperty controller)
        {
            controller.DeselectBorders(Borders(controller.SelectedUnit.Coords));
            controller.SelectedAction = null;
        }

        public override void Execute(UnitControllerProperty controller)
        {
            controller.DeselectBorders(Borders(controller.SelectedUnit.Coords));
            controller.SelectedAction = null;
        }

        public List<GridEdge> Borders(GridVertex source)
        {
            return TriangularGrid.Utilities.ExternalBorders(TriangularGrid.Utilities.Expand(source.Touches(), Range));
        }
    }
}

using RuneWeaver.TriangularGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes
{
    public abstract class BasicHitbox
    {
        /// <summary>
        /// Returns this hitbox' affected faces.
        /// </summary>
        /// <param name="source">The source vertex of the hitbox.</param>
        /// <param name="direction">The direction of the hitbox.</param>
        /// <returns>This hitbox' affected faces.</returns>
        public abstract List<GridEdge> Borders(GridVertex source, int direction);

        /// <summary>
        /// Returns this hitbox' borders.
        /// </summary>
        /// <param name="source">The source vertex of the hitbox.</param>
        /// <param name="direction">The direction of the hitbox.</param>
        /// <returns>This hitbox' borders.</returns>
        public abstract List<GridFace> Faces(GridVertex source, int direction);
    }
}

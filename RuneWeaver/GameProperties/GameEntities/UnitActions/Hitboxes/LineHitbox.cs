using RuneWeaver.TriangularGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes
{
    public class LineHitbox : BasicHitbox
    {
        public override List<GridFace> Faces(GridVertex source, int direction)
        {
            switch (direction)
            {
                case 0:
                    return new List<GridFace>(new GridFace[] {
                    new GridFace(source.U - 1, source.V + 1, 0),
                    new GridFace(source.U - 1, source.V + 1, 1),
                    new GridFace(source.U, source.V + 1, 0),
                    new GridFace(source.U, source.V, 1),
                    new GridFace(source.U - 1, source.V + 2, 0),
                    new GridFace(source.U - 1, source.V + 2, 1),
                    new GridFace(source.U, source.V + 2, 0),
                    new GridFace(source.U, source.V + 1, 1)});
                default:
                    return new List<GridFace>();
            }
        }

        public override List<GridEdge> Borders(GridVertex source, int direction)
        {
            switch (direction)
            {
                case 0:
                    return new List<GridEdge>(new GridEdge[] {
                    new GridEdge(source.U - 1, source.V + 1, 1),
                    new GridEdge(source.U - 1, source.V + 2, 1),
                    new GridEdge(source.U - 1, source.V + 3, 0),
                    new GridEdge(source.U, source.V + 2, 2),
                    new GridEdge(source.U + 1, source.V + 1, 1),
                    new GridEdge(source.U + 1, source.V, 1)});
                default:
                    return new List<GridEdge>();
            }
        }
    }
}

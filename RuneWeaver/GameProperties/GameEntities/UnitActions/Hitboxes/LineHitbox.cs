using RuneWeaver.TriangularGrid;
using System.Collections.Generic;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes
{
    public class LineHitbox : BasicHitbox
    {
        public int Range;

        public LineHitbox(int range)
        {
            this.Range = range;
        }

        public override List<GridFace> Faces(GridVertex source, int direction)
        {
            List<GridFace> list = new List<GridFace>();
            switch (direction)
            {
                case 0:
                    for(int i = 0; i < Range; i++)
                    {
                        int V = source.V + i;
                        list.AddRange(new GridFace[] {
                            new GridFace(source.U - 1, V + 1, 0),
                            new GridFace(source.U - 1, V + 1, 1),
                            new GridFace(source.U, V + 1, 0),
                            new GridFace(source.U, V, 1)});
                    }
                    break;
                default:
                    return new List<GridFace>();
            }
            return list;
        }

        public override List<GridEdge> Borders(GridVertex source, int direction)
        {
            return TriangularGrid.Utilities.ExternalBorders(Faces(source, direction));
        }
    }
}

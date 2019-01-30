using FreneticGameCore;
using OpenTK;
using RuneWeaver.TriangularGrid;
using System.Collections.Generic;
using System.Linq;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes
{
    public class LineHitbox : BasicHitbox
    {
        public int Range;

        public int Width;

        public int Offset;

        public LineHitbox(int range, int width, int offset)
        {
            this.Range = range;
            this.Width = width;
            this.Offset = offset;
        }

        public override List<GridFace> Faces(GridVertex source, GridVertex direction)
        {
            List<GridFace> faces = new List<GridFace>();
            GridVertex left = direction.Rotate(-2);
            GridVertex right = direction.Rotate(2);
            for (int i = Offset; i < Range + Offset; i++)
            {
                int U = source.U + i * direction.U;
                int V = source.V + i * direction.V;
                for(int j = 0; j < Width; j++)
                {
                    int a = left.U * j;
                    int b = left.V * j;
                    int c = right.U * j;
                    int d = right.V * j;
                    faces = new List<GridFace>(faces.Union(new GridVertex(U + a, V + b).Touches()).Union(new GridVertex(U + c, V + d).Touches()));
                }
            }
            return new List<GridFace>(faces.Except(Utilities.GridHelper.Expand(source.Touches(), (Offset - 1) * 2)));
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace RuneWeaver.TriangularGrid
{
    public static class Utilities
    {
        public static List<GridEdge> ExternalBorders(List<GridFace> faces)
        {
            List<GridEdge> edges = new List<GridEdge>();
            foreach(GridFace face in faces)
            {
                edges.AddRange(face.Borders().Except(edges));
            }
            return edges;
        }

        public static List<GridFace> Expand(List<GridFace> faces, int times)
        {
            List<GridFace> f = new List<GridFace>(faces);
            for(int i = 0; i < times; i++)
            {
                foreach(GridFace face in new List<GridFace>(f))
                {
                    f.AddRange(face.Neighbors().Except(f));
                }
            }
            return f;
        }
    }
}

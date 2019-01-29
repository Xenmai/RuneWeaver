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
                edges = new List<GridEdge>(edges.Union(face.Borders()).Except(edges.Intersect(face.Borders())));
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
                    f = new List<GridFace>(f.Union(face.Neighbors()));
                }
            }
            return f;
        }

        public static GridVertex[] Directions = new GridVertex[6] {
            new GridVertex(1, 0),
            new GridVertex(0, 1),
            new GridVertex(-1, 1),
            new GridVertex(-1, 0),
            new GridVertex(0, -1),
            new GridVertex(1, -1)};
    }
}

using System.Collections.Generic;

namespace RuneWeaver.TriangularGrid
{
    /// <summary>
    /// Represents a vertex object in the triangular grid.
    /// </summary>
    public struct GridVertex
    {
        /// <summary>
        /// Constructs a new grid vertex object.
        /// </summary>
        /// <param name="u">The first coordinate.</param>
        /// <param name="v">The second coordinate.</param>
        public GridVertex(int u, int v)
        {
            this.U = u;
            this.V = v;
        }

        /// <summary>
        /// The first coordinate.
        /// </summary>
        public int U;

        /// <summary>
        /// The second coordinate.
        /// </summary>
        public int V;

        public List<GridFace> Touches()
        {
            return new List<GridFace>(new GridFace[] {
                new GridFace(U - 1, V, 1),
                new GridFace(U, V, 0),
                new GridFace(U, V - 1, 1),
                new GridFace(U, V - 1, 0),
                new GridFace(U - 1, V - 1, 1),
                new GridFace(U - 1, V, 0)});
        }

        public List<GridEdge> Protrudes()
        {
            return new List<GridEdge>(new GridEdge[] {
                    new GridEdge(U, V, 1),
                    new GridEdge(U, V, 0),
                    new GridEdge(U, V - 1, 2),
                    new GridEdge(U, V - 1, 1),
                    new GridEdge(U - 1, V, 0),
                    new GridEdge(U - 1, V, 2)});
        }

        public List<GridVertex> Adjacent()
        {
            return new List<GridVertex>(new GridVertex[] {
                    new GridVertex(U, V + 1),
                    new GridVertex(U + 1, V),
                    new GridVertex(U + 1, V - 1),
                    new GridVertex(U, V - 1),
                    new GridVertex(U - 1, V),
                    new GridVertex(U - 1, V + 1)});
        }


    }
}

using System.Collections.Generic;

namespace RuneWeaver.TriangularGrid
{
    /// <summary>
    /// Represents an edge object in the triangular grid.
    /// </summary>
    public struct GridEdge
    {
        /// <summary>
        /// Constructs a new grid edge object.
        /// </summary
        /// <param name="u">The first coordinate.</param>
        /// <param name="v">The second coordinate.</param>
        /// <param name="side">The side coordinate: 0 = S, 1 = E, 2 = W.</param>
        public GridEdge(int u, int v, int side)
        {
            this.U = u;
            this.V = v;
            this.Side = side;
        }

        /// <summary>
        /// The first coordinate.
        /// </summary>
        public int U;

        /// <summary>
        /// The second coordinate.
        /// </summary>
        public int V;

        /// <summary>
        /// The side coordinate.
        /// </summary>
        public int Side;

        public List<GridFace> Joins()
        {
            if (Side == 0)
            {
                return new List<GridFace>(new GridFace[] {
                    new GridFace(U, V, 0),
                    new GridFace(U, V - 1, 1)});
            }
            else if(Side == 1)
            {
                return new List<GridFace>(new GridFace[] {
                    new GridFace(U, V, 0),
                    new GridFace(U - 1, V, 1)});
            }
            else
            {
                return new List<GridFace>(new GridFace[] {
                    new GridFace(U, V, 1),
                    new GridFace(U, V, 0)});
            }
        }

        public List<GridEdge> Continues()
        {
            if (Side == 0)
            {
                return new List<GridEdge>(new GridEdge[] {
                    new GridEdge(U + 1, V, 0),
                    new GridEdge(U - 1, V, 0)});
            }
            else if (Side == 1)
            {
                return new List<GridEdge>(new GridEdge[] {
                    new GridEdge(U, V + 1, 2),
                    new GridEdge(U, V - 1, 2)});
            }
            else
            {
                return new List<GridEdge>(new GridEdge[] {
                    new GridEdge(U + 1, V - 1, 1),
                    new GridEdge(U - 1, V + 1, 1)});
            }
        }

        public List<GridVertex> EndPoints()
        {
            if (Side == 0)
            {
                return new List<GridVertex>(new GridVertex[] {
                    new GridVertex(U + 1, V),
                    new GridVertex(U, V)});
            }
            else if (Side == 1)
            {
                return new List<GridVertex>(new GridVertex[] {
                    new GridVertex(U, V + 1),
                    new GridVertex(U, V)});
            }
            else
            {
                return new List<GridVertex>(new GridVertex[] {
                    new GridVertex(U + 1, V),
                    new GridVertex(U, V + 1)});
            }
        }

    }
}

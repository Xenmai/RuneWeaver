using OpenTK;
using System;
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

        /// <summary>
        /// Converts this vertex' position from triangular to 3D cartesian coords.
        /// </summary>
        /// <returns>The vertex position in cartesian coordinates.</returns>
        public Vector3 ToCartesianCoords3D(float h)
        {
            return new Vector3(U + V * 0.5f, V * 0.866f, h);
        }

        /// <summary>
        /// Converts this vertex' position from triangular to 2D cartesian coords.
        /// </summary>
        /// <returns>The vertex position in cartesian coordinates.</returns>
        public Vector2 ToCartesianCoords2D()
        {
            return new Vector2(U + V * 0.5f, V * 0.866f);
        }

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

        public GridVertex Rotate(int times)
        {
            GridVertex old = this;
            int index = Array.FindIndex(Utilities.GridHelper.Directions, item => item.Equals(old));
            return Utilities.GridHelper.Directions[(index + times + 6) % 6];
        }
    }
}

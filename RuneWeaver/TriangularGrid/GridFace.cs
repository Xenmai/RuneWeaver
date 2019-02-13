using OpenTK;
using System;
using System.Collections.Generic;

namespace RuneWeaver.TriangularGrid
{
    /// <summary>
    /// Represents a face object in the triangular grid.
    /// </summary>
    public struct GridFace
    {
        /// <summary>
        /// The first coordinate.
        /// </summary>
        public int U;

        /// <summary>
        /// The second coordinate.
        /// </summary>
        public int V;

        /// <summary>
        /// wether the triangle face points upwards.
        /// </summary>
        /// <returns>True if the triangle points up.</returns>
        public bool PointsUp()
        {
            return (U + V) % 2 == 1;
        }

        /// <summary>
        /// Converts this vertex' position from triangular to cartesian coords.
        /// </summary>
        /// <returns></returns>
        public Vector2 ToCartesianCoords2D()
        {
            return new Vector2(U * 0.6f, V * 0.866f);
        }

        /// <summary>
        /// Constructs a new grid face object.
        /// </summary>
        /// <param name="u">The first coordinate.</param>
        /// <param name="v">The second coordinate.</param>
        public GridFace(int u, int v)
        {
            this.U = u;
            this.V = v;
        }

        /// <summary>
        /// Returns the list of faces that are neighbors of this face.
        /// </summary>
        /// <returns>A list of neighbor faces.</returns>
        public List<GridFace> Neighbors()
        {
            if (PointsUp())
            {
                return new List<GridFace>(new GridFace[] {
                    new GridFace(U, V - 1),
                    new GridFace(U + 1, V),
                    new GridFace(U - 1, V)});
            }
            else
            {
                return new List<GridFace>(new GridFace[] {
                    new GridFace(U, V + 1),
                    new GridFace(U + 1, V),
                    new GridFace(U - 1, V)});
            }
        }

        /// <summary>
        /// Returns the list of vertices that are corners of this face.
        /// </summary>
        /// <returns>A list of corner vertices.</returns>
        public List<GridVertex> Corners()
        {
            if (PointsUp())
            {
                return new List<GridVertex>(new GridVertex[] {
                    new GridVertex(U, V + 1),
                    new GridVertex(U + 1, V),
                    new GridVertex(U - 1, V)});
            }
            else
            {
                return new List<GridVertex>(new GridVertex[] {
                    new GridVertex(U, V),
                    new GridVertex(U - 1, V + 1),
                    new GridVertex(U + 1, V + 1)});
            }
        }

        /// <summary>
        /// Wether this grid face equals an object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>Wether they are equal.</returns>
        public override bool Equals(object obj)
        {
            GridFace face = (GridFace)obj;
            return face.U == U && face.V == V;
        }

        /// <summary>
        /// Gets the hashcode of this grid vertex.
        /// </summary>
        /// <returns>The hashcode of this grid vertex.</returns>
        public override int GetHashCode()
        {
            return U.GetHashCode() + V.GetHashCode();
        }
    }
}

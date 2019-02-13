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
        /// The side coordinate.
        /// </summary>
        public int Side;

        /// <summary>
        /// Converts this vertex' position from triangular to cartesian coords.
        /// </summary>
        /// <returns></returns>
        public Vector2 ToCartesianCoords()
        {
            switch (Side)
            {
                case 0:
                    float x = (U + 0.5f) + V * 0.5f;
                    float y = (V + 0.333f) * 0.866f;
                    return new Vector2(x, y);
                default:
                    x = (U + 1f) + V * 0.5f;
                    y = (V + 0.667f) * 0.866f;
                    return new Vector2(x, y);
            }
        }

        /// <summary>
        /// Constructs a new grid face object.
        /// </summary>
        /// <param name="u">The first coordinate.</param>
        /// <param name="v">The second coordinate.</param>
        /// <param name="side">The side coordinate: 0 = L, 1 = R.</param>
        public GridFace(int u, int v, int side)
        {
            this.U = u;
            this.V = v;
            this.Side = side;
        }

        /// <summary>
        /// Constructs a new GridFace from a 2D position and a scaling factor.
        /// </summary>
        /// <param name="vector">The 2D position.</param>
        /// <param name="scaling">The scaling factor.</param>
        /// <returns>The GridFace in triangular coordinates.</returns>
        public static GridFace FromVector2(Vector2 vector, float scaling)
        {
            float v = vector.Y / (100 * scaling * 0.866f);
            float u = vector.X / (100 * scaling) - (v * 0.5f);
            int side = (int)Math.Floor(u % 1 + v % 1);
            return new GridFace((int)Math.Floor(u), (int)Math.Floor(v), side);
        }

        public List<GridFace> Neighbors()
        {
            switch (Side)
            {
                case 0:
                    return new List<GridFace>(new GridFace[] {
                    new GridFace(U, V, 1),
                    new GridFace(U, V - 1, 1),
                    new GridFace(U - 1, V, 1)});
                default:
                    return new List<GridFace>(new GridFace[] {
                    new GridFace(U, V + 1, 0),
                    new GridFace(U + 1, V, 0),
                    new GridFace(U, V, 0)});
            }
        }

        public List<GridEdge> Borders()
        {
            switch (Side)
            {
                case 0:
                    return new List<GridEdge>(new GridEdge[] {
                    new GridEdge(U, V, 0),
                    new GridEdge(U, V, 1),
                    new GridEdge(U, V, 2)});
                default:
                    return new List<GridEdge>(new GridEdge[] {
                    new GridEdge(U, V + 1, 0),
                    new GridEdge(U + 1, V, 1),
                    new GridEdge(U, V, 2)});
            }
        }

        public List<GridVertex> Corners()
        {
            switch (Side)
            {
                case 0:
                    return new List<GridVertex>(new GridVertex[] {
                    new GridVertex(U, V),
                    new GridVertex(U, V + 1),
                    new GridVertex(U + 1, V) });
                default:
                    return new List<GridVertex>(new GridVertex[] {
                    new GridVertex(U + 1, V + 1),
                    new GridVertex(U + 1, V),
                    new GridVertex(U, V + 1)});
            }
        }

    }
}

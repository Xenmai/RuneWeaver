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

        public static GridVertex FromXY(double x, double y)
        {
            int newY = (int)Math.Round(y / 0.866);
            int newX;
            if (newY % 2 == 1)
            {
                newX = (int)Math.Round(x + 0.5) * 2 - 1;
            }
            else
            {
                newX = (int)Math.Round(x) * 2;
            }
            return new GridVertex(newX, newY);
        }

        /// <summary>
        /// Converts this vertex' position from triangular to 3D cartesian coords.
        /// </summary>
        /// <returns>The vertex position in cartesian coordinates.</returns>
        public Vector3 ToCartesianCoords3D(double[,] map)
        {
            return new Vector3(U * 0.5f, V * 0.866f, (float)map[U, V]);
        }

        /// <summary>
        /// Converts this vertex' position from triangular to 2D cartesian coords.
        /// </summary>
        /// <returns>The vertex position in cartesian coordinates.</returns>
        public Vector2 ToCartesianCoords2D()
        {
            return new Vector2(U * 0.5f, V * 0.866f);
        }

        /// <summary>
        /// Returns the list of faces this vertex touches.
        /// </summary>
        /// <returns>A list of touched faces.</returns>
        public HashSet<GridFace> Touches()
        {
            return new HashSet<GridFace> {
                new GridFace(U + 1, V),
                new GridFace(U + 1, V - 1),
                new GridFace(U, V - 1),
                new GridFace(U - 1, V - 1),
                new GridFace(U - 1, V),
                new GridFace(U, V)};
        }

        /// <summary>
        /// Returns the list of vertices that are adjacent to this vertex.
        /// </summary>
        /// <returns>A list of adjacent vertices.</returns>
        public HashSet<GridVertex> Adjacent()
        {
            return new HashSet<GridVertex> {
                    new GridVertex(U + 2, V),
                    new GridVertex(U + 1, V - 1),
                    new GridVertex(U - 1, V - 1),
                    new GridVertex(U - 2, V),
                    new GridVertex(U - 1, V + 1),
                    new GridVertex(U + 1, V + 1) };
        }

        /// <summary>
        /// Rotates a grid vector the specified amount of times. +1 is 60 degrees clockwise.
        /// </summary>
        /// <param name="times">How many times to rotate.</param>
        /// <returns>The new rotated grid vector.</returns>
        public GridVertex Rotate(int times)
        {
            GridVertex old = this;
            int index = Array.FindIndex(Directions, vert => vert.Equals(old));
            return Directions[(index + times + 6) % 6];
        }

        /// <summary>
        /// A list of all valid grid directions.
        /// </summary>
        public static readonly GridVertex[] Directions = new GridVertex[6] {
            new GridVertex(2, 0),
            new GridVertex(1, -1),
            new GridVertex(-1, -1),
            new GridVertex(-2, 0),
            new GridVertex(-1, 1),
            new GridVertex(1, 1)
        };

        /// <summary>
        /// Wether this grid vertex equals an object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>Wether they are equal.</returns>
        public override bool Equals(object obj)
        {
            GridVertex vert = (GridVertex)obj;
            return vert.U == U && vert.V == V;
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

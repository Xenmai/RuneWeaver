using FreneticGameCore;
using RuneWeaver.TriangularGrid;
using System.Collections.Generic;
using System.Linq;

namespace RuneWeaver.Utilities
{
    public static class GridHelper
    {
        /// <summary>
        /// Expands a list of faces to occupy the touching faces as well.
        /// </summary>
        /// <param name="faces">The starting faces.</param>
        /// <param name="times">How many times to expand the faces.</param>
        /// <returns></returns>
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

        /// <summary>
        /// A list of all valid grid directions.
        /// </summary>
        public static readonly GridVertex[] Directions = new GridVertex[6] {
            new GridVertex(1, 0),
            new GridVertex(0, 1),
            new GridVertex(-1, 1),
            new GridVertex(-1, 0),
            new GridVertex(0, -1),
            new GridVertex(1, -1)};

        /// <summary>
        /// The grass grid material -> green.
        /// </summary>
        public static readonly GridMaterial Grass = new GridMaterial("Grass", new Color4F(0.3f, 0.8f, 0), false);

        /// <summary>
        /// The dirt grid material -> brown.
        /// </summary>
        public static readonly GridMaterial Dirt = new GridMaterial("Dirt", new Color4F(0.6f, 0.25f, 0), false);

        /// <summary>
        /// The rock grid material -> gray.
        /// </summary>
        public static readonly GridMaterial Rock = new GridMaterial("Rock", new Color4F(0.6f, 0.6f, 0.6f), true);
    }
}

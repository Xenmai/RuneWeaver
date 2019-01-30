using FreneticGameCore;

namespace RuneWeaver.TriangularGrid
{
    public class GridMaterial
    {
        /// <summary>
        /// The grid material name;
        /// </summary>
        public string Name;

        /// <summary>
        /// The grid material color;
        /// </summary>
        public Color4F Color;

        /// <summary>
        /// Constructs a new grid material.
        /// </summary>
        /// <param name="name">The material's name.</param>
        /// <param name="color">The material's color.</param>
        public GridMaterial(string name, Color4F color)
        {
            this.Name = name;
            this.Color = color;
        }
    }
}

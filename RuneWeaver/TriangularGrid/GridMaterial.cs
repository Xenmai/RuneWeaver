using FreneticGameCore.MathHelpers;

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
        /// Whether the material is solid.
        /// </summary>
        public bool Solid;

        /// <summary>
        /// Constructs a new grid material.
        /// </summary>
        /// <param name="name">The material's name.</param>
        /// <param name="color">The material's color.</param>
        public GridMaterial(string name, Color4F color, bool solid)
        {
            this.Name = name;
            this.Color = color;
            this.Solid = solid;
        }
    }
}

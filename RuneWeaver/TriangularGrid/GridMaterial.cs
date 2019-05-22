using FGECore.MathHelpers;

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

        /// <summary>
        /// The grass grid material -> green.
        /// </summary>
        public static readonly GridMaterial Grass = new GridMaterial("Grass", new Color4F(0.3f, 0.8f, 0));

        /// <summary>
        /// The dirt grid material -> brown.
        /// </summary>
        public static readonly GridMaterial Dirt = new GridMaterial("Dirt", new Color4F(0.6f, 0.25f, 0));

        /// <summary>
        /// The rock grid material -> gray.
        /// </summary>
        public static readonly GridMaterial Rock = new GridMaterial("Rock", new Color4F(0.6f, 0.6f, 0.6f));

        /// <summary>
        /// The water grid material -> blue.
        /// </summary>
        public static readonly GridMaterial Water = new GridMaterial("Water", new Color4F(0.0f, 0.2f, 0.8f));
    }
}

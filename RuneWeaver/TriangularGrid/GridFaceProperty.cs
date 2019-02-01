using FreneticGameCore.MathHelpers;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.MainGame;

namespace RuneWeaver.TriangularGrid
{
    /// <summary>
    /// Represents a an entity that rotates.
    /// </summary>
    public class GridFaceProperty : ClientEntityProperty
    {

        /// <summary>
        /// This face's triangular coordinates.
        /// </summary>
        public GridFace Coords;

        /// <summary>
        /// This face's material, currently used for coloring.
        /// </summary>
        public GridMaterial Material;

        /// <summary>
        /// The grid triangle renderable.
        /// </summary>
        public EntitySimple2DRenderableBoxProperty Renderable;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Game game = Engine2D.Source as Game;
            game.Faces[Coords.U, Coords.V, Coords.Side] = this;
            float scaling = game.GetScaling();
            Renderable = new EntitySimple2DRenderableBoxProperty()
            {
                BoxSize = new Vector2(scaling * 100, scaling * 86.6f),
                BoxTexture = Engine2D.Textures.GetTexture("Triangle"),
                CastShadows = false
            };
            Entity.AddProperties(Renderable);
            float x;
            float y;
            if (Coords.Side == 0) {
                x = (Coords.U + 0.5f + Coords.V * 0.5f) * 100 * scaling;
                y = (Coords.V + 0.5f) * 86.6f * scaling;
            }
            else
            {
                x = (Coords.U + 1 + Coords.V * 0.5f) * 100 * scaling;
                y = (Coords.V + 0.5f) * 86.6f * scaling;
                Renderable.RenderAngle = MathHelper.Pi;
            }
            Renderable.BoxColor = Material.Color;
            Entity.SetPosition(new Location(x, y, 0));
        }

        public void ChangeMaterial(GridMaterial newMat)
        {
            Material = newMat;
            Renderable.BoxColor = newMat.Color;
        }

        /// <summary>
        /// Fired when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
        }

        /// <summary>
        /// Ticks the entity.
        /// </summary>
        public void Tick()
        {
            
        }
    }
}

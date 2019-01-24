using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.MainGame;

namespace RuneWeaver.TriangularGrid
{
    /// <summary>
    /// Represents a an entity that rotates.
    /// </summary>
    public class GridEdgeProperty : ClientEntityProperty
    {

        /// <summary>
        /// This face's triangular coordinates.
        /// </summary>
        public GridEdge Coords;

        /// <summary>
        /// This face's material, currently used for coloring.
        /// </summary>
        public string Material;

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
            game.Edges[Coords.U, Coords.V, Coords.Side] = this;
            float scaling = game.GetScaling();
            Renderable = new EntitySimple2DRenderableBoxProperty()
            {
                BoxSize = new Vector2(scaling * 100, scaling * 2),
                BoxTexture = Engine2D.Textures.GetTexture("White"),
                CastShadows = false
            };
            Entity.AddProperties(Renderable);
            Renderable.BoxColor = Color4F.Black;
            float x;
            float y;
            if (Coords.Side == 0) {
                x = (Coords.U + 0.5f + Coords.V * 0.5f) * 100 * scaling;
                y = Coords.V * 0.866f * 100 * scaling;
            }
            else if (Coords.Side == 1)
            {
                x = (Coords.U + 0.25f + Coords.V * 0.5f) * 100 * scaling;
                y = (Coords.V + 0.5f) * 86.6f * scaling;
                Renderable.RenderAngle = MathHelper.PiOver3;
            }
            else
            {
                x = (Coords.U + 0.75f + Coords.V * 0.5f) * 100 * scaling;
                y = (Coords.V + 0.5f) * 86.6f * scaling;
                Renderable.RenderAngle = -MathHelper.PiOver3;
            }
            Entity.SetPosition(new Location(x, y, 2));
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

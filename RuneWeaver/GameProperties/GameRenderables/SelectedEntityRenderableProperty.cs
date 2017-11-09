using FreneticGameCore;
using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using FreneticGameGraphics.GraphicsHelpers;
using OpenTK;

namespace RuneWeaver.GameProperties.GameRenderables
{
    /// <summary>
    /// Renders a simple 2D circle effect.
    /// </summary>
    public class SelectedEntityRenderableProperty : Entity2DRenderableProperty
    {
        /// <summary>
        /// The circle's center.
        /// </summary>
        public Vector2 Center
        {
            get
            {
                return new Vector2((float)RenderAt.X, (float)RenderAt.Y);
            }
            set
            {
                Entity.SetPosition(new Location(value.X, value.Y, -5));
            }
        }

        /// <summary>
        /// The circle radius.
        /// </summary>
        public float Radius;

        /// <summary>
        /// What color to render the box as.
        /// </summary>
        public Color4F Color = Color4F.White;

        /// <summary>
        /// Render the entity as seen normally, in 2D.
        /// </summary>
        /// <param name="context">The render context.</param>
        public override void RenderStandard2D(RenderContext2D context)
        {
            context.Engine.Textures.White.Bind();
            context.Engine.RenderHelper.SetColor(Color);
            Vector2 center = Center;
            context.Engine.RenderHelper.RenderRectangle(context, center.X - Radius, center.Y + Radius,
                center.X + Radius, center.Y - Radius);
        }
    }
}

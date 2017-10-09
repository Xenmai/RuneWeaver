using FreneticGameCore;
using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using FreneticGameGraphics.GraphicsHelpers;

namespace RuneWeaver.GameProperties.GameRenderables
{
    /// <summary>
    /// Renders a simple 2D circle effect.
    /// </summary>
    public class SelectedEntityRenderableProperty : Entity2DRenderableProperty
    {
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
            context.Engine.RenderHelper.RenderRectangle(context, (float)RenderAt.X - Radius, (float)RenderAt.Y + Radius,
                (float)RenderAt.X + Radius, (float)RenderAt.Y - Radius);
        }
    }
}

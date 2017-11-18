using FreneticGameCore;
using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using System;

namespace RuneWeaver.GameProperties.GameRenderables
{
    public class ArrowHitboxRenderableProperty : Entity2DRenderableProperty
    {
        /// <summary>
        /// The arrow's start point.
        /// </summary>
        public Vector2 Start
        {
            get
            {
                return new Vector2((float)RenderAt.X, (float)RenderAt.Y);
            }
            set
            {
                Entity.SetPosition(new Location(value.X, value.Y, 3));
            }
        }

        /// <summary>
        /// The arrow's length.
        /// </summary>
        public float Length;

        /// <summary>
        /// The arrow's width.
        /// </summary>
        public float Width;

        /// <summary>
        /// The arrow's angle.
        /// </summary>
        public float Angle;

        /// <summary>
        /// What color to render the arrow as.
        /// </summary>
        public Color4F Color = Color4F.Black;

        /// <summary>
        /// Render the entity as seen normally, in 2D.
        /// </summary>
        /// <param name="context">The render context.</param>
        public override void RenderStandard2D(RenderContext2D context)
        {
            context.Engine.Textures.GetTexture("5arrows").Bind();
            context.Engine.RenderHelper.SetColor(Color);
            float halfWidth = Width * 0.5f;
            context.Engine.RenderHelper.RenderRectangle(context, Start.X, Start.Y - halfWidth, Start.X + Length, Start.Y + halfWidth, new Vector3(0, -0.5f, Angle));
        }
    }
}

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
            Vector2 corner = Start + new Vector2((float)Math.Sin(Angle) * halfWidth, (float)-Math.Cos(Angle) * halfWidth);
            context.Engine.RenderHelper.RenderRectangle(context, corner.X, corner.Y, corner.X + Length, corner.Y + Width, new Vector3(0, 0, Angle));
        }
    }
}

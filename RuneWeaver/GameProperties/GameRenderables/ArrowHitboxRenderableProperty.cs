﻿using FreneticGameCore;
using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using FreneticGameGraphics.GraphicsHelpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneWeaver.GameProperties.GameRenderables
{
    class ArrowHitboxRenderableProperty : Entity2DRenderableProperty
    {
        /// <summary>
        /// The start point.
        /// </summary>
        public Vector2 Start;

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
        public Color4F Color = Color4F.White;

        /// <summary>
        /// Render the entity as seen normally, in 2D.
        /// </summary>
        /// <param name="context">The render context.</param>
        public override void RenderStandard2D(RenderContext2D context)
        {
            context.Engine.Textures.White.Bind();
            context.Engine.RenderHelper.SetColor(Color);
            float halfWidth = Width * 0.5f;
            Vector2 corner = Start + new Vector2((float)Math.Sin(Angle) * halfWidth, (float)-Math.Cos(Angle) * halfWidth);
            context.Engine.RenderHelper.RenderRectangle(context, corner.X, corner.Y, corner.X + Length, corner.Y + Width, new Vector3(0, 0, Angle));
        }
    }
}
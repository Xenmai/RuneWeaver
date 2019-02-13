using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using FreneticGameCore.MathHelpers;
using FreneticGameGraphics;
using FreneticGameGraphics.GraphicsHelpers;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using FreneticGameCore.CoreSystems;

namespace RuneWeaver.GameRenderables
{
    public class BasicMeshRenderableProperty : EntityRenderableProperty
    {
        /// <summary>
        /// The render scale.
        /// </summary>
        public Location Scale = new Location(1, 1, 1);

        /// <summary>
        /// The terrain grid renderable.
        /// </summary>
        public Renderable Rend;

        /// <summary>
        /// The diffuse color texture.
        /// </summary>
        public Texture DiffuseTexture;

        /// <summary>
        /// Render the entity as seen normally, in 3D.
        /// </summary>
        /// <param name="context">The render context.</param>
        public override void RenderStandard(RenderContext context)
        {
            if (DiffuseTexture != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                DiffuseTexture.Bind();
            }
            context.Engine.Rendering.SetColor(Color4F.White, context.Engine.MainView);
            Matrix4d mat = Matrix4d.Scale(Scale.ToOpenTK3D()) * Matrix4d.CreateFromQuaternion(RenderOrientation.ToOpenTKDoubles()) * Matrix4d.CreateTranslation(RenderAt.ToOpenTK3D());
            context.Engine.MainView.SetMatrix(ShaderLocations.Common.WORLD, mat);
            Rend.Render(context, true);
        }

        /// <summary>
        /// Render the entity as seen by a top-down map.
        /// </summary>
        /// <param name="context">The render context.</param>
        public override void RenderForTopMap(RenderContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Non-implemented 2D option.
        /// </summary>
        /// <param name="context">The 2D render context.</param>
        public override void RenderStandard2D(RenderContext2D context)
        {
            throw new NotImplementedException();
        }
    }
}

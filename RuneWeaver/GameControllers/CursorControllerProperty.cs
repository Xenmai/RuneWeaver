using FreneticGameCore.MathHelpers;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.MainGame;
using RuneWeaver.TriangularGrid;

namespace RuneWeaver.GameControllers
{
    public class CursorControllerProperty : ClientEntityProperty
    {
        /// <summary>
        /// The cursor renderable property.
        /// </summary>
        public EntitySimple3DRenderableModelProperty Rend;

        /// <summary>
        /// The target grid vertex the cursor is on.
        /// </summary>
        public GridVertex Target;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Game game = Engine3D.Source as Game;
            Rend = new EntitySimple3DRenderableModelProperty()
            {
                EntityModel = game.Client.Models.Cylinder,
                Scale = new Location(0.1, 0.1, 1),
                DiffuseTexture = game.Client.Textures.White,
                Color = Color4F.Red,
                IsVisible = false
            };
            Entity.AddProperty(Rend);
            Entity.OnTick += Tick;
        }

        /// <summary>
        /// Fired when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
            Entity.OnTick -= Tick;
        }

        /// <summary>
        /// Fired when the entity ticks.
        /// </summary>
        public void Tick()
        {
            Game game = Engine3D.Source as Game;
            Matrix4 m = Engine3D.MainView.PrimaryMatrix.Inverted();
            m.Transpose();
            float x = 2.0f * Engine3D.Client.MouseX / Engine3D.Window.Width - 1.0f;
            float y = 1.0f - 2.0f * Engine3D.Client.MouseY / Engine3D.Window.Height;
            Vector4 vIn = new Vector4(x, y, 1, 1);
            Vector4 vOut = Vector4.Transform(m, vIn);
            float mul = 1.0f / vOut.W;
            BEPUutilities.Vector3 dir = new BEPUutilities.Vector3(vOut.X * mul, vOut.Y * mul, vOut.Z * mul);
            if (game.Terrain.Body.RayCast(new BEPUutilities.Ray(Engine3D.MainCamera.Position.ToBVector(), dir), 100.0, out BEPUutilities.RayHit hit))
            {
                BEPUutilities.Vector3 loc = hit.Location;
                Target = GridVertex.FromXY(loc.X, loc.Y);
                Rend.IsVisible = true;
                Vector2 pos = Target.ToCartesianCoords2D();
                Entity.SetPosition(new Location(pos.X, pos.Y, game.Terrain.HeightMap[Target.U, Target.V] + 0.5));
            }
            else
            {
                Rend.IsVisible = false;
            }
        }
    }
}

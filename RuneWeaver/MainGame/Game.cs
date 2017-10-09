using FreneticGameCore;
using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using FreneticGameCore.EntitySystem.PhysicsHelpers;
using RuneWeaver.GameProperties;
using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.GameProperties.GameEntities;

namespace RuneWeaver.MainGame
{
    /// <summary>
    /// The game's entry point.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The primary backing game client.
        /// </summary>
        public GameClientWindow Client;

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void Start()
        {
            Client = new GameClientWindow(threed: false);
            Client.Engine2D.UseLightEngine = true;
            Client.OnWindowLoad += Engine_WindowLoad;
            Client.Engine2D.Zoom = 0.1f;
            Client.Engine2D.Source = this;
            Client.Start();
        }

        /// <summary>
        /// Called by the engine when it loads up.
        /// </summary>
        public void Engine_WindowLoad()
        {
            Client.Window.KeyDown += Window_KeyDown;
            SysConsole.OutputCustom("info", "width: " + Client.Window.Width + " & height: " + Client.Window.Height);
            Client.Engine2D.PhysicsWorld.Gravity = new Location(0, 0, -9.81);
            // Ground
            Client.Engine2D.SpawnEntity(new EntitySimple2DRenderableBoxProperty()
            {
                BoxSize = new Vector2(204.8f, 200),
                BoxColor = Color4F.Green,
                BoxTexture = Client.Engine2D.Textures.White,
                CastShadows = false
            }, new ClientEntityPhysicsProperty()
            {
                Position = new Location(0, 0, -10),
                Shape = new EntityBoxShape() { Size = new Location(204.8f, 200, 10) },
                Mass = 0
            });
            // Entity 1
            Client.Engine2D.SpawnEntity(new EntitySimple2DRenderableBoxProperty(), new ClientEntityPhysicsProperty()
            {
                Mass = 10
            }, new UnitEntityProperty()
            {
                Size = 10f,
                Position = new Vector2(75, 75)
            });
            // Entity 2
            Client.Engine2D.SpawnEntity(new EntitySimple2DRenderableBoxProperty(), new ClientEntityPhysicsProperty()
            {
                Mass = 20
            }, new UnitEntityProperty()
            {
                Size = 15f,
                Position = new Vector2(20, 35)
            });
            // Controller (Camera + Unit Selector + Unit Action Handler)
            Client.Engine2D.SpawnEntity(new CameraControllerProperty(), new UnitSelectorProperty(), new UnitActionHandlerProperty());
            // Sky light
            Client.Engine2D.SpawnEntity(new EntityLight2DCasterProperty()
            {
                LightPosition = new Vector2(-20, 20),
                LightColor = Color4F.White,
                LightStrength = 128f,
                IsSkyLight = true
            });
            /* UI3DSubEngine subeng = new UI3DSubEngine(new UIPositionHelper(Client.MainUI).Anchor(UIAnchor.TOP_LEFT).ConstantXY(0, 0).ConstantWidthHeight(350, 350));
            Client.MainUI.DefaultScreen.AddChild(subeng);
            // Ground
            subeng.SubEngine.SpawnEntity(new EntitySimple3DRenderableModelProperty()
            {
                EntityModel = Client.Models.Cube,
                Scale = new Location(10, 10, 10),
                RenderAt = new Location(0, 0, -10),
                DiffuseTexture = Client.Textures.White
            });
            // Light
            subeng.SubEngine.SpawnEntity(new EntityPointLight3DProperty()
            {
                LightPosition = new Location(0, 0, 1),
                LightStrength = 15f
            });
            subeng.SubEngine.MainCamera.Position = new Location(0, 0, 0);
            subeng.SubEngine.MainCamera.Direction = new Location(0.1, -0.1, -1).Normalize(); */
        }

        /// <summary>
        /// Handles escape key pressing to exit.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Client.Window.Close();
            }
        }
    }
}

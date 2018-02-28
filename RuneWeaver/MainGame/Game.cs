using FreneticGameCore;
using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.GameProperties.GameEntities;
using FreneticGameGraphics.UISystem;
using RuneWeaver.GameScreens;
using System.Collections.Generic;
using FreneticGameCore.EntitySystem.PhysicsHelpers;

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
            Client.Engine2D.Zoom = 0.01f;
            Client.Engine2D.Source = this;
            Client.Start(GameWindowFlags.Default);
        }

        /// <summary>
        /// The main selector entity.
        /// </summary>
        public UnitSelectorProperty UnitSelector;

        /// <summary>
        /// The main camera controller entity.
        /// </summary>
        public CameraControllerProperty CameraController;
        
        /// <summary>
        /// Called by the engine when it loads up.
        /// </summary>
        public void Engine_WindowLoad()
        {
            Client.MainUI.CurrentScreen = new GameScreen(Client.MainUI);
            Client.Window.KeyDown += Window_KeyDown;
            Client.Engine2D.PhysicsWorld.Gravity = new Location(0, 0, -10);
            // Ground
            ClientEntity ground = Client.Engine2D.SpawnEntity(new EntitySimple2DRenderableBoxProperty()
            {
                BoxSize = new Vector2(20.48f, 20),
                BoxColor = Color4F.Green,
                BoxTexture = Client.Engine2D.Textures.White,
                CastShadows = false
            }, new ClientEntityPhysicsProperty()
            {
                Shape = new EntityBoxShape()
                {
                    Size = new Location(20.48f, 20, 2)
                },
                Position = new Location(0, 0, -1),
                Mass = 0,
                Friction = 0.6
            });
            // Entity 1
            Client.Engine2D.SpawnEntity(new GoblinUnitProperty()
            {
                Position = new Vector2(7.5f, 7.5f),
                Ally = true
            });
            // Entity 2
            Client.Engine2D.SpawnEntity(new WolfUnitProperty()
            {
                Position = new Vector2(2.0f, 3.5f),
                Ally = true
            });
            // Entity 3
            Client.Engine2D.SpawnEntity(new TrollUnitProperty()
            {
                Position = new Vector2(1.0f, 0.5f),
                Ally = false
            });
            // Camera Controller
            CameraController = new CameraControllerProperty();
            Client.Engine2D.SpawnEntity(CameraController);
            // Selector
            UnitSelector = new UnitSelectorProperty();
            Client.Engine2D.SpawnEntity(UnitSelector);
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

        private void ResetTurn()
        {
            SysConsole.OutputCustom("info", "banana");
        }
    }
}

using FreneticGameCore.CoreSystems;
using FreneticGameCore.MathHelpers;
using FreneticGameCore.UtilitySystems;
using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.GameScreens;
using RuneWeaver.TriangularGrid;
using System.Collections.Generic;

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
            Client = new GameClientWindow(threed: true);
            Client.OnWindowLoad += Engine_WindowLoad;
            Client.Engine3D.Forward_Shadows = true;
            Client.Engine3D.EnforceAudio = false;
            Client.Engine3D.MainView.ShadowTexSize = () => 1024;
            Client.Engine3D.Source = this;
            Client.Start(GameWindowFlags.Default);
        }

        /// <summary>
        /// The main unit controller property.
        /// </summary>
        public UnitControllerProperty UnitController;

        /// <summary>
        /// The main camera controller property.
        /// </summary>
        public CameraControllerProperty CameraController;

        /// <summary>
        /// The main UI screen.
        /// </summary>
        public GameScreen MainUIScreen;

        /// <summary>
        /// The static random.
        /// </summary>
        public MTRandom Random = new MTRandom();

        /// <summary>
        /// The playing grid's size.
        /// </summary>
        public int GridSize = 30;

        /// <summary>
        /// The number of grid layers applied on top of the base one.
        /// </summary>
        public int GridLayers = 2;

        /// <summary>
        /// The X and Y seeds used for applying grid layers.
        /// </summary>
        public Vector2[] LayerSeeds;

        /// <summary>
        /// The spawned units' faces.
        /// </summary>
        public BasicUnitProperty[,,] UnitFaces;

        /// <summary>
        /// The spawned units.
        /// </summary>
        public List<BasicUnitProperty> Units = new List<BasicUnitProperty>();

        /// <summary>
        /// Called by the engine when it loads up.
        /// </summary>
        public void Engine_WindowLoad()
        {
            // Events
            Client.Window.KeyDown += Window_KeyDown;
            // UI Screens
            // MainUIScreen = new GameScreen(Client.MainUI);
            // Client.MainUI.CurrentScreen = MainUIScreen;
            // Camera
            Client.Engine3D.MainCamera.Position = new Location(0, 0, 10);
            Client.Engine3D.MainCamera.Direction = MathUtilities.ForwardVector_Deg(0, -45);
            CameraController = new CameraControllerProperty();
            Client.Engine3D.SpawnEntity(CameraController);
            // Terrain
            Client.Engine3D.SpawnEntity(new TerrainGridProperty()
            {
                Scale = new Location(10, 10, 1),
                DiffuseTexture = Client.Textures.White
            }).SetPosition(new Location(0, 0, -5));
            // Units
            Client.Engine3D.SpawnEntity(new GoblinUnitProperty()
            {
            }).SetPosition(new Location(0, 0, 1.25));
            // Sky light
            Client.Engine3D.SpawnEntity(new EntitySkyLight3DProperty());
            // Center light
            Client.Engine3D.SpawnEntity(new EntityPointLight3DProperty()
            {
                LightPosition = new Location(0, 0, 50),
                LightStrength = 25f
            });
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

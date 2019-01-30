using FreneticGameCore;
using FreneticGameGraphics.ClientSystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.GameScreens;
using RuneWeaver.TriangularGrid;

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
            Client.Engine2D.UseLightEngine = false;
            Client.OnWindowLoad += Engine_WindowLoad;
            Client.Engine2D.Zoom = 1.0f;
            Client.Engine2D.Source = this;
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
        /// The static random.
        /// </summary>
        public MTRandom Random;

        public GridFaceProperty[,,] Faces;

        public BasicUnitProperty[,,] Units;

        /// <summary>
        /// Called by the engine when it loads up.
        /// </summary>
        public void Engine_WindowLoad()
        {
            Client.MainUI.CurrentScreen = new GameScreen(Client.MainUI);
            Client.Window.KeyDown += Window_KeyDown;
            Client.Engine2D.PhysicsWorld.Gravity = new Location(0, 0, -10);
            Random = new MTRandom();
            // Triangular Grid
            int gridSize = 30;
            Faces = new GridFaceProperty[gridSize, gridSize, 2];
            Units = new BasicUnitProperty[gridSize, gridSize, 2];
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Client.Engine2D.SpawnEntity(new GridFaceProperty()
                    {
                        Coords = new GridFace(i, j, 0)
                    });
                    Client.Engine2D.SpawnEntity(new GridFaceProperty()
                    {
                        Coords = new GridFace(i, j, 1)
                    });
                }
            }
            // Units
            Client.Engine2D.SpawnEntity(new GoblinUnitProperty()
            {
                Coords = new GridVertex(6, 3)
            });
            Client.Engine2D.SpawnEntity(new TrollUnitProperty()
            {
                Coords = new GridVertex(7, 7)
            });
            // Camera Controller
            CameraController = new CameraControllerProperty();
            Client.Engine2D.SpawnEntity(CameraController);
            // Selector
            UnitController = new UnitControllerProperty();
            Client.Engine2D.SpawnEntity(UnitController);
        }

        /// <summary>
        /// Gets the current scaling of the game.
        /// </summary>
        /// <returns>The scaling as a float.</returns>
        public float GetScaling()
        {
            return 2048 * 0.4f / 800;
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

using FreneticGameCore;
using FreneticGameGraphics.ClientSystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.GameScreens;
using RuneWeaver.TriangularGrid;
using RuneWeaver.Utilities;

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
        /// The grid faces.
        /// </summary>
        public GridFaceProperty[,,] Faces;

        /// <summary>
        /// The spawned units.
        /// </summary>
        public BasicUnitProperty[,,] Units;

        /// <summary>
        /// Called by the engine when it loads up.
        /// </summary>
        public void Engine_WindowLoad()
        {
            // Events
            Client.Window.KeyDown += Window_KeyDown;
            // UI Screens
            MainUIScreen = new GameScreen(Client.MainUI);
            Client.MainUI.CurrentScreen = MainUIScreen;
            // World Constants
            Client.Engine2D.PhysicsWorld.Gravity = new Location(0, 0, 0);
            // Triangular Grid
            LayerSeeds = new Vector2[GridLayers];
            for (int i = 0; i < GridLayers; i++)
            {
                LayerSeeds[i] = new Vector2((float)Random.NextDouble(), (float)Random.NextDouble());
            }
            Faces = new GridFaceProperty[GridSize, GridSize, 2];
            GenerateGrid(GridHelper.Grass);
            ApplyGridLayer(GridHelper.Dirt, 0.25, LayerSeeds[0], 800);
            ApplyGridLayer(GridHelper.Rock, 0.05, LayerSeeds[1], 500);
            // Units
            Units = new BasicUnitProperty[GridSize, GridSize, 2];
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
            // Unit Selector
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

        /// <summary>
        /// Generates the first grid layer.
        /// </summary>
        /// <param name="mat">The material that will be used.</param>
        public void GenerateGrid(GridMaterial mat)
        {
            for (int i = 0; i < Faces.GetLength(0); i++)
            {
                for (int j = 0; j < Faces.GetLength(1); j++)
                {
                    Client.Engine2D.SpawnEntity(new GridFaceProperty()
                    {
                        Coords = new GridFace(i, j, 0),
                        Material = mat
                    });
                    Client.Engine2D.SpawnEntity(new GridFaceProperty()
                    {
                        Coords = new GridFace(i, j, 1),
                        Material = mat
                    });
                }
            }
        }

        /// <summary>
        /// Applies a new layer on top of the grid.
        /// </summary>
        /// <param name="mat">The material that will be used.</param>
        /// <param name="chance">The chance to apply the material.</param>
        /// <param name="seedX">The random seed on the X coordinate.</param>
        /// <param name="seedY">The random seed on the Y coordinate.</param>
        public void ApplyGridLayer(GridMaterial mat, double chance, Vector2 seed, double modifier)
        {
            for (int i = 0; i < Faces.GetLength(0); i++)
            {
                for (int j = 0; j < Faces.GetLength(1); j++)
                {
                    GridFaceProperty face1 = Faces[i, j, 0];
                    Vector2 pos1 = face1.Entity.LastKnownPosition.toVector2();
                    if (SimplexNoise.Generate(seed.X + pos1.X / modifier, seed.Y + pos1.Y / modifier) < chance)
                    {
                        face1.ChangeMaterial(mat);
                    }
                    GridFaceProperty face2 = Faces[i, j, 1];
                    Vector2 pos2 = face2.Entity.LastKnownPosition.toVector2();
                    if (SimplexNoise.Generate(seed.X + pos2.X / modifier, seed.Y + pos2.Y / modifier) < chance)
                    {
                        face2.ChangeMaterial(mat);
                    }
                }
            }
        }

        /// <summary>
        /// Resets the turn, restoring every unit's energy.
        /// </summary>
        public void ResetTurn()
        {
            SysConsole.OutputCustom("info", "banana");
        }
    }
}

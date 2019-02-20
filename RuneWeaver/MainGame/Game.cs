using FreneticGameCore.CoreSystems;
using FreneticGameCore.MathHelpers;
using FreneticGameCore.UtilitySystems;
using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.GameControllers;
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
        /// The main cursor controller property.
        /// </summary>
        public CursorControllerProperty CursorController;

        /// <summary>
        /// The main camera controller property.
        /// </summary>
        public CameraControllerProperty CameraController;

        /// <summary>
        /// The terrain grid property.
        /// </summary>
        public TerrainGridProperty Terrain;

        /// <summary>
        /// The main UI screen.
        /// </summary>
        public GameScreen MainUIScreen;

        /// <summary>
        /// The static random.
        /// </summary>
        public MTRandom Random = new MTRandom();

        /// <summary>
        /// The spawned units' faces.
        /// </summary>
        public BasicUnitProperty[,] UnitFaces;

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
            MainUIScreen = new GameScreen(Client.MainUI);
            Client.MainUI.CurrentScreen = MainUIScreen;
            // Terrain
            Terrain = new TerrainGridProperty()
            {
                Size = 100,
                DiffuseTexture = Client.Textures.White
            };
            Client.Engine3D.SpawnEntity(Terrain).SetPosition(new Location(0, 0, 0));
            // Camera Controller
            CameraController = new CameraControllerProperty();
            Client.Engine3D.SpawnEntity(CameraController);
            // Unit Controller
            UnitController = new UnitControllerProperty();
            Client.Engine3D.SpawnEntity(UnitController);
            // Cursor Controller
            CursorController = new CursorControllerProperty();
            Client.Engine3D.SpawnEntity(CursorController);
            // Units
            UnitFaces = new BasicUnitProperty[Terrain.Size, Terrain.Size];
            SpawnUnit(new GoblinUnitProperty(), new GridVertex(30, 30));
            SpawnUnit(new TrollUnitProperty(), new GridVertex(50, 50));
            // Sky light
            Client.Engine3D.SpawnEntity(new EntitySkyLight3DProperty());
        }

        /// <summary>
        /// Spawns a new unit entity at the specified location if it's empty.
        /// </summary>
        /// <param name="unit">The unit property for this entity.</param>
        /// <param name="pos">The position in grid vertex coordinates.</param>
        /// <returns></returns>
        public ClientEntity SpawnUnit(BasicUnitProperty unit, GridVertex pos)
        {
            foreach (GridFace face in UnitController.OccupiedFaces(unit.Size, pos))
            {
                if (UnitFaces[face.U, face.V] != null)
                {
                    SysConsole.OutputCustom("Error", "Grid position already occupied by another unit!");
                    return null;
                }
            }
            unit.Coords = pos;
            return Client.Engine3D.SpawnEntity(unit);
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

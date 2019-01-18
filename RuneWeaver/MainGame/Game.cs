﻿using FreneticGameCore;
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
            Client.Engine2D.Zoom = 0.01f;
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

        public GridEdgeProperty[,,] Edges;

        public GridFaceProperty[,,] Faces;

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
            List<GridVertex> vertices = new List<GridVertex>();
            Edges = new GridEdgeProperty[10, 10, 3];
            Faces = new GridFaceProperty[10, 10, 2];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    vertices.Add(new GridVertex(i, j));
                    Edges[i, j, 0] = new GridEdgeProperty()
                    {
                        Coords = new GridEdge(i, j, 0)
                    };
                    Edges[i, j, 1] = new GridEdgeProperty()
                    {
                        Coords = new GridEdge(i, j, 1)
                    };
                    Edges[i, j, 2] = new GridEdgeProperty()
                    {
                        Coords = new GridEdge(i, j, 2)
                    };
                    Faces[i, j, 0] = new GridFaceProperty()
                    {
                        Coords = new GridFace(i, j, 0)
                    };
                    Faces[i, j, 1] = new GridFaceProperty()
                    {
                        Coords = new GridFace(i, j, 1)
                    };
                }
            }
            foreach (GridEdgeProperty prop in Edges)
            {
                Client.Engine2D.SpawnEntity(prop);
            }
            foreach (GridFaceProperty prop in Faces)
            {
                Client.Engine2D.SpawnEntity(prop);
            }
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
            return 2048 * Client.Engine2D.Zoom / 800;
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

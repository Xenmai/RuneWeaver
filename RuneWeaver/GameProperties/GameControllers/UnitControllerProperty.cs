using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.GameProperties.GameEntities.UnitActions;
using RuneWeaver.MainGame;
using RuneWeaver.TriangularGrid;
using RuneWeaver.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuneWeaver.GameProperties.GameControllers
{
    /// <summary>
    /// The property that handles unit selection.
    /// </summary>
    public class UnitControllerProperty : ClientEntityProperty
    {
        /// <summary>
        /// The selected unit, if any.
        /// </summary>
        public BasicUnitProperty SelectedUnit;

        /// <summary>
        /// The selected face, if any.
        /// </summary>
        public GridFaceProperty SelectedFace;

        /// <summary>
        /// The selected action, if any.
        /// </summary>
        public BasicUnitAction SelectedAction;

        /// <summary>
        /// The current action direction.
        /// </summary>
        public int Angle;
        
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Engine.Window.MouseDown += Window_MouseDown;
            Engine.Window.MouseUp += Window_MouseUp;
            Engine.Window.KeyDown += Window_KeyDown;
            Engine.Window.KeyUp += Window_KeyUp;
            Entity.OnTick += Tick;
        }

        /// <summary>
        /// Fired when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
            Engine.Window.MouseDown -= Window_MouseDown;
            Engine.Window.MouseUp -= Window_MouseUp;
            Engine.Window.KeyDown -= Window_KeyDown;
            Engine.Window.KeyUp -= Window_KeyUp;
            Entity.OnTick -= Tick;
        }

        /// <summary>
        /// Tracks mouse presses.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                Game game = Engine2D.Source as Game;
                List<GridEdge> edges = new List<GridEdge>();
                if (SelectedUnit != null)
                {
                    if (SelectedAction != null)
                    {
                        SelectedAction.Cancel();
                        SelectedAction = null;
                    }
                    SelectedUnit.Renderable.BoxTexture = Engine2D.Textures.GetTexture("Hexagon");
                    game.MainUIScreen.UnitNameLabel.Text = String.Empty;
                    SelectedUnit = null;
                }
                else if (SelectedFace != null)
                {
                    SelectedFace.Renderable.BoxTexture = Engine2D.Textures.GetTexture("Triangle");
                    SelectedFace = null;
                }                
            }
            else if (e.Button == MouseButton.Right)
            {
                if (SelectedAction != null)
                {
                    SelectedAction.Execute();
                    SelectedAction = null;
                }
            }
        }

        /// <summary>
        /// Tracks mouse releases.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                Game game = Engine2D.Source as Game;
                float scaling = game.GetScaling();
                GridFace face = GridFace.fromVector2(Engine2D.MouseCoords, scaling);
                List<GridEdge> edges = new List<GridEdge>();
                if (game.Units[face.U, face.V, face.Side] != null)
                {
                    SelectedUnit = game.Units[face.U, face.V, face.Side];
                    SelectedUnit.Renderable.BoxTexture = Engine2D.Textures.GetTexture("Hexagon_Outline");
                    game.MainUIScreen.UnitNameLabel.Text = "^!" + SelectedUnit.Name;
                }
                else
                {
                    SelectedFace = game.Faces[face.U, face.V, face.Side];
                    SelectedFace.Renderable.BoxTexture = Engine2D.Textures.GetTexture("Triangle_Outline");
                    game.MainUIScreen.UnitNameLabel.Text = "^!" + SelectedFace.Material.Name;
                }
            }
            else if (e.Button == MouseButton.Right)
            {
                
            }
        }

        /// <summary>
        /// Tracks key presses.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (SelectedUnit != null && SelectedAction == null)
            {
                switch (e.Key)
                {
                    case Key.Number1:
                        SelectedAction = SelectAction(1);
                        SelectedAction.Prepare();
                        break;
                    case Key.Number2:
                        SelectedAction = SelectAction(2);
                        SelectedAction.Prepare();
                        break;
                }
            }
        }

        /// <summary>
        /// Selects an action from the selected unit, based on an index.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public BasicUnitAction SelectAction(int num)
        {
            List<BasicUnitAction>.Enumerator e = SelectedUnit.Actions.GetEnumerator();
            for (int i = 0; i < num; i++)
            {
                e.MoveNext();
            }
            return e.Current;
        }

        /// <summary>
        /// Tracks key releases.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.ControlLeft:
                    break;
            }
        }

        /// <summary>
        /// Fired when the entity ticks.
        /// </summary>
        public void Tick()
        {
            if(SelectedUnit != null && SelectedAction != null)
            {
                Game game = Engine2D.Source as Game;
                float scaling = game.GetScaling();
                Vector2 distance = Engine2D.MouseCoords - SelectedUnit.Entity.LastKnownPosition.toVector2() / scaling;
                float degrees = (float)(Math.Atan2(distance.Y, distance.X) * 180 / Math.PI);
                Angle = (int)(((degrees + 390) % 360) / 60);
                SelectedAction.Update();
            }
        }
    }
}

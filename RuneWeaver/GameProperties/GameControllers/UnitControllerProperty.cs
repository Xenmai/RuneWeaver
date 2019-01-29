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
        public GridVertex ActionDirection;
        
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

        public void SelectBorders(List<GridEdge> edges)
        {
            Game game = Engine2D.Source as Game;
            float scaling = game.GetScaling();
            foreach (GridEdge edge in edges)
            {
                GridEdgeProperty gep = game.Edges[edge.U, edge.V, edge.Side];
                gep.Entity.SetPosition(new Location(gep.Entity.LastKnownPosition.X, gep.Entity.LastKnownPosition.Y, 4));
                gep.Renderable.BoxSize = new Vector2(scaling * 100, scaling * 5);
                gep.Renderable.BoxColor = Color4F.Red;
            }
        }

        public void DeselectBorders(List<GridEdge> edges)
        {
            Game game = Engine2D.Source as Game;
            float scaling = game.GetScaling();
            foreach (GridEdge edge in edges)
            {
                GridEdgeProperty gep = game.Edges[edge.U, edge.V, edge.Side];
                gep.Entity.SetPosition(new Location(gep.Entity.LastKnownPosition.X, gep.Entity.LastKnownPosition.Y, 2));
                gep.Renderable.BoxSize = new Vector2(scaling * 100, scaling * 2);
                gep.Renderable.BoxColor = Color4F.Black;
            }
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
                List<GridEdge> edges = new List<GridEdge>();
                if (SelectedUnit != null)
                {
                    DeselectBorders(SelectedUnit.Borders());
                    if (SelectedAction != null)
                    {
                        SelectedAction.Cancel(this);
                        SelectedAction = null;
                    }
                    SelectedUnit = null;
                }
                else if (SelectedFace != null)
                {
                    DeselectBorders(SelectedFace.Coords.Borders());
                    SelectedFace = null;
                }                
            }
            else if (e.Button == MouseButton.Right)
            {
                if (SelectedAction != null)
                {
                    SelectedAction.Execute(this);
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
                    SelectBorders(SelectedUnit.Borders());
                }
                else
                {
                    SelectedFace = game.Faces[face.U, face.V, face.Side];
                    SelectBorders(face.Borders());
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
                        SelectedAction.Prepare(this);
                        break;
                    case Key.Number2:
                        SelectedAction = SelectAction(2);
                        SelectedAction.Prepare(this);
                        break;
                }
            }
        }

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
                int dir = (int)(((degrees + 390) % 360) / 60);
                SelectedAction.Cancel(this);
                SelectBorders(SelectedUnit.Borders());
                ActionDirection = TriangularGrid.Utilities.Directions[dir];
                SelectedAction.Prepare(this);
            }
        }
    }
}

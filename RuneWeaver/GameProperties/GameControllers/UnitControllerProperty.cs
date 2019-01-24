using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.GameProperties.GameEntities.UnitActions;
using RuneWeaver.MainGame;
using RuneWeaver.TriangularGrid;
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
        /// Whether the selected unit is executing an action;
        /// </summary>
        public bool ExecutingAction;
        
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
                gep.Entity.MoveRelative(0, 0, 2);
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
                gep.Entity.MoveRelative(0, 0, -2);
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
                    if (ExecutingAction)
                    {
                        DeselectBorders(((AttackUnitAction)SelectedUnit.Actions.First()).Hitbox.Borders(SelectedUnit.Coords, 0));
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
                if (ExecutingAction)
                {
                    DeselectBorders(((AttackUnitAction)SelectedUnit.Actions.First()).Hitbox.Borders(SelectedUnit.Coords, 0));
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
            switch (e.Key)
            {
                case Key.Number1:
                    if (SelectedUnit != null)
                    {
                        Game game = Engine2D.Source as Game;
                        float scaling = game.GetScaling();
                        BasicUnitAction action = SelectedUnit.Actions.First();
                        if (action is AttackUnitAction)
                        {
                            SelectBorders(((AttackUnitAction)action).Hitbox.Borders(SelectedUnit.Coords, 0));
                        }
                        ExecutingAction = true;
                    }
                    break;
            }
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
            
        }
    }
}

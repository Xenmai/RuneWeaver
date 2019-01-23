using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK.Input;
using RuneWeaver.MainGame;
using RuneWeaver.TriangularGrid;
using System.Collections.Generic;

namespace RuneWeaver.GameProperties.GameControllers
{
    /// <summary>
    /// The property that handles unit selection.
    /// </summary>
    public class UnitControllerProperty : ClientEntityProperty
    {
        /// <summary>
        /// A list of selected entities.
        /// </summary>
        public List<GridEdgeProperty> Selected;
        
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Selected = new List<GridEdgeProperty>();
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
                foreach (GridEdgeProperty edge in Selected)
                {
                    edge.Entity.MoveRelative(0, 0, -2);
                    edge.Renderable.BoxColor = Color4F.Black;
                }
                Selected.Clear();
            }
            else if (e.Button == MouseButton.Right)
            {
                
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
                GridFace face = GridFace.fromVector2(Engine2D.MouseCoords, game.GetScaling());
                List<GridEdge> edges = new List<GridEdge>();
                if (game.Units[face.U, face.V, face.Side] != null)
                {
                    edges = game.Units[face.U, face.V, face.Side].Borders();
                }
                else
                {
                    edges = face.Borders();
                }
                foreach (GridEdge edge in edges)
                {
                    GridEdgeProperty gep = game.Edges[edge.U, edge.V, edge.Side];
                    gep.Entity.MoveRelative(0, 0, 2);
                    gep.Renderable.BoxColor = Color4F.Red;
                    Selected.Add(gep);
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
                case Key.ControlLeft:
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

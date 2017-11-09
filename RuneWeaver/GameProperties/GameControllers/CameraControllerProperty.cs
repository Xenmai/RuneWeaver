using OpenTK.Input;

namespace RuneWeaver.GameProperties.GameControllers
{
    /// <summary>
    /// Represents a an entity that rotates.
    /// </summary>
    public class CameraControllerProperty : CustomClientEntityProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Engine.Window.KeyDown += Window_KeyDown;
            Engine.Window.KeyUp += Window_KeyUp;
            Entity.OnTick += Tick;
        }

        /// <summary>
        /// Fired when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
            Engine.Window.KeyDown -= Window_KeyDown;
            Engine.Window.KeyUp -= Window_KeyUp;
            Entity.OnTick -= Tick;
        }

        /// <summary>
        /// Ticks the entity.
        /// </summary>
        public void Tick()
        {
            if (KeyZoomIn)
            {
                Engine2D.Zoom *= 0.975f;
            }
            if (KeyZoomOut)
            {
                Engine2D.Zoom *= 1.025f;
            }
            OpenTK.Vector2 motion = OpenTK.Vector2.Zero;
            if (Engine2D.Window.Mouse.X < 50)
            {
                motion.X = Engine2D.Window.Mouse.X * 0.002f - 0.1f;
            }
            if (Engine2D.Window.Mouse.X > Engine2D.Window.Width - 50)
            {
                motion.X = 0.1f - (Engine2D.Window.Width - Engine2D.Window.Mouse.X) * 0.002f;
            }
            if (Engine2D.Window.Mouse.Y < 50)
            {
                motion.Y = 0.1f - Engine2D.Window.Mouse.Y * 0.002f;
            }
            if (Engine2D.Window.Mouse.Y > Engine2D.Window.Height - 50)
            {
                motion.Y = (Engine2D.Window.Height - Engine2D.Window.Mouse.Y) * 0.002f - 0.1f;
            }
            Engine2D.ViewCenter += motion;
        }

        /// <summary>
        /// Is the right key down.
        /// </summary>
        public bool KeyZoomIn;

        /// <summary>
        /// Is the forward key down.
        /// </summary>
        public bool KeyZoomOut;

        /// <summary>
        /// Tracks key presses.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Plus:
                    KeyZoomIn = true;
                    break;
                case Key.Minus:
                    KeyZoomOut = true;
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
                case Key.Plus:
                    KeyZoomIn = false;
                    break;
                case Key.Minus:
                    KeyZoomOut = false;
                    break;
            }
        }
    }
}

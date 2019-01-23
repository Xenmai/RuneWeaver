using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK.Input;

namespace RuneWeaver.GameProperties.GameControllers
{
    /// <summary>
    /// Represents a an entity that rotates.
    /// </summary>
    public class CameraControllerProperty : ClientEntityProperty
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
            float x = Engine2D.Window.Mouse.X / (float)Engine2D.Window.Width;
            float y = Engine2D.Window.Mouse.Y / (float)Engine2D.Window.Height;
            if (x < 0.075f)
            {
                motion.X = (x - 0.075f) * 150;
            }
            if (x > 0.925f)
            {
                motion.X = (x - 0.925f) * 150;
            }
            if (y < 0.075f)
            {
                motion.Y = (0.075f - y) * 150;
            }
            if (y > 0.925f)
            {
                motion.Y = (0.925f - y) * 150;
            }
            Engine2D.ViewCenter += motion * Engine2D.Zoom;
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

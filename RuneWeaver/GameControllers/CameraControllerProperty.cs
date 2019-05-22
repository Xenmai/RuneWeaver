using System;
using FGECore.MathHelpers;
using FGEGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.MainGame;

namespace RuneWeaver.GameProperties.GameControllers
{
    /// <summary>
    /// The main camera property.
    /// </summary>
    public class CameraControllerProperty : ClientEntityProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Game game = Engine3D.Source as Game;
            Engine3D.MainCamera.Position = new Location(0, game.Terrain.Size * 0.433, 50);
            Engine3D.MainCamera.Direction = MathUtilities.ForwardVector_Deg(Yaw, Pitch);
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
        /// Is the right key down.
        /// </summary>
        public bool KeyFlyUp;

        /// <summary>
        /// Is the forward key down.
        /// </summary>
        public bool KeyFlyDown;

        /// <summary>
        /// Is the left key down.
        /// </summary>
        public bool KeyLeft;

        /// <summary>
        /// Is the right key down.
        /// </summary>
        public bool KeyRight;

        /// <summary>
        /// Is the forward key down.
        /// </summary>
        public bool KeyForward;

        /// <summary>
        /// Is the back key down.
        /// </summary>
        public bool KeyBack;

        /// <summary>
        /// Is the up key down.
        /// </summary>
        public bool KeyUp;

        /// <summary>
        /// Is the down key down.
        /// </summary>
        public bool KeyDown;

        /// <summary>
        /// Is the 'fast' key down.
        /// </summary>
        public bool KeyFast;

        /// <summary>
        /// This camera's yaw angle.
        /// </summary>
        public double Yaw = 180;

        /// <summary>
        /// This camera's pitch angle.
        /// </summary>
        public double Pitch = -60;

        /// <summary>
        /// Ticks the entity.
        /// </summary>
        public void Tick()
        {
            float x = Engine3D.Client.MouseX / (float)(Engine3D.Window.Width * 0.75);
            float y = Engine3D.Client.MouseY / (float)Engine3D.Window.Height;
            Location motion = Location.Zero;
            if (y < 0.075f)
            {
                motion += new Location(Engine3D.MainCamera.Direction.X, Engine3D.MainCamera.Direction.Y, 0).Normalize();
            }
            else if (y > 0.925f)
            {
                motion -= new Location(Engine3D.MainCamera.Direction.X, Engine3D.MainCamera.Direction.Y, 0).Normalize();
            }
            if (x < 0.075f)
            {
                motion -= new Location(Engine3D.MainCamera.Side.X, Engine3D.MainCamera.Side.Y, 0).Normalize();
            }
            else if (x > 0.925f && x < 1)
            {
                motion += new Location(Engine3D.MainCamera.Side.X, Engine3D.MainCamera.Side.Y, 0).Normalize();
            }
            if (KeyFlyUp)
            {
                motion.Z += 1;
            }
            if (KeyFlyDown)
            {
                motion.Z -= 1;
            }
            if (motion.LengthSquared() > 0)
            {
                motion *= KeyFast ? 40 : 8;
                Engine3D.MainCamera.Position += motion * Engine.Delta;
            }
            Vector2 rot = Vector2.Zero;
            float angle = 50 * (float)Engine.Delta;
            if (KeyLeft)
            {
                rot.X += angle;
            }
            if (KeyRight)
            {
                rot.X -= angle;
            }
            if (KeyUp)
            {
                rot.Y += angle;
            }
            if (KeyDown)
            {
                rot.Y -= angle;
            }
            if (rot.LengthSquared > 0)
            {
                RotateAndUpdate(rot.X, rot.Y);
            }
        }

        /// <summary>
        /// Rotates the controller and updates the main camera rotation.
        /// </summary>
        /// <param name="addYaw">The added yaw rotation.</param>
        /// <param name="addPitch">The added pitch rotation.</param>
        public void RotateAndUpdate(double addYaw, double addPitch)
        {
            Yaw += addYaw;
            while (Yaw < 0)
            {
                Yaw += 360;
            }
            while (Yaw >= 360)
            {
                Yaw -= 360;
            }
            Pitch += addPitch;
            if (Pitch < -89.9)
            {
                Pitch = -89.9;
            }
            if (Pitch > 0)
            {
                Pitch = 0;
            }
            Engine3D.MainCamera.Direction = MathUtilities.ForwardVector_Deg(Yaw, Pitch);
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
                case Key.Left:
                    KeyLeft = true;
                    break;
                case Key.Right:
                    KeyRight = true;
                    break;
                case Key.Up:
                    KeyUp = true;
                    break;
                case Key.Down:
                    KeyDown = true;
                    break;
                case Key.LShift:
                    KeyFast = true;
                    break;
                case Key.Period:
                    KeyFlyUp = true;
                    break;
                case Key.Comma:
                    KeyFlyDown = true;
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
                case Key.Left:
                    KeyLeft = false;
                    break;
                case Key.Right:
                    KeyRight = false;
                    break;
                case Key.Up:
                    KeyUp = false;
                    break;
                case Key.Down:
                    KeyDown = false;
                    break;
                case Key.LShift:
                    KeyFast = false;
                    break;
                case Key.Period:
                    KeyFlyUp = false;
                    break;
                case Key.Comma:
                    KeyFlyDown = false;
                    break;
            }
        }
    }
}

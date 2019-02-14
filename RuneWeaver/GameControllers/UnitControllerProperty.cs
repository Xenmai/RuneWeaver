using FreneticGameCore;
using FreneticGameCore.CoreSystems;
using FreneticGameCore.MathHelpers;
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
                if (SelectedUnit != null)
                {
                    SelectedUnit.Renderable.Color = Color4F.Blue;
                    SelectedUnit = null;
                    if (SelectedAction != null)
                    {
                        SelectedAction.Cancel();
                    }
                }
            }
            else if (e.Button == MouseButton.Right)
            {
                if (SelectedUnit != null && SelectedAction != null)
                {
                    SelectedAction.Execute();
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
                Matrix4 m = Engine3D.MainView.PrimaryMatrix.Inverted();
                m.Transpose();
                float x = 2.0f * Engine3D.Client.MouseX / Engine3D.Window.Width - 1.0f;
                float y = 1.0f - 2.0f * Engine3D.Client.MouseY / Engine3D.Window.Height;
                Vector4 vIn = new Vector4(x, y, 1, 1);
                Vector4 vOut = Vector4.Transform(m, vIn);
                float mul = 1.0f / vOut.W;
                Location dir = new Location(vOut.X * mul, vOut.Y * mul, vOut.Z * mul);
                ClientEntity ent = Engine3D.PhysicsWorld.RayTraceSingle(Engine3D.MainCamera.Position, dir, 100);
                if (ent != null)
                {
                    SelectedUnit = ent.GetFirstSubType<BasicUnitProperty>();
                    SelectedUnit.Renderable.Color = Color4F.Red;
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
                        SelectAction(1);
                        SelectedAction.Prepare();
                        break;
                    case Key.Number2:
                        SelectAction(2);
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
        public void SelectAction(int num)
        {
            List<BasicUnitAction>.Enumerator e = SelectedUnit.Actions.GetEnumerator();
            for (int i = 0; i < num; i++)
            {
                e.MoveNext();
            }
            SelectedAction = e.Current;
        }

        /// <summary>
        /// The grid faces occupied by this unit.
        /// </summary>
        /// <returns></returns>
        public HashSet<GridFace> OccupiedFaces(int size, GridVertex pos)
        {
            switch (size)
            {
                case 1:
                    return pos.Touches();
                case 2:
                    HashSet<GridFace> faces = new HashSet<GridFace>();
                    foreach (GridVertex vert in pos.Adjacent())
                    {
                        faces.UnionWith(vert.Touches());
                    }
                    return faces;
                default:
                    return null;
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
            //Vector2 distance = Engine2D.MouseCoords - SelectedUnit.Entity.LastKnownPosition.toVector2() / scaling;
            //float degrees = (float)(Math.Atan2(distance.Y, distance.X) * 180 / Math.PI);
            //Angle = (int)(((degrees + 390) % 360) / 60);
        }
    }
}

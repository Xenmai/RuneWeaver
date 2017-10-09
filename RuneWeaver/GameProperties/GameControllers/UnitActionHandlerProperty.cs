using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.GameProperties.GameRenderables;
using System;

namespace RuneWeaver.GameProperties.GameControllers
{
    class UnitActionHandlerProperty : CustomClientEntityProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Engine.Window.MouseDown += Window_MouseDown;
            Engine.Window.KeyDown += Window_KeyDown;
            Entity.OnTick += Tick;
            Renderable = new ArrowHitboxRenderableProperty()
            {
                IsVisible = false,
                CastShadows = false,
                Color = new Color4F(0.4f, 0.4f, 0.4f, 0.25f)
            };
            Engine.SpawnEntity(Renderable);
            Selector = Entity.GetProperty<UnitSelectorProperty>();
        }

        /// <summary>
        /// Fired when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
            Engine.Window.MouseDown -= Window_MouseDown;
            Engine.Window.KeyDown -= Window_KeyDown;
            Entity.OnTick -= Tick;
        }

        /// <summary>
        /// The hitbox renderable property.
        /// </summary>
        public ArrowHitboxRenderableProperty Renderable;

        /// <summary>
        /// The main selector.
        /// </summary>
        public UnitSelectorProperty Selector;

        /// <summary>
        /// Whether the entity is using an action.
        /// </summary>
        public Boolean UsingAction = false;

        /// <summary>
        /// The hitbox current length.
        /// </summary>
        public float Length;

        /// <summary>
        /// the hitbox current angle.
        /// </summary>
        public float Angle;

        /// <summary>
        /// Tracks mouse presses.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                if (UsingAction)
                {
                    UsingAction = false;
                    Renderable.IsVisible = false;
                    Selector.Selected.MoveRelative(Length * Math.Cos(Angle), Length * Math.Sin(Angle));
                }
            }
            else if (e.Button == MouseButton.Right)
            {
                if (UsingAction)
                {
                    UsingAction = false;
                    Renderable.IsVisible = false;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Number1)
            {
                if (Selector.Selected != null)
                {
                    UsingAction = true;
                    ClientEntity selected = Selector.Selected;
                    Renderable.Start = new Vector2((float)selected.LastKnownPosition.X, (float)selected.LastKnownPosition.Y);
                    Renderable.RenderingPriorityOrder = 5;
                    Renderable.Width = selected.GetProperty<UnitEntityProperty>().Size;
                    Renderable.IsVisible = true;
                }
            }
        }

        private void Tick()
        {
            if (UsingAction)
            {
                Vector2 distance = (Engine2D.MouseCoords - Renderable.Start);
                Length = Math.Min(distance.Length, 50f);
                Angle = (float)Math.Atan2(distance.Y, distance.X);
                Renderable.Length = Length;
                Renderable.Angle = Angle;
            }
        }
    }
}

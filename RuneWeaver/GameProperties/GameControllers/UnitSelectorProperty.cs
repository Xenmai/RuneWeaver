using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.GameProperties.GameInterfaces;
using RuneWeaver.GameProperties.GameRenderables;
using System.Linq;

namespace RuneWeaver.GameProperties.GameControllers
{
    public class UnitSelectorProperty : CustomClientEntityProperty
    {
        /// <summary>
        /// The outline renderable.
        /// </summary>
        public SelectedEntityRenderableProperty Renderable;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Engine.Window.MouseDown += Window_MouseDown;
            Engine.Window.MouseUp += Window_MouseUp;
            Engine.Window.KeyDown += Window_KeyDown;
            ActionHandler = Entity.GetProperty<UnitActionHandlerProperty>();
            Renderable = new SelectedEntityRenderableProperty()
            {
                CastShadows = false,
                IsVisible = false
            };
            Entity.AddProperty(Renderable);
        }

        /// <summary>
        /// Fired when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
            Engine.Window.MouseDown -= Window_MouseDown;
            Engine.Window.MouseUp -= Window_MouseUp;
            Engine.Window.KeyDown -= Window_KeyDown;
            Entity.RemoveProperty<SelectedEntityRenderableProperty>();
        }

        /// <summary>
        /// The main action handler.
        /// </summary>
        public UnitActionHandlerProperty ActionHandler;

        /// <summary>
        /// Which entity is selected.
        /// </summary>
        public ClientEntity Selected = null;

        /// <summary>
        /// Tracks mouse presses.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                if (Selected != null && (ActionHandler.Action == null || !ActionHandler.Action.Preparing))
                {
                    Renderable.IsVisible = false;
                    Selected?.SignalAllInterfacedProperties<ISelectable>((p) => p.Deselect());
                    Selected = null;
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
                if (Selected == null)
                {
                    foreach (ClientEntity ent in Game.Units)
                    {
                        BasicUnitProperty unit = ent.GetAllSubTypes<BasicUnitProperty>().First();
                        double radius = unit.Size * 0.5;
                        if (ent.LastKnownPosition.DistanceSquared_Flat(new Location(Engine2D.MouseCoords.X, Engine2D.MouseCoords.Y, 0)) < radius * radius)
                        {
                            Selected = ent;
                            Selected?.SignalAllInterfacedProperties<ISelectable>((p) => p.Select());
                            Renderable.Radius = (float)radius * 2;
                            Renderable.Center = new Vector2((float)Selected.LastKnownPosition.X, (float)Selected.LastKnownPosition.Y);
                            Renderable.RenderAngle = (float)unit.Direction;
                            Renderable.IsVisible = true;
                        }
                    }
                }
            }
        }

        private void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (Selected != null)
                {
                    Engine2D.ViewCenter = new Vector2((float)Selected.LastKnownPosition.X, (float)Selected.LastKnownPosition.Y);
                }
            }
        }
    }
}

using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.GameProperties.GameEntities.UnitActions;
using RuneWeaver.GameProperties.GameInterfaces;
using System.Linq;

namespace RuneWeaver.GameProperties.GameControllers
{
    public class UnitSelectorProperty : CustomClientEntityProperty
    {
        /// <summary>
        /// The outline renderable.
        /// </summary>
        public EntitySimple2DRenderableBoxProperty Renderable;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Engine.Window.MouseDown += Window_MouseDown;
            Engine.Window.MouseUp += Window_MouseUp;
            Engine.Window.KeyDown += Window_KeyDown;
            Renderable = new EntitySimple2DRenderableBoxProperty()
            {
                BoxTexture = Engine2D.Textures.GetTexture("SelectedOutline"),
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
            Entity.RemoveProperty<EntitySimple2DRenderableBoxProperty>();
        }

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
                if (Selected != null)
                {
                    BasicActionProperty action = Game.UnitActionHandler.GetProperty<UnitActionHandlerProperty>().Action;
                    if (action == null || (!action.Preparing && !action.Executing))
                    {
                        Selected.OnPositionChanged -= UpdatePosition;
                        Selected.OnOrientationChanged -= UpdateOrientation;
                        Renderable.IsVisible = false;
                        Selected?.SignalAllInterfacedProperties<ISelectable>((p) => p.Deselect());
                        Selected = null;

                    }
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
                            Entity.SetPosition(new Location(Selected.LastKnownPosition.X, Selected.LastKnownPosition.Y, 3));
                            Entity.SetOrientation(FreneticGameCore.Quaternion.FromAxisAngle(Location.UnitZ, unit.Direction));
                            Renderable.BoxSize = new Vector2(unit.Size * 2);
                            Renderable.IsVisible = true;
                            Selected.OnPositionChanged += UpdatePosition;
                            Selected.OnOrientationChanged += UpdateOrientation;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the renderable's position.
        /// </summary>
        /// <param name="loc">New location.</param>
        private void UpdatePosition(Location loc)
        {
            Entity.SetPosition(new Location(loc.X, loc.Y, 3));
        }

        /// <summary>
        /// Updates the renderable's orientation.
        /// </summary>
        /// <param name="q">New orientation.</param>
        private void UpdateOrientation(FreneticGameCore.Quaternion q)
        {
            Entity.SetOrientation(q);
        }

        /// <summary>
        /// Listens to keyboard presses.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
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

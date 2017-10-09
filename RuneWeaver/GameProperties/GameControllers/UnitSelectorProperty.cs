using FreneticGameCore;
using FreneticGameCore.Collision;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.GameProperties.GameInterfaces;
using RuneWeaver.GameProperties.GameRenderables;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RuneWeaver.GameProperties.GameControllers
{
    class UnitSelectorProperty : CustomClientEntityProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Engine.Window.MouseDown += Window_MouseDown;
            Engine.Window.MouseUp += Window_MouseUp;
            Engine.Window.KeyDown += Window_KeyDown;
            Renderable = new SelectedEntityRenderableProperty()
            {
                IsVisible = false,
                CastShadows = false,
            };
            Engine.SpawnEntity(Renderable);
            ActionHandler = Entity.GetProperty<UnitActionHandlerProperty>();
        }

        /// <summary>
        /// Fired when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
            Engine.Window.MouseDown -= Window_MouseDown;
            Engine.Window.MouseUp -= Window_MouseUp;
            Engine.Window.KeyDown -= Window_KeyDown;
        }

        /// <summary>
        /// The selected entity renderable.
        /// </summary>
        public SelectedEntityRenderableProperty Renderable;

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
                    Selected.OnPositionChanged -= PositionChanged;
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
                    Location loc = new Location(Engine2D.MouseCoords.X, Engine2D.MouseCoords.Y, 0);
                    AABB box = new AABB()
                    {
                        Min = loc,
                        Max = loc
                    };
                    List<ClientEntity> results = Engine2D.PhysicsWorld.GetEntitiesInBox(box).Where((ent) => ent.GetAllInterfacedProperties<ISelectable>().Count() > 0).ToList<ClientEntity>();
                    if (results.Count > 0)
                    {
                        Selected = results.First<ClientEntity>();
                        Selected?.SignalAllInterfacedProperties<ISelectable>((p) => p.Select());
                        Renderable.Radius = Selected.GetAllSubTypes<BasicUnitProperty>().First<BasicUnitProperty>().Size * 0.75f;
                        Renderable.IsVisible = true;
                        Renderable.Center = new Vector2((float)Selected.LastKnownPosition.X, (float)Selected.LastKnownPosition.Y);
                        Selected.OnPositionChanged += PositionChanged;
                        Renderable.Entity.SetPosition(Selected.LastKnownPosition - new Location(0, 0, 5));
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

        private void PositionChanged(Location loc)
        {
            Renderable.Entity.SetPosition(loc - new Location(0, 0, 5));
        }
    }
}

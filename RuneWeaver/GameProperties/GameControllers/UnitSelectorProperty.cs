using FreneticGameCore;
using FreneticGameCore.Collision;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using RuneWeaver.GameProperties.GameEntities;
using System.Collections.Generic;
using System.Linq;

namespace RuneWeaver.GameProperties.GameControllers
{
    public class UnitSelectorProperty : CustomClientEntityProperty
    {
        /// <summary>
        /// The selection box renderable.
        /// </summary>
        public EntitySimple2DRenderableBoxProperty Renderable;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Selected = new List<BasicUnitProperty>();
            Renderable = new EntitySimple2DRenderableBoxProperty()
            {
                BoxTexture = Engine2D.Textures.GetTexture("white"),
                BoxColor = new Color4F(1, 1, 1, 0.25f),
                CastShadows = false,
                IsVisible = false
            };
            Entity.AddProperty(Renderable);
            Engine.Window.MouseDown += Window_MouseDown;
            Engine.Window.MouseUp += Window_MouseUp;
            Entity.OnTick += Tick;
        }

        /// <summary>
        /// Fired when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
            Engine.Window.MouseDown -= Window_MouseDown;
            Engine.Window.MouseUp -= Window_MouseUp;
            Entity.OnTick -= Tick;
        }

        /// <summary>
        /// A list of selected entities.
        /// </summary>
        public List<BasicUnitProperty> Selected;

        /// <summary>
        /// Whether the left click is down.
        /// </summary>
        public bool Selecting;

        /// <summary>
        /// Whether the right click is down.
        /// </summary>
        public bool Moving;

        /// <summary>
        /// The first clicked position when selecting or moving.
        /// </summary>
        public Vector2 FirstPosition;

        /// <summary>
        /// Tracks mouse presses.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                Selecting = true;
                FirstPosition = Engine2D.MouseCoords;
                Renderable.IsVisible = true;
                Renderable.BoxSize = Vector2.Zero;
                Entity.SetPosition(new Location(FirstPosition.X, FirstPosition.Y, 1));
                if (!Selected.IsEmpty())
                {
                    foreach (BasicUnitProperty unit in Selected)
                    {
                        unit.Deselect();
                    }
                    Selected.Clear();
                }
            }
            else if (e.Button == MouseButton.Right)
            {
                if (!Selected.IsEmpty())
                {
                    Moving = true;
                    FirstPosition = Engine2D.MouseCoords;
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
                Selecting = false;
                Renderable.IsVisible = false;
                AABB box = new AABB()
                {
                    Min = new Location(FirstPosition.X, FirstPosition.Y, 0.5),
                    Max = new Location(FirstPosition.X, FirstPosition.Y, 0.5)
                };
                box.Include(new Location(Engine2D.MouseCoords.X, Engine2D.MouseCoords.Y, 1.5));
                IEnumerable<ClientEntity> entities = Engine.PhysicsWorld.GetEntitiesInBox(box);
                if (!entities.IsEmpty())
                {
                    foreach (ClientEntity ent in entities)
                    {
                        BasicUnitProperty unit = ent.GetAllSubTypes<BasicUnitProperty>().First();
                        if (unit.Ally)
                        {
                            unit.Select();
                            Selected.Add(unit);
                        }
                    }
                }
            }
        }

        public void Tick()
        {
            if (Selecting)
            {
                Vector2 center = (FirstPosition + Engine2D.MouseCoords) / 2;
                Vector2 size = FirstPosition - Engine2D.MouseCoords;
                Renderable.BoxSize = size;
                Entity.SetPosition(new Location(center.X, center.Y, 1));
            }
        }
    }
}

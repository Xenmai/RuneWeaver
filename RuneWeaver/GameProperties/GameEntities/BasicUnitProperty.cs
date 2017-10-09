using FreneticGameCore;
using FreneticGameCore.EntitySystem.PhysicsHelpers;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameInterfaces;

namespace RuneWeaver.GameProperties.GameEntities
{
    class BasicUnitProperty : CustomClientEntityProperty, ISelectable
    {
        /// <summary>
        /// The unit's renderable.
        /// </summary>
        public EntitySimple2DRenderableBoxProperty Renderable;

        /// <summary>
        /// The unit's physics body.
        /// </summary>
        public ClientEntityPhysicsProperty Body;

        /// <summary>
        /// The unit's current health.
        /// </summary>
        public float Health;

        /// <summary>
        /// The unit's maximum health.
        /// </summary>
        public float MaxHealth;

        /// <summary>
        /// The unit's resistance.
        /// </summary>
        public float Resistance;

        /// <summary>
        /// The unit's current energy.
        /// </summary>
        public int Energy;

        /// <summary>
        /// The unit's energy per turn.
        /// </summary>
        public int MaxEnergy;

        /// <summary>
        /// The unit's radius.
        /// </summary>
        public float Size;

        /// <summary>
        /// The unit's position.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Renderable = new EntitySimple2DRenderableBoxProperty()
            {
                BoxColor = Color4F.Red,
                BoxSize = new Vector2(Size, Size),
                BoxTexture = Engine2D.Textures.White
            };
            Body = new ClientEntityPhysicsProperty()
            {
                Position = new Location(Position.X, Position.Y, 0),
                Shape = new EntityBoxShape() { Size = new Location(Size, Size, 10) }
            };
            Entity.AddProperties(Renderable, Body);   
        }

        /// <summary>
        /// Highlights the entity.
        /// </summary>
        public void Select()
        {
            Renderable.BoxColor = Color4F.Blue;
        }

        /// <summary>
        /// Stops highlighting the entity.
        /// </summary>
        public void Deselect()
        {
            Renderable.BoxColor = Color4F.Red;
        }
    }
}

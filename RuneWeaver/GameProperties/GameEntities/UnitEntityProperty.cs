using FreneticGameCore;
using FreneticGameCore.EntitySystem.PhysicsHelpers;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameInterfaces;

namespace RuneWeaver.GameProperties.GameEntities
{
    class UnitEntityProperty : CustomClientEntityProperty, ISelectable
    {
        /// <summary>
        /// The unit's health.
        /// </summary>
        public float Health;

        /// <summary>
        /// The unit's resistance.
        /// </summary>
        public float Resistance;

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
        public override void OnAdded()
        {
            Entity.GetProperty<EntitySimple2DRenderableBoxProperty>().BoxColor = Color4F.Red;
            Entity.GetProperty<EntitySimple2DRenderableBoxProperty>().BoxSize = new OpenTK.Vector2(Size, Size);
            Entity.GetProperty<EntitySimple2DRenderableBoxProperty>().BoxTexture = Engine2D.Textures.White;
            Entity.GetProperty<ClientEntityPhysicsProperty>().Position = new Location(Position.X, Position.Y, 0);
            Entity.GetProperty<ClientEntityPhysicsProperty>().Shape = new EntityBoxShape() { Size = new Location(Size, Size, 10) };
        }

        /// <summary>
        /// Highlights the entity.
        /// </summary>
        public void Select()
        {
            Entity.GetProperty<EntitySimple2DRenderableBoxProperty>().BoxColor = Color4F.Blue;
        }

        /// <summary>
        /// Stops highlighting the entity.
        /// </summary>
        public void Deselect()
        {
            Entity.GetProperty<EntitySimple2DRenderableBoxProperty>().BoxColor = Color4F.Red;
        }
    }
}

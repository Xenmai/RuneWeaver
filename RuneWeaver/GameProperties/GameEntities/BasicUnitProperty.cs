using BEPUphysics.Entities;
using FreneticGameCore;
using FreneticGameCore.EntitySystem.PhysicsHelpers;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameInterfaces;

namespace RuneWeaver.GameProperties.GameEntities
{
    public class BasicUnitProperty : CustomClientEntityProperty, ISelectable
    {
        /// <summary>
        /// The unit's renderable circle base.
        /// </summary>
        public EntitySimple2DRenderableBoxProperty Circle;

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
        /// The unit's stability.
        /// </summary>
        public float Stability;

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
        /// The unit's looking direction.
        /// </summary>
        public double Direction;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Size *= 2048 * Engine2D.Zoom / Game.Client.WindowWidth;
            Direction = 0;
            Health = MaxHealth;
            Energy = MaxEnergy;
            Circle = new EntitySimple2DRenderableBoxProperty()
            {
                BoxColor = Color4F.Red,
                BoxSize = new Vector2(Size, Size),
                BoxTexture = Engine2D.Textures.GetTexture("BaseCircle")
            };
            Body = new ClientEntityPhysicsProperty()
            {
                Shape = new EntityCylinderShape()
                {
                    Radius = Size * 0.5,
                    Height = 2
                },
                Position = new Location(Position.X, Position.Y, 0),
                Mass = Stability,
                Friction = 0.5
            };
            Entity.AddProperties(Circle, Body, new ClientEntityPhysics2DLimitProperty()
            {
                ForcePosition = false
            });
            Entity physEnt = Body.SpawnedBody;
            physEnt.AngularDamping = 1;
            Game.Units.Add(Entity);
        }

        /// <summary>
        /// Highlights the entity.
        /// </summary>
        public void Select()
        {
            Circle.BoxColor = Color4F.Blue;
        }

        /// <summary>
        /// Stops highlighting the entity.
        /// </summary>
        public void Deselect()
        {
            Circle.BoxColor = Color4F.Red;
        }
    }
}

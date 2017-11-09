using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using RuneWeaver.GameProperties.GameEntities;
using System.Linq;

namespace RuneWeaver.GameProperties.PhysicProperties
{
    /// <summary>
    /// Prevents units from rotating on the Z axis.
    /// </summary>
    class Entity2DForceOrientationProperty : CustomClientEntityProperty
    {
        /// <summary>
        /// The unit property.
        /// </summary>
        public BasicUnitProperty Unit;

        /// <summary>
        /// The physics body property.
        /// </summary>
        public ClientEntityPhysicsProperty Body;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Entity.OnTick += Tick;
            Unit = Entity.GetAllSubTypes<BasicUnitProperty>().First();
            Body = Entity.GetProperty<ClientEntityPhysicsProperty>();
        }

        /// <summary>
        /// Fired when entity despawns.
        /// </summary>
        public override void OnDespawn()
        {
            Entity.OnTick -= Tick;
        }

        /// <summary>
        /// Adjusts angular velocity to zero every tick.
        /// </summary>
        private void Tick()
        {
            Body.AngularVelocity = Location.Zero;
            Entity.SetOrientation(Quaternion.FromAxisAngle(Location.UnitZ, Unit.Direction));
        }
    }
}

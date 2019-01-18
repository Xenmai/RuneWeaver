using FreneticGameCore;
using FreneticGameCore.EntitySystem.PhysicsHelpers;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameEntities.UnitActions;
using RuneWeaver.GameProperties.GameInterfaces;
using RuneWeaver.Utilities;
using System;
using System.Collections.Generic;

namespace RuneWeaver.GameProperties.GameEntities
{
    /// <summary>
    /// The basic unit property.
    /// </summary>
    public class BasicUnitProperty : ClientEntityProperty
    {
        /// <summary>
        /// The unit's main renderable.
        /// </summary>
        public EntitySimple2DRenderableBoxProperty Renderable;
                
        /// <summary>
        /// Whether the unit is ally.
        /// </summary>
        public bool Ally;

        /// <summary>
        /// The unit's name.
        /// </summary>
        public string Name;

        /// <summary>
        /// The unit's size: 1 = small, 2 = medium, 3 = big, 4 = giant.
        /// </summary>
        public int Size;

        /// <summary>
        /// The unit's vision radius.
        /// </summary>
        public int Vision;

        /// <summary>
        /// The unit's maximum health.
        /// </summary>
        public float MaxHealth;

        /// <summary>
        /// The unit's current health.
        /// </summary>
        public float Health;

        /// <summary>
        /// The unit's resistance.
        /// </summary>
        public float Resistance;

        /// <summary>
        /// The unit's stability.
        /// </summary>
        public float Stability;

        /// <summary>
        /// The list of actions this unit will perform.
        /// </summary>
        public List<BasicUnitAction> Actions;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            float scaling = 2048 * Engine2D.Zoom / 800;
            
            Health = MaxHealth;
            Renderable = new EntitySimple2DRenderableBoxProperty()
            {
                BoxSize = new Vector2(Size, Size),
                BoxTexture = Engine2D.Textures.GetTexture("BaseCircle"),
                CastShadows = false
            };
            Entity.AddProperties(Renderable);
            Renderable.BoxColor = Color4F.Blue;
        }

        /// <summary>
        /// Fires when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
        }

        /// <summary>
        /// Fired every tick while the entity is spawned.
        /// </summary>
        public void Tick()
        {
            
        }
    }
}

using BEPUphysics.Entities;
using FreneticGameCore;
using FreneticGameCore.EntitySystem.PhysicsHelpers;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameInterfaces;
using RuneWeaver.GameScreens;
using System;

namespace RuneWeaver.GameProperties.GameEntities
{
    public class BasicUnitProperty : CustomClientEntityProperty, ISelectable
    {
        /// <summary>
        /// The unit's main renderable.
        /// </summary>
        public EntitySimple2DRenderableBoxProperty Renderable;

        /// <summary>
        /// The unit's selected outline renderable.
        /// </summary>
        public EntitySimple2DRenderableBoxProperty Outline;

        /// <summary>
        /// The unit's physics body.
        /// </summary>
        public ClientEntityPhysicsProperty Body;
        
        /// <summary>
        /// The unit's primary light caster.
        /// </summary>
        public EntityLight2DCasterProperty Light1;

        /// <summary>
        /// The unit's secondary light caster entity.
        /// </summary>
        public EntityLight2DCasterProperty Light2;

        /// <summary>
        /// Whether the unit is ally.
        /// </summary>
        public bool Ally;

        /// <summary>
        /// The unit's name.
        /// </summary>
        public string Name;

        /// <summary>
        /// The unit's radius.
        /// </summary>
        public float Size;

        /// <summary>
        /// The unit's primary vision radius.
        /// </summary>
        public float Vision;

        /// <summary>
        /// The unit's maximum health.
        /// </summary>
        public float MaxHealth;

        /// <summary>
        /// The unit's current health.
        /// </summary>
        public float Health;

        /// <summary>
        /// The unit's energy per turn.
        /// </summary>
        public int MaxEnergy;

        /// <summary>
        /// The unit's current energy.
        /// </summary>
        public int Energy;

        /// <summary>
        /// The unit's resistance.
        /// </summary>
        public float Resistance;

        /// <summary>
        /// The unit's stability.
        /// </summary>
        public float Stability;

        /// <summary>
        /// The unit's position.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The unit's looking direction.
        /// </summary>
        public double Direction;

        /// <summary>
        /// The base movement speed.
        /// </summary>
        public float Speed;

        /// <summary>
        /// The base attack damage.
        /// </summary>
        public float AttackDamage;

        /// <summary>
        /// The base attack range.
        /// </summary>
        public float AttackRange;

        /// <summary>
        /// The base attack speed.
        /// </summary>
        public float AttackSpeed;

        /// <summary>
        /// The base attack force.
        /// </summary>
        public float AttackForce;

        /// <summary>
        /// Whether this unit can attack while moving.
        /// </summary>
        public bool CanMoveWhileAttacking;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            float scaling = 2048 * Engine2D.Zoom / 800;
            Size *= scaling;
            Vision *= scaling;
            Direction = 0;
            Health = MaxHealth;
            Energy = MaxEnergy;
            Renderable = new EntitySimple2DRenderableBoxProperty()
            {
                BoxSize = new Vector2(Size, Size),
                BoxTexture = Engine2D.Textures.GetTexture("BaseCircle"),
                CastShadows = false
            };
            Body = new ClientEntityPhysicsProperty()
            {
                Shape = new EntityCylinderShape()
                {
                    Radius = Size * 0.5,
                    Height = 2
                },
                Position = new Location(Position.X, Position.Y, 1),
                Mass = Stability,
                Friction = 0.5
            };
            Entity.AddProperties(Renderable, Body, new ClientEntityPhysics2DLimitProperty()
            {
                ForcePosition = false
            });
            Body.SpawnedBody.AngularDamping = 1;
            if (Ally)
            {
                Renderable.BoxColor = Color4F.Blue;
                Light1 = new EntityLight2DCasterProperty()
                {
                    LightColor = Color4F.White,
                    LightStrength = Vision,
                    LightPosition = Position
                };
                Entity.AddProperty(Light1);
                Light2 = new EntityLight2DCasterProperty()
                {
                    LightColor = Color4F.White,
                    LightStrength = Vision * 1.5f,
                    LightPosition = Position + new Vector2(Vision, 0)
                };
                Engine.SpawnEntity(Light2);
                Entity.OnPositionChanged += UpdateLight;
                Entity.OnOrientationChanged += UpdateLight;
                Outline = new EntitySimple2DRenderableBoxProperty()
                {
                    BoxTexture = Engine2D.Textures.GetTexture("SelectedOutline"),
                    BoxSize = new Vector2(Size * 2, Size * 2),
                    CastShadows = false,
                    IsVisible = false
                };
                Engine.SpawnEntity(Outline);
                Outline.Entity.SetPosition(Entity.LastKnownPosition);
                Entity.OnPositionChanged += UpdateOutline;
            }
            else
            {
                Renderable.BoxColor = Color4F.Red;
            }
        }

        /// <summary>
        /// Fires when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
            Engine.RemoveEntity(Outline.Entity);
        }
        /// <summary>
        /// Highlights the entity.
        /// </summary>
        public void Select()
        {
            Outline.IsVisible = true;
        }

        /// <summary>
        /// Stops highlighting the entity.
        /// </summary>
        public void Deselect()
        {
            Outline.IsVisible = false;
        }

        /// <summary>
        /// Updates the light's position when the entity moves.
        /// </summary>
        /// <param name="pos"></param>
        public void UpdateLight(Location pos)
        {
            Light2.Entity.SetPosition(pos + new Location((float)Math.Cos(Direction) * Vision, (float)Math.Sin(Direction) * Vision, 0));
        }

        /// <summary>
        /// Updates the light's position when the entity rotates.
        /// </summary>
        /// <param name="rot"></param>
        public void UpdateLight(FreneticGameCore.Quaternion rot)
        {
            Light2.Entity.SetPosition(Entity.LastKnownPosition + new Location((float)Math.Cos(Direction) * Vision, (float)Math.Sin(Direction) * Vision, 0));
        }

        /// <summary>
        /// Updates the outline renderable when the entity moves.
        /// </summary>
        /// <param name="pos"></param>
        public void UpdateOutline(Location pos)
        {
            Outline.Entity.SetPosition(pos);
        }
    }
}

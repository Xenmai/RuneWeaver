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
        /// The unit's renderable circle base.
        /// </summary>
        public EntitySimple2DRenderableBoxProperty Circle;

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
        public ClientEntity Light2;

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
            Game.Units.Add(Entity);
            Circle = new EntitySimple2DRenderableBoxProperty()
            {
                BoxColor = Color4F.Red,
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
                Position = new Location(Position.X, Position.Y, 0),
                Mass = Stability,
                Friction = 0.5
            };
            Entity.AddProperties(Circle, Body, new ClientEntityPhysics2DLimitProperty()
            {
                ForcePosition = false
            });
            Body.SpawnedBody.AngularDamping = 1;
            if (Ally)
            {
                Game.AllyUnits.Add(Entity);
                Light1 = new EntityLight2DCasterProperty()
                {
                    LightColor = Color4F.White,
                    LightStrength = Vision,
                    LightPosition = Position
                };
                Entity.AddProperty(Light1);
                Light2 = Engine.SpawnEntity(new EntityLight2DCasterProperty()
                {
                    LightColor = Color4F.White,
                    LightStrength = Vision * 1.5f,
                    LightPosition = Position + new Vector2(Vision, 0)
                });
                Entity.OnPositionChanged += FixPosition;
                Entity.OnOrientationChanged += FixPosition;
            }
            else
            {
                Game.EnemyUnits.Add(Entity);
            }
        }

        /// <summary>
        /// Highlights the entity.
        /// </summary>
        public void Select()
        {
            Circle.BoxColor = Color4F.Blue;
            (Game.Client.MainUI.CurrentScreen as GameScreen).UnitNameLabel.Text = "^1" + Name;
        }

        /// <summary>
        /// Stops highlighting the entity.
        /// </summary>
        public void Deselect()
        {
            Circle.BoxColor = Color4F.Red;
            (Game.Client.MainUI.CurrentScreen as GameScreen).UnitNameLabel.Text = string.Empty;
        }

        /// <summary>
        /// Fixes the light's position when the entity moves.
        /// </summary>
        /// <param name="pos"></param>
        public void FixPosition(Location pos)
        {
            Light2.SetPosition(pos + new Location((float)Math.Cos(Direction) * Vision, (float)Math.Sin(Direction) * Vision, 0));
        }

        /// <summary>
        /// Fixes the light's position when the entity rotates.
        /// </summary>
        /// <param name="rot"></param>
        public void FixPosition(FreneticGameCore.Quaternion rot)
        {
            Light2.SetPosition(Entity.LastKnownPosition + new Location((float)Math.Cos(Direction) * Vision, (float)Math.Sin(Direction) * Vision, 0));
        }
    }
}

using FreneticGameCore;
using FreneticGameCore.EntitySystem.PhysicsHelpers;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameRenderables;
using System;
using System.Linq;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions
{
    public class ShootActionProperty : BasicActionProperty
    {
        /// <summary>
        /// The maximum movement distance.
        /// </summary>
        public float MaxDistance;

        /// <summary>
        /// The projectile's strength.
        /// </summary>
        public float Strength;

        /// <summary>
        /// The movement direction.
        /// </summary>
        public Location Target;

        /// <summary>
        /// How long the entity should keep moving.
        /// </summary>
        public double TimeLeft;

        /// <summary>
        /// The physics body.
        /// </summary>
        public ClientEntityPhysicsProperty Projectile;

        /// <summary>
        /// The arrow hitbox renderable.
        /// </summary>
        public ArrowHitboxRenderableProperty Renderable;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Entity.OnTick += Tick;
            Projectile = new ClientEntityPhysicsProperty()
            {
                Position = new Location(0, 0, 0),
                Shape = new EntityBoxShape() { Size = new Location(5, 5, 10) },
                Friction = 0
            };
            Unit = Entity.GetAllSubTypes<BasicUnitProperty>().First();
            Renderable = new ArrowHitboxRenderableProperty()
            {
                CastShadows = false,
                Color = new Color4F(0.4f, 0.4f, 0.4f, 0.25f),
                Width = Unit.Size
            };
        }

        /// <summary>
        /// Fired when entity despawns.
        /// </summary>
        public override void OnDespawn()
        {
            Entity.OnTick -= Tick;
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        public override void Execute()
        {
            Unit.Energy -= Cost;
            TimeLeft = 1;
            Game.UnitActionHandler.RemoveProperty<ArrowHitboxRenderableProperty>();
            base.Execute();
        }

        /// <summary>
        /// Prepares the action.
        /// </summary>
        public override void Prepare()
        {
            Renderable.Start = new Vector2((float)Entity.LastKnownPosition.X, (float)Entity.LastKnownPosition.Y);
            Game.UnitActionHandler.AddProperty(Renderable);
            base.Prepare();
        }

        /// <summary>
        /// Cancels the action.
        /// </summary>
        public override void Cancel()
        {
            Game.UnitActionHandler.RemoveProperty<ArrowHitboxRenderableProperty>();
            base.Cancel();
        }

        private void Tick()
        {
            if (Preparing)
            {
                Vector2 distance = (Engine2D.MouseCoords - Renderable.Start);
                float length = Math.Min(distance.Length, MaxDistance);
                float angle = (float)Math.Atan2(distance.Y, distance.X);
                Renderable.Length = length;
                Renderable.Angle = angle;
                Target = new Location((float)Math.Cos(angle) * length, (float)Math.Sin(angle) * length, 0);
            }
            else if (Executing)
            {
                if (TimeLeft == 0)
                {
                    Projectile.LinearVelocity = Location.Zero;
                }
                else if (TimeLeft > Engine.Delta)
                {
                    TimeLeft -= Engine.Delta;
                    Projectile.LinearVelocity = Target;
                }
                else
                {
                    Projectile.LinearVelocity = Target * TimeLeft / Engine.Delta;
                    TimeLeft = 0;
                }
            }
        }
    }
}

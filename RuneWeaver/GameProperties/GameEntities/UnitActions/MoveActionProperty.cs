using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameRenderables;
using System;
using System.Linq;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions
{
    /// <summary>
    /// The basic movement action. Can push other units.
    /// </summary>
    public class MoveActionProperty : BasicActionProperty
    {
        /// <summary>
        /// The maximum movement distance.
        /// </summary>
        public float MaxDistance;

        /// <summary>
        /// The starting speed factor of the charge.
        /// </summary>
        public float Force;

        /// <summary>
        /// How starting point of the movement.
        /// </summary>
        public Vector2 Start;

        /// <summary>
        /// How long the entity should keep moving.
        /// </summary>
        public double TimeLeft;

        /// <summary>
        /// The direction angle of the movement.
        /// </summary>
        public double Angle;

        /// <summary>
        /// The current movement distance.
        /// </summary>
        public double Distance;

        /// <summary>
        /// The physics body property.
        /// </summary>
        public ClientEntityPhysicsProperty Body;

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
            Unit = Entity.GetAllSubTypes<BasicUnitProperty>().First();
            Body = Entity.GetProperty<ClientEntityPhysicsProperty>();
            Renderable = new ArrowHitboxRenderableProperty()
            {
                CastShadows = false,
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
        /// Prepares the action.
        /// </summary>
        public override void Prepare()
        {
            if (CheckEnergy())
            {
                Start = new Vector2((float)Entity.LastKnownPosition.X, (float)Entity.LastKnownPosition.Y);
                Vector2 offset = (Engine2D.MouseCoords - Start);
                Distance = Math.Min(offset.Length, MaxDistance);
                Renderable.Length = (float)Distance;
                Angle = Math.Atan2(offset.Y, offset.X);
                Renderable.Angle = (float)Angle;
                Game.UnitActionHandler.AddProperty(Renderable);
                Renderable.Start = Start;
                base.Prepare();
            }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        public override void Execute()
        {
            Game.UnitActionHandler.RemoveProperty<ArrowHitboxRenderableProperty>();
            Unit.Direction = Angle;
            double speed = MaxDistance * Force;
            TimeLeft = Distance / speed;
            Body.Friction = 0;
            Entity.SetOrientation(FreneticGameCore.Quaternion.FromAxisAngle(Location.UnitZ, Angle));
            Body.LinearVelocity = new Location((float)Math.Cos(Angle) * speed, (float)Math.Sin(Angle) * speed, 0);
            base.Execute();
        }

        /// <summary>
        /// Cancels the action.
        /// </summary>
        public override void Cancel()
        {
            Game.UnitActionHandler.RemoveProperty<ArrowHitboxRenderableProperty>();
            base.Cancel();
        }

        /// <summary>
        /// Fires every tick.
        /// </summary>
        private void Tick()
        {
            if (Preparing)
            {
                Vector2 offset = (Engine2D.MouseCoords - Start);
                Distance = Math.Min(offset.Length, MaxDistance);
                Renderable.Length = (float)Distance;
                Angle = Math.Atan2(offset.Y, offset.X);
                Renderable.Angle = (float)Angle;
            }
            else if (Executing)
            {
                if (TimeLeft == 0)
                {
                    Executing = false;
                    Body.LinearVelocity = Location.Zero;
                    Body.Friction = 0.5;
                }
                else if (TimeLeft > Engine.Delta)
                {
                    TimeLeft -= Engine.Delta;
                }
                else
                {
                    Body.LinearVelocity *= TimeLeft / Engine.Delta;
                    TimeLeft = 0;
                }
            }
        }
    }
}

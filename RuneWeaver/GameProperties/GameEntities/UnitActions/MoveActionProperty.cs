using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameRenderables;
using System;
using System.Linq;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions
{
    /// <summary>
    /// The basic movement action. Can't push other units.
    /// </summary>
    class MoveActionProperty : BasicActionProperty
    {
        /// <summary>
        /// The maximum movement distance.
        /// </summary>
        public float MaxDistance;

        /// <summary>
        /// The movement direction.
        /// </summary>
        public Location Target;

        /// <summary>
        /// How long the entity should keep moving.
        /// </summary>
        public double TimeLeft;

        /// <summary>
        /// The unit property.
        /// </summary>
        public BasicUnitProperty Unit;

        /// <summary>
        /// The unit property.
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
            Vector2 start = new Vector2((float)Entity.LastKnownPosition.X, (float)Entity.LastKnownPosition.Y);
            Vector2 distance = (Engine2D.MouseCoords - start);
            float length = Math.Min(distance.Length, MaxDistance);
            Renderable.Length = length;
            float angle = (float)Math.Atan2(distance.Y, distance.X);
            Renderable.Angle = angle;
            Game.HitboxRenderable.AddProperty(Renderable);
            Renderable.Start = start;
            base.Prepare();
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        public override void Execute()
        {
            Game.HitboxRenderable.RemoveProperty<ArrowHitboxRenderableProperty>();
            Unit.Energy -= Cost;
            TimeLeft = 1;
            base.Execute();
        }

        /// <summary>
        /// Cancels the action.
        /// </summary>
        public override void Cancel()
        {
            Game.HitboxRenderable.RemoveProperty<ArrowHitboxRenderableProperty>();
            base.Cancel();
        }

        private void Tick()
        {
            if (Preparing)
            {
                Vector2 distance = (Engine2D.MouseCoords - Renderable.Start);
                float length = Math.Min(distance.Length, MaxDistance);
                Renderable.Length = length;
                float angle = (float)Math.Atan2(distance.Y, distance.X);
                Renderable.Angle = angle;
                Target = new Location((float)Math.Cos(angle) * length, (float)Math.Sin(angle) * length, 0);
            }
            else if (Executing)
            {
                if (TimeLeft == 0)
                {
                    Executing = false;
                }
                else if (TimeLeft > Engine.Delta)
                {
                    Location speed = Target * Engine.Delta;
                    Location position = Entity.LastKnownPosition + speed;
                    Boolean stop = false;
                    foreach (ClientEntity otherEnt in Game.Units)
                    {
                        if (otherEnt != Entity)
                        {
                            BasicUnitProperty otherUnit = otherEnt.GetAllSubTypes<BasicUnitProperty>().First();
                            double totalSize = (otherUnit.Size + Unit.Size) * 0.5;
                            double totalSizeSquared = totalSize * totalSize;
                            if (otherEnt.LastKnownPosition.DistanceSquared_Flat(position) < totalSizeSquared)
                            {
                                stop = true;
                                double a = (Target.Y * otherEnt.LastKnownPosition.X - Target.X * otherEnt.LastKnownPosition.Y + position.X * Entity.LastKnownPosition.Y - position.Y * Entity.LastKnownPosition.X) / Target.Length();
                                double b = Target.Dot(otherEnt.LastKnownPosition - Entity.LastKnownPosition) - Math.Sqrt(totalSizeSquared - a * a);
                                SysConsole.Output(OutputType.DEBUG, totalSize + "     " + a + "     " + Target.Dot(otherEnt.LastKnownPosition - Entity.LastKnownPosition) + "     " + Math.Sqrt(totalSizeSquared - a * a) + "     " + b + "     " + speed.Length());
                                Entity.MoveRelative(speed * b / speed.Length());
                                TimeLeft = 0;
                            }
                        }
                    }
                    if (!stop)
                    {
                        Entity.MoveRelative(speed);
                        TimeLeft -= Engine.Delta;
                    }
                }
                else
                {
                    Entity.MoveRelative(Target * TimeLeft);
                    TimeLeft = 0;
                }
            }
        }
    }
}

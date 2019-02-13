using FreneticGameCore.EntitySystem.PhysicsHelpers;
using FreneticGameCore.MathHelpers;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameEntities.UnitActions;
using RuneWeaver.MainGame;
using RuneWeaver.TriangularGrid;
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
        /// The unit's renderable.
        /// </summary>
        public EntitySimple3DRenderableModelProperty Renderable;

        /// <summary>
        /// The unit's physics body.
        /// </summary>
        public ClientEntityPhysicsProperty Body;
                
        /// <summary>
        /// Whether the unit is ally.
        /// </summary>
        public bool Ally;

        /// <summary>
        /// The unit's name.
        /// </summary>
        public string Name;

        /// <summary>
        /// The unit's size: 1 = small, 2 = medium, 3 = big.
        /// </summary>
        public int Size;

        /// <summary>
        /// The unit's upgrades.
        /// </summary>
        public int[] Upgrades = new int[3];

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
        /// The unit's maximum energy.
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
        /// The list of actions this unit can perform.
        /// </summary>
        public List<BasicUnitAction> Actions = new List<BasicUnitAction>();

        /// <summary>
        /// This unit's center triangular coordinates.
        /// </summary>
        public GridVertex Coords;

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Game game = Engine3D.Source as Game;
            double mul = 0.866 * Size;
            // Build the renderable property
            Renderable = new EntitySimple3DRenderableModelProperty()
            {
                EntityModel = game.Client.Models.Cylinder,
                Scale = new Location(mul, mul, 2 * Size),
                DiffuseTexture = game.Client.Textures.White,
                Color = Color4F.Blue
            };
            // Build the physics body property
            Body = new ClientEntityPhysicsProperty()
            {
                Shape = new EntityCylinderShape()
                {
                    FixedOrientation = true,
                    Height = 2 * Size,
                    Radius = 0.5 * Size
                },
                Mass = 0
            };
            // Add properties and set position
            Entity.AddProperties(Renderable, Body);
            Vector2 pos = Coords.ToCartesianCoords2D();
            Entity.SetPosition(new Location(pos.X, pos.Y, game.Terrain.HeightMap[Coords.U, Coords.V] + Size));
            // Update the UnitFaces array with this unit's occupied faces
            foreach (GridFace face in game.UnitController.OccupiedFaces(Size, Coords))
            {
                game.UnitFaces[face.U, face.V] = this;
            }
        }

        /// <summary>
        /// Hurts this entity for the specified amount.
        /// </summary>
        /// <param name="amount">Number of damage points.</param>
        public void Hurt(int amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                Engine2D.DespawnEntity(Entity);
            }
        }

        /// <summary>
        /// Heals this entity for the specified amount.
        /// </summary>
        /// <param name="amount">Number of healing points.</param>
        public void Heal(int amount)
        {
            Health = Math.Min(Health + amount, MaxHealth);
        }

        public void UpdateEnergy(int amount)
        {
            Energy = amount;
            Game game = Engine2D.Source as Game;
            if (game.UnitController.SelectedUnit == this)
            {
                game.MainUIScreen.UnitEnergyLabel.Text = "^!" + amount;
            }
        }

        /// <summary>
        /// Fires when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
            Game game = Engine2D.Source as Game;
            foreach (GridFace face in game.UnitController.OccupiedFaces(Size, Coords))
            {
                game.UnitFaces[face.U, face.V] = null;
            }
        }

        /// <summary>
        /// Fired every tick while the entity is spawned.
        /// </summary>
        public void Tick()
        {
            
        }
    }
}

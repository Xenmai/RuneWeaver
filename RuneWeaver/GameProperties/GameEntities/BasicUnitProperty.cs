using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameEntities.UnitActions;
using RuneWeaver.MainGame;
using RuneWeaver.TriangularGrid;
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
        public List<BasicUnitAction> Actions;

        /// <summary>
        /// This unit's center triangular coordinates.
        /// </summary>
        public GridVertex Coords;

        /// <summary>
        /// The border edges of this unit.
        /// </summary>
        /// <returns>The list of border edge coordinates.</returns>
        public List<GridEdge> Borders()
        {
            return Coords.Surrounded(Size);
        }

        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Game game = Engine2D.Source as Game;
            foreach (GridFace face in Coords.Touches())
            {
                game.Units[face.U, face.V, face.Side] = this;
            }
            float scaling = game.GetScaling();
            float factor = scaling * Size * 2;
            Renderable = new EntitySimple2DRenderableBoxProperty()
            {
                BoxSize = new Vector2(factor * 100, factor * 86.6f),
                BoxTexture = Engine2D.Textures.GetTexture("Hexagon"),
                CastShadows = false
            };
            Health = MaxHealth;
            Entity.AddProperties(Renderable);
            Renderable.BoxColor = Color4F.Blue;
            float x = (Coords.U + Coords.V * 0.5f) * 100 * scaling;
            float y = Coords.V * 86.6f * scaling;
            Entity.SetPosition(new Location(x, y, 3));
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

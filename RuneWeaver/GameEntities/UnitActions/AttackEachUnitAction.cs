using FreneticGameCore.MathHelpers;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes;
using RuneWeaver.MainGame;
using RuneWeaver.TriangularGrid;
using System;
using System.Collections.Generic;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions
{
    /// <summary>
    /// The move unit action class.
    /// </summary>
    public class AttackEachUnitAction : BasicUnitAction
    {
        /// <summary>
        /// The damage amount of this attack action.
        /// </summary>
        public int Damage;

        /// <summary>
        /// The hitbox of this attack action.
        /// </summary>
        public LineHitbox Hitbox;

        public ClientEntity[] Entities = new ClientEntity[2];

        public EntitySimple2DRenderableBoxProperty Renderable1;

        public EntitySimple2DRenderableBoxProperty Renderable2;

        public AttackEachUnitAction(BasicUnitProperty unit, int cost, int damage, LineHitbox hitbox) : base(unit, cost)
        {
            this.Damage = damage;
            this.Hitbox = hitbox;
        }

        public override void Prepare()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void Cancel()
        {
            
        }

        public override void Execute()
        {
            
        }
    }
}

﻿using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using RuneWeaver.GameProperties.GameControllers;
using RuneWeaver.GameProperties.GameEntities.UnitActions.Hitboxes;
using RuneWeaver.MainGame;
using RuneWeaver.TriangularGrid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuneWeaver.GameProperties.GameEntities.UnitActions
{
    /// <summary>
    /// The move unit action class.
    /// </summary>
    public class AttackUnitAction : BasicUnitAction
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

        public AttackUnitAction(BasicUnitProperty unit, int cost, int damage, LineHitbox hitbox) : base(unit, cost)
        {
            this.Damage = damage;
            this.Hitbox = hitbox;
        }

        public override void Prepare()
        {
            Game game = Unit.Engine2D.Source as Game;
            float scaling = game.GetScaling();
            UnitControllerProperty c = game.UnitController;
            Renderable1 = new EntitySimple2DRenderableBoxProperty()
            {
                BoxDownRight = new Vector2(scaling * 100 * Hitbox.Range, -scaling * 86.6f * Hitbox.Width),
                BoxUpLeft = new Vector2(0, scaling * 86.6f * Hitbox.Width),
                BoxTexture = c.Engine2D.Textures.GetTexture("Line_Hitbox_1"),
                BoxColor = Color4F.Red,
                IsVisible = false,
                CastShadows = false
            };
            Entities[0] = Unit.Engine2D.SpawnEntity(Renderable1);
            Renderable2 = new EntitySimple2DRenderableBoxProperty()
            {
                BoxDownRight = new Vector2(scaling * 27.4f * Hitbox.Width * 2, -scaling * 86.6f * Hitbox.Width),
                BoxUpLeft = new Vector2(0, scaling * 86.6f * Hitbox.Width),
                BoxTexture = c.Engine2D.Textures.GetTexture("Line_Hitbox_2"),
                BoxColor = Color4F.Red,
                IsVisible = false,
                CastShadows = false
            };
            Entities[1] = Unit.Engine2D.SpawnEntity(Renderable2);
        }

        public override void Update()
        {
            Game game = Unit.Engine2D.Source as Game;
            float scaling = game.GetScaling();
            UnitControllerProperty c = game.UnitController;
            double angle = c.Angle * Math.PI / 3;
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            Location source = c.SelectedUnit.Entity.LastKnownPosition;
            double x1 = source.X + (Hitbox.Offset - Hitbox.Width / 2f) * cos * scaling * 100;
            double y1 = source.Y + (Hitbox.Offset - Hitbox.Width / 2f) * sin * scaling * 100;
            double x2 = x1 + Hitbox.Range * cos * scaling * 100;
            double y2 = y1 + Hitbox.Range * sin * scaling * 100;
            Renderable1.RenderAngle = (float)angle;
            Renderable1.IsVisible = true;
            Renderable2.RenderAngle = (float)angle;
            Renderable2.IsVisible = true;
            Entities[0].SetPosition(new Location(x1, y1, 2));
            Entities[1].SetPosition(new Location(x2, y2, 2));
        }

        public override void Cancel()
        {
            Game game = Unit.Engine2D.Source as Game;
            UnitControllerProperty c = game.UnitController;
            c.Engine2D.DespawnEntity(Entities[0]);
            c.Engine2D.DespawnEntity(Entities[1]);
        }

        public override void Execute()
        {
            Game game = Unit.Engine2D.Source as Game;
            UnitControllerProperty c = game.UnitController;
            c.Engine2D.DespawnEntity(Entities[0]);
            c.Engine2D.DespawnEntity(Entities[1]);
            GridVertex vector = Utilities.GridHelper.Directions[c.Angle];
            List<BasicUnitProperty> targets = new List<BasicUnitProperty>();
            foreach (GridFace face in Hitbox.Faces(c.SelectedUnit.Coords, vector))
            {
                if (game.Units[face.U, face.V, face.Side] != null)
                {
                    targets.Add(game.Units[face.U, face.V, face.Side]);
                }
            }
            foreach(BasicUnitProperty t in targets.Distinct())
            {
                t.Hurt(Damage);
            }
        }
    }
}

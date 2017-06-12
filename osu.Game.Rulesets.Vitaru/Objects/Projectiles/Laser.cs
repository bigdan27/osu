﻿using System;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;

namespace osu.Game.Rulesets.Vitaru.Objects.Projectiles
{
    public class Laser : VitaruHitObject
    {/*
        public float LaserDamage { get; set; } = 10;
        public Color4 LaserColor { get; set; } = Color4.White;
        public float LaserWidth { get; set; } = 12f;
        public float LaserLength { get; set; } = 1000f;
        public int Team { get; set; }
        public float LaserAngleRadian { get; set; }
        private Container laserPiece;

        public Laser(Projectile projectile) : base(projectile)
        {
            projectile.Team = Team;
        }

        protected override void LoadComplete()
        {
            Children = new Drawable[]
            {
                laserPiece = new Container
                {
                    Masking = true,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Rotation = LaserAngleRadian,
                    Size = new Vector2(LaserWidth , LaserLength),
                    Colour = Color4.White,
                    Alpha = 1,
                    BorderColour = LaserColor,
                    BorderThickness = LaserWidth / 4,
                    EdgeEffect = new EdgeEffect
                    {
                        Type = EdgeEffectType.Shadow,
                        Colour = LaserColor,
                        Radius = (LaserWidth / 2),
                    }
                }
            };
        }

        private void fadeOut()
        {
            if (Alpha == 1)
                FadeOut(500);
        }

        internal void DeleteLaser()
        {
            Dispose();
        }*/
        public override HitObjectType Type
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}

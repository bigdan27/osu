using osu.Framework.Graphics;
using OpenTK;
using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Framework.Graphics.Containers;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;

namespace osu.Game.Rulesets.Vitaru.Objects.Projectiles
{
    public class SeekingBullet : Bullet
    {
        private Vector2 seekingBulletVelocity;
        public DrawableVitaruEnemy NearestEnemy;
        private float enemyPos;

        private Container bulletRing;
        private CircularContainer bulletCircle;

        public SeekingBullet(int team) : base(team)
        {
            Team = team;
        }

        protected override void LoadComplete()
        {
            DynamicBulletVelocity = true;
            Children = new Drawable[]
            {
                bulletRing = new Container
                {
                    Masking = true,
                    AutoSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    BorderThickness = 3,
                    Depth = 5,
                    AlwaysPresent = true,
                    BorderColour = BulletColor,
                    Alpha = 1f,
                    CornerRadius = BulletWidth / 4,
                    Children = new[]
                    {
                        new Box
                        {
                            Colour = Color4.White,
                            Alpha = 1,
                            Width = BulletWidth * 2,
                            Height = BulletWidth * 2,
                            Depth = 5,
                        },
                    },
                },
                bulletCircle = new CircularContainer
                {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(BulletWidth * 2),
                        Depth = 6,
                        AlwaysPresent = true,
                        Masking = true,
                        EdgeEffect = new EdgeEffect
                        {
                            Type = EdgeEffectType.Shadow,
                            Colour = (BulletColor).Opacity(0.5f),
                            Radius = 2f,
                        }
                }
};
            nearestEnemy();
            enemyRelativePositionAngle();
        }

        private void nearestEnemy()
        {
            if (VitaruPlayfield.vitaruPlayfield != null)
            {
                foreach (Drawable draw in VitaruPlayfield.vitaruPlayfield.Children)
                {
                    if (draw is DrawableVitaruEnemy)
                    {
                        DrawableVitaruEnemy drawableVitaruEnemy = draw as DrawableVitaruEnemy;

                        if(drawableVitaruEnemy.Alpha > 0)
                        {
                            float minDist = 9999;
                            float dist = Vector2.Distance(drawableVitaruEnemy.Position, Position);
                            if (dist < minDist)
                            {
                                NearestEnemy = drawableVitaruEnemy;
                                minDist = dist;
                            }
                        }
                    }
                }
            }
            else
                throw new Exception();
        }

        public float enemyRelativePositionAngle()
        {
            //Returns a Radian
            if(NearestEnemy != null)
                enemyPos = (float)Math.Atan2((NearestEnemy.Position.Y - Position.Y), (NearestEnemy.Position.X - Position.X));
            return enemyPos;
        }

        private Vector2 BulletVelocity()
        {
            seekingBulletVelocity.X = BulletSpeed * (((float)Math.Cos(enemyPos)));
            seekingBulletVelocity.Y = BulletSpeed * ((float)Math.Sin(enemyPos));
            return seekingBulletVelocity;
        }

        protected override void Update()
        {
            base.Update();
            nearestEnemy();
            enemyRelativePositionAngle();
            BulletVelocity();
            MoveToOffset(new Vector2((seekingBulletVelocity.X) * (float)Clock.ElapsedFrameTime, (seekingBulletVelocity.Y) * (float)Clock.ElapsedFrameTime));
        }
    }
}
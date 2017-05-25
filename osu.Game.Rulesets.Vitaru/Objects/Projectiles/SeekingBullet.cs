using osu.Framework.Graphics;
using OpenTK;
using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Framework.Graphics.Containers;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Rulesets.Vitaru.Beatmaps;
using System.Linq;

namespace osu.Game.Rulesets.Vitaru.Objects.Projectiles
{
    public class SeekingBullet : Bullet
    {
        public DrawableVitaruEnemy NearestEnemy;
        private float enemyPos = -10;
        private double startTime;

        private Container bulletRing;

        public float StartAngle { get; set; }

        public SeekingBullet(int team) : base(team)
        {
            Team = team;
        }

        protected override void LoadComplete()
        {
            startTime = Time.Current;
            Children = new Drawable[]
            {
                bulletRing = new Container
                {
                    Scale = new Vector2(0.1f),
                    Masking = true,
                    AutoSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    BorderThickness = BulletWidth / 4,
                    Depth = 5,
                    AlwaysPresent = true,
                    BorderColour = BulletColor,
                    CornerRadius = BulletWidth / 2,
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
                    EdgeEffect = new EdgeEffect
                    {
                        Type = EdgeEffectType.Shadow,
                        Radius = BulletWidth / 2,
                        Colour = BulletColor.Opacity(0.25f),
                    },
                },
            };
            bulletRing.FadeInFromZero(150, EasingTypes.OutCubic);
            bulletRing.ScaleTo(new Vector2(1), 150, EasingTypes.OutCubic);
            enemyPos = MathHelper.DegreesToRadians(StartAngle - 90);
            GetBulletVelocity();
        }

        private void nearestEnemy()
        {
            if (VitaruBeatmapConverter.EnemyList != null)
            {
                foreach (DrawableVitaruEnemy enemy in VitaruBeatmapConverter.EnemyList.OfType<DrawableVitaruEnemy>())
                {
                    float minDist = 9999;
                    float dist = Vector2.Distance(enemy.Position, Position);
                    if (dist < minDist)
                    {
                        NearestEnemy = enemy;
                        minDist = dist;
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

        public new Vector2 GetBulletVelocity()
        {
            BulletVelocity.X = BulletSpeed * (((float)Math.Cos(enemyPos)));
            BulletVelocity.Y = BulletSpeed * ((float)Math.Sin(enemyPos));
            return BulletVelocity;
        }

        protected override void Update()
        {
            base.Update();
            Rotation = Rotation + 1f;

            //IdleTimer
            if(startTime + 300 <= Time.Current)
            {
                nearestEnemy();
                if(NearestEnemy != null)
                    if (NearestEnemy.CharacterHealth > 0)
                    {
                        enemyRelativePositionAngle();
                        GetBulletVelocity();
                    }
            }
        }
    }
}
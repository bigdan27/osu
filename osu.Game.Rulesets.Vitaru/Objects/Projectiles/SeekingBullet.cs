using osu.Framework.Graphics;
using OpenTK;
using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Rulesets.Vitaru.Objects.Characters;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;

namespace osu.Game.Rulesets.Vitaru.Objects.Projectiles
{
    public class SeekingBullet : Bullet
    {
        private Vector2 seekingBulletVelocity;
        private Sprite seekingBulletSprite;
        public DrawableVitaruEnemy NearestEnemy;
        private float enemyPos;

        public SeekingBullet(int team) : base(team)
        {
            Team = team;
        }

        protected override void LoadComplete()
        {
            DynamicBulletVelocity = true;
            nearestEnemy();
            enemyRelativePositionAngle();
            Children = new Drawable[]
            {
                seekingBulletSprite = new Sprite
                {
                    //TODO: find a good sprite and load it here
                },
            };
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
            MoveToOffset(new Vector2(seekingBulletVelocity.X * (float)Clock.ElapsedFrameTime, seekingBulletVelocity.Y * (float)Clock.ElapsedFrameTime));
        }
    }
}
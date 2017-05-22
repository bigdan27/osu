using osu.Framework.Graphics;
using OpenTK;
using System;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Vitaru.Objects.Projectiles
{
    public class SeekingBullet : Bullet
    {
        private Vector2 seekingBulletVelocity;
        private Sprite seekingBulletSprite;

        public SeekingBullet(int team) : base(team)
        {
            Team = team;
        }

        protected override void LoadComplete()
        {
            Children = new Drawable[]
            {
                seekingBulletSprite = new Sprite
                {
                    //TODO: find a good sprite and load it here
                },
            };
        }

        public int SeekingBulletTarget()
        {
            return 0;
        }

        public Vector2 SeekingBulletVelocity()
        {
            seekingBulletVelocity.X = BulletSpeed * (((float)Math.Cos(BulletAngleRadian)));
            seekingBulletVelocity.Y = BulletSpeed * ((float)Math.Sin(BulletAngleRadian));
            return seekingBulletVelocity;
        }

        protected override void Update()
        {
            base.Update();
            SeekingBulletTarget();
            SeekingBulletVelocity();
            MoveToOffset(new Vector2(seekingBulletVelocity.X * (float)Clock.ElapsedFrameTime, seekingBulletVelocity.Y * (float)Clock.ElapsedFrameTime));
        }
    }
}
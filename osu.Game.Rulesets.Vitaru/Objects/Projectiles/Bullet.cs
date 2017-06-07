using osu.Framework.Graphics;
using OpenTK;
using System;
using OpenTK.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Vitaru.Objects.Projectiles
{
    public class Bullet : DrawableVitaruHitObject
    {
        //Different stats for Bullet that should always be changed
        public float BulletDamage { get; set; } = 10;
        public Color4 BulletColor { get; set; } = Color4.White;
        public float BulletSpeed { get; set; } = 1f;
        public float BulletWidth { get; set; } = 12f;
        public float BulletAngleRadian { get; set; } = -10;
        public bool DynamicBulletVelocity { get; set; } = false;
        public bool Piercing { get; set; } = false;
        public int Team { get; set; }

        public HitResult BulletResult = HitResult.None;
        public VitaruScoreResult BulletScore;

        //Used like a multiple
        public static float BulletSpeedModifier = 1;

        //This is an extra 10 outside of playerbounds intentionally. There is No escape.
        public Vector4 BulletBounds = new Vector4(-10, -10, 522, 830);

        //Result of bulletSpeed + bulletAngle math, should never be modified outside of this class
        public Vector2 BulletVelocity;

        private Container bulletRing;
        private CircularContainer bulletCircle;

        protected override void CheckJudgement(bool userTriggered)
        {
            base.CheckJudgement(userTriggered);

            if(BulletResult != HitResult.None)
            {
                Judgement.Result = BulletResult;
                Judgement.Score = BulletScore;
            }
        }

        protected override void UpdateState(ArmedState state)
        {
            base.UpdateState(state);

            switch (State)
            {
                case ArmedState.Idle:
                    break;
                case ArmedState.Hit:
                    Dispose();
                    break;
                case ArmedState.Miss:
                    Dispose();
                    break;
            }
        }

        public Bullet(Projectile projectile) : base(projectile)
        {
            projectile.Team = Team;
        }

        protected override void LoadComplete()
        {
            GetBulletVelocity();
            Children = new Drawable[]
            {
                bulletRing = new Container
                {
                    Scale = new Vector2(0.1f),
                    Masking = true,
                    AutoSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    BorderThickness = 3,
                    Depth = 5,
                    AlwaysPresent = true,
                    BorderColour = BulletColor,
                    Alpha = 0f,
                    CornerRadius = BulletWidth,
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
                            Radius = 1.25f,
                        }
                }
            };
            bulletRing.FadeIn(100, EasingTypes.OutCubic);
            bulletRing.ScaleTo(new Vector2(1), 100, EasingTypes.OutCubic);
        }

        public Vector2 GetBulletVelocity()
        {
            BulletVelocity.X = BulletSpeed * (((float)Math.Cos(BulletAngleRadian)));
            BulletVelocity.Y = BulletSpeed * ((float)Math.Sin(BulletAngleRadian));
            return BulletVelocity;
        }

        protected override void Update()
        {
            base.Update();

            //Will be useful for makin bullets stop, like if a certain character / boss could freeze time.
            if (DynamicBulletVelocity)
                GetBulletVelocity();

            if (Alpha < 0.05)
                DeleteBullet();

            if (Position.Y < BulletBounds.Y | Position.X < BulletBounds.X | Position.Y > BulletBounds.W | Position.X > BulletBounds.Z)   
                fadeOut();
            MoveBullet();
        }

        private void fadeOut()
        {
            if(Alpha == 1)
                FadeOut(200);
        }

        public void MoveBullet()
        {
            MoveToOffset(new Vector2((BulletVelocity.X * BulletSpeedModifier) * (float)Clock.ElapsedFrameTime, (BulletVelocity.Y * BulletSpeedModifier) * (float)Clock.ElapsedFrameTime));
        }

        internal void DeleteBullet()
        {
            Dispose();
        }
    }
}
 
using osu.Framework.Graphics;
using OpenTK;
using System;
using OpenTK.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;
using osu.Game.Graphics.Containers;
using osu.Framework.Audio.Track;
using osu.Game.Beatmaps.ControlPoints;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableBullet : BeatSyncedContainer
    { 
        //Different stats for Bullet that can be changed
        public float BulletDamage { get; set; } = 10;
        public Color4 BulletColor { get; set; } = Color4.White;
        public float BulletSpeed { get; set; } = 1f;
        public float BulletWidth { get; set; } = 12f;
        public float BulletAngleRadian { get; set; } = -10;
        public bool DynamicBulletVelocity { get; set; } = false;
        public bool Piercing { get; set; } = false;
        public int Team { get; set; }
        public string Result { get; set; }

        public static int BulletCount = 0;

        //Used like a multiple
        public static float BulletSpeedModifier = 1;

        public Vector4 BulletBounds = new Vector4(-1, -1, 513, 821);

        //Result of bulletSpeed + bulletAngle math, should never be modified outside of this class
        public Vector2 BulletVelocity;

        private double bulletDeleteTime = -1;

        private Container bulletRing;
        private CircularContainer bulletCircle;
        private readonly DrawablePattern pattern;

        public DrawableBullet(DrawablePattern drawablePattern)
        {
            pattern = drawablePattern;
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            bulletCircle.Alpha = 0.75f;
            using (BeginDelayedSequence(100))
                bulletCircle.FadeTo(0.25f, Math.Max(0, timingPoint.BeatLength - 100), EasingTypes.OutSine);
        }

        protected override void LoadComplete()
        {
            BulletCount++;
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
                    BorderColour = BulletColor,
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
                        Alpha = 0.25f,
                        Masking = true,
                        EdgeEffect = new EdgeEffectParameters
                        {
                            Type = EdgeEffectType.Shadow,
                            Colour = BulletColor,
                            Radius = 2f,
                        }
                }
            };
            bulletRing.FadeInFromZero(pattern.TIME_PREEMPT / 8, EasingTypes.OutCubic);
            bulletRing.ScaleTo(new Vector2(1), pattern.TIME_PREEMPT / 8, EasingTypes.OutCubic);
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

            if (DynamicBulletVelocity)
                GetBulletVelocity();

            if (Time.Current >= bulletDeleteTime && bulletDeleteTime != -1)
                DeleteBullet();

            if (Position.Y < BulletBounds.Y | Position.X < BulletBounds.X | Position.Y > BulletBounds.W | Position.X > BulletBounds.Z && Alpha >= 1)
            {
                AlwaysPresent = true;
                bulletDeleteTime = Time.Current + 200;
                FadeOutFromOne(200);
                Hit();
            }
            MoveBullet();
        }

        public void MoveBullet()
        {
            MoveToOffset(new Vector2((BulletVelocity.X * BulletSpeedModifier) * (float)Clock.ElapsedFrameTime, (BulletVelocity.Y * BulletSpeedModifier) * (float)Clock.ElapsedFrameTime));
        }

        internal void DeleteBullet()
        {
            pattern.BulletCount--;
            BulletCount--;
            Dispose();
        }

        public void Miss()
        {
            pattern.Miss = true;
            DeleteBullet();
        }
        public void Hit()
        {
            pattern.Hit = true;
            pattern.Score = 10;
        }
    }
}

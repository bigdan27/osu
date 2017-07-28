using osu.Framework.Graphics;
using OpenTK;
using System;
using OpenTK.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Framework.Audio.Track;
using osu.Game.Beatmaps.ControlPoints;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableBullet : BeatSyncedContainer
    { 
        //Different stats for Bullet that can be changed
        public float BulletDamage { get; set; } = 10;
        public Color4 BulletColor { get; set; } = Color4.White;
        public float BulletSpeed { get; set; } = 1f;
        public float BulletWidth { get; set; } = 12f;
        public float BorderWidth { get; set; } = 3f;
        public float BulletAngleRadian { get; set; } = -10;
        public bool DynamicBulletVelocity { get; set; } = false;
        public bool Piercing { get; set; } = false;
        public int Team { get; set; }
        public string Result { get; set; }
        public bool DummyMode { get; set; } = false;

        public static int BulletCount = 0;

        //Used like a multiple
        public static float BulletSpeedModifier = 1;

        public Vector4 BulletBounds = new Vector4(-1, -1, 513, 385);

        //Result of bulletSpeed + bulletAngle math, should never be modified outside of this class
        public Vector2 BulletVelocity;

        public double BulletDeleteTime = -1;

        private Container bulletCircle;
        private CircularContainer bulletGlow;
        private readonly DrawablePattern pattern;
        private Sprite bulletKiaiSprite;

        public DrawableBullet(DrawablePattern drawablePattern)
        {
            AlwaysPresent = true;
            pattern = drawablePattern;
            if (VitaruRuleset.TouhosuMode)
                BulletBounds = new Vector4(-1, -1, 513, 821);
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            bulletGlow.Alpha = 0.75f;
            using (BeginDelayedSequence(100))
                bulletGlow.FadeTo(0.25f, Math.Max(0, timingPoint.BeatLength - 100), Easing.OutSine);

            if (effectPoint.KiaiMode && bulletCircle.Alpha == 1)
            {
                bulletKiaiSprite.FadeInFromZero(timingPoint.BeatLength / 4);
                bulletCircle.FadeOutFromOne(timingPoint.BeatLength / 4);
            }
            if (!effectPoint.KiaiMode && bulletKiaiSprite.Alpha == 1)
            {
                bulletCircle.FadeInFromZero(timingPoint.BeatLength);
                bulletKiaiSprite.FadeOutFromOne(timingPoint.BeatLength);
            }
        }

        protected override void LoadComplete()
        {
            BulletCount++;
            GetBulletVelocity();
            Children = new Drawable[]
            {
                bulletCircle = new Container
                {
                    Scale = new Vector2(0.1f),
                    Masking = true,
                    AutoSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    BorderThickness = BorderWidth,
                    Depth = 2,
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
                bulletGlow = new CircularContainer
                {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(BulletWidth * 2),
                        Depth = 3,
                        Alpha = 0.25f,
                        Masking = true,
                        EdgeEffect = new EdgeEffectParameters
                        {
                            Type = EdgeEffectType.Shadow,
                            Colour = BulletColor,
                            Radius = 2f,
                        }
                },
                bulletKiaiSprite = new Sprite
                {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Depth = 1,
                        Alpha = 0,
                        Colour = BulletColor,
                        Texture = VitaruRuleset.VitaruTextures.Get("bulletKiai"),
                }
            };
            bulletCircle.FadeInFromZero(pattern.TIME_PREEMPT / 8, Easing.OutCubic);
            bulletCircle.ScaleTo(new Vector2(1), pattern.TIME_PREEMPT / 8, Easing.OutCubic);
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

            if (bulletKiaiSprite.Alpha > 0)
                bulletKiaiSprite.RotateTo((float)((Clock.CurrentTime / 1000) * 90));

            if (DynamicBulletVelocity)
                GetBulletVelocity();

            if (Time.Current >= BulletDeleteTime && BulletDeleteTime != -1)
                DeleteBullet();

            if (Position.Y < BulletBounds.Y | Position.X < BulletBounds.X | Position.Y > BulletBounds.W | Position.X > BulletBounds.Z && Alpha >= 1)
            {
                AlwaysPresent = true;
                BulletDeleteTime = Time.Current + 200;
                this.FadeOutFromOne(200);
                Hit();
            }
            MoveBullet();
        }

        public void MoveBullet()
        {
            this.MoveToOffset(new Vector2((BulletVelocity.X * BulletSpeedModifier) * (float)Clock.ElapsedFrameTime, (BulletVelocity.Y * BulletSpeedModifier) * (float)Clock.ElapsedFrameTime));
        }

        public void DeleteBullet(bool miss = false)
        {
            if (!DummyMode)
            {
                pattern.BulletCount--;
                BulletCount--;
            }
            if (miss)
                Miss();
            LifetimeEnd = Time.Current + 30;
        }

        public void Miss()
        {
            pattern.Miss = true;
            pattern.ForceJudgment();
        }
        public void Hit()
        {
            pattern.Hit = true;
            pattern.Score = 300;
        }
    }
}

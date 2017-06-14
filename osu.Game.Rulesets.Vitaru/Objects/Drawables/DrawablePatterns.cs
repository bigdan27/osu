using osu.Framework.Graphics.Containers;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using System;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;
using osu.Game.Rulesets.Vitaru.Beatmaps;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Extensions.Color4Extensions;

namespace osu.Game.Rulesets.Vitaru.Objects.Projectiles
{
    public class DrawablePattern : DrawableVitaruHitObject
    {
        public virtual int PatternID { get; set; }

        protected Pattern Pattern;

        public DrawablePattern(Pattern pattern) : base(pattern)
        {
            AlwaysPresent = true;
            Pattern = pattern;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (Pattern.PatternAngleRadian == -10)
                Pattern.PatternAngleRadian = MathHelper.DegreesToRadians(Pattern.PatternAngleDegree - 90);
            /*
            Children = new Drawable[]
            {
                new Container
                {
                    Scale = new Vector2(0.1f),
                    Masking = true,
                    AutoSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    BorderThickness = 3,
                    Depth = 5,
                    AlwaysPresent = true,
                    BorderColour = Pattern.PatternColor,
                    Alpha = 0f,
                    CornerRadius = Pattern.PatternBulletWidth,
                    Children = new[]
                    {
                        new Box
                        {
                            Colour = Color4.White,
                            Alpha = 1,
                            Width = Pattern.PatternBulletWidth * 2,
                            Height = Pattern.PatternBulletWidth * 2,
                            Depth = 5,
                        },
                    },
                },
                new CircularContainer
                {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(Pattern.PatternBulletWidth * 2),
                        Depth = 6,
                        AlwaysPresent = true,
                        Masking = true,
                        EdgeEffect = new EdgeEffectParameters
                        {
                            Type = EdgeEffectType.Shadow,
                            Colour = (Pattern.PatternColor).Opacity(0.5f),
                            Radius = 1.25f,
                        }
                }
            };*/
        }

        protected void PatternStart()
        {
            Position = new Vector2(Pattern.PatternPosition.Y, Pattern.PatternPosition.Y + 200);
            MoveTo(Pattern.PatternPosition, 500);
            FadeInFromZero(250);
        }

        protected override void Update()
        {
            base.Update();

            //if (HitObject.StartTime + Pattern.PatternDuration <= Time.Current)
            //Dispose();
            if (HitObject.StartTime - 500 <= Time.Current && Alpha <= 0)
            {
                PatternStart();
            }

            if (HitObject.StartTime <= Time.Current)
            {
                CreatePatternWave();
                PlaySamples();
                Dispose();
            }
        }

        protected void bulletAddRad(float speed, float angle)
        {
            Bullet bullet = new Bullet
            {
                Team = Pattern.PatternTeam,
                StartTime = Pattern.StartTime,
            };

            DrawableBullet drawableBullet;
            VitaruPlayfield.vitaruPlayfield.Add(drawableBullet = new DrawableBullet(bullet)
            {
                Origin = Anchor.Centre,
                Depth = 5,
                BulletColor = Pattern.PatternColor,
                BulletAngleRadian = angle,
                BulletSpeed = speed,
                BulletWidth = Pattern.PatternBulletWidth,
                BulletDamage = Pattern.PatternDamage,
                DynamicBulletVelocity = Pattern.DynamicPatternVelocity,
            });
            drawableBullet.MoveTo(ToSpaceOfOtherDrawable(Pattern.PatternPosition, drawableBullet));
        }

        protected virtual void CreatePattern() { }

        public void CreatePatternWave()
        {
            int numberBullets = (int)Pattern.PatternDifficulty * 2 + 1;
            float directionModifier = -0.1f * ((numberBullets - 1) / 2);
            for (int i = 1; i <= numberBullets; i++)
            {
                bulletAddRad(Pattern.PatternSpeed, Pattern.PatternAngleRadian + directionModifier);
                directionModifier += 0.1f;
            }
        }
    }

        /// <summary>
        /// BASE PATTERNS
        /// Well, following their structure you will be able to make your own once vitaru is more matured.
        /// </summary>
        /*
        public class Wave : DrawablePattern
        {
            public override int PatternID => 0;

            public Wave(Pattern pattern) : base(pattern)
            {
            }

            protected override 
        }*/
        public class Line : DrawablePattern
    {
        public override int PatternID => 1;

        public Line(Pattern pattern) : base(pattern)
        {
        }

        protected override void CreatePattern()
        {
            for (int i = 1; i <= Pattern.PatternDifficulty + 1; i++)
            {
                bulletAddRad(0.12f + Pattern.PatternSpeed, Pattern.PatternAngleRadian);
                Pattern.PatternSpeed += 0.02f;
            }
        }
    }
    public class Flower : DrawablePattern
    {
        public override int PatternID => 2;

        public Flower(Pattern pattern) : base(pattern)
        {
        }

        protected override void CreatePattern()
        {
            double timeSaved = Time.Current;
            int a = 0;
            for (int j = 1; j <= 16 * Pattern.PatternDifficulty; j++)
            {
                a = a + 21;
                Pattern.PatternAngleRadian = MathHelper.DegreesToRadians(a - 90);
                bulletAddRad(Pattern.PatternSpeed, a);
            }
        }
    }
    public class CoolWave : DrawablePattern
    {
        public override int PatternID => 3;

        public CoolWave(Pattern pattern) : base(pattern)
        {
        }

        protected override void CreatePattern()
        {
            float speedModifier = 0.01f + 0.01f * (Pattern.PatternDifficulty);
            float directionModifier = -0.075f - 0.075f * (Pattern.PatternDifficulty);
            for (int i = 1; i <= 3 + (Pattern.PatternDifficulty * 2); i++)
            {
                bulletAddRad(
                    Pattern.PatternSpeed + Math.Abs(speedModifier),
                    directionModifier + Pattern.PatternAngleRadian
                );
                speedModifier -= 0.01f;
                directionModifier += 0.075f;
            }
        }
    }

    public class Spin : DrawablePattern
    {
        public override int PatternID => 4;
        
        public Spin(Pattern pattern) : base(pattern)
        {
        }

        protected override void CreatePattern()
        {
            double numberbullets = 30 * Pattern.PatternDifficulty;
            int numberspins = (int)(Pattern.PatternDifficulty + 2) / 2;
            float spinModifier = MathHelper.DegreesToRadians((float)(360 / numberspins));
            float directionModifier = (float)(360 / numberbullets);
            directionModifier = MathHelper.DegreesToRadians(directionModifier);
            double originalDuration = Pattern.PatternDuration;
            Pattern.PatternDuration /= numberbullets;
            Pattern.PatternDuration /= Pattern.PatternRepeatTimes;
            int i = 1;
            while(i <= Pattern.PatternRepeatTimes)
            {
                int j = 1;
                while (j <= numberbullets)
                {
                    Scheduler.AddDelayed(() =>
                    {
                        for(int k = 1; k <= numberspins; k++)
                            bulletAddRad(Pattern.PatternSpeed, Pattern.PatternAngleRadian + (spinModifier * (k - 1)));
                        Pattern.PatternAngleRadian -= directionModifier;
                    }, Pattern.PatternDuration * (j - 1) + (originalDuration * (i - 1)));
                    j++;
                }
                i++;
            }
        }
    }
    public class Trianglewave : DrawablePattern
    {
        public override int PatternID => 5;

        public Trianglewave(Pattern pattern) : base(pattern)
        {
        }

        protected override void CreatePattern()
        {
            bool reversed = true;
            int numberwaves = (int)(Pattern.PatternDifficulty + 2) / 2;
            float originalDirection = 0f;
            int numberbullets;
            Pattern.PatternDuration /= numberwaves;
            float speedModifier;
            for (int i = 1; i <= numberwaves; i++)
            {
                numberbullets = i;
                if (reversed)
                    speedModifier = 0.30f - (i - 1) * 0.03f;
                else
                    speedModifier = (i - 1) * 0.03f;
                for (int j = 1; j <= numberbullets; j++)
                {
                    float directionModifier = ((j - 1) * 0.1f);
                        bulletAddRad(
                            Pattern.PatternSpeed + speedModifier,
                            Pattern.PatternAngleRadian + (originalDirection - directionModifier)
                        );
                }
                originalDirection = 0.05f * i;
            }
        }
    }
}

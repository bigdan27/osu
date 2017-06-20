using osu.Framework.Graphics.Containers;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using System;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawablePattern : DrawableVitaruHitObject
    {
        protected Pattern Pattern;
        private bool visable = false;
        private bool patternCreated = false;

        public int BulletCount = 0;
        public static int PatternCount = 0;
        public int ScorePattern;

        public bool Miss = false;
        public bool Hit = false;
        public bool Ten = false;
        public int Score = 0;

        public DrawablePattern(Pattern pattern) : base(pattern)
        {
            AlwaysPresent = true;
            Pattern = pattern;
        }

        protected override void CheckJudgement(bool userTriggered)
        {
            base.CheckJudgement(userTriggered);
            if (Miss)
            {
                Judgement.Result = HitResult.Miss;
                Judgement.Score = VitaruScoreResult.Miss;
                BulletCount = 0;
            }
            if (Hit)
            {
                Judgement.Result = HitResult.Hit;
                switch (Score)
                {
                    case 10:
                        Judgement.Score = VitaruScoreResult.Graze10;
                        break;
                    case 50:
                        Judgement.Score = VitaruScoreResult.Graze50;
                        break;
                    case 100:
                        Judgement.Score = VitaruScoreResult.Graze100;
                        break;
                    case 300:
                        Judgement.Score = VitaruScoreResult.Graze300;
                        break;
                }
                BulletCount = 0;
            }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (Pattern.PatternAngleRadian == -10)
                Pattern.PatternAngleRadian = MathHelper.DegreesToRadians(Pattern.PatternAngleDegree - 90);

            Alpha = 0;
        }

        protected void PatternStart()
        {
            Children = new Drawable[]
{
                new Container
                {
                    Masking = true,
                    AutoSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    BorderThickness = 3,
                    Depth = 5,
                    BorderColour = Pattern.PatternColor,
                    Alpha = 1f,
                    CornerRadius = 16,
                    Children = new[]
                    {
                        new Box
                        {
                            Colour = Color4.White,
                            Alpha = 1,
                            Width = 16 * 2,
                            Height = 16 * 2,
                            Depth = 5,
                        },
                    },
                },
                new CircularContainer
                {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(16 * 2),
                        Depth = 6,
                        Masking = true,
                        EdgeEffect = new EdgeEffectParameters
                        {
                            Type = EdgeEffectType.Shadow,
                            Colour = (Pattern.PatternColor).Opacity(0.5f),
                            Radius = 2f,
                        }
                }
            };

            visable = true;
            Position = new Vector2(Pattern.PatternPosition.X, Pattern.PatternPosition.Y - 200);
            MoveTo(Pattern.PatternPosition, TIME_PREEMPT);
            FadeInFromZero(TIME_FADEIN);
        }

        protected void PatternPop()
        {
            ScaleTo(0.1f, TIME_PREEMPT / 8);
        }

        protected override void Update()
        {
            base.Update();

            if (HitObject.StartTime - TIME_PREEMPT <= Time.Current && !visable)
            {
                PatternStart();
            }

            if (HitObject.StartTime - (TIME_PREEMPT / 8) <= Time.Current)
            {
                PatternPop();
            }

            if (HitObject.StartTime <= Time.Current && !patternCreated)
            {
                CreatePattern();
                PlaySamples();
                Alpha = 0;
            }
            if (patternCreated)
            {
                if (BulletCount == 0)
                {
                    PatternCount--;
                    Dispose();
                }
            }
        }

        protected void bulletAddRad(float speed, float angle)
        {
            BulletCount++;

            DrawableBullet drawableBullet;
            VitaruPlayfield.vitaruPlayfield.Add(drawableBullet = new DrawableBullet(this)
            {
                Origin = Anchor.Centre,
                Depth = 5,
                BulletColor = Pattern.PatternColor,
                BulletAngleRadian = angle,
                BulletSpeed = speed,// + PatternSpeed,
                BulletWidth = Pattern.PatternBulletWidth,
                BulletDamage = Pattern.PatternDamage,
                DynamicBulletVelocity = Pattern.DynamicPatternVelocity,
            });
            drawableBullet.MoveTo(Pattern.PatternPosition);
        }

        public void CreatePattern()
        {
            patternCreated = true;
            PatternCount++;
            int patternID = Pattern.PatternID;
            switch (patternID)
            {
                case 1:
                    PatternWave();
                    break;
                case 2:
                    PatternLine();
                    break;
                case 3:
                    PatternTriangleWave();
                    break;
                case 4:
                    PatternCoolWave();
                    break;
                case 5:
                    PatternSpin();
                    break;
            }
        }

        private float calculateDifficulty(float height, float speed, int quantity, bool aiming, int amountpattern, float bonus, int multiplied)
        {
            float difficulty = (float)(Math.Pow(1.25, (quantity / 10) + height) * Math.Pow(1.5, speed) * Math.Pow(1.1, amountpattern));
            if(aiming)
            {
                 difficulty /= 1.5f;
            }
            difficulty += bonus;
            difficulty *= multiplied;
            return difficulty;
        }
        /// <summary>
        /// These Will be the Base Patterns
        /// </summary>
        public void PatternWave()
        {
            int numberbullets = (int)Pattern.PatternDifficulty * 2 + 1;
            float directionModifier = -0.1f * ((numberbullets - 1) / 2);
            for (int i = 1; i <= numberbullets; i++)
            {
                bulletAddRad(Pattern.PatternSpeed, Pattern.PatternAngleRadian + directionModifier);
                directionModifier += 0.1f;
            }
            ScorePattern = (int)calculateDifficulty(Pattern.PatternBulletWidth, Pattern.PatternSpeed, numberbullets, true, PatternCount, 0, 1);
        }
        public void PatternLine()
        {
            int numberbullets = (int)Pattern.PatternDifficulty + 1;
            for (int i = 1; i <= numberbullets; i++)
            {
                bulletAddRad(0.12f + Pattern.PatternSpeed, Pattern.PatternAngleRadian);
                Pattern.PatternSpeed += 0.02f;
            }
            ScorePattern = (int)calculateDifficulty(Pattern.PatternBulletWidth, Pattern.PatternSpeed, numberbullets, true, PatternCount, 0, 1);
        }
        public void PatternFlower()
        {
            int numberbullets = (int)Pattern.PatternDifficulty * 16;
            double timeSaved = Time.Current;
            int a = 0;
            for (int j = 1; j <= numberbullets; j++)
            {
                a = a + 21;
                Pattern.PatternAngleRadian = MathHelper.DegreesToRadians(a - 90);
                bulletAddRad(Pattern.PatternSpeed, a);
            }
            ScorePattern = (int)calculateDifficulty(Pattern.PatternBulletWidth, Pattern.PatternSpeed, numberbullets, true, PatternCount, 0, 1);
        }
        public void PatternCoolWave()
        {
            int numberbullets = (int)(Pattern.PatternDifficulty * 2) + 3;
            float speedModifier = 0.01f + 0.01f * (Pattern.PatternDifficulty);
            float directionModifier = -0.075f - 0.075f * (Pattern.PatternDifficulty);
            for (int i = 1; i <= numberbullets; i++)
            {
                bulletAddRad(
                    Pattern.PatternSpeed + Math.Abs(speedModifier),
                    directionModifier + Pattern.PatternAngleRadian
                );
                speedModifier -= 0.01f;
                directionModifier += 0.075f;
            }
            ScorePattern = (int)calculateDifficulty(Pattern.PatternBulletWidth, Pattern.PatternSpeed, numberbullets, true, PatternCount, 0, 1);
        }
        public void PatternSpin()
        {
            int numberbullets = (int)(30 * Pattern.PatternDifficulty);
            int numberspins = (int)(Pattern.PatternDifficulty + 2) / 2;
            float spinModifier = MathHelper.DegreesToRadians((float)(360 / numberspins));
            float directionModifier = (float)(360 / numberbullets);
            directionModifier = MathHelper.DegreesToRadians(directionModifier);
            double originalDuration = Pattern.PatternDuration;
            Pattern.PatternDuration /= numberbullets;
            Pattern.PatternDuration /= Pattern.PatternRepeatTimes;
            int i = 1;
            while (i <= Pattern.PatternRepeatTimes)
            {
                int j = 1;
                while (j <= numberbullets)
                {
                    Scheduler.AddDelayed(() =>
                    {
                        for (int k = 1; k <= numberspins; k++)
                            bulletAddRad(Pattern.PatternSpeed, Pattern.PatternAngleRadian + (spinModifier * (k - 1)));
                        Pattern.PatternAngleRadian -= directionModifier;
                    }, Pattern.PatternDuration * (j - 1) + (originalDuration * (i - 1)));
                    j++;
                }
                i++;
            }
            ScorePattern = (int)calculateDifficulty(Pattern.PatternBulletWidth, Pattern.PatternSpeed, numberbullets, true, PatternCount, 0, (int)Pattern.PatternRepeatTimes);
        }
        public void PatternTriangleWave()
        {
            bool reversed = true;
            int numberwaves = (int)(Pattern.PatternDifficulty + 2) / 2;
            float originalDirection = 0f;
            int numberbullets;
            int totalbullets = 0;
            Pattern.PatternDuration /= numberwaves;
            float speedModifier;
            for (int i = 1; i <= numberwaves; i++)
            {
                numberbullets = i;
                totalbullets += numberbullets;
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
            ScorePattern = (int)calculateDifficulty(Pattern.PatternBulletWidth, Pattern.PatternSpeed, totalbullets, true, PatternCount, 0, 1);
        }

        public void PatternCurve()
        {
            bool lefttoright = true;
            int numberbullets = (int)(Pattern.PatternDifficulty + 10) / 2;
            float originalDirection = 0.01f * (numberbullets / 2);
            float speedModifier = 0f;
            float directionModifier = 0f;
            for(int i = 1; i <= numberbullets; i++)
            {
                if (lefttoright)
                {
                    bulletAddRad(
                        Pattern.PatternSpeed + speedModifier,
                        Pattern.PatternAngleRadian - originalDirection + directionModifier
                    );
                }
                else
                {
                    bulletAddRad(
                        Pattern.PatternSpeed + speedModifier,
                        Pattern.PatternAngleRadian + originalDirection - directionModifier
                    );
                }
                directionModifier += 0.015f;
                speedModifier -= (i * 0.002f);
            }
            ScorePattern = (int)calculateDifficulty(Pattern.PatternBulletWidth, Pattern.PatternSpeed, numberbullets, true, PatternCount, 0, 1);
        }
        public void PatternCircle()
        {
            int numberbullets = (int)(Pattern.PatternDifficulty + 1) * 8;
            float directionModifier = (float)(360 / numberbullets);
            directionModifier = MathHelper.DegreesToRadians(directionModifier);
            for (int i = 1; i <= numberbullets; i++)
            {
                bulletAddRad(Pattern.PatternSpeed, Pattern.PatternAngleRadian + (directionModifier * (i - 1)));
            }
            ScorePattern = (int)calculateDifficulty(Pattern.PatternBulletWidth, Pattern.PatternSpeed, numberbullets, false, PatternCount, 0, 1);
        }
    }
}

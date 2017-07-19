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
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Vitaru.Objects.Characters;
using System.Collections.Generic;
using osu.Framework.Audio.Sample;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawablePattern : DrawableVitaruHitObject
    {
        protected Pattern Pattern;
        private bool visable = false;
        private bool patternCreated = false;
        private Vector2 patternStartPosition;
        public bool DummyMode = false;

        public int BulletCount = 0;
        public static int PatternCount = 0;
        public int ScorePattern;

        private Enemy enemy;

        public bool Miss = false;
        public bool Hit = false;
        public bool Ten = false;
        public int Score = 0;

        private bool upwards = false;

        public DrawablePattern(Pattern pattern) : base(pattern)
        {
            AlwaysPresent = true;
            Pattern = pattern;
        }

        public void ForceJudgment()
        {
            UpdateJudgement(true);
        }

        private List<SampleChannel> temporalSamples = new List<SampleChannel>();

        protected override void CheckJudgement(bool userTriggered)
        {
            base.CheckJudgement(userTriggered);

            if (enemy != null && enemy.Dead && HitObject.StartTime <= Time.Current)
            {
                Judgement.Result = HitResult.Hit;
                Judgement.Score = VitaruScoreResult.Graze300;
                DeletePattern();
            }
            if (Miss && !Judged)
            {
                temporalSamples = Samples;
                Samples = new List<SampleChannel>();
                Judgement.Result = HitResult.Miss;
                Judgement.Score = VitaruScoreResult.Miss;
                DeletePattern();
            }
            if (Hit && Waiting)
            {
                Samples = new List<SampleChannel>();
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
                DeletePattern();
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
            if (!VitaruRuleset.TouhosuMode)
                Children = new Drawable[]
                {
                new Container
                {
                    Masking = true,
                    AutoSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    BorderThickness = 10,
                    Depth = 5,
                    BorderColour = Pattern.ComboColour,
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
                            Colour = (Pattern.ComboColour).Opacity(0.5f),
                            Radius = 2f,
                        }
                }
                };
            else
            {
                VitaruPlayfield.vitaruPlayfield.Add(enemy = new Enemy()
                {
                    Colour = Pattern.ComboColour,
                    Alpha = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Depth = 5,
                    Team = 1,
                });
                enemy.MoveTo(Position);
                enemy.FadeInFromZero(TIME_FADEIN);
            }

            visable = true;
            GetPatternStartPosition();
            Position = patternStartPosition;
            if (patternStartPosition.Y > 512)
                upwards = true;
            MoveTo(Pattern.PatternPosition, TIME_PREEMPT);
            FadeInFromZero(TIME_FADEIN);
        }

        protected override void UpdateState(ArmedState state)
        {
            base.UpdateState(state);

            if (Pattern.IsSlider)
                Pattern.EndTime = Pattern.StartTime + Pattern.RepeatCount * Pattern.Curve.Distance / Pattern.Velocity;

            if (Pattern.IsSpinner)
                Pattern.EndTime = (HitObject as IHasEndTime)?.EndTime ?? Pattern.StartTime;
        }

        private Vector2 GetPatternStartPosition()
        {
            if (Pattern.PatternPosition.X <= (384 / 2) && Pattern.PatternPosition.Y <= (512 / 2))
                patternStartPosition = Pattern.PatternPosition - new Vector2(384 / 2, 512 / 2);
            else if (Pattern.PatternPosition.X > (384 / 2) && Pattern.PatternPosition.Y <= (512 / 2))
                patternStartPosition = new Vector2(Pattern.PatternPosition.X + 384 / 2, Pattern.PatternPosition.Y - 512 / 2);
            else if (Pattern.PatternPosition.X > (384 / 2) && Pattern.PatternPosition.Y > (512 / 2))
                patternStartPosition = Pattern.PatternPosition + new Vector2(384 / 2, 512 / 2);
            else
                patternStartPosition = new Vector2(Pattern.PatternPosition.X - 384 / 2, Pattern.PatternPosition.Y + 512 / 2);

            return patternStartPosition;
        }

        protected void PatternPop()
        {
            ScaleTo(0.1f, TIME_PREEMPT / 8);
        }

        public bool Waiting = false;

        protected override void Update()
        {
            base.Update();

            if (!sliderDone && Judgement.Result == HitResult.Miss && temporalSamples.Count > 0)
                Samples = temporalSamples;

            if (enemy != null && !enemy.Dead)
                enemy.MoveTo(Position);

            if (patternCreated && BulletCount == 0)
            {
                PatternCount--;
                DeletePattern();
            }

            if (Waiting)
            {

            }

            if (HitObject.StartTime - TIME_PREEMPT <= Time.Current && !visable)
                PatternStart();

            if (!Pattern.IsSlider && !Pattern.IsSpinner && visable)
                hitcircle();

            if (Pattern.IsSlider && visable)
                slider();

            if (Pattern.IsSpinner && visable)
                spinner();
        }

        private bool hasShot = false;
        private bool sliderDone = false;
        private int currentRepeat;

        /// <summary>
        /// All the hitcircle stuff
        /// </summary>
        private void hitcircle()
        {
            if (Pattern.StartTime - (TIME_PREEMPT / 8) <= Time.Current && hasShot && !sliderDone)
                PatternPop();

            if (HitObject.StartTime <= Time.Current && !hasShot)
            {
                WaitForJudge();
                hasShot = true;
                CreatePattern();
            }
        }

        /// <summary>
        /// All The Slider Stuff
        /// </summary>
        private void slider()
        {
            if (HitObject.StartTime <= Time.Current && !hasShot)
            {
                hasShot = true;
                CreatePattern();
            }

            if (Pattern.EndTime - (TIME_PREEMPT / 8) <= Time.Current && hasShot && !sliderDone)
                PatternPop();

            if (Pattern.EndTime <= Time.Current && hasShot && !sliderDone)
            {
                CreatePattern();
                WaitForJudge();
                sliderDone = true;
            }

            double progress = MathHelper.Clamp((Time.Current - Pattern.StartTime) / Pattern.Duration, 0, 1);

            int repeat = Pattern.RepeatAt(progress);
            progress = Pattern.ProgressAt(progress);

            if (repeat > currentRepeat)
            {
                if (repeat < Pattern.RepeatCount)
                {
                    CreatePattern();
                    if (temporalSamples.Count > 0)
                        Samples = temporalSamples;
                }
                currentRepeat = repeat;
            }
            if (!sliderDone && HitObject.StartTime <= Time.Current)
                UpdateProgress(progress, repeat);
        }

        public void UpdateProgress(double progress, int repeat)
        {
            Position = Pattern.Curve.PositionAt(progress);
        }

        /// <summary>
        /// all the spinner stuff
        /// </summary>
        private void spinner()
        {
            if (Pattern.EndTime - (TIME_PREEMPT / 8) <= Time.Current && hasShot && !sliderDone)
                PatternPop();

            if (Pattern.StartTime <= Time.Current && !hasShot)
            {
                hasShot = true;
                CreatePattern();
            }
            if (Pattern.EndTime <= Time.Current && hasShot)
                WaitForJudge();
        }

        public void WaitForJudge()
        {
            Waiting = true;
            FadeOutFromOne(TIME_FADEOUT);
            ScaleTo(new Vector2(0.1f), TIME_FADEOUT);
            if (enemy != null)
            {
                enemy.FadeOutFromOne(TIME_FADEOUT);
                enemy.ScaleTo(new Vector2(0.1f), TIME_FADEOUT);
            }
        }

        protected void bulletAddRad(float speed, float angle)
        {
            if (upwards && !VitaruRuleset.TouhosuMode)
                angle += MathHelper.Pi;
            BulletCount++;
            DrawableBullet drawableBullet;
            VitaruPlayfield.vitaruPlayfield.Add(drawableBullet = new DrawableBullet(this)
            {
                Origin = Anchor.Centre,
                Depth = 5,
                BulletColor = Pattern.ComboColour,
                BulletAngleRadian = angle,
                BulletSpeed = speed,// + PatternSpeed,
                BulletWidth = Pattern.PatternBulletWidth,
                BulletDamage = Pattern.PatternDamage,
                DynamicBulletVelocity = Pattern.DynamicPatternVelocity,
                Team = 1,
            });
            drawableBullet.MoveTo(Position);
        }

        public void DeletePattern()
        {
            if (enemy != null)
                enemy.LifetimeEnd = Time.Current + 50;
            LifetimeEnd = Pattern.EndTime + 50;
        }

        public void CreatePattern()
        {
            PlaySamples();
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
            if (aiming)
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
            int numberbullets = (int)(30 * (Pattern.PatternDifficulty / 3) * (Pattern.PatternDuration / 1000));
            int numberspirals = (int)Pattern.PatternDifficulty;
            float spinModifier = MathHelper.DegreesToRadians(360 / numberspirals);
            float directionModifier = 360 / numberbullets;
            directionModifier = MathHelper.DegreesToRadians(directionModifier);
            double modifiedDuration = Pattern.PatternDuration / numberbullets / Pattern.PatternRepeatTimes;
            int i = 1;
            while (i <= Pattern.PatternRepeatTimes)
            {
                int j = 1;
                while (j <= numberbullets)
                {
                    Scheduler.AddDelayed(() =>
                    {
                        for (int k = 1; k <= numberspirals; k++)
                            bulletAddRad(Pattern.PatternSpeed, Pattern.PatternAngleRadian + (spinModifier * (k - 1)));
                        Pattern.PatternAngleRadian -= directionModifier;
                    }, modifiedDuration * (j - 1) + (modifiedDuration * (i - 1)));
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
            double duration = Pattern.PatternDuration / numberwaves;
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
            for (int i = 1; i <= numberbullets; i++)
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

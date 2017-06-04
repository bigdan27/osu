using OpenTK.Graphics;
using OpenTK;
using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;
using osu.Game.Rulesets.Vitaru.Objects.Characters;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Objects.Types;
using osu.Framework.MathUtils;
using System.Collections.Generic;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Audio;
using System.Linq;
using osu.Game.Rulesets.Vitaru.Beatmaps;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableVitaruEnemy : DrawableCharacter
    {
        private readonly Enemy enemy;
        public bool Shoot = false;
        private float playerPos;
        private Color4 enemyColor = Color4.Green;
        private int patternID = -1;
        private float OD = 6f;

        public static int EnemyIDCount = 0;
        public int EnemyID = 0;

        private readonly List<ISliderProgress> components = new List<ISliderProgress>();
        private int currentRepeat;
        private bool returnJudge = false;
        private bool leaving = false;

        public DrawableVitaruEnemy(Enemy enemy) : base(enemy)
        {
            this.enemy = enemy;
            AlwaysPresent = true;
            Origin = Anchor.Centre;
            Position = enemy.Position;
            CharacterType = HitObjectType.Enemy;
            CharacterHealth = 50;
            Team = 1;
            HitboxWidth = 24;
            CharacterColor = Color4.Cyan;
            Alpha = 1;
            EnemyIDCount++;
            EnemyID = EnemyIDCount;
            OnDeath = dyingAnimation;
            VitaruBeatmapConverter.EnemyList.Add(this);
        }

        private bool hasShot = false;
        private bool sliderDone = false;

        private void death()
        {
            pop();
            returnJudge = true;
            VitaruBeatmapConverter.EnemyList.Remove(this);
            Dispose();
        }

        private void dyingAnimation()
        {
            if (leaving)
                death();
            double deathDuration = 1000;
            Dead = true;
            VitaruBeatmapConverter.EnemyList.Remove(this);
            RotateTo(60, deathDuration, EasingTypes.InExpo);
            MoveTo(new Vector2(Position.X, Position.Y - 400), deathDuration, EasingTypes.InBack);
            ScaleTo(new Vector2(0.25f), deathDuration, EasingTypes.OutSine);
        }

        protected override void Update()
        {
            enemy.EnemyPosition = enemy.Position;

            if (patternID == -1) // Should maybe be only on spawn
                getPatternID();

            HitDetect();

            if (enemy.IsSpinner)
                spinner();
            else if (enemy.IsSlider)
                slider(patternID);
            else
                hitcircle(patternID);
        }

        /// <summary>
        /// Generic Enemy stuff
        /// </summary>

        private int getPatternID()
        {
            SampleInfoList samples = enemy.Samples;
            bool snipe = samples.Any(s => s.Name == SampleInfo.HIT_WHISTLE);
            bool wave = samples.Any(s => s.Name == SampleInfo.HIT_NORMAL);
            bool circle = samples.Any(s => s.Name == SampleInfo.HIT_FINISH);
            bool line = samples.Any(s => s.Name == SampleInfo.HIT_CLAP);

            if (snipe)
                return patternID = 1;
            if (circle)
                return patternID = 3;
            if (line)
                return patternID = 4;
            if (wave)
                return patternID = 2;
            else
                return patternID = 0;
        }

        protected override void CheckJudgement(bool userTriggered)
        {
        }

        protected override void UpdateInitialState()
        {
            base.UpdateInitialState();

            if (enemy.IsSlider)
                enemy.EndTime = enemy.StartTime + enemy.RepeatCount * enemy.Curve.Distance / enemy.Velocity;

            Alpha = 0f;
            Scale = new Vector2(0.5f);
        }

        protected override void UpdatePreemptState()
        {
            base.UpdatePreemptState();

            FadeIn(Math.Min(TIME_FADEIN * 2, TIME_PREEMPT), EasingTypes.OutQuart);
            ScaleTo(1f, TIME_PREEMPT, EasingTypes.OutQuart);
        }

        double endTime;

        protected override void UpdateState(ArmedState state)
        {
            base.UpdateState(state);

            if (enemy.IsSlider)
                endTime = (HitObject as IHasEndTime)?.EndTime ?? HitObject.StartTime;
            double duration = endTime - HitObject.StartTime;

            Delay(HitObject.StartTime - Time.Current + Judgement.TimeOffset, true);

            switch (State)
            {
                case ArmedState.Idle:
                    Delay(duration + TIME_PREEMPT);
                    Expire(true);
                    break;
                case ArmedState.Hit:
                    Expire();
                    break;
            }
            Expire();
        }

        private void leave()
        {
            leaving = true;
            int r = RNG.Next(-100, 612);
            MoveTo(new Vector2(r, -300), 2000, EasingTypes.InCubic);
            FadeOut(2000, EasingTypes.InCubic);
            ScaleTo(new Vector2(0.75f), 2000, EasingTypes.InCubic);
        }

        /// <summary>
        /// All the hitcircle stuff
        /// </summary>
        private void hitcircle(int patternID)
        {
            if (Dead && leaving)
            {
                death();
            }

            if (HitObject.StartTime <= Time.Current && !hasShot && Dead)
            {
                PlaySamples();
                death();
            }

            if (HitObject.StartTime <= Time.Current && !hasShot && !Dead)
            {
                enemyShoot(patternID);
                leave();
                hasShot = true;
            }

            if (HitObject.StartTime <= Time.Current && hasShot && Position.Y <= -300)
            {
                death();
            }
        }

        /// <summary>
        /// All The Slider Stuff
        /// </summary>
        private void slider(int patternID)
        {
            if (Dead && leaving)
            {
                death();
            }

            if (HitObject.StartTime <= Time.Current && !hasShot && Dead && !leaving)
            {
                PlaySamples();
                death();
            }

            if (HitObject.StartTime <= Time.Current && !hasShot && !Dead)
            {
                enemyShoot(patternID);
                hasShot = true;
            }

            if (enemy.EndTime <= Time.Current && hasShot && !sliderDone && !Dead)
            {
                enemyShoot(patternID);
                leave();
                sliderDone = true;
            }

            if (enemy.EndTime <= Time.Current && hasShot && !sliderDone && Dead)
            {
                death();
                PlaySamples();
                sliderDone = true;
            }

            if (enemy.EndTime <= Time.Current && hasShot && Position.Y <= -300)
            {
                death();
            }

            double progress = MathHelper.Clamp((Time.Current - enemy.StartTime) / enemy.Duration, 0, 1);

            int repeat = enemy.RepeatAt(progress);
            progress = enemy.ProgressAt(progress);

            if (repeat > currentRepeat)
            {
                if (repeat < enemy.RepeatCount)
                {
                    enemyShoot(patternID);
                }
                currentRepeat = repeat;
            }
            if (!sliderDone)
                UpdateProgress(progress, repeat);
        }

        internal interface ISliderProgress
        {
            void UpdateProgress(double progress, int repeat);
        }

        public void UpdateProgress(double progress, int repeat)
        {
            if (!sliderDone)
                Position = enemy.Curve.PositionAt(progress);
        }

        /// <summary>
        /// all the spinner stuff
        /// </summary>
        private void spinner()
        {
            double totalDuration = enemy.EndTime - enemy.StartTime;

            if (enemy.StartTime <= Time.Current && !hasShot)
            {
                enemyShoot(5);
                hasShot = true;
            }

            if (enemy.EndTime <= Time.Current && hasShot == true && !leaving)
            {
                leave();
                leaving = true;
            }
            if (Position.Y <= -300)
                Dispose();
        }

        /// <summary>
        /// All the shooting stuff
        /// </summary>

        private void enemyShoot(int patternID)
        {
            playerRelativePositionAngle();
            PlaySamples();
            switch (patternID)
            {
                case 2: // Wave
                    Wave w;
                    VitaruPlayfield.vitaruPlayfield.Add(w = new Wave(Team)
                    {
                        Origin = Anchor.Centre,
                        Depth = 6,
                        PatternColor = Color4.Green,
                        PatternAngleRadian = playerPos,
                        PatternSpeed = 0.2f,
                        PatternBulletWidth = 8,
                        PatternDifficulty = OD,
                        PatternDamage = 10,
                    });
                    w.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), w));
                    break;

                case 4: // Line
                    Line l;
                    VitaruPlayfield.vitaruPlayfield.Add(l = new Line(Team)
                    {
                        Origin = Anchor.Centre,
                        Depth = 6,
                        PatternColor = Color4.Yellow,
                        PatternAngleRadian = playerPos,
                        PatternSpeed = 0.2f,
                        PatternBulletWidth = 8,
                        PatternDifficulty = OD,
                        PatternDamage = 10,
                    });
                    l.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), l));
                    break;

                case 0: // Cool wave
                    CoolWave cw;
                    VitaruPlayfield.vitaruPlayfield.Add(cw = new CoolWave(Team)
                    {
                        Origin = Anchor.Centre,
                        Depth = 6,
                        PatternColor = Color4.Green,
                        PatternAngleRadian = playerPos,
                        PatternSpeed = 0.2f,
                        PatternBulletWidth = 8,
                        PatternDifficulty = OD,
                        PatternDamage = 10,
                    });
                    cw.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), cw));
                    break;

                case 3: // Circle is a spin with 1 bullet
                    Spin c;
                    VitaruPlayfield.vitaruPlayfield.Add(c = new Spin(Team)
                    {
                        Origin = Anchor.Centre,
                        Depth = 6,
                        PatternColor = Color4.Cyan,
                        PatternAngleRadian = playerPos,
                        PatternSpeed = 0.15f,
                        PatternBulletWidth = 12,
                        PatternDifficulty = OD,
                        PatternDamage = 5,
                    });
                    c.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), c));
                    break;

                case 1: // Snipe is a line with 1 bullet
                    Line f;
                    VitaruPlayfield.vitaruPlayfield.Add(f = new Line(Team)
                    {
                        Origin = Anchor.Centre,
                        Depth = 6,
                        PatternColor = Color4.Yellow,
                        PatternAngleRadian = playerPos,
                        PatternSpeed = 0.38f,
                        PatternBulletWidth = 8,
                        PatternDifficulty = 0,
                        PatternDamage = 20,
                    });
                    f.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), f));
                    break;

                case 5: // Spin

                    double totalDuration = enemy.EndTime - enemy.StartTime;

                    Spin s;
                    VitaruPlayfield.vitaruPlayfield.Add(s = new Spin(Team)
                    {
                        Origin = Anchor.Centre,
                        Depth = 6,
                        PatternColor = Color4.Purple,
                        PatternAngleRadian = playerPos,
                        PatternSpeed = 0.25f,
                        PatternBulletWidth = 9,
                        PatternDifficulty = OD,
                        PatternDamage = 5,
                        PatternRepeatTimes = 1f,
                        PatternRepeatDelay = 0,
                        PatternDuration = (double)totalDuration,
                    });
                    s.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), s));
                    break;

                case 6: // Triangle

                    Trianglewave t;
                    VitaruPlayfield.vitaruPlayfield.Add(t = new Trianglewave(Team)
                    {
                        Origin = Anchor.Centre,
                        Depth = 6,
                        PatternColor = Color4.Pink,
                        PatternAngleRadian = playerPos,
                        PatternSpeed = 0.1f,
                        PatternBulletWidth = 9,
                        PatternDifficulty = OD,
                        PatternDamage = 5,
                        PatternDuration = 500,
                    });
                    t.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), t));
                    break;
            }
        }

        public float playerRelativePositionAngle()
        {
            //Returns a Radian
            playerPos = (float)Math.Atan2((VitaruPlayer.PlayerPosition.Y - Position.Y), (VitaruPlayer.PlayerPosition.X - Position.X));
            return playerPos;
        }

        private void pop()
        {
            GlowRing.Alpha = 1;
            CharacterSprite.Alpha = 0;
            GlowRing.ScaleTo(new Vector2(1), 300);
            GlowRing.FadeOut(300);
            GlowRing.Colour = Color4.Green;
        }

        protected override void CharacterJudgment()
        {
            Bullet.BulletScore = VitaruScoreResult.Kill20;
            Bullet.BulletResult = HitResult.Hit;
        }
    }
}
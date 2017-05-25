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

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableVitaruEnemy : DrawableCharacter
    {
        private readonly Enemy enemy;
        public bool Shoot = false;
        private float playerPos;
        private Color4 enemyColor = Color4.Green;
        private int patternID = -1;

        public static int EnemyIDCount = 0;
        public int EnemyID = 0;

        private readonly List<ISliderProgress> components = new List<ISliderProgress>();
        private int currentRepeat;

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
            OnDeath = death;
            VitaruBeatmapConverter.EnemyList.Add(this);
        }

        private bool hasShot = false;
        private bool sliderDone = false;
        
        private void death()
        {
            VitaruBeatmapConverter.EnemyList.Remove(this);
            Dispose();
        }

        protected override void Update()
        {
            enemy.EnemyPosition = enemy.Position;

            if(patternID == -1)
                getPatternID();

            HitDetect();

            if (!enemy.IsSlider && !enemy.IsSpinner)
                hitcircle(patternID);

            if (enemy.IsSlider)
                slider(patternID);

            if (enemy.IsSpinner)
                spinner();
                
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
            if (CharacterHealth <= 0)
            {
                if (!hasShot)
                    PlaySamples();
                Judgement.Result = HitResult.Hit;
                Judgement.Score = VitaruScoreResult.Kill10;
            }
        }

        protected override void UpdateInitialState()
        {
            base.UpdateInitialState();

            if(enemy.IsSlider)
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

            if(enemy.IsSlider)
                endTime = (HitObject as IHasEndTime)?.EndTime ?? HitObject.StartTime;
            double duration = endTime - HitObject.StartTime;

            Delay(HitObject.StartTime - Time.Current + Judgement.TimeOffset, true);

            //Does nothing atm
            switch (State)
            {
                case ArmedState.Idle:
                    Delay(duration + TIME_PREEMPT);
                    FadeOut(TIME_FADEOUT * 2);
                    Expire(true);
                    break;
                case ArmedState.Hit:
                    FadeOut(TIME_FADEOUT / 4);
                    Expire();
                    break;
            }

            Expire();
        }

        private void leave()
        {
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
            if (HitObject.StartTime <= Time.Current && hasShot == false)
            {
                enemyShoot(patternID);
                leave();
                hasShot = true;
            }
            if (HitObject.StartTime <= Time.Current && hasShot == true && Position.Y <= -300)
            {
                death();
            }
        }

        /// <summary>
        /// All The Slider Stuff
        /// </summary>
        private void slider(int patternID)
        {
            if (HitObject.StartTime <= Time.Current && hasShot == false)
            {
                enemyShoot(patternID);
                hasShot = true;
            }

            if (enemy.EndTime <= Time.Current && hasShot == true && sliderDone == false)
            {
                enemyShoot(patternID);
                leave();
                sliderDone = true;
            }

            if (enemy.EndTime <= Time.Current && hasShot == true && Position.Y <= -300)
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
            if(!sliderDone)
                UpdateProgress(progress, repeat);
        }

        public void UpdateProgress(double progress, int repeat)
        {
            if(!sliderDone)
                Position = enemy.Curve.PositionAt(progress);
        }

        internal interface ISliderProgress
        {
            void UpdateProgress(double progress, int repeat);
        }

        /// <summary>
        /// all the spinner stuff
        /// </summary>
        private void spinner()
        {

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
                        PatternDifficulty = 2f,
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
                        PatternColor = Color4.Green,
                        PatternAngleRadian = playerPos,
                        PatternSpeed = 0.2f,
                        PatternBulletWidth = 8,
                        PatternDifficulty = 2f,
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
                        PatternDifficulty = 4f,
                        PatternDamage = 10,
                    });
                    cw.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), cw));
                    break;

                case 3: // Circle
                    Circle c;
                    VitaruPlayfield.vitaruPlayfield.Add(c = new Circle(Team)
                    {
                        Origin = Anchor.Centre,
                        Depth = 6,
                        PatternColor = Color4.Cyan,
                        PatternAngleRadian = playerPos,
                        PatternSpeed = 0.15f,
                        PatternBulletWidth = 12,
                        PatternDifficulty = 2f,
                        PatternDamage = 5,
                    });
                    c.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), c));
                    break;

                case 1: // Snipe!
                    Wave f;
                    VitaruPlayfield.vitaruPlayfield.Add(f = new Wave(Team)
                    {
                        Origin = Anchor.Centre,
                        Depth = 6,
                        PatternColor = Color4.Green,
                        PatternAngleRadian = playerPos,
                        PatternSpeed = 0.5f,
                        PatternBulletWidth = 8,
                        PatternDifficulty = 0.4f,
                        PatternDamage = 20,
                    });
                    f.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), f));
                    break;
            }
        }

        public float playerRelativePositionAngle()
        {
            //Returns a Radian
            playerPos = (float)Math.Atan2((VitaruPlayer.PlayerPosition.Y - Position.Y),(VitaruPlayer.PlayerPosition.X - Position.X));
            return playerPos;
        }
    }
}
using OpenTK;
using OpenTK.Input;
using osu.Framework.Graphics;
using osu.Framework.Input;
using System.Collections.Generic;
using OpenTK.Graphics;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Configuration;
using osu.Framework.Allocation;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Framework.Audio.Track;
using osu.Game.Beatmaps.ControlPoints;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;

namespace osu.Game.Rulesets.Vitaru.Objects
{
    public class VitaruPlayer : Character
    {
        public static Vector2 PlayerPosition = new Vector2(256, 700);

        public static float PlayerHealth = 100;

        public int ProjectileDamage { get; set; }

        public bool Shooting = false;

        private Dictionary<Key, bool> keys = new Dictionary<Key, bool>();

        //(MinX,MaxX,MinY,MaxY)
        private Vector4 playerBounds = new Vector4(0, 512, 0, 384);

        public VitaruPlayer()
        {
            //Gross Key stuff
            keys[Key.Up] = false;
            keys[Key.Right] = false;
            keys[Key.Down] = false;
            keys[Key.Left] = false;
            keys[Key.LShift] = false;
            keys[Key.RShift] = false;
            keys[Key.Z] = false;

            CharacterName = "player";
            Team = 0;
            CharacterHealth = 100;
            Position = PlayerPosition;

            if(VitaruRuleset.TouhosuMode)
            {
                playerBounds = new Vector4(0, 512, 0, 820);
            }
        }

        private const float playerSpeed = 0.25f;
        private Vector2 positionChange = Vector2.Zero;

        double nextHalfBeat = -1;
        double beatLength;

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            float amplitudeAdjust = Math.Min(1, 0.4f + amplitudes.Maximum);

            beatLength = timingPoint.BeatLength;

            if (nextHalfBeat == -1)
                nextHalfBeat = timingPoint.Time;

            double beat_in_time = 60;

            CharacterSign.ScaleTo(1 - 0.02f * amplitudeAdjust, beat_in_time, EasingTypes.Out);
            using (CharacterSign.BeginDelayedSequence(beat_in_time))
                CharacterSign.ScaleTo(1, beatLength * 2, EasingTypes.OutQuint);

            if (effectPoint.KiaiMode)
            {
                CharacterSign.FadeTo(0.25f * amplitudeAdjust, beat_in_time, EasingTypes.Out);
                using (CharacterSign.BeginDelayedSequence(beat_in_time))
                    CharacterSign.FadeOut(beatLength);
            }

            if (effectPoint.KiaiMode && CharacterSprite.Alpha == 1)
            {
                CharacterKiaiSprite.FadeInFromZero(timingPoint.BeatLength / 4);
                CharacterSprite.FadeOutFromOne(timingPoint.BeatLength / 4);
                CharacterSign.FadeTo(0.15f , timingPoint.BeatLength / 4);
            }
            if(!effectPoint.KiaiMode && CharacterKiaiSprite.Alpha == 1)
            {
                CharacterSprite.FadeInFromZero(timingPoint.BeatLength);
                CharacterKiaiSprite.FadeOutFromOne(timingPoint.BeatLength);
                CharacterSign.FadeTo(0f, timingPoint.BeatLength);
            }
        }

        protected void OnHalfBeat()
        {
            nextHalfBeat = nextHalfBeat + (beatLength / 2);

            if (Shooting)
                Shoot();
        }

        public void Shoot()
        {
            PatternWave();
        }

        protected override void Update()
        {
            base.Update();

            PlayerHealth = CharacterHealth;

            if (nextHalfBeat + (beatLength / 2) <= Time.Current)
                OnHalfBeat();

            HitDetect();

            if (CharacterSign.Alpha > 0)
                CharacterSign.RotateTo((float)((Clock.CurrentTime / 1000) * 90));

            if (keys[Key.Z] && VitaruRuleset.TouhosuMode)
            {
                Shooting = true;
            }
            if (keys[Key.Z] == false && VitaruRuleset.TouhosuMode)
            {
                Shooting = false;
            }

            playerMovement();
        }

        private void playerMovement()
        {
            //Handles Player Speed
            float yTranslationDistance = playerSpeed * (float)(Clock.ElapsedFrameTime);
            float xTranslationDistance = playerSpeed * (float)(Clock.ElapsedFrameTime);

            if (keys[Key.LShift] | keys[Key.RShift])
            {
                xTranslationDistance /= 2;
                yTranslationDistance /= 2;
            }
            if (keys[Key.Up])
            {
                PlayerPosition.Y -= yTranslationDistance;
            }
            if (keys[Key.Left])
            {
                PlayerPosition.X -= xTranslationDistance;
            }
            if (keys[Key.Down])
            {
                PlayerPosition.Y += yTranslationDistance;
            }
            if (keys[Key.Right])
            {
                PlayerPosition.X += xTranslationDistance;
            }

            PlayerPosition = Vector2.ComponentMin(PlayerPosition, playerBounds.Yw);
            PlayerPosition  = Vector2.ComponentMax(PlayerPosition, playerBounds.Xz);
            Position = PlayerPosition;
        }

        protected void bulletAddRad(float speed, float angle)
        {
            DrawableBullet drawableBullet;
            VitaruPlayfield.vitaruPlayfield.Add(drawableBullet = new DrawableBullet(new DrawablePattern(new Pattern()) { DummyMode = true, })
            {
                Origin = Anchor.Centre,
                Depth = 5,
                BulletColor = Color4.Red,
                BulletAngleRadian = angle,
                BulletSpeed = speed,
                BulletWidth = 10,
                BulletDamage = 20,
                Team = Team,
                DummyMode = true,
            });
            drawableBullet.MoveTo(Position);
        }

        public void PatternWave()
        {
            int numberbullets = 3;
            float directionModifier = -0.1f * ((numberbullets - 1) / 2);
            for (int i = 1; i <= numberbullets; i++)
            {
                bulletAddRad(1, MathHelper.DegreesToRadians(0 - 90) + directionModifier);
                directionModifier += 0.1f;
            }
        }

        public override void Death()
        {
            PlayerHealth = CharacterHealth;
            LifetimeEnd = Time.Current + 30;
        }

        protected override bool OnKeyDown(InputState state, KeyDownEventArgs args)
        {
            keys[args.Key] = true;
            if (args.Key == Key.LShift || args.Key == Key.RShift)
                Hitbox.Alpha = 1;
            return base.OnKeyDown(state, args);
        }
        protected override bool OnKeyUp(InputState state, KeyUpEventArgs args)
        {
            keys[args.Key] = false;
            if (args.Key == Key.LShift || args.Key == Key.RShift)
                Hitbox.Alpha = 0;
            return base.OnKeyUp(state, args);
        }


    }
}

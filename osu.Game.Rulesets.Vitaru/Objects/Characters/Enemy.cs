using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Game.Beatmaps.ControlPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu.Game.Rulesets.Vitaru.Objects.Characters
{
    public class Enemy : Character
    {
        public Enemy()
        {
            CharacterName = "enemy";
            Team = 1;
            CharacterHealth = 40;
        }

        protected override void LoadComplete()
        {
            Hitbox.BorderWidth = 0;
            //24 + 3 to offset for the player's bullet width being 3
            Hitbox.HitboxWidth = 27;
        }

        protected override void Update()
        {
            base.Update();

            HitDetect();
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            float amplitudeAdjust = Math.Min(1, 0.4f + amplitudes.Maximum);

            var beatLength = timingPoint.BeatLength;

            if (effectPoint.KiaiMode && CharacterSprite.Alpha == 1)
            {
                CharacterKiaiSprite.FadeInFromZero(timingPoint.BeatLength / 4);
                CharacterSprite.FadeOutFromOne(timingPoint.BeatLength / 4);
            }
            if (!effectPoint.KiaiMode && CharacterKiaiSprite.Alpha == 1)
            {
                CharacterSprite.FadeInFromZero(timingPoint.BeatLength);
                CharacterKiaiSprite.FadeOutFromOne(timingPoint.BeatLength);
            }
        }

        public override void Death()
        {
            Dead = true;
        }
    }
}

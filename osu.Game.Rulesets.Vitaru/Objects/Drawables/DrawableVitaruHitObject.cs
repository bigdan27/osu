using osu.Game.Rulesets.Objects.Drawables;
using System;
using System.ComponentModel;
using osu.Game.Rulesets.Vitaru.Judgements;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableVitaruHitObject : DrawableHitObject<VitaruHitObject, VitaruJudgement>
    {
        public const float TIME_PREEMPT = 800;
        public const float TIME_FADEIN = 600;

        //Does nothing atm
        public const float TIME_FADEOUT = 800;

        public DrawableVitaruHitObject(VitaruHitObject hitObject)
            : base(hitObject)
        {
        }

        protected override VitaruJudgement CreateJudgement() => new VitaruJudgement { MaxScore = VitaruScoreResult.Kill1500 };

        protected override void UpdateState(ArmedState state)
        {
            Flush();

            UpdateInitialState();

            Delay(HitObject.StartTime - Time.Current - TIME_PREEMPT + Judgement.TimeOffset, true);

            UpdatePreemptState();

            Delay(TIME_PREEMPT, true);
        }

        protected virtual void UpdatePreemptState()
        {
            FadeIn(TIME_FADEIN);
        }

        protected virtual void UpdateInitialState()
        {
            Alpha = 1;
        }
    }

    public enum ComboResult
    {
        [Description(@"")]
        None,
        [Description(@"Good")]
        Good,
        [Description(@"Amazing")]
        Perfect
    }

    public enum VitaruScoreResult
    {
        [Description(@"Hit")]
        Hit,
        [Description(@"2")]
        Graze2,
        [Description(@"10")]
        Kill10,
        [Description(@"20")]
        Kill20,
        [Description(@"30")]
        Kill30,
        [Description(@"1500")]
        Kill1500,
        [Description(@"")]
        Miss
    }
}

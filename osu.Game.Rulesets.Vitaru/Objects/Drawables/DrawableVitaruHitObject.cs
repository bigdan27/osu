using osu.Game.Rulesets.Objects.Drawables;
using System;
using System.ComponentModel;
using osu.Game.Rulesets.Vitaru.Judgements;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableVitaruHitObject : DrawableHitObject<VitaruHitObject, VitaruJudgement>
    {
        public float TIME_PREEMPT = 1600;
        public float TIME_FADEIN = 800;
        public float TIME_FADEOUT = 200;

        public DrawableVitaruHitObject(VitaruHitObject hitObject)
            : base(hitObject)
        {
        }

        protected override VitaruJudgement CreateJudgement() => new VitaruJudgement { MaxScore = VitaruScoreResult.Graze300 };

        protected override void UpdateState(ArmedState state)
        {

        }

        protected virtual void UpdatePreemptState()
        {

        }

        protected virtual void UpdateInitialState()
        {

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
        Miss,
        [Description(@"10")]
        Graze10,
        [Description(@"50")]
        Graze50,
        [Description(@"100")]
        Graze100,
        [Description(@"300")]
        Graze300,
    }
}

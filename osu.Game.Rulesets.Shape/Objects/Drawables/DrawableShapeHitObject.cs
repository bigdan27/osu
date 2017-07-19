using osu.Game.Rulesets.Objects.Drawables;
using System;
using System.ComponentModel;
using osu.Game.Rulesets.Shape.Judgements;

namespace osu.Game.Rulesets.Shape.Objects.Drawables
{
    public class DrawableShapeHitObject : DrawableHitObject<ShapeHitObject, ShapeJudgement>
    {
        public float TIME_PREEMPT = 800;
        public float TIME_FADEIN = 400;
        public float TIME_FADEOUT = 400;

        public DrawableShapeHitObject(ShapeHitObject hitObject)
            : base(hitObject)
        {
        }

        protected override ShapeJudgement CreateJudgement() => new ShapeJudgement { MaxScore = ShapeScoreResult.Hit300 };

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
        [Description(@"Amazing!")]
        Perfect
    }

    public enum ShapeScoreResult
    {
        [Description(@"Sad")]
        Miss,
        [Description(@"Safe")]
        Hit50,
        [Description(@"Fine")]
        Hit100,
        [Description(@"Cool")]
        Hit300,
    }
}

using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Shape.Judgements;
using OpenTK;
using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Shape.Objects.Drawables
{
    public class DrawableShapeJudgement : DrawableJudgement<ShapeJudgement>
    {
        public DrawableShapeJudgement(ShapeJudgement judgement) : base(judgement)
        {
        }

        protected override void LoadComplete()
        {
            if (Judgement.Result != HitResult.Miss)
                JudgementText.TransformSpacingTo(new Vector2(14, 0), 1800, Easing.OutQuint);

            base.LoadComplete();
        }
    }
}
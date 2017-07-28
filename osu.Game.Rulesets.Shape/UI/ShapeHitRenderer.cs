using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Shape.Judgements;
using osu.Game.Rulesets.Shape.Objects;
using osu.Game.Rulesets.Shape.Objects.Drawables;
using osu.Game.Rulesets.Shape.Beatmaps;
using osu.Game.Rulesets.Shape.UI;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Beatmaps;
using OpenTK;
using osu.Game.Rulesets.Shape.Scoring;

namespace osu.Game.Rulesets.Shape
{
    internal class ShapeHitRenderer : HitRenderer<ShapeHitObject, ShapeJudgement>
    {
        public ShapeHitRenderer(WorkingBeatmap beatmap, bool isForCurrentRuleset)
            : base(beatmap, isForCurrentRuleset)
        {
        }

        public override ScoreProcessor CreateScoreProcessor() => new ShapeScoreProcessor(this);

        protected override BeatmapConverter<ShapeHitObject> CreateBeatmapConverter() => new ShapeBeatmapConverter();

        protected override BeatmapProcessor<ShapeHitObject> CreateBeatmapProcessor() => new ShapeBeatmapProcessor();

        protected override Playfield<ShapeHitObject, ShapeJudgement> CreatePlayfield() => new ShapePlayfield();

        protected override DrawableHitObject<ShapeHitObject, ShapeJudgement> GetVisualRepresentation(ShapeHitObject h)
        {
            var shape = h as Objects.BaseShape;
            if (shape != null)
                return new DrawableBaseShape(shape);
            return null;
        }

        protected override Vector2 GetPlayfieldAspectAdjust() => new Vector2(0.75f);
    }
}

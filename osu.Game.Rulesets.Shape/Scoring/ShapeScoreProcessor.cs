using System.Collections.Generic;
using osu.Framework.Extensions;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Shape.Judgements;
using osu.Game.Rulesets.Shape.Objects;
using osu.Game.Rulesets.Shape.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Shape.Scoring
{
    internal class ShapeScoreProcessor : ScoreProcessor<ShapeHitObject, ShapeJudgement>
    {
        public ShapeScoreProcessor()
        {
        }

        public ShapeScoreProcessor(HitRenderer<ShapeHitObject, ShapeJudgement> hitRenderer)
            : base(hitRenderer)
        {
        }

        protected override void Reset()
        {
            base.Reset();

            TotalScore.Value = 0;

            Health.Value = 1;

            scoreResultCounts.Clear();
            comboResultCounts.Clear();
        }

        private readonly Dictionary<ShapeScoreResult, int> scoreResultCounts = new Dictionary<ShapeScoreResult, int>();
        private readonly Dictionary<ComboResult, int> comboResultCounts = new Dictionary<ComboResult, int>();

        public override void PopulateScore(Score score)
        {
            base.PopulateScore(score);

            score.Statistics[@"300"] = scoreResultCounts.GetOrDefault(ShapeScoreResult.Hit300);
            score.Statistics[@"100"] = scoreResultCounts.GetOrDefault(ShapeScoreResult.Hit100);
            score.Statistics[@"50"] = scoreResultCounts.GetOrDefault(ShapeScoreResult.Hit50);
            score.Statistics[@"x"] = scoreResultCounts.GetOrDefault(ShapeScoreResult.Miss);
        }

        protected override void OnNewJudgement(ShapeJudgement judgement)
        {
            if (judgement != null)
            {
                if (judgement.Result != HitResult.None)
                {
                    scoreResultCounts[judgement.Score] = scoreResultCounts.GetOrDefault(judgement.Score) + 1;
                    comboResultCounts[judgement.Combo] = comboResultCounts.GetOrDefault(judgement.Combo) + 1;
                }

                switch (judgement.Result)
                {
                    case HitResult.Hit:
                        Health.Value = Health.Value + 0.05f;
                        break;
                    case HitResult.Miss:
                        Health.Value = Health.Value - 0.1f;
                        break;
                }

                int score = 0;
                int maxScore = 0;

                foreach (var j in Judgements)
                {
                    score += j.ScoreValue;
                    maxScore += j.MaxScoreValue;
                }

                TotalScore.Value = score;
                Accuracy.Value = (double)score / maxScore;
            }
        }
    }
}
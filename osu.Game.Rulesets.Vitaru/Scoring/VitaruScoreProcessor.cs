using System.Collections.Generic;
using osu.Framework.Extensions;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Judgements;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;

namespace osu.Game.Rulesets.Vitaru.Scoring
{
    internal class VitaruScoreProcessor : ScoreProcessor<VitaruHitObject, VitaruJudgement>
    {
        public VitaruScoreProcessor()
        {
        }

        public VitaruScoreProcessor(HitRenderer<VitaruHitObject, VitaruJudgement> hitRenderer)
            : base(hitRenderer)
        {
        }

        protected override void Reset()
        {
            base.Reset();

            VitaruPlayer.PlayerEnergy = 0;
            VitaruPlayer.PlayerHealth = 100;

            Health.Value = VitaruPlayer.PlayerHealth / 100;
            Accuracy.Value = VitaruPlayer.PlayerEnergy / 100;

            TotalScore.Value = 0;

            scoreResultCounts.Clear();
            comboResultCounts.Clear();
        }

        private readonly Dictionary<VitaruScoreResult, int> scoreResultCounts = new Dictionary<VitaruScoreResult, int>();
        private readonly Dictionary<ComboResult, int> comboResultCounts = new Dictionary<ComboResult, int>();

        public override void PopulateScore(Score score)
        {
            base.PopulateScore(score);

            score.Statistics[@"300"] = scoreResultCounts.GetOrDefault(VitaruScoreResult.Graze300);
            //score.Statistics[@"200"] = scoreResultCounts.GetOrDefault(VitaruScoreResult.Graze200);
            score.Statistics[@"100"] = scoreResultCounts.GetOrDefault(VitaruScoreResult.Graze100);
            score.Statistics[@"50"] = scoreResultCounts.GetOrDefault(VitaruScoreResult.Graze50);
            score.Statistics[@"10"] = scoreResultCounts.GetOrDefault(VitaruScoreResult.Graze10);
            score.Statistics[@"x"] = scoreResultCounts.GetOrDefault(VitaruScoreResult.Miss);
        }

        protected override void OnNewJudgement(VitaruJudgement judgement)
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
                        Health.Value += 0.1f;
                        break;
                    case HitResult.Miss:
                        Health.Value -= 0.2f;
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
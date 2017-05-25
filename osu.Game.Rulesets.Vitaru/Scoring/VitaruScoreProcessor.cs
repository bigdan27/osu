// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

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

        public static float VitaruScore = 0;
        public static float PlayerEnergy = 0;
        public static float PlayerHealth = 1;

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

            Health.Value = 1;
            Accuracy.Value = 1;

            PlayerEnergy = 0;
            PlayerHealth = 1;
            VitaruScore = 0;

            Health.Value = PlayerHealth;
            TotalScore.Value = VitaruScore;
            Accuracy.Value = PlayerEnergy;

            scoreResultCounts.Clear();
            comboResultCounts.Clear();
        }

        private readonly Dictionary<VitaruScoreResult, int> scoreResultCounts = new Dictionary<VitaruScoreResult, int>();
        private readonly Dictionary<ComboResult, int> comboResultCounts = new Dictionary<ComboResult, int>();

        public override void PopulateScore(Score score)
        {
            base.PopulateScore(score);

            score.Statistics[@"30"] = scoreResultCounts.GetOrDefault(VitaruScoreResult.Kill30);
            score.Statistics[@"20"] = scoreResultCounts.GetOrDefault(VitaruScoreResult.Kill20);
            score.Statistics[@"10"] = scoreResultCounts.GetOrDefault(VitaruScoreResult.Kill10);
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
                        break;
                    case HitResult.Miss:
                        Health.Value -= 1f;
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
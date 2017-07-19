using System;
using OpenTK;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Shape.Objects.Drawables;
using osu.Framework.Extensions;

namespace osu.Game.Rulesets.Shape.Judgements
{
    public class ShapeJudgement : Judgement
    {
        /// <summary>
        /// The positional hit offset.
        /// </summary>
        public Vector2 PositionOffset;

        /// <summary>
        /// The score the user achieved.
        /// </summary>
        public ShapeScoreResult Score;

        public ShapeScoreResult MaxScore = ShapeScoreResult.Hit300;

        public override string ResultString => Score.GetDescription();

        public override string MaxResultString => MaxScore.GetDescription();

        public int ScoreValue => scoreToInt(Score);

        public int MaxScoreValue => scoreToInt(MaxScore);

        private int scoreToInt(ShapeScoreResult result)
        {
            switch (result)
            {
                default:
                    return 0;
                case ShapeScoreResult.Hit50:
                    return 50;
                case ShapeScoreResult.Hit100:
                    return 100;
                case ShapeScoreResult.Hit300:
                    return 300;
                case ShapeScoreResult.Miss:
                    return 0;
            }
        }

        public ComboResult Combo;
    }
}
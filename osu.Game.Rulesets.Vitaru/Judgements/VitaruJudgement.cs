using System;
using OpenTK;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Framework.Extensions;

namespace osu.Game.Rulesets.Vitaru.Judgements
{
    public class VitaruJudgement : Judgement
    {
        /// <summary>
        /// The positional hit offset.
        /// </summary>
        public Vector2 PositionOffset;

        /// <summary>
        /// The score the user achieved.
        /// </summary>
        public VitaruScoreResult Score;

        public VitaruScoreResult MaxScore = VitaruScoreResult.Graze300;

        public override string ResultString => Score.GetDescription();

        public override string MaxResultString => MaxScore.GetDescription();

        public int ScoreValue => scoreToInt(Score);

        public int MaxScoreValue => scoreToInt(MaxScore);

        private int scoreToInt(VitaruScoreResult result)
        {
            switch (result)
            {
                default:
                    return 0;
                case VitaruScoreResult.Graze10:
                    return 10;
                case VitaruScoreResult.Graze50:
                    return 50;
                case VitaruScoreResult.Graze100:
                    return 100;
                case VitaruScoreResult.Graze300:
                    return 300;
                case VitaruScoreResult.Miss:
                    return 0;
            }
        }

        public ComboResult Combo;
    }
}
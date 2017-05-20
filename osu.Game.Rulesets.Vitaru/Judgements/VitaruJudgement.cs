// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

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

        /// <summary>
        /// The score which would be achievable on a perfect hit.
        /// </summary>
        public VitaruScoreResult MaxScore = VitaruScoreResult.Kill1500;

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
                case VitaruScoreResult.Graze2:
                    return 2;
                case VitaruScoreResult.Kill10:
                    return 10;
                case VitaruScoreResult.Kill20:
                    return 20;
                case VitaruScoreResult.Kill30:
                    return 30;
                case VitaruScoreResult.Kill1500:
                    return 1500;
                case VitaruScoreResult.Hit:
                    return 0;
            }
        }

        public ComboResult Combo;
    }
}
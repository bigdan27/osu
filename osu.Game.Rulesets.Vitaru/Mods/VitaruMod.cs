// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Vitaru.Mods
{
    public class VitaruModNoFail : ModNoFail
    {

    }

    public class VitaruModEasy : ModEasy
    {

    }

    public class VitaruModHidden : ModHidden
    {
        public override string Description => @"Play with bullets dissapearing once they leave enemies immediate area.";
        public override double ScoreMultiplier => 1.18;
    }

    public class VitaruModHardRock : ModHardRock
    {
        public override double ScoreMultiplier => 1.08;
        public override bool Ranked => true;
    }

    public class VitaruModSuddenDeath : ModSuddenDeath
    {
        public override string Description => "Don't get hit.";
        public override bool Ranked => true;
    }

    public class VitaruModDaycore : ModDaycore
    {
        public override double ScoreMultiplier => 0.3;
    }

    public class VitaruModDoubleTime : ModDoubleTime
    {
        public override double ScoreMultiplier => 1.16;
    }

    public class VitaruModHalfTime : ModHalfTime
    {
        public override double ScoreMultiplier => 0.3;
    }

    public class VitaruModNightcore : ModNightcore
    {
        public override double ScoreMultiplier => 1.16;
    }

    public class VitaruModFlashlight : ModFlashlight
    {
        public override string Description => @"Play with bullets only appearing when they are close.";
        public override double ScoreMultiplier => 1.18;
    }

    public class VitaruRelax : ModRelax
    {
        public override bool Ranked => false;
    }

    public class VitaruModAutoplay : ModAutoplay<VitaruHitObject>
    {
        protected override Score CreateReplayScore(Beatmap<VitaruHitObject> beatmap) => new Score
        {
            Replay = new VitaruAutoReplay(beatmap)
        };
    }
}

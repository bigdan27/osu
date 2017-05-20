// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Vitaru.Mods
{
    public class VitaruModNoFail : ModNoFail
    {
        //Health = 666f;
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

    public class VitaruModDoubleTime : ModDoubleTime
    {
        public override double ScoreMultiplier => 1.36;
    }

    public class VitaruModHalfTime : ModHalfTime
    {
        public override double ScoreMultiplier => 0.5;
    }

    public class VitaruModNightcore : ModNightcore
    {
        public override double ScoreMultiplier => 1.36;
    }

    public class VitaruModFlashlight : ModFlashlight
    {
        public override string Description => @"Play with bullets only appearing when they are close.";
        public override double ScoreMultiplier => 1.18;
    }
    /*
    public class VitaruModDoubleTrouble : ModDoubleTrouble
    {
        public override double ScoreMultiplier => 1.18;
    }

    public class VitaruModCoop : ModCoop
    {
        public override double ScoreMultiplier => 0.5;
        public override bool Ranked => true;
    }

    public class VitaruMod1V1 : Mod1V1
    {
        public override double ScoreMultiplier => 0.5;
        public override bool Ranked => false;
    }*/

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

    public class OsuModTarget : Mod
    {
        public override string Name => "Target";
        public override FontAwesome Icon => FontAwesome.fa_osu_mod_target;
        public override string Description => @"";
        public override double ScoreMultiplier => 1;
    }
}

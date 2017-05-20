// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Vitaru.Objects;

namespace osu.Game.Rulesets.Vitaru.Mods
{
    internal class VitaruAutoReplay : Replay
    {
        private Beatmap<VitaruHitObject> beatmap;

        public VitaruAutoReplay(Beatmap<VitaruHitObject> beatmap)
        {
            this.beatmap = beatmap;
        }
    }
}
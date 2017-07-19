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
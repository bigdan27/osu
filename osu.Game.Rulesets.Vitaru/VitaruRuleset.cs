// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using osu.Game.Rulesets.UI;
using System;
using osu.Game.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Screens.Play;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Vitaru.Mods;
using OpenTK.Input;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Vitaru.Scoring;

namespace osu.Game.Rulesets.Vitaru
{
    public class VitaruRuleset : Ruleset
    {
        public override HitRenderer CreateHitRendererWith(WorkingBeatmap beatmap, bool isForCurrentRuleset) => new VitaruHitRenderer(beatmap, isForCurrentRuleset);

        public override string Description => "vitaru!";

        public override DifficultyCalculator CreateDifficultyCalculator(Beatmap beatmap) => new VitaruDifficultyCalculator(beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[]
                    {
                        new VitaruModEasy(),
                        new VitaruModNoFail(),
                        new VitaruModHalfTime(),
                    };

                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new VitaruModHardRock(),
                        new VitaruModSuddenDeath(),
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new VitaruModDoubleTime(),
                                new VitaruModNightcore(),
                            },
                        },
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new VitaruModHidden(),
                                new VitaruModFlashlight(),
                            },
                        },
                    };

                case ModType.Special:
                    return new Mod[]
                    {
                        new VitaruRelax(),
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new ModAutoplay(),
                                new ModCinema(),
                            },
                        },
                    };
                default : return new Mod[] { };
            }
        }

        public override ScoreProcessor CreateScoreProcessor() => new VitaruScoreProcessor();

        public override IEnumerable<KeyCounter> CreateGameplayKeys() => new KeyCounter[]
        {
            new KeyCounterKeyboard(Key.LShift),
            new KeyCounterKeyboard(Key.Z),
            new KeyCounterKeyboard(Key.X),
            new KeyCounterKeyboard(Key.Up),
            new KeyCounterKeyboard(Key.Right),
            new KeyCounterKeyboard(Key.Left),
            new KeyCounterKeyboard(Key.Down),
        };

        public override FontAwesome Icon => FontAwesome.fa_bathtub;
    }
}

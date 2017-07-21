using System.Collections.Generic;
using osu.Game.Rulesets.UI;
using osu.Game.Beatmaps;
using osu.Game.Screens.Play;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Vitaru.Mods;
using osu.Game.Rulesets.Vitaru.UI;
using OpenTK.Input;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Vitaru.Scoring;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Audio;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Vitaru
{
    public class VitaruRuleset : Ruleset
    {
        /// <summary>
        /// Setting this to true will load a taller playfield, spawn enemies rather than normal BasePatterns and most importantly enable shooting for the player to kill the enemies.
        /// </summary>
        public static Bindable<bool> TouhosuMode = new Bindable<bool>(false);

        public override HitRenderer CreateHitRendererWith(WorkingBeatmap beatmap, bool isForCurrentRuleset) => new VitaruHitRenderer(beatmap, isForCurrentRuleset);

        public override int LegacyID => 0;

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
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new VitaruModHalfTime(),
                                new VitaruModDaycore(),
                            },
                        },
                    };

                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new VitaruModHardRock(),
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new VitaruModSuddenDeath(),
                                new VitaruModPerfect(),
                            },
                        },
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
            new KeyCounterKeyboard(Key.Up),
            new KeyCounterKeyboard(Key.Right),
            new KeyCounterKeyboard(Key.Left),
            new KeyCounterKeyboard(Key.Down),
        };

        public override SettingsSubsection CreateSettings() => new VitaruSettings();

        //public override FontAwesome Icon => VitaruFontAwesome.fa_osu_vitaru_o;

        public static ResourceStore<byte[]> VitaruResources;
        public static TextureStore VitaruTextures;
        public static FontStore VitaruFont;
        public static AudioManager VitaruAudio;
        public static bool AssetsLoaded = false;

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config, TextureStore textures)
        {
            if (!AssetsLoaded)
            {
                AssetsLoaded = true;
                VitaruResources = new ResourceStore<byte[]>();
                VitaruResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Game.Rulesets.Vitaru.dll"), ("Assets")));
                VitaruResources.AddStore(new DllResourceStore("osu.Game.Rulesets.Vitaru.dll"));

                VitaruTextures = new TextureStore(new RawTextureLoaderStore(new NamespacedResourceStore<byte[]>(VitaruResources, @"Textures")));
                VitaruTextures.AddStore(new RawTextureLoaderStore(new OnlineStore()));

                VitaruFont = new FontStore(new GlyphStore(VitaruResources, @"Font/vitaruFont"))
                {
                    ScaleAdjust = 100
                };
            }
        }
    }

    public enum VitaruFontAwesome
    {
        fa_osu_vitaru_o = 0xe04b,
    }
}

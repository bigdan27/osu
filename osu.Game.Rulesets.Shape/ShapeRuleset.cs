using System.Collections.Generic;
using osu.Game.Rulesets.UI;
using osu.Game.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Screens.Play;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Shape.Mods;
using OpenTK.Input;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Shape.Scoring;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Audio;

namespace osu.Game.Rulesets.Shape
{
    public class ShapeRuleset : Ruleset
    {
        public override HitRenderer CreateHitRendererWith(WorkingBeatmap beatmap, bool isForCurrentRuleset) => new ShapeHitRenderer(beatmap, isForCurrentRuleset);

        public override string Description => "shape!";

        public override DifficultyCalculator CreateDifficultyCalculator(Beatmap beatmap) => new ShapeDifficultyCalculator(beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[]
                    {
                        new ShapeModEasy(),
                        new ShapeModNoFail(),
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new ShapeModHalfTime(),
                                new ShapeModDaycore(),
                            },
                        },
                    };

                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new ShapeModHardRock(),
                        new ShapeModSuddenDeath(),
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new ShapeModDoubleTime(),
                                new ShapeModNightcore(),
                            },
                        },
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new ShapeModHidden(),
                                new ShapeModFlashlight(),
                            },
                        },
                    };

                case ModType.Special:
                    return new Mod[]
                    {
                        new ShapeRelax(),
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

        public override ScoreProcessor CreateScoreProcessor() => new ShapeScoreProcessor();

        public override IEnumerable<KeyCounter> CreateGameplayKeys() => new KeyCounter[]
        {
            new KeyCounterKeyboard(Key.A),
            new KeyCounterKeyboard(Key.S),
            new KeyCounterKeyboard(Key.D),
            new KeyCounterKeyboard(Key.F),
            new KeyCounterKeyboard(Key.J),
            new KeyCounterKeyboard(Key.K),
            new KeyCounterKeyboard(Key.L),
            new KeyCounterKeyboard(Key.Semicolon),
        };

        //public override FontAwesome Icon => SymcolFontAwesome.sf_shape_o;

        public static ResourceStore<byte[]> SymcolResources;
        public static TextureStore SymcolTextures;
        public static FontStore SymcolFont;
        public static AudioManager SymcolAudio;
        public static bool AssetsLoaded = false;

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config, TextureStore textures)
        {
            if (!AssetsLoaded)
            {
                AssetsLoaded = true;
                SymcolResources = new ResourceStore<byte[]>();
                SymcolResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Game.Rulesets.Shape.dll"), ("Assets")));
                SymcolResources.AddStore(new DllResourceStore("osu.Game.Rulesets.Shape.dll"));

                SymcolTextures = new TextureStore(new RawTextureLoaderStore(new NamespacedResourceStore<byte[]>(SymcolResources, @"Textures")));
                SymcolTextures.AddStore(new RawTextureLoaderStore(new OnlineStore()));

                SymcolFont = new FontStore(new GlyphStore(SymcolResources, @"Font/symcolFont"))
                {
                    ScaleAdjust = 100
                };
            }
        }
    }

    public enum SymcolFontAwesome
    {
        sf_shape_o = 0xe003,
    }
}

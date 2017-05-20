// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Rulesets.Objects;
using OpenTK;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Objects;
using OpenTK.Graphics;
using osu.Game.Database;
using osu.Game.Beatmaps.Timing;

namespace osu.Game.Rulesets.Vitaru.Objects
{
    public abstract class VitaruHitObject : HitObject
    {
        public const double HitboxSize = 8;

        public float BPM;

        public bool Kiai { get; protected set; }

        //I have no idea where enemies are getting pos info from if its not from here
        public Vector2 Position { get; set; }

        public Vector2 StackedPosition => Position + StackOffset;

        public virtual Vector2 EndPosition => Position;

        public Vector2 StackedEndPosition => EndPosition + StackOffset;

        public virtual int StackHeight { get; set; }

        public Vector2 StackOffset => new Vector2(0,0);

        public double Radius => HitboxSize * Scale;

        public float Scale { get; set; } = 1;

        public abstract HitObjectType Type { get; }

        public Color4 ComboColour { get; set; }
        public virtual bool NewCombo { get; set; }
        public int ComboIndex { get; set; }

        public double HitWindowFor(VitaruScoreResult result)
        {
            switch (result)
            {
                default:
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
            }
        }

        public override void ApplyDefaults(TimingInfo timing, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaults(timing, difficulty);

            Scale = (1.0f - 0.7f * (difficulty.CircleSize - 5) / 5) / 2;
        }
    }
}

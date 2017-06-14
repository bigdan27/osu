using System;
using osu.Game.Rulesets.Vitaru.Objects;
using OpenTK;
using OpenTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Beatmaps
{
    public class Pattern : VitaruHitObject
    {
        public override HitObjectType Type => HitObjectType.Pattern;

        public int PatternID { get; set; }
        public Vector2 PatternPosition { get; set; }
        public Color4 PatternColor { get; set; } = Color4.Green;
        public float PatternSpeed { get; set; }
        public float PatternDifficulty { get; set; } = 1;
        public float PatternAngleRadian { get; set; } = -10;
        public float PatternAngleDegree { get; set; } = 0;
        public float PatternBulletWidth { get; set; } = 4;
        public float PatternDamage { get; set; } = 10;
        public float PatternRepeatTimes { get; set; } = 1f;
        public double PatternDuration { get; set; } = 0;
        public double PatternRepeatDelay { get; set; } = 0;
        public bool DynamicPatternVelocity { get; set; } = false;
        public int PatternTeam { get; set; }
        public Pattern() : base () { }
    }
}
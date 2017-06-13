using System;
using osu.Game.Rulesets.Vitaru.Objects;
using OpenTK;

namespace osu.Game.Rulesets.Vitaru.Beatmaps
{
    public class BulletPattern : VitaruHitObject
    {
        public override HitObjectType Type => HitObjectType.Pattern;

        public Vector2 PatternPosition { get; set; }
        public float PatternSpeed { get; set; }
        public float PatternDifficulty { get; set; } = 1;
        public float PatternAngleRadian { get; set; } = -10;
        public float PatternAngleDegree { get; set; } = 0;
        public float PatternBulletWidth { get; set; } = 2;
        public float PatternDamage { get; set; } = 10;
        public float PatternRepeatTimes { get; set; } = 1f;
        public double PatternDuration { get; set; } = 0;
        public double PatternRepeatDelay { get; set; } = 0;
        public bool DynamicPatternVelocity { get; set; } = false;
        public int PatternTeam { get; set; }
        public BulletPattern() : base () { }
    }
}
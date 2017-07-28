using System;
using osu.Game.Rulesets.Vitaru.Objects;
using OpenTK;
using OpenTK.Graphics;
using osu.Game.Audio;
using System.Collections.Generic;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Database;
using osu.Game.Rulesets.Objects;
using osu.Game.Beatmaps;

namespace osu.Game.Rulesets.Vitaru.Objects.Projectiles
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
        public double PatternDuration => EndTime - StartTime;
        public double PatternRepeatDelay { get; set; } = 0;
        public bool DynamicPatternVelocity { get; set; } = false;
        public int PatternTeam { get; set; }
        public Pattern() : base () { }
        public bool IsSlider { get; set; } = false;
        public bool IsSpinner { get; set; } = false;
        public List<SampleInfoList> RepeatSamples { get; set; } = new List<SampleInfoList>();
        private const float base_scoring_distance = 100;
        public readonly SliderCurve Curve = new SliderCurve();
        public double EndTime;
        public double Duration => EndTime - StartTime;
        public int RepeatCount { get; set; } = 1;
        public double Velocity;
        public double TickDistance;

        public override Vector2 EndPosition => PositionAt(1);
        public Vector2 PositionAt(double progress) => Curve.PositionAt(ProgressAt(progress));

        public double ProgressAt(double progress)
        {
            double p = progress * RepeatCount % 1;
            if (RepeatAt(progress) % 2 == 1)
                p = 1 - p;
            return p;
        }

        public int RepeatAt(double progress) => (int)(progress * RepeatCount);

        public List<Vector2> ControlPoints
        {
            get { return Curve.ControlPoints; }
            set { Curve.ControlPoints = value; }
        }

        public CurveType CurveType
        {
            get { return Curve.CurveType; }
            set { Curve.CurveType = value; }
        }

        public double Distance
        {
            get { return Curve.Distance; }
            set { Curve.Distance = value; }
        }

        public override void ApplyDefaults(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaults(controlPointInfo, difficulty);

            TimingControlPoint timingPoint = controlPointInfo.TimingPointAt(StartTime);
            DifficultyControlPoint difficultyPoint = controlPointInfo.DifficultyPointAt(StartTime);

            double scoringDistance = base_scoring_distance * difficulty.SliderMultiplier / difficultyPoint.SpeedMultiplier;

            Velocity = scoringDistance / timingPoint.BeatLength;
            TickDistance = scoringDistance / difficulty.SliderTickRate;
        }
    }
}
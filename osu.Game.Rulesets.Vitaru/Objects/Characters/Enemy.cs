using OpenTK;
using osu.Framework.Audio.Sample;
using osu.Game.Beatmaps.Timing;
using osu.Game.Database;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;
using System;
using System.Collections.Generic;
using osu.Game.Audio;
using osu.Game.Beatmaps.ControlPoints;

namespace osu.Game.Rulesets.Vitaru.Objects.Characters
{
    public class Enemy : Character
    {
        public bool Shoot = false;
        public Vector2 EnemyPosition;
        public Vector2 EnemySpeed { get; set; } = new Vector2(0.5f, 0.5f);
        public double HitWindowMiss = 1000;
        
        public Vector2 EnemyVelocity;
        public float EnemyAngle;
        public Action OnShoot;

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

        private int stackHeight = 0;
        public override int StackHeight
        {
            get { return stackHeight; }
            set
            {
                stackHeight = value;
                Curve.Offset = StackOffset;
            }
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

        //Main Enemy Function
        public Enemy() : base () { }
        public override HitObjectType Type => HitObjectType.Enemy;
        public bool IsSlider { get; set; } = false;
        public bool IsSpinner { get; set; } = false;
        public List<SampleInfoList> RepeatSamples { get; set; } = new List<SampleInfoList>();
    }
}
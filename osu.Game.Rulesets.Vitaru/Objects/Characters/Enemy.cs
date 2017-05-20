// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK;
using osu.Framework.Audio.Sample;
using osu.Game.Beatmaps.Timing;
using osu.Game.Database;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Vitaru.Objects.Characters
{
    public class Enemy : Character
    {
        public bool Shoot = false;
        public Vector2 EnemyPosition;
        public Vector2 EnemySpeed { get; set; } = new Vector2(0.5f, 0.5f);
        public BulletPattern Pattern { get; set; }
        public double kill30 = 30;
        public double HitWindowMiss = 1000;
        
        public Vector2 EnemyVelocity;
        public float EnemyAngle;
        public Action OnShoot;

        private const float base_scoring_distance = 100;
        public readonly SliderCurve Curve = new SliderCurve();
        public double EndTime => StartTime + RepeatCount * Curve.Distance / Velocity;
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

        public override void ApplyDefaults(TimingInfo timing, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaults(timing, difficulty);

            double scoringDistance = base_scoring_distance * difficulty.SliderMultiplier / timing.SpeedMultiplierAt(StartTime);

            Velocity = scoringDistance / timing.BeatLengthAt(StartTime);
            TickDistance = scoringDistance / difficulty.SliderTickRate;
        }

        public static Vector2 EnemyPos;

        //Main Enemy Function
        public Enemy() : base () { }
        public override HitObjectType Type => HitObjectType.Enemy;
        public bool IsSlider { get; set; } = false;
        public bool IsSpinner { get; set; } = false;
    }
}
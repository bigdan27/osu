﻿using OpenTK;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Vitaru.Objects;
using System.Collections.Generic;
using osu.Game.Rulesets.Objects.Types;
using System;
using osu.Game.Rulesets.Beatmaps;
using osu.Game.Audio;
using System.Linq;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;

namespace osu.Game.Rulesets.Vitaru.Beatmaps
{
    internal class VitaruBeatmapConverter : BeatmapConverter<VitaruHitObject>
    {
        protected override IEnumerable<Type> ValidConversionTypes { get; } = new[] { typeof(IHasPosition) };

        protected override IEnumerable<VitaruHitObject> ConvertHitObject(HitObject original, Beatmap beatmap)
        {
            var curveData = original as IHasCurve;
            var endTimeData = original as IHasEndTime;
            var positionData = original as IHasPosition;
            var comboData = original as IHasCombo;

            SampleInfoList samples = original.Samples;

            bool isLine = samples.Any(s => s.Name == SampleInfo.HIT_WHISTLE);
            bool isTriangleWave = samples.Any(s => s.Name == SampleInfo.HIT_FINISH);
            bool isCoolWave = samples.Any(s => s.Name == SampleInfo.HIT_CLAP);
            if (curveData != null)
            {
                if (isLine)
                {
                    yield return new Pattern
                    {
                        StartTime = original.StartTime,
                        PatternPosition = positionData?.Position ?? Vector2.Zero,
                        Samples = original.Samples,
                        PatternID = 2,
                        PatternAngleDegree = 180,
                        PatternSpeed = 0.25f,
                        PatternBulletWidth = 8f,
                        PatternTeam = 1,
                        ControlPoints = curveData.ControlPoints,
                        CurveType = curveData.CurveType,
                        Distance = curveData.Distance,
                        RepeatSamples = curveData.RepeatSamples,
                        RepeatCount = curveData.RepeatCount,
                        NewCombo = comboData?.NewCombo ?? false,
                        IsSlider = true,
                    };
                }
                else if (isTriangleWave)
                {
                    yield return new Pattern
                    {
                        StartTime = original.StartTime,
                        PatternPosition = positionData?.Position ?? Vector2.Zero,
                        Samples = original.Samples,
                        PatternID = 3,
                        PatternAngleDegree = 180,
                        PatternSpeed = 0.25f,
                        PatternBulletWidth = 8f,
                        PatternTeam = 1,
                        ControlPoints = curveData.ControlPoints,
                        CurveType = curveData.CurveType,
                        Distance = curveData.Distance,
                        RepeatSamples = curveData.RepeatSamples,
                        RepeatCount = curveData.RepeatCount,
                        NewCombo = comboData?.NewCombo ?? false,
                        IsSlider = true,
                    };
                }
                else if (isCoolWave)
                {
                    yield return new Pattern
                    {
                        StartTime = original.StartTime,
                        PatternPosition = positionData?.Position ?? Vector2.Zero,
                        Samples = original.Samples,
                        PatternID = 4,
                        PatternAngleDegree = 180,
                        PatternSpeed = 0.25f,
                        PatternBulletWidth = 8f,
                        PatternTeam = 1,
                        ControlPoints = curveData.ControlPoints,
                        CurveType = curveData.CurveType,
                        Distance = curveData.Distance,
                        RepeatSamples = curveData.RepeatSamples,
                        RepeatCount = curveData.RepeatCount,
                        NewCombo = comboData?.NewCombo ?? false,
                        IsSlider = true,
                    };
                }
                else
                {
                    yield return new Pattern
                    {
                        StartTime = original.StartTime,
                        PatternPosition = positionData?.Position ?? Vector2.Zero,
                        Samples = original.Samples,
                        PatternID = 1,
                        PatternAngleDegree = 180,
                        PatternSpeed = 0.25f,
                        PatternBulletWidth = 8f,
                        PatternTeam = 1,
                        ControlPoints = curveData.ControlPoints,
                        CurveType = curveData.CurveType,
                        Distance = curveData.Distance,
                        RepeatSamples = curveData.RepeatSamples,
                        RepeatCount = curveData.RepeatCount,
                        NewCombo = comboData?.NewCombo ?? false,
                        IsSlider = true,
                    };
                }
            }
            else if (endTimeData != null)
            {
                yield return new Pattern
                {
                    StartTime = original.StartTime,
                    PatternPosition = positionData?.Position ?? Vector2.Zero,
                    Samples = original.Samples,
                    IsSpinner = true,
                    PatternSpeed = 0.25f,
                    PatternBulletWidth = 8f,
                    PatternTeam = 1,
                    PatternID = 5,
                    EndTime = endTimeData.EndTime,
                    PatternDifficulty = 4,
                };
            }
            else
            {
                if (isLine)
                {
                    yield return new Pattern
                    {
                        StartTime = original.StartTime,
                        PatternPosition = positionData?.Position ?? Vector2.Zero,
                        Samples = original.Samples,
                        PatternID = 2,
                        PatternAngleDegree = 180,
                        PatternSpeed = 0.25f,
                        PatternBulletWidth = 8f,
                        PatternTeam = 1,
                        NewCombo = comboData?.NewCombo ?? false
                    };
                }
                else if (isTriangleWave)
                {
                    yield return new Pattern
                    {
                        StartTime = original.StartTime,
                        PatternPosition = positionData?.Position ?? Vector2.Zero,
                        Samples = original.Samples,
                        PatternID = 3,
                        PatternAngleDegree = 180,
                        PatternSpeed = 0.25f,
                        PatternBulletWidth = 8f,
                        PatternTeam = 1,
                        NewCombo = comboData?.NewCombo ?? false
                    };
                }
                else if (isCoolWave)
                {
                    yield return new Pattern
                    {
                        StartTime = original.StartTime,
                        PatternPosition = positionData?.Position ?? Vector2.Zero,
                        Samples = original.Samples,
                        PatternID = 4,
                        PatternAngleDegree = 180,
                        PatternSpeed = 0.25f,
                        PatternBulletWidth = 8f,
                        PatternTeam = 1,
                        NewCombo = comboData?.NewCombo ?? false
                    };
                }
                else
                {
                    yield return new Pattern
                    {
                        StartTime = original.StartTime,
                        PatternPosition = positionData?.Position ?? Vector2.Zero,
                        Samples = original.Samples,
                        PatternID = 1,
                        PatternAngleDegree = 180,
                        PatternSpeed = 0.25f,
                        PatternBulletWidth = 8f,
                        PatternTeam = 1,
                        NewCombo = comboData?.NewCombo ?? false
                    };
                }
            }
        }
    }
}
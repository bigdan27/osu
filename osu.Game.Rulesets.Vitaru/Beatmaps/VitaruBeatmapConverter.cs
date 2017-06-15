using OpenTK;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Vitaru.Objects;
using System.Collections.Generic;
using osu.Game.Rulesets.Objects.Types;
using System;
using osu.Game.Rulesets.Beatmaps;
using osu.Game.Audio;
using System.Linq;

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
                };
            }
            if (isTriangleWave)
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
                };
            }
            if (isCoolWave)
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
                };
            }
        }
    }
}

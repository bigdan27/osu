using OpenTK;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Vitaru.Objects;
using System.Collections.Generic;
using osu.Game.Rulesets.Objects.Types;
using System;
using osu.Game.Rulesets.Beatmaps;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;

namespace osu.Game.Rulesets.Vitaru.Beatmaps
{
    internal class VitaruBeatmapConverter : BeatmapConverter<VitaruHitObject>
    {
        private bool initialLoad = false;

        protected override IEnumerable<Type> ValidConversionTypes { get; } = new[] { typeof(IHasPosition) };

        protected override IEnumerable<VitaruHitObject> ConvertHitObject(HitObject original, Beatmap beatmap)
        {
            var curveData = original as IHasCurve;
            var endTimeData = original as IHasEndTime;
            var positionData = original as IHasPosition;
            var comboData = original as IHasCombo;
            
            if (initialLoad == false)
            {                
                VitaruPlayer.PlayerPosition = new Vector2(256, 700);
                initialLoad = true;
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

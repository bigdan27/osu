using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Vitaru.Judgements;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Objects.Characters;
using osu.Game.Rulesets.Vitaru.Beatmaps;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Beatmaps;
using OpenTK;
using osu.Game.Rulesets.Vitaru.Scoring;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;

namespace osu.Game.Rulesets.Vitaru
{
    internal class VitaruHitRenderer : HitRenderer<VitaruHitObject, VitaruJudgement>
    {
        public VitaruHitRenderer(WorkingBeatmap beatmap, bool isForCurrentRuleset)
            : base(beatmap, isForCurrentRuleset)
        {
        }

        public override ScoreProcessor CreateScoreProcessor() => new VitaruScoreProcessor(this);

        protected override BeatmapConverter<VitaruHitObject> CreateBeatmapConverter() => new VitaruBeatmapConverter();

        protected override BeatmapProcessor<VitaruHitObject> CreateBeatmapProcessor() => new VitaruBeatmapProcessor();

        protected override Playfield<VitaruHitObject, VitaruJudgement> CreatePlayfield() => new VitaruPlayfield();

        protected override DrawableHitObject<VitaruHitObject, VitaruJudgement> GetVisualRepresentation(VitaruHitObject h)
        {
            var player = h as VitaruPlayer;
            if (player != null)
                return new DrawableVitaruPlayer(player);
            
            var enemy = h as Enemy;
            if (enemy != null)
                return new DrawableVitaruEnemy(enemy);

            var boss = h as Boss;
            if (boss != null)
                return new DrawableVitaruBoss(boss);

            var bullet = h as Bullet;
            if (bullet != null)
                return new DrawableBullet(bullet);
            /*
            var laser = h as Laser;
            if (laser != null)
                return new DrawableLaser(laser);*/
            return null;
        }

        protected override Vector2 GetPlayfieldAspectAdjust() => new Vector2(0.75f);
    }
}

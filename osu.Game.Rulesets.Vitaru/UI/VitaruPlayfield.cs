using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.UI;
using OpenTK;
using osu.Game.Rulesets.Vitaru.Judgements;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class VitaruPlayfield : Playfield<VitaruHitObject, VitaruJudgement>
    {
        public static Container vitaruPlayfield;
        private readonly Container judgementLayer;

        public override bool ProvidingUserCursor => true;

        public static Vector2 BASE_SIZE = new Vector2(512, 384);
        private VitaruUI vitaruUI;

        public override Vector2 Size
        {
            get
            {
                var parentSize = Parent.DrawSize;
                var aspectSize = parentSize.X * 0.75f < parentSize.Y ? new Vector2(parentSize.X, parentSize.X * 0.75f) : new Vector2(parentSize.Y * 4f / 3f, parentSize.Y);

                if (VitaruRuleset.TouhosuMode)
                {
                    aspectSize = parentSize.X * 0.75f < parentSize.Y ? new Vector2(parentSize.X, parentSize.X * 0.75f) : new Vector2(parentSize.Y * 5f / 8f, parentSize.Y);
                    BASE_SIZE = new Vector2(512, 820);
                }

                return new Vector2(aspectSize.X / parentSize.X, aspectSize.Y / parentSize.Y) * base.Size;
            }
        }

        public VitaruPlayfield() : base(BASE_SIZE.X)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            AddRange(new Drawable[]
            {
                vitaruUI = new VitaruUI
                {
                    Depth = 1,
                    Masking = false,
                    Position = new Vector2(-10),
                    RelativeSizeAxes = Axes.Both,
                    Origin = Anchor.TopLeft,
                    Anchor = Anchor.TopLeft,
                },
                vitaruPlayfield = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Depth = -10,
                },
                judgementLayer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Depth = 3,
                },
            });

            VitaruPlayer vitaruPlayer;
            vitaruPlayfield.Add(vitaruPlayer = new VitaruPlayer
            {
                Alpha = 1,
                AlwaysPresent = true,
            });
            VitaruPlayer.PlayerPosition = BASE_SIZE / 2;
            if (VitaruRuleset.TouhosuMode)
                VitaruPlayer.PlayerPosition = new Vector2(256, 700);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        public override void Add(DrawableHitObject<VitaruHitObject, VitaruJudgement> h)
        {
            h.Depth = (float)h.HitObject.StartTime;

            IDrawableHitObjectWithProxiedApproach c = h as IDrawableHitObjectWithProxiedApproach;
            if (c != null)
                vitaruPlayfield.Add(c.ProxiedLayer.CreateProxy());

            base.Add(h);
        }

        public override void OnJudgement(DrawableHitObject<VitaruHitObject, VitaruJudgement> judgedObject)
        {
            DrawableVitaruJudgement explosion = new DrawableVitaruJudgement(judgedObject.Judgement)
            {
                Alpha = 0.5f,
                Origin = Anchor.Centre,
                Position = new Vector2(VitaruPlayer.PlayerPosition.X , VitaruPlayer.PlayerPosition.Y + 50)
            };

            judgementLayer.Add(explosion);
        }
    }
}

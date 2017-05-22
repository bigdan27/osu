using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class Helper : Container
    {
        private Container helperRing;
        private Box helperBody;
        private CircularContainer helperContainer;

        public float HelperWidth { get; set; } = 4;
        public int HelperTeam { get; set; }
        public Color4 HelperColor { get; set; } = Color4.Red;

        public Helper()
        {

        }

        public void shoot()
        {

        }

        protected override void LoadComplete()
        {
            Children = new Drawable[]
            {
                helperRing = new Container
                {
                    Masking = true,
                    AutoSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    BorderThickness = 2,
                    Depth = 1,
                    BorderColour = HelperColor,
                    Alpha = 1f,
                    CornerRadius = HelperWidth,
                    Children = new[]
                    {
                        helperBody = new Box
                        {
                            Colour = Color4.White,
                            Alpha = 1,
                            Width = HelperWidth * 2,
                            Height = HelperWidth * 2,
                        },
                    },
                },
                helperContainer = new CircularContainer
                {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(HelperWidth * 2),
                        Depth = 2,
                        Masking = true,
                }
            };
        }
    }
}

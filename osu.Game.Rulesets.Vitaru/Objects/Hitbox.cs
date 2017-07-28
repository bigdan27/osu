using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using OpenTK.Graphics;
using OpenTK;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shapes;

namespace osu.Game.Rulesets.Vitaru.Objects
{
    public class Hitbox : Container
    {
        //Different stats for Hitboxes
        public Color4 HitboxColor { get; set; } = Color4.White;
        public float HitboxWidth { get; set; } = 8f;
        public float BorderWidth { get; set; } = 3f;

        private Container hitboxContainer;
        
        public Hitbox()
        {
        }

        protected override void LoadComplete()
        {
            Children = new Drawable[]
            {
                new Container
                {
                    Masking = true,
                    AutoSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    BorderThickness = BorderWidth,
                    Depth = 1,
                    BorderColour = HitboxColor,
                    Alpha = 1f,
                    CornerRadius = HitboxWidth,
                    Children = new[]
                    {
                        new Box
                        {
                            Colour = Color4.White,
                            Alpha = 1,
                            Width = HitboxWidth * 2,
                            Height = HitboxWidth * 2,
                        },
                    },
                },
                hitboxContainer = new CircularContainer
                {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(HitboxWidth * 2),
                        Depth = 2,
                        Masking = true,
                        EdgeEffect = new EdgeEffectParameters
                        {
                            Type = EdgeEffectType.Shadow,
                            Colour = (HitboxColor).Opacity(0.3f),
                            Radius = 2f,
                        }
                }
            };
        }
    }
}

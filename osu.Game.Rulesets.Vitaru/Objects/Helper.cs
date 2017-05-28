using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class Helper : Container
    {
        /// <summary>
        /// The "Helper" is basically a character that only shoots "SeekingBullet"s for the player (atm, however it will work for any character in the future)
        /// </summary>

        private Container helperRing;
        private Box helperBody;
        private int Team;
        private CircularContainer helperContainer;

        public float HelperWidth { get; set; } = 5;
        public int HelperTeam { get; set; }
        public Color4 HelperColor { get; set; }
        public float StartAngle { get; set; } = 0;

        public Helper(int team)
        {
            Team = team;
        }

        public void shoot()
        {
            SeekingBullet s;
            VitaruPlayfield.vitaruPlayfield.Add(s = new SeekingBullet(Team)
            {
                Origin = Anchor.Centre,
                Depth = 5,
                BulletSpeed = 0.8f,
                BulletColor = HelperColor,
                StartAngle = StartAngle,
                BulletDamage = 5,
            });
            s.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), s));
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
                    BorderThickness = HelperWidth / 2,
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

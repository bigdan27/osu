using osu.Framework.Allocation;
using osu.Framework.Graphics;
using OpenTK;
using OpenTK.Graphics;
using osu.Game.Screens.Symcol.Pieces;
using osu.Framework.Audio.Sample;
using osu.Framework.Audio;
using osu.Game.Overlays.Settings;
using osu.Framework.Configuration;
using osu.Framework.Timing;
using osu.Framework.Audio.Track;
using osu.Game.Beatmaps.IO;
using osu.Game.Database;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Graphics.Containers;
using osu.Game.Beatmaps.ControlPoints;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Screens.Symcol.Screens
{
    class SymcolTestScreen : OsuScreen
    {
        private BaseDial dial;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            Children = new Drawable[]
            {
                dial = new BaseDial
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                },
                new SymcolButton
                {
                    ButtonName = "Spin",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkGray,
                    ButtonColorBottom = Color4.Gray,
                    ButtonSize = 50,
                    Action = () => dial.StartSpinning(1000),
                    Position = new Vector2(0 , 200),
                },
                new SymcolButton
                {
                    ButtonName = "Reset",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkGray,
                    ButtonColorBottom = Color4.Gray,
                    ButtonSize = 50,
                    Action = () => dial.Reset(),
                    Position = new Vector2(0 , -200),
                }
            };
        }

        protected override void Update()
        {
            base.Update();
        }
    }

    internal class BaseDial : BeatSyncedContainer
    {
        public float DialWidth { get; set; } = 40;

        private Container ring;
        private Container arrow;

        public BaseDial()
        {

        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Children = new Drawable[]
            {
                ring = new Container
                {
                    Size = new Vector2(DialWidth),
                    Masking = true,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    BorderThickness = DialWidth / 6,
                    Depth = 1,
                    BorderColour = Color4.Gray,
                    CornerRadius = DialWidth / 2,
                    Children = new[]
                    {
                        new Box
                        {
                            AlwaysPresent = true,
                            Alpha = 0,
                            RelativeSizeAxes = Axes.Both
                        },
                    },
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Hollow = true,
                        Type = EdgeEffectType.Shadow,
                        Colour = Color4.White.Opacity(0.25f),
                        Radius = DialWidth / 4,
                    }
                },
                arrow = new Container
                {
                    Depth = 0,
                    Origin = Anchor.BottomCentre,
                    Anchor = Anchor.Centre,
                    Size = new Vector2(DialWidth / 6 , DialWidth * 0.6f),
                    Colour = Color4.DarkGray,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Alpha = 1,
                            RelativeSizeAxes = Axes.Both
                        },
                        new Triangle
                        {
                            Colour = Color4.DarkGray,
                            Size = new Vector2(DialWidth / 4),
                            Origin = Anchor.BottomCentre,
                            Anchor = Anchor.TopCentre,
                        }
                    }
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {

        }

        public void StartSpinning(float time)
        {
            arrow.RotateTo(360, time);
        }

        public void Reset()
        {
            arrow.RotateTo(0, 0);
        }
    }
}

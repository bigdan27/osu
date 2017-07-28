using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Graphics.Containers;
using osu.Game.Screens.Backgrounds;
using osu.Framework.Audio.Track;
using osu.Game.Overlays.Music;
using osu.Framework.Audio;
using osu.Game.Database;
using osu.Framework.Threading;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Graphics;
using osu.Game.Screens.Menu;
using osu.Game.Screens.Symcol.Pieces;
using osu.Game.Screens.Pokeosu;
using osu.Game.Screens.Symcol.Screens;
using osu.Game.Screens.Multiplayer;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Select;
using SQLite.Net;

namespace osu.Game.Screens.Symcol
{
    public class SymcolMenu : OsuScreen
    {
        internal override bool ShowOverlays => true;

        private OsuLogo Logo;

        private BeatmapStore database;
        private TrackManager trackManager;
        private static readonly Vector2 background_blur = new Vector2(10);
        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBeatmap(Beatmap);

        protected override void Update()
        {
            base.Update();

            changeBackground(Beatmap);
        }

        [BackgroundDependencyLoader]
        private void load(BeatmapManager beatmaps, AudioManager audio)
        {

            if (manager == null)
                manager = beatmaps;

            trackManager = audio.Track;
            if (database == null)
                database = BeatmapManager.BeatmapStore;
            preloadSongSelect();

            Children = new Drawable[]
            {
                background = new SymcolBackground
                {
                    Depth = 0,
                    Scale = new Vector2(),
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Alpha = 0.5f,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        new Triangles
                        {
                            Depth = 0,
                            TriangleScale = 4,
                            ColourLight = OsuColour.FromHex(@"ff0050"),
                            ColourDark = OsuColour.FromHex(@"3aba1a"),
                            RelativeSizeAxes = Axes.Both,
                        },
                    }
                },
                new MenuSideFlashes(),
                Logo = new OsuLogo
                {
                    Scale = new Vector2(1.25f),
                    Action = () => open(Logo),
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Depth = -30,
                },
                new SymcolButton
                {
                    ButtonName = "Pokeosu",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkOrange,
                    ButtonColorBottom = Color4.Orange,
                    ButtonSize = 75,
                    Action = delegate { Push(new PokeosuMenu()); },
                    ButtonPosition = new Vector2(200 , 100),
                },
                new SymcolButton
                {
                    ButtonName = "Mixer",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.Purple,
                    ButtonColorBottom = Color4.HotPink,
                    ButtonSize = 120,
                    Action = delegate { Push(new SymcolMixer()); },
                    ButtonPosition = new Vector2(-200 , -150),
                },
                new SymcolButton
                {
                    ButtonName = "Play",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkBlue,
                    ButtonColorBottom = Color4.Blue,
                    ButtonSize = 130,
                    Action = delegate { Push(consumeSongSelect()); },
                    ButtonPosition = new Vector2(300 , -20),
                },
                new SymcolButton
                {
                    ButtonName = "Multi",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkGreen,
                    ButtonColorBottom = Color4.Green,
                    ButtonSize = 100,
                    Action = delegate { Push(new Lobby()); },
                    ButtonPosition = new Vector2(140 , -120),
                },
                new SymcolButton
                {
                    ButtonName = "Edit",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkGoldenrod,
                    ButtonColorBottom = Color4.Gold,
                    ButtonSize = 90,
                    Action = delegate { Push(new Editor()); },
                    ButtonPosition = new Vector2(250 , -150),
                },
                new SymcolButton
                {
                    ButtonName = "Tests",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkCyan,
                    ButtonColorBottom = Color4.Cyan,
                    ButtonSize = 100,
                    Action = delegate { Push(new SymcolTestScreen()); },
                    ButtonPosition = new Vector2(-150 , 200),
                },
                new SymcolButton
                {
                    ButtonName = "Back",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    ButtonSize = 80,
                    //Action = () => backButton.TriggerOnClick(),
                    ButtonPosition = new Vector2(-350 , 300),
                },
            };
        }

        private Screen songSelect;

        private void preloadSongSelect()
        {
            if (songSelect == null)
                LoadComponentAsync(songSelect = new PlaySongSelect());
        }

        private Screen consumeSongSelect()
        {
            var s = songSelect;
            songSelect = null;
            return s;
        }

        private double animationTime = 600;

        private void open(Container container)
        {
            Logo.Action = () => close(container);
            container.ScaleTo(new Vector2(0.4f) , animationTime, Easing.InOutBack);
            foreach(Drawable draw in Children)
            {
                if (draw is SymcolButton)
                {
                    SymcolButton button = draw as SymcolButton;
                    button.MoveTo(button.ButtonPosition , animationTime, Easing.InOutBack);
                    button.ScaleTo(new Vector2(1), animationTime, Easing.InOutBack);
                }
            }
        }

        private void close(Container container)
        {
            Logo.Action = () => open(container);
            container.ScaleTo(new Vector2(1.2f), animationTime, Easing.InOutBack);
            foreach (Drawable draw in Children)
            {
                if (draw is SymcolButton)
                {
                    SymcolButton button = draw as SymcolButton;
                    button.MoveTo(new Vector2(0) , animationTime, Easing.InOutBack);
                    button.ScaleTo(new Vector2(0.1f), animationTime, Easing.InOutBack);
                }
            }
        }

        private void changeBackground(WorkingBeatmap beatmap)
        {
            var backgroundModeBeatmap = Background as BackgroundScreenBeatmap;
            if (backgroundModeBeatmap != null)
            {
                backgroundModeBeatmap.Beatmap = beatmap;
                backgroundModeBeatmap.BlurTo(background_blur, 1000);
                backgroundModeBeatmap.FadeTo(1, 250);
            }
        }

        protected override void OnEntering(Screen last)
        {
            base.OnEntering(last);
            ensurePlayingSelected();
            changeBackground(Beatmap);
            Content.FadeInFromZero(250);
        }
        
        protected override void OnResuming(Screen last)
        {
            preloadSongSelect();
            changeBackground(Beatmap);
            ensurePlayingSelected();
            base.OnResuming(last);
            Content.FadeIn(250);
            Content.ScaleTo(1, 250, Easing.OutSine);
        }

        protected override void OnSuspending(Screen next)
        {
            Content.ScaleTo(1.1f, 250, Easing.InSine);
            Content.FadeOut(250);
            base.OnSuspending(next);
        }

        protected override bool OnExiting(Screen next)
        {
            Content.FadeOut(100);
            return base.OnExiting(next);
        }

        private ScheduledDelegate selectionChangedDebounce;
        private BeatmapInfo selectionChangeNoBounce;
        private BeatSyncedContainer background;
        private BeatmapManager manager;

        private void selectionChanged(BeatmapInfo beatmap)
        {
            selectionChangedDebounce?.Cancel();

            if (beatmap?.Equals(Beatmap.Value.BeatmapInfo) != true)
                return;

            bool beatmapSetChange = false;
            if (beatmap.BeatmapSetInfoID != selectionChangeNoBounce?.BeatmapSetInfoID)
                beatmapSetChange = true;

            selectionChangeNoBounce = beatmap;

            selectionChangedDebounce = Scheduler.AddDelayed(delegate
            {
                Beatmap.Value = manager.GetWorkingBeatmap(beatmap, Beatmap);
                ensurePlayingSelected(beatmapSetChange);
            }, 100);
        }

        private void ensurePlayingSelected(bool preview = false)
        {
            Track track = Beatmap.Value.Track;

            if (!track.IsRunning)
            {
                if (preview) track.Seek(Beatmap.Value.Metadata.PreviewTime);
                track.Start();
            }
        }
    }
}

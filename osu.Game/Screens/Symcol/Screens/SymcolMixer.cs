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
using osu.Framework.Screens;

namespace osu.Game.Screens.Symcol.Screens
{
    public class SymcolMixer : OsuScreen
    {
        private SampleChannel nNormal;
        private SampleChannel sNormal;
        private SampleChannel dNormal;

        private SampleChannel nWhistle;
        private SampleChannel sWhistle;
        private SampleChannel dWhistle;

        private SampleChannel nFinish;
        private SampleChannel sFinish;
        private SampleChannel dFinish;

        private SampleChannel nClap;
        private SampleChannel sClap;
        private SampleChannel dClap;

        private SettingsSlider<double> clockSpeed;
        private double pitch = 1;
        private BindableDouble clockPitch;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {

            nNormal = audio.Sample.Get($@"Gameplay/normal-hitnormal");
            sNormal = audio.Sample.Get($@"Gameplay/soft-hitnormal");
            dNormal = audio.Sample.Get($@"Gameplay/drum-hitnormal");

            nWhistle = audio.Sample.Get($@"Gameplay/normal-hitwhistle");
            sWhistle = audio.Sample.Get($@"Gameplay/soft-hitwhistle");
            dWhistle = audio.Sample.Get($@"Gameplay/drum-hitwhistle");

            nFinish = audio.Sample.Get($@"Gameplay/normal-hitfinish");
            sFinish = audio.Sample.Get($@"Gameplay/soft-hitfinish");
            dFinish = audio.Sample.Get($@"Gameplay/drum-hitfinish");

            nClap= audio.Sample.Get($@"Gameplay/normal-hitclap");
            sClap = audio.Sample.Get($@"Gameplay/soft-hitclap");
            dClap = audio.Sample.Get($@"Gameplay/drum-hitclap");

            clockPitch = new BindableDouble() { MinValue = 0.5f, Value = pitch, MaxValue = 2 };

            Children = new Drawable[]
            {
                new SymcolVisualiser
                {
                    Height = 0.9f
                },
                new Container
                {
                    Position = new Vector2(0 , -25),
                    Height = 50,
                    RelativeSizeAxes = Axes.X,
                    Origin = Anchor.BottomCentre,
                    Anchor = Anchor.BottomCentre,
                    Children = new Drawable[]
                    {
                        clockSpeed = new SettingsSlider<double>
                        {
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            LabelText = "Clock Pitch",
                            Bindable = clockPitch,
                        },
                    }
                },

                //Pitch Settings
                new SymcolButton
                {
                    ButtonName = "1x",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkGoldenrod,
                    ButtonColorBottom = Color4.Goldenrod,
                    ButtonSize = 50,
                    Action = () => clockPitch.Value = 1f,
                    Position = new Vector2(0 , 250),
                },
                new SymcolButton
                {
                    ButtonName = "1.5x",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkGoldenrod,
                    ButtonColorBottom = Color4.Goldenrod,
                    ButtonSize = 50,
                    Action = () => clockPitch.Value = 1.5f,
                    Position = new Vector2(200 , 250),
                },
                new SymcolButton
                {
                    ButtonName = "0.75x",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkGoldenrod,
                    ButtonColorBottom = Color4.Goldenrod,
                    ButtonSize = 50,
                    Action = () => clockPitch.Value = 0.75f,
                    Position = new Vector2(-200, 250),
                },

                //Noramal
                new SymcolButton
                {
                    ButtonName = "Normal",
                    ButtonLabel = 'N',
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    ButtonSize = 100,
                    Action = () => playSample(nNormal),
                    Position = new Vector2(-150 , -100),
                },
                new SymcolButton
                {
                    ButtonName = "Normal",
                    ButtonLabel = 'S',
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkBlue,
                    ButtonColorBottom = Color4.Blue,
                    ButtonSize = 100,
                    Action = () => playSample(sNormal),
                    Position = new Vector2(-150 , 100),
                },
                new SymcolButton
                {
                    ButtonName = "Normal",
                    ButtonLabel = 'D',
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkGreen,
                    ButtonColorBottom = Color4.Green,
                    ButtonSize = 100,
                    Action = () => playSample(dNormal),
                    Position = new Vector2(-150, 0),
                },

                //Whistle
                new SymcolButton
                {
                    ButtonName = "Whistle",
                    ButtonLabel = 'N',
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    ButtonSize = 100,
                    Action = () => playSample(nWhistle),
                    Position = new Vector2(-50 , -100),
                },
                new SymcolButton
                {
                    ButtonName = "Whistle",
                    ButtonLabel = 'S',
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkBlue,
                    ButtonColorBottom = Color4.Blue,
                    ButtonSize = 100,
                    Action = () => playSample(sWhistle),
                    Position = new Vector2(-50 , 100),
                },
                new SymcolButton
                {
                    ButtonName = "Whistle",
                    ButtonLabel = 'D',
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkGreen,
                    ButtonColorBottom = Color4.Green,
                    ButtonSize = 100,
                    Action = () => playSample(dWhistle),
                    Position = new Vector2(-50, 0),
                },

                //Finish
                new SymcolButton
                {
                    ButtonName = "Finish",
                    ButtonLabel = 'N',
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    ButtonSize = 100,
                    Action = () => playSample(nFinish),
                    Position = new Vector2(50 , -100),
                },
                new SymcolButton
                {
                    ButtonName = "Finish",
                    ButtonLabel = 'S',
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkBlue,
                    ButtonColorBottom = Color4.Blue,
                    ButtonSize = 100,
                    Action = () => playSample(sFinish),
                    Position = new Vector2(50 , 100),
                },
                new SymcolButton
                {
                    ButtonName = "Finish",
                    ButtonLabel = 'D',
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkGreen,
                    ButtonColorBottom = Color4.Green,
                    ButtonSize = 100,
                    Action = () => playSample(dFinish),
                    Position = new Vector2(50, 0),
                },

                //Clap
                new SymcolButton
                {
                    ButtonName = "Clap",
                    ButtonLabel = 'N',
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    ButtonSize = 100,
                    Action = () => playSample(nClap),
                    Position = new Vector2(150 , -100),
                },
                new SymcolButton
                {
                    ButtonName = "Clap",
                    ButtonLabel = 'S',
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkBlue,
                    ButtonColorBottom = Color4.Blue,
                    ButtonSize = 100,
                    Action = () => playSample(sClap),
                    Position = new Vector2(150 , 100),
                },
                new SymcolButton
                {
                    ButtonName = "Clap",
                    ButtonLabel = 'D',
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.1f),
                    ButtonColorTop = Color4.DarkGreen,
                    ButtonColorBottom = Color4.Green,
                    ButtonSize = 100,
                    Action = () => playSample(dClap),
                    Position = new Vector2(150, 0),
                },
            };
        }

        protected override void Update()
        {
            base.Update();
            applyRateAdjustments();
        }

        protected override void OnEntering(Screen last)
        {
            base.OnEntering(last);
            setClockSpeed(WorkingBeatmap.PlayingTrack);
        }

        private void setClockSpeed(IAdjustableClock clock)
        {
            var pitchAdjust = clock as IHasPitchAdjust;
            clockSpeed.Bindable.Value = pitchAdjust.PitchAdjust;
        }

        private void applyRateAdjustments()
        {
            if (WorkingBeatmap.PlayingTrack == null) return;
            else
                ApplyToClock(WorkingBeatmap.PlayingTrack);
        }

        private void ApplyToClock(IAdjustableClock clock)
        {
            var pitchAdjust = clock as IHasPitchAdjust;
            if (pitchAdjust != null)
            {
                pitchAdjust.PitchAdjust = clockSpeed.Bindable.Value;
                pitch = pitchAdjust.PitchAdjust;
            }
                
            else
                clock.Rate = clockSpeed.Bindable.Value;
        }

        private void playSample(SampleChannel sample)
        {
            sample.Play();
        }
    }
}

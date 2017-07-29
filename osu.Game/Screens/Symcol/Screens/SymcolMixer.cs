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
using osu.Game.Screens.Menu;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Beatmaps.ControlPoints;
using System;

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
        public static BindableDouble ClockPitch;

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

            ClockPitch = new BindableDouble() { MinValue = 0.5f, Value = pitch, MaxValue = 2 };

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
                            Bindable = ClockPitch,
                        },
                    }
                },

                //Sounds Bar
                new musicBar
                {
                    RelativeSizeAxes = Axes.X,
                    Origin = Anchor.TopCentre,
                    Anchor = Anchor.TopCentre,
                    Position = new Vector2(0 , 100),
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
                    Action = () => ClockPitch.Value = 1f,
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
                    Action = () => ClockPitch.Value = 1.5f,
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
                    Action = () => ClockPitch.Value = 0.75f,
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

    internal class musicBar : BeatSyncedContainer
    {
        private Box seekBar;
        private float beatLength = 1;
        private float lastBeatTime = 1;
        private int measure = 0;
        private float measureLength = 1;
        private float lastMeasureTime = 1;

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Children = new Drawable[]
            {
                new Box
                {
                    Colour = Color4.White,
                    Size = new Vector2(600 , 4),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new Box
                {
                    Position = new Vector2(300 , 0),
                    Colour = Color4.White,
                    Size = new Vector2(4 , 30),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new Box
                {
                    Position = new Vector2(-300 , 0),
                    Colour = Color4.White,
                    Size = new Vector2(4 , 30),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new Box
                {
                    Position = new Vector2(-300 / 2 , 0),
                    Colour = Color4.White,
                    Size = new Vector2(3.5f , 22),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new Box
                {
                    Position = new Vector2(-0 , 0),
                    Colour = Color4.White,
                    Size = new Vector2(4 , 26),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new Box
                {
                    Position = new Vector2(300 / 2 , 0),
                    Colour = Color4.White,
                    Size = new Vector2(3.5f , 22),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                seekBar = new Box
                {
                    Position = new Vector2(-300 , 0),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(2 , 20),
                },
            };
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);
            beatLength = (float)timingPoint.BeatLength;
            measureLength = (float)timingPoint.BeatLength * 4;
            if (lastMeasureTime <= (float)(WorkingBeatmap.PlayingTrack.CurrentTime - measureLength * 0.9f) || lastMeasureTime > (float)WorkingBeatmap.PlayingTrack.CurrentTime)
                lastMeasureTime = (float)WorkingBeatmap.PlayingTrack.CurrentTime;
            lastBeatTime = (float)WorkingBeatmap.PlayingTrack.CurrentTime;
            if(SymcolMixer.ClockPitch.Value > 0)
                measure++;
            if (SymcolMixer.ClockPitch.Value < 0)
                measure--;
            if (measure > 4)
                measure = 1;
            if (measure < 1)
                measure = 4;
        }

        protected override void Update()
        {
            base.Update();

            if (WorkingBeatmap.PlayingTrack.IsRunning)
                seekBarPosition();
        }

        private Vector2 seekBarPosition()
        {
            measure = (int)((((float)WorkingBeatmap.PlayingTrack.CurrentTime - lastMeasureTime) / measureLength) * 4);
            float minX = (measure) * 150;
            
            Vector2 position = new Vector2((((((float)WorkingBeatmap.PlayingTrack.CurrentTime - lastBeatTime) / beatLength) * 150) + 300), 0);
            
            position.X %= 150;
            position.X += minX - 300;

            seekBar.Position = position;
            return seekBar.Position;
        }

        private void halfBeat()
        {

        }

        private void quarterBeat()
        {

        }

        private void generateMeasure(float x)
        {

        }
    }



    internal class musicBarTick : Container
    {
        private Box box;
        private Container glow;

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Children = new Drawable[]
            {
                box = new Box
                {
                    Depth = -2,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                glow = new Container
                {
                    Alpha = 0.25f,
                    Depth = 0,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Radius = 4,
                    },
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                    }
                },
            };
        }
        public void Activate(float beatLength , float flashIntensity)
        {
            glow.Alpha = 0.5f * flashIntensity;
            glow.FadeTo(0.25f, beatLength);
        }
    }
}

// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK.Graphics;
using osu.Framework.Screens;
using osu.Game.Screens.Backgrounds;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using OpenTK;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shapes;
using osu.Game.Screens.Edit.EditorPieces;

namespace osu.Game.Screens.Edit
{
    internal class Editor : OsuScreen
    {
        private EditorToolbox toolbox;
        private EditorToolbox guides;
        private EditorToolbox hitsounds;

        protected override BackgroundScreen CreateBackground() => new BackgroundScreenCustom(@"Backgrounds/bg4");

        internal override bool ShowOverlays => false;

        protected override void LoadComplete()
        {
            base.LoadComplete();
            toolbox.Title.Text = "TOOLBOX";
            guides.Title.Text = "GUIDES";
            hitsounds.Title.Text = "HIT SOUND";
        }

        protected override void OnResuming(Screen last)
        {
            Beatmap.Value.Track?.Stop();
            base.OnResuming(last);
        }
        protected override void OnEntering(Screen last)
        {
            base.OnEntering(last);
            Background.FadeColour(Color4.DarkGray, 500);
            Beatmap.Value.Track?.Stop();
        }
        protected override bool OnExiting(Screen next)
        {
            Background.FadeColour(Color4.White, 500);
            Beatmap.Value.Track?.Start();
            return base.OnExiting(next);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new Container
                {
                    Size = new Vector2(600 , 450),
                    Masking = true,
                    BorderColour = Color4.White,
                    BorderThickness = 4,
                    CornerRadius = 4,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = Color4.White.Opacity(0),
                            RelativeSizeAxes = Axes.Both,
                        }
                    }
                },
                toolbox = new EditorToolbox
                {
                    Position = new Vector2(10 , -150),
                    Size = new Vector2(120, 140),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                },
                guides = new EditorToolbox
                {
                    Position = new Vector2(10 , 0),
                    Size = new Vector2(120, 140),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                },
                hitsounds = new EditorToolbox
                {
                    Position = new Vector2(10 , 150),
                    Size = new Vector2(120, 140),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                }
            };
        }
    }
}

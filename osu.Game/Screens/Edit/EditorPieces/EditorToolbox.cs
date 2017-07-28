// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using System;
using System.Diagnostics;

namespace osu.Game.Screens.Edit.EditorPieces
{
    public class EditorToolbox : Container
    {
        public Container Toolbox;
        public FillFlowContainer BaseBox;
        public SpriteText Title;
        public IconButton MinimizeButton;

        public EditorToolbox()
        {
            Children = new Drawable[]
            {
                Toolbox = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 4,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = Color4.Black.Opacity(0.75f),
                            RelativeSizeAxes = Axes.Both,
                        },
                        Title = new SpriteText
                        {
                            Depth = -1,
                            Position = new Vector2(4),
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Text = "NULL",
                            TextSize = 16,
                        },
                        MinimizeButton = new IconButton
                        {
                            Scale = new Vector2(0.75f),
                            Position = new Vector2(-4 , 4),
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Icon = FontAwesome.fa_chevron_down,
                            Action = Minimize,
                        }
                    },
                }
            };
        }

        protected override bool OnDragStart(InputState state) => true;

        protected override bool OnDrag(InputState state)
        {
            Trace.Assert(state.Mouse.PositionMouseDown != null, "state.Mouse.PositionMouseDown != null");

            Position += state.Mouse.Delta;
            return base.OnDrag(state);
        }

        public void Minimize()
        {
            MinimizeButton.Icon = FontAwesome.fa_chevron_right;
            MinimizeButton.Action = Maximize;
            this.ResizeTo(new Vector2(120, 30), 400, Easing.OutSine);
        }
        public void Maximize()
        {
            MinimizeButton.Icon = FontAwesome.fa_chevron_down;
            MinimizeButton.Action = Minimize;
            this.ResizeTo(new Vector2(120, 140), 400, Easing.OutSine);
        }
    }
}

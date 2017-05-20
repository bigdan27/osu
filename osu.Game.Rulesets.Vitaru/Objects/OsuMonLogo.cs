using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Graphics;
using osu.Game.Graphics.Backgrounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu.Game.Rulesets.Vitaru.Objects
{
    public class OsuMonLogo : Container
    {
        private CircularContainer upperHalf;
        private CircularContainer lowerHalf;
        private CircularContainer centerCut;
        private Container centerLine;
        private readonly CircularContainer logoContainer;
        private readonly Container logoBounceContainer;
        private readonly Container logoHoverContainer;

        public Color4 OsuPink = OsuColour.FromHex(@"e967a1");

        private Sprite icon;

        private readonly Container colourAndTriangles;

        public bool Triangles
        {
            set
            {
                colourAndTriangles.Alpha = value ? 1 : 0;
            }
        }

        public OsuMonLogo()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            AutoSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                logoBounceContainer = new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        logoHoverContainer = new Container
                        {
                            AutoSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new BufferedContainer
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Children = new Drawable[]
                                    {
                                        logoContainer = new CircularContainer
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            RelativeSizeAxes = Axes.Both,
                                            Scale = new Vector2(0.8f),
                                            Masking = true,
                                            Children = new Drawable[]
                                            {
                                                upperHalf = new CircularContainer
                                                {
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                    RelativeSizeAxes = Axes.Both,
                                                    Scale = new Vector2(1.1f),
                                                    Masking = true,
                                                    Children = new Drawable[]
                                                    {
                                                        new Box
                                                        {
                                                            RelativeSizeAxes = Axes.Both,
                                                            Colour = OsuPink,
                                                        },
                                                        new Triangles
                                                        {
                                                            TriangleScale = 4,
                                                            ColourLight = OsuColour.FromHex(@"ff7db7"),
                                                            ColourDark = OsuColour.FromHex(@"de5b95"),
                                                            RelativeSizeAxes = Axes.Both,
                                                        },
                                                    }
                                                },
                                                lowerHalf = new CircularContainer
                                                {
                                                    Masking = true,
                                                },
                                                centerCut = new CircularContainer
                                                {
                                                    Scale = new Vector2(0.4f),
                                                    Masking = true,
                                                },
                                                centerLine = new Container
                                                {
                                                    Masking = true,
                                                },
                                            },
                                        },
                                        icon = new Sprite
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                        },
                                    }
                                }
                            }
                        }
                    }
                }




                /*
                upperHalf = new CircularContainer()
                {
                    Depth = 1,
                    Masking = true,
                    Size = new Vector2(1024),
                    Colour = Color4.Pink,
                    Alpha = 1,
                },
                lowerHalf = new CircularContainer()
                {
                    Depth = 1,
                    Masking = true,
                },
                centerCut = new CircularContainer()
                {
                    Depth = 2,
                    Masking = true,
                },
                centerLine = new Container()
                {
                    Depth = 2,
                    Masking = true,
                },
                icon = new Sprite()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1,
                    Depth = 0,
                    Scale = new Vector2(0.5f),
                },*/
            };
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            icon.Texture = textures.Get(@"Menu/osumonLogo");
        }
    }
}

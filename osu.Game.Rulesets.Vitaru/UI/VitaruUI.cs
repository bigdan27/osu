using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Scoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class VitaruUI : Container
    {
        private bool debugInfo = true;

        //User stuff
        private SpriteText energy;
        private Container energyBar;
        private Container friendlyBar;
        private SpriteText health;
        private Container healthBar;
        private Container opponentBar;
        private Triangles energyTriangles;
        private Triangles healthTriangles;
        private float textSize = 40;
        private Box energyBarBox;
        private Box healthBarBox;
        private Box friendlyBarBox;
        private Box opponentBarBox;

        //Debug section
        private SpriteText frameTime;

        public VitaruUI()
        {
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new Box
                {
                    Alpha = 0.25f,
                    Colour = Color4.Black,
                    Depth = 10,
                    Position = new Vector2(10),
                    Size = new Vector2(512 , 820),
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                },
                energy = new SpriteText
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreLeft,
                    Position = new Vector2(10 , 0),
                    TextSize = textSize,
                    Colour = Color4.SkyBlue,
                    Text = "Energy Value Here",
                },
                health = new SpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreRight,
                    Position = new Vector2(-10 , 0),
                    TextSize = textSize,
                    Colour = Color4.Green,
                    Text = "Health Value Here",
                },
                energyBar = new Container
                {
                    Masking = true,
                    Alpha = 1f,
                    Depth = 1,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Colour = Color4.SkyBlue,
                    Size = new Vector2(10,820),
                    Position = new Vector2(0),
                    Children = new Drawable[]
                    {
                        energyBarBox = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.White,
                        },
                        energyTriangles = new Triangles
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            ColourLight = Color4.LightSkyBlue,
                            ColourDark = Color4.DeepSkyBlue,
                            TriangleScale = 1,
                        }
                    },
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Type = EdgeEffectType.Shadow,
                        Colour = Color4.SkyBlue.Opacity(0.5f),
                        Radius = 8,
                    }
                },
                healthBar = new Container
                {
                    Masking = true,
                    Alpha = 1f,
                    Depth = 0,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Colour = Color4.LightGreen,
                    Size = new Vector2(10,820),
                    Position = new Vector2(0),
                    Children = new Drawable[]
                    {
                        healthBarBox = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.White,
                        },
                        healthTriangles = new Triangles
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            ColourLight = Color4.DarkGreen,
                            ColourDark = Color4.LightGreen,
                            TriangleScale = 1,
                        }
                    },
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Type = EdgeEffectType.Shadow,
                        Colour = Color4.Green.Opacity(0.5f),
                        Radius = 8,
                    }
                },
                opponentBar = new Container
                {
                    Masking = true,
                    Alpha = 1f,
                    Depth = 0,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Colour = Color4.YellowGreen,
                    Size = new Vector2(532 , 10),
                    Children = new Drawable[]
                    {
                        opponentBarBox = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.White,
                        },
                    },
                },
                friendlyBar = new Container
                {
                    Masking = true,
                    Alpha = 1f,
                    Depth = 0,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Colour = Color4.Red,
                    Size = new Vector2(532 , 10),
                    Children = new Drawable[]
                    {
                        friendlyBarBox = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.White,
                        },
                    },
                },
                frameTime = new SpriteText
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreLeft,
                    Position = new Vector2(10 , 30),
                    TextSize = 30,
                    Colour = Color4.Purple,
                    Text = "Frametime Value Here",
                },
            };

            if(debugInfo)
            {
                frameTime.Alpha = 1;
            }
        }

        protected override void Update()
        {
            base.Update();

            if ((VitaruPlayer.PlayerHealth) > 100)
                health.Colour = Color4.SkyBlue;
            if ((VitaruPlayer.PlayerHealth) > 50 && (VitaruPlayer.PlayerHealth) <= 100)
                health.Colour = Color4.Green;
            if ((VitaruPlayer.PlayerHealth) <= 50 && (VitaruPlayer.PlayerHealth) > 20)
                health.Colour = Color4.Yellow;
            if ((VitaruPlayer.PlayerHealth) <= 20 && (VitaruPlayer.PlayerHealth) > 0)
                health.Colour = Color4.Red;
            if ((VitaruPlayer.PlayerHealth) <= 0)
                health.Colour = Color4.Black;

            healthBar.Colour = health.Colour;
            energyBar.ResizeTo(new Vector2(10 , VitaruPlayer.PlayerEnergy * 8.20f) , 100 , EasingTypes.OutCubic);
            healthBar.ResizeTo(new Vector2(10, VitaruPlayer.PlayerHealth * 8.20f), 100, EasingTypes.OutCubic);
            energyBarBox.ResizeTo(new Vector2(10, VitaruPlayer.PlayerEnergy * 8.20f), 100, EasingTypes.OutCubic);
            healthBarBox.ResizeTo(new Vector2(10, VitaruPlayer.PlayerHealth * 8.20f), 100, EasingTypes.OutCubic);
            energy.Text = (VitaruPlayer.PlayerEnergy).ToString() + "% Charge";
            health.Text = (Math.Floor(VitaruPlayer.PlayerHealth)).ToString() + "% Health";
            frameTime.Text = (Math.Floor((float)Clock.ElapsedFrameTime)).ToString() + " ms Delay";
        }
    }
}

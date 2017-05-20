using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Backgrounds;
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
        private SpriteText energy;
        private Container energyBar;
        private Container friendlyBar;
        private SpriteText health;
        private Container healthBar;
        private Container opponentBar;
        private Triangles energyTriangles;
        private Triangles healthTriangles;
        private float textSize = 40;

        public VitaruUI()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                energy = new SpriteText
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreLeft,
                    TextSize = textSize,
                    Colour = Color4.SkyBlue,
                    Text = "Energy Value Here",
                },
                health = new SpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreRight,
                    TextSize = textSize,
                    Colour = Color4.Green,
                    Text = "Health Value Here",
                },
                energyBar = new Container
                {
                    Depth = 1,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreLeft,
                    Colour = Color4.SkyBlue.Opacity(0.5f),
                    Size = new Vector2(10,820),
                    BorderColour = Color4.SkyBlue,
                    BorderThickness = 2,
                    Position = new Vector2(0),
                    Children = new Drawable[]
                    {
                        energyTriangles = new Triangles
                        {
                            ColourLight = Color4.LightSkyBlue,
                            ColourDark = Color4.DeepSkyBlue,
                            TriangleScale = 1,
                        }
                    },
                    EdgeEffect = new EdgeEffect
                    {
                        Type = EdgeEffectType.Shadow,
                        Colour = Color4.SkyBlue,
                        Radius = 2,
                    }
                },
                healthBar = new Container
                {
                    Depth = 0,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreRight,
                    Colour = Color4.Green.Opacity(0.5f),
                    Size = new Vector2(10,820),
                    BorderColour = Color4.Green,
                    BorderThickness = 2,
                    Position = new Vector2(0),
                    Children = new Drawable[]
                    {
                        healthTriangles = new Triangles
                        {
                            ColourLight = Color4.DarkGreen,
                            ColourDark = Color4.LightGreen,
                            TriangleScale = 1,
                        }
                    },
                    EdgeEffect = new EdgeEffect
                    {
                        Type = EdgeEffectType.Shadow,
                        Colour = Color4.Green,
                        Radius = 2,
                    }
                },
                opponentBar = new Container
                {
                    Depth = 0,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.BottomCentre,
                    Colour = Color4.YellowGreen,
                    Size = new Vector2(512 , 10),
                },
                friendlyBar = new Container
                {
                    Depth = 0,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.TopCentre,
                    Colour = Color4.Red,
                    Size = new Vector2(512 , 10),
                },
            };
        }

        protected override void Update()
        {
            base.Update();

            if ((VitaruScoreProcessor.PlayerHealth * 100) > 100)
                health.Colour = Color4.SkyBlue;
            if ((VitaruScoreProcessor.PlayerHealth * 100) > 50 && (VitaruScoreProcessor.PlayerHealth * 100) <= 100)
                health.Colour = Color4.Green;
            if ((VitaruScoreProcessor.PlayerHealth * 100) <= 50 && (VitaruScoreProcessor.PlayerHealth * 100) > 20)
                health.Colour = Color4.Yellow;
            if ((VitaruScoreProcessor.PlayerHealth * 100) <= 20 && (VitaruScoreProcessor.PlayerHealth * 100) > 0)
                health.Colour = Color4.Red;
            if ((VitaruScoreProcessor.PlayerHealth * 100) <= 0)
                health.Colour = Color4.Black;


            energy.Text = (VitaruScoreProcessor.PlayerEnergy * 100).ToString() + "% Charge";
            health.Text = (Math.Floor(VitaruScoreProcessor.PlayerHealth * 100)).ToString() + "% Health";
        }
    }
}

using osu.Framework.Graphics.Containers;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using System;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;
using osu.Game.Rulesets.Vitaru.Beatmaps;

namespace osu.Game.Rulesets.Vitaru.Objects.Projectiles
{
    public abstract class DrawableBulletPattern : DrawableVitaruHitObject
    {
        public abstract int PatternID { get; }

        public Color4 PatternColor { get; set; } = Color4.White;

        protected BulletPattern BulletPattern;

        public DrawableBulletPattern(BulletPattern bulletPattern) : base(bulletPattern)
        {
            BulletPattern = bulletPattern;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (BulletPattern.PatternAngleRadian == -10)
                BulletPattern.PatternAngleRadian = MathHelper.DegreesToRadians(BulletPattern.PatternAngleDegree - 90);

            if(HitObject.StartTime <= Time.Current)
                CreatePattern();
        }

        protected void PatternStart()
        {
            Position = new Vector2(BulletPattern.Position.Y, BulletPattern.Position.Y + 200);
            MoveTo(BulletPattern.Position, 500);
            FadeInFromZero(250);
        }

        protected override void Update()
        {
            base.Update();

            if (HitObject.StartTime + BulletPattern.PatternDuration <= Time.Current)
                Dispose();
        }
        
        protected void bulletAddRad(float speed, float angle)
        {
            Bullet bullet= new Bullet { };
            DrawableBullet drawableBullet;
            VitaruPlayfield.vitaruPlayfield.Add(drawableBullet = new DrawableBullet(bullet)
            {
                Origin = Anchor.Centre,
                Depth = 5,
                BulletColor = PatternColor,
                BulletAngleRadian = angle,
                BulletSpeed = speed,
                BulletWidth = BulletPattern.PatternBulletWidth,
                BulletDamage = BulletPattern.PatternDamage,
                DynamicBulletVelocity = BulletPattern.DynamicPatternVelocity,
            });
            drawableBullet.MoveTo(ToSpaceOfOtherDrawable(BulletPattern.PatternPosition, drawableBullet));
        }
        protected abstract void CreatePattern();
    }
    public class Wave : DrawableBulletPattern
    {
        public override int PatternID => 0;

        public Wave(BulletPattern bulletPattern) : base(bulletPattern)
        {
        }

        protected override void CreatePattern()
        {
            int numberBullets = (int)BulletPattern.PatternDifficulty * 2 + 1;
            float directionModifier = -0.1f * ((numberBullets - 1) / 2);
            for (int i = 1; i <= numberBullets; i++)
            {
                bulletAddRad(BulletPattern.PatternSpeed, BulletPattern.PatternAngleRadian + directionModifier);
                directionModifier += 0.1f;
            }
        }
    }
    public class Line : DrawableBulletPattern
    {
        public override int PatternID => 1;

        public Line(BulletPattern bulletPattern) : base(bulletPattern)
        {
        }

        protected override void CreatePattern()
        {
            for (int i = 1; i <= BulletPattern.PatternDifficulty + 1; i++)
            {
                bulletAddRad(0.12f + BulletPattern.PatternSpeed, BulletPattern.PatternAngleRadian);
                BulletPattern.PatternSpeed += 0.02f;
            }
        }
    }
    public class Flower : DrawableBulletPattern
    {
        public override int PatternID => 2;

        public Flower(BulletPattern bulletPattern) : base(bulletPattern)
        {
        }

        protected override void CreatePattern()
        {
            double timeSaved = Time.Current;
            int a = 0;
            for (int j = 1; j <= 16 * BulletPattern.PatternDifficulty; j++)
            {
                a = a + 21;
                BulletPattern.PatternAngleRadian = MathHelper.DegreesToRadians(a - 90);
                bulletAddRad(BulletPattern.PatternSpeed, a);
            }
        }
    }
    public class CoolWave : DrawableBulletPattern
    {
        public override int PatternID => 4;

        public CoolWave(BulletPattern bulletPattern) : base(bulletPattern)
        {
        }

        protected override void CreatePattern()
        {
            float speedModifier = 0.01f + 0.01f * (BulletPattern.PatternDifficulty);
            float directionModifier = -0.075f - 0.075f * (BulletPattern.PatternDifficulty);
            for (int i = 1; i <= 3 + (BulletPattern.PatternDifficulty * 2); i++)
            {
                bulletAddRad(
                    BulletPattern.PatternSpeed + Math.Abs(speedModifier),
                    directionModifier + BulletPattern.PatternAngleRadian
                );
                speedModifier -= 0.01f;
                directionModifier += 0.075f;
            }
        }
    }

    public class Spin : DrawableBulletPattern
    {
        public override int PatternID => 5;
        
        public Spin(BulletPattern bulletPattern) : base(bulletPattern)
        {
        }

        protected override void CreatePattern()
        {
            double numberbullets = 30 * BulletPattern.PatternDifficulty;
            int numberspins = (int)(BulletPattern.PatternDifficulty + 2) / 2;
            float spinModifier = MathHelper.DegreesToRadians((float)(360 / numberspins));
            float directionModifier = (float)(360 / numberbullets);
            directionModifier = MathHelper.DegreesToRadians(directionModifier);
            double originalDuration = BulletPattern.PatternDuration;
            BulletPattern.PatternDuration /= numberbullets;
            BulletPattern.PatternDuration /= BulletPattern.PatternRepeatTimes;
            int i = 1;
            while(i <= BulletPattern.PatternRepeatTimes)
            {
                int j = 1;
                while (j <= numberbullets)
                {
                    Scheduler.AddDelayed(() =>
                    {
                        for(int k = 1; k <= numberspins; k++)
                            bulletAddRad(BulletPattern.PatternSpeed, BulletPattern.PatternAngleRadian + (spinModifier * (k - 1)));
                        BulletPattern.PatternAngleRadian -= directionModifier;
                    }, BulletPattern.PatternDuration * (j - 1) + (originalDuration * (i - 1)));
                    j++;
                }
                i++;
            }
        }
    }
    public class Trianglewave : DrawableBulletPattern
    {
        public override int PatternID => 5;

        public Trianglewave(BulletPattern bulletPattern) : base(bulletPattern)
        {
        }

        protected override void CreatePattern()
        {
            bool reversed = true;
            int numberwaves = (int)(BulletPattern.PatternDifficulty + 2) / 2;
            float originalDirection = 0f;
            int numberbullets;
            BulletPattern.PatternDuration /= numberwaves;
            float speedModifier;
            for (int i = 1; i <= numberwaves; i++)
            {
                numberbullets = i;
                if (reversed)
                    speedModifier = 0.30f - (i - 1) * 0.03f;
                else
                    speedModifier = (i - 1) * 0.03f;
                for (int j = 1; j <= numberbullets; j++)
                {
                    float directionModifier = ((j - 1) * 0.1f);
                        bulletAddRad(
                            BulletPattern.PatternSpeed + speedModifier,
                            BulletPattern.PatternAngleRadian + (originalDirection - directionModifier)
                        );
                }
                originalDirection = 0.05f * i;
            }
        }
    }
}

using osu.Framework.Graphics.Containers;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using System;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;

namespace osu.Game.Rulesets.Vitaru.Objects.Projectiles
{
    public abstract class BulletPattern : Container
    {
        public abstract int PatternID { get; }
        public float PatternSpeed { get; set; }
        public float PatternDifficulty { get; set; } = 1;
        public float PatternAngleRadian { get; set; } = -10;
        public float PatternAngleDegree { get; set; } = 0;
        public float PatternBulletWidth { get; set; } = 2;
        public float PatternDamage { get; set; } = 10;
        public float PatternRepeatTimes { get; set; } = 1f;
        public double PatternDuration { get; set; } = 0;
        public double PatternRepeatDelay { get; set; } = 0;
        public bool DynamicPatternVelocity { get; set; } = false;
        public int Team { get; set; }
        public double StartTime;

        public Color4 PatternColor { get; set; } = Color4.White;

        public BulletPattern()
        {
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            StartTime = Time.Current;
            if (PatternAngleRadian == -10)
                PatternAngleRadian = MathHelper.DegreesToRadians(PatternAngleDegree - 90);

            CreatePattern();
        }

        protected override void Update()
        {
            base.Update();
            if (StartTime + PatternDuration <= Time.Current)
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
                BulletWidth = PatternBulletWidth,
                BulletDamage = PatternDamage,
                DynamicBulletVelocity = DynamicPatternVelocity,
                Team = Team,
            });
            drawableBullet.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), drawableBullet));
        }
        protected abstract void CreatePattern();
    }
    public class Wave : BulletPattern
    {
        public override int PatternID => 0;

        public Wave(int team)
        {
            Team = team;
        }

        protected override void CreatePattern()
        {
            int numberBullets = (int)PatternDifficulty * 2 + 1;
            float directionModifier = -0.1f * ((numberBullets - 1) / 2);
            for (int i = 1; i <= numberBullets; i++)
            {
                bulletAddRad(PatternSpeed, PatternAngleRadian + directionModifier);
                directionModifier += 0.1f;
            }
        }
    }
    public class Line : BulletPattern
    {
        public override int PatternID => 1;

        public Line(int team)
        {
            Team = team;
        }

        protected override void CreatePattern()
        {
            for (int i = 1; i <= PatternDifficulty + 1; i++)
            {
                bulletAddRad(0.12f + PatternSpeed, PatternAngleRadian);
                PatternSpeed += 0.02f;
            }
        }
    }
    public class Flower : BulletPattern
    {
        public override int PatternID => 2;

        public Flower(int team)
        {
            Team = team;
        }

        protected override void CreatePattern()
        {
            double timeSaved = Time.Current;
            int a = 0;
            for (int j = 1; j <= 16 * PatternDifficulty; j++)
            {
                a = a + 21;
                PatternAngleRadian = MathHelper.DegreesToRadians(a - 90);
                bulletAddRad(PatternSpeed, a);
            }
        }
    }
    public class CoolWave : BulletPattern
    {
        public override int PatternID => 4;

        public CoolWave(int team)
        {
            Team = team;
        }

        protected override void CreatePattern()
        {
            float speedModifier = 0.01f + 0.01f * (PatternDifficulty);
            float directionModifier = -0.075f - 0.075f * (PatternDifficulty);
            for (int i = 1; i <= 3 + (PatternDifficulty * 2); i++)
            {
                bulletAddRad(
                    PatternSpeed + Math.Abs(speedModifier),
                    directionModifier + PatternAngleRadian
                );
                speedModifier -= 0.01f;
                directionModifier += 0.075f;
            }
        }
    }

    public class Spin : BulletPattern
    {
        public override int PatternID => 5;
        
        public Spin(int team)
        {
            Team = team;
        }

        protected override void CreatePattern()
        {
            double numberbullets = 30 * PatternDifficulty;
            int numberspins = (int)(PatternDifficulty + 2) / 2;
            float spinModifier = MathHelper.DegreesToRadians((float)(360 / numberspins));
            float directionModifier = (float)(360 / numberbullets);
            directionModifier = MathHelper.DegreesToRadians(directionModifier);
            double originalDuration = PatternDuration;
            PatternDuration /= numberbullets;
            PatternDuration /= PatternRepeatTimes;
            int i = 1;
            while(i <= PatternRepeatTimes)
            {
                int j = 1;
                while (j <= numberbullets)
                {
                    Scheduler.AddDelayed(() =>
                    {
                        for(int k = 1; k <= numberspins; k++)
                            bulletAddRad(PatternSpeed, PatternAngleRadian + (spinModifier * (k - 1)));
                        PatternAngleRadian -= directionModifier;
                    }, PatternDuration * (j - 1) + (originalDuration * (i - 1)));
                    j++;
                }
                i++;
            }
        }
    }
    public class Trianglewave : BulletPattern
    {
        public override int PatternID => 5;

        public Trianglewave(int team)
        {
            Team = team;
        }

        protected override void CreatePattern()
        {
            bool reversed = true;
            int numberwaves = (int)(PatternDifficulty + 2) / 2;
            float originalDirection = 0f;
            int numberbullets;
            PatternDuration /= numberwaves;
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
                            PatternSpeed + speedModifier,
                            PatternAngleRadian + (originalDirection - directionModifier)
                        );
                }
                originalDirection = 0.05f * i;
            }
        }
    }
}

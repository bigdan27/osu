﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

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
        public double Duration { get; set; }
        public bool DynamicPatternVelocity { get; set; } = false;
        public int Team { get; set; }

        public Color4 PatternColor { get; set; } = Color4.White;
        protected int bulletCount { get; set; } = 0;

        public BulletPattern()
        {
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (PatternAngleRadian == -10)
                PatternAngleRadian = MathHelper.DegreesToRadians(PatternAngleDegree - 90);

            CreatePattern();
            Dispose();
        }

        protected override void Update()
        {
            base.Update();

            if (bulletCount <= 0)
                Dispose();
        }
        protected void bulletAddRad(float speed, float angle)
        {
            bulletCount++;
            Projectile projectile = new Projectile { };
            Bullet bullet;
            VitaruPlayfield.vitaruPlayfield.Add(bullet = new Bullet(projectile)
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
            bullet.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), bullet));
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
            float directionModifier = -0.1f * numberBullets;
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
    public class Circle : BulletPattern
    {
        public override int PatternID => 3;

        public Circle(int team)
        {
            Team = team;
        }

        protected override void CreatePattern()
        {
            int numberbullets = (int)Math.Pow(2, (PatternDifficulty + 4) / 4);
            float directionModifier = (float)(360 / numberbullets);
            directionModifier = MathHelper.DegreesToRadians(directionModifier);
            float circleAngle = 0;
            for (int j = 1; j <= Math.Pow(2, PatternDifficulty * 2); j++)
            {
                bulletAddRad(PatternSpeed, circleAngle);
                circleAngle += directionModifier;
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
            int numberbullets = (int)(16 * PatternDifficulty * (PatternDifficulty / 5f + 1));
            float directionModifier = (float)(360 / (16 * PatternDifficulty));
            directionModifier = MathHelper.DegreesToRadians(directionModifier);
            Duration /= numberbullets;
            int j = 1;
            while (j <= numberbullets)
            {
                Scheduler.AddDelayed(() =>
                {
                    bulletAddRad(PatternSpeed, PatternAngleRadian);
                    PatternAngleRadian -= directionModifier;
                }, Duration * j);
                j++;
                // Delay each bullet by Duration
            }
        }
    }
}

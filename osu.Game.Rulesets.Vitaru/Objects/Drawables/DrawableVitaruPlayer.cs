﻿using OpenTK;
using OpenTK.Input;
using osu.Framework.Graphics;
using osu.Framework.Input;
using System.Collections.Generic;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;
using OpenTK.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Scoring;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Rulesets.Vitaru.Objects.Characters;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableVitaruPlayer : DrawableCharacter
    {
        private readonly VitaruPlayer player;

        private Dictionary<Key, bool> keys = new Dictionary<Key, bool>();

        private double savedTime = -10000;

        private int healEnergy = 10;
        private int maxEnergy = 100;

        //(MinX,MaxX,MinY,MaxY)
        private Vector4 playerBounds = new Vector4(0, 512, 0, 820);

        public DrawableVitaruPlayer(VitaruPlayer player) : base(player)
        {
            this.player = player;

            //Gross Key stuff
            keys[Key.Up] = false;
            keys[Key.Right] = false;
            keys[Key.Down] = false;
            keys[Key.Left] = false;
            keys[Key.Z] = false;
            keys[Key.X] = false;
            keys[Key.LShift] = false;
            keys[Key.RShift] = false;

            Origin = Anchor.Centre;
            Position = player.Position;
            CharacterType = HitObjectType.Player;
            CharacterHealth = 100;
            Team = 0;
            HitboxColor = Color4.Yellow;
            HitboxWidth = 4;
            CharacterShoot = shoot;
        }

        protected override void CheckJudgement(bool userTriggered)
        {
            if(CharacterHealth <= 0)
                Judgement.Result = HitResult.Miss;
        }

        private const float playerSpeed = 0.4f;
        private Vector2 positionChange = Vector2.Zero;

        protected override void Update()
        {
            base.Update();

            HitDetect();

            if (savedTime < Time.Current - 1000 && CharacterEnergy < maxEnergy)
                energyAdd();
            playerInput();
            playerMovement();
            VitaruScoreProcessor.PlayerHealth = CharacterHealth / 100;
            VitaruScoreProcessor.PlayerEnergy = CharacterEnergy / 100;
        }

        private void energyAdd()
        {
            CharacterEnergy++;
            savedTime = Time.Current;
        }

        private void playerMovement()
        {
            //Handles Player Speed
            float yTranslationDistance = playerSpeed * (float)(Clock.ElapsedFrameTime);
            float xTranslationDistance = playerSpeed * (float)(Clock.ElapsedFrameTime);

            if (keys[Key.LShift] | keys[Key.RShift])
            {
                xTranslationDistance /= 2;
                yTranslationDistance /= 2;
            }
            if (keys[Key.Up])
            {
                VitaruPlayer.PlayerPosition.Y -= yTranslationDistance;
            }
            if (keys[Key.Left])
            {
                VitaruPlayer.PlayerPosition.X -= xTranslationDistance;
            }
            if (keys[Key.Down])
            {
                VitaruPlayer.PlayerPosition.Y += yTranslationDistance;
            }
            if (keys[Key.Right])
            {
                VitaruPlayer.PlayerPosition.X += xTranslationDistance;
            }

            VitaruPlayer.PlayerPosition = Vector2.ComponentMin(VitaruPlayer.PlayerPosition, playerBounds.Yw);
            VitaruPlayer.PlayerPosition  = Vector2.ComponentMax(VitaruPlayer.PlayerPosition, playerBounds.Xz);
            Position = VitaruPlayer.PlayerPosition;
        }

        private void playerInput()
        {
            if (keys[Key.Z])
            {
                Shooting = true;
            }
            if (keys[Key.Z] == false)
            {
                Shooting = false;
            }
        }

        private void spell()
        {
            if(CharacterEnergy >= healEnergy)
            {
                helper1.Alpha = 1;
                helper2.Alpha = 1;
                helper1.Position = new Vector2(20,-30);
                helper2.Position = new Vector2(-20,-30);
                CharacterSign.Colour = Color4.Red;
                CharacterEnergy = CharacterEnergy - healEnergy;
                if((healEnergy + 5) <= maxEnergy)
                healEnergy = healEnergy + 5;
                CharacterSign.Alpha = 1;
                CharacterHealth = 100;
                CharacterSign.FadeOut(2500 , EasingTypes.InQuint);
            }
        }

        private void shoot()
        {
            helper1.shoot();
            helper2.shoot();
            Wave a;
            VitaruPlayfield.vitaruPlayfield.Add(a = new Wave(Team)
            {
                Origin = Anchor.Centre,
                Depth = 5,
                PatternColor = Color4.Red,
                PatternAngleDegree = 0,
                PatternSpeed = 1f,
                PatternBulletWidth = 8,
                PatternDifficulty = 1,
            });
            a.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, -30), a));
        }

        protected override bool OnKeyDown(InputState state, KeyDownEventArgs args)
        {
            if (args.Key == Key.X)
                spell();
            keys[args.Key] = true;
            if (args.Key == Key.LShift || args.Key == Key.RShift)
                Hitbox.Alpha = 1;
            return base.OnKeyDown(state, args);
        }
        protected override bool OnKeyUp(InputState state, KeyUpEventArgs args)
        {
            keys[args.Key] = false;
            if (args.Key == Key.LShift || args.Key == Key.RShift)
                Hitbox.Alpha = 0;
            return base.OnKeyUp(state, args);
        }
    }
}

using OpenTK;
using OpenTK.Input;
using osu.Framework.Graphics;
using osu.Framework.Input;
using System.Collections.Generic;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;
using OpenTK.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Scoring;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Rulesets.Vitaru.Beatmaps;
using System;
using osu.Framework.Graphics.Sprites;
using osu.Framework.IO.Stores;
using osu.Framework.Graphics.Textures;
using osu.Game.Graphics.Containers;
using osu.Framework.Configuration;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class VitaruPlayer : Container
    {
        public static Vector2 PlayerPosition = new Vector2(0);

        protected Sprite PlayerSprite;
        protected Sprite PlayerKiaiSprite;
        protected Sprite PlayerSign;
        protected Hitbox Hitbox;

        public float DegreesPerSecond = 80;
        public float NormalSize = 200;
        public float SineHeight = 100;
        public float SineSpeed = 0.001f;

        public float CharacterHealth { get; set; } = 100;
        public float CharacterEnergy { get; set; } = 0;
        public int Team { get; set; } = 0; // 0 = Player, 1 = Ememies + Boss(s) in Singleplayer
        public int ProjectileDamage { get; set; }
        public bool Kiai { get; set; }

        public bool Dead = false;

        public static bool AssetsLoaded = false;

        public DrawableBullet DrawableBullet;

        private Dictionary<Key, bool> keys = new Dictionary<Key, bool>();

        private double savedTime = -10000;

        private int healEnergy = 10;
        private int maxEnergy = 100;

        public static ResourceStore<byte[]> VitaruResources;
        public static TextureStore VitaruTextures;

        //(MinX,MaxX,MinY,MaxY)
        private Vector4 playerBounds = new Vector4(0, 512, 0, 820);

        public VitaruPlayer()
        {
            //Gross Key stuff
            keys[Key.Up] = false;
            keys[Key.Right] = false;
            keys[Key.Down] = false;
            keys[Key.Left] = false;
            keys[Key.Z] = false;
            keys[Key.X] = false;
            keys[Key.LShift] = false;
            keys[Key.RShift] = false;
            Alpha = 1;
            Origin = Anchor.Centre;
            Anchor = Anchor.TopLeft;
            Position = PlayerPosition;
        }

        private const float playerSpeed = 0.4f;
        private Vector2 positionChange = Vector2.Zero;
        private float savedTime2;

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        protected override void Update()
        {
            base.Update();

            HitDetect();

            if (Kiai && PlayerSprite.Alpha == 1)
            {
                PlayerKiaiSprite.FadeInFromZero(100);
                PlayerSprite.FadeOutFromOne(100);
            }
            if (!Kiai && PlayerSprite.Alpha == 0)
            {
                PlayerSprite.FadeInFromZero(100);
                PlayerKiaiSprite.FadeOutFromOne(100);
            }

            if (PlayerSign.Alpha > 0)
            {
                PlayerSign.ResizeTo((float)Math.Abs(Math.Sin(Clock.CurrentTime * SineSpeed)) * SineHeight + NormalSize);
                PlayerSign.RotateTo((float)((Clock.CurrentTime / 1000) * DegreesPerSecond));
            }

            if (savedTime <= Time.Current - 1000 && VitaruScoreProcessor.PlayerEnergy < maxEnergy)
                energyAdd();
            playerMovement();
        }

        private void energyAdd()
        {
            VitaruScoreProcessor.PlayerEnergy++;
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
                PlayerPosition.Y -= yTranslationDistance;
            }
            if (keys[Key.Left])
            {
                PlayerPosition.X -= xTranslationDistance;
            }
            if (keys[Key.Down])
            {
                PlayerPosition.Y += yTranslationDistance;
            }
            if (keys[Key.Right])
            {
                PlayerPosition.X += xTranslationDistance;
            }

            if (DrawableBullet.BulletSpeedModifier < 1)
                DrawableBullet.BulletSpeedModifier = (((float)Time.Current - savedTime2) / 2500);

            PlayerPosition = Vector2.ComponentMin(PlayerPosition, playerBounds.Yw);
            PlayerPosition  = Vector2.ComponentMax(PlayerPosition, playerBounds.Xz);
            Position = PlayerPosition;
        }

        private void spell()
        {
            if(VitaruScoreProcessor.PlayerEnergy >= healEnergy)
            {
                savedTime2 = (float)Time.Current;
                DrawableBullet.BulletSpeedModifier = 0;
                PlayerSign.Colour = Color4.Red;
                VitaruScoreProcessor.PlayerEnergy = VitaruScoreProcessor.PlayerEnergy - healEnergy;
                if((healEnergy + 5) <= maxEnergy)
                healEnergy = healEnergy + 5;
                PlayerSign.Alpha = 1;
                VitaruScoreProcessor.PlayerHealth = 100;
                PlayerSign.FadeOut(2500 , EasingTypes.InQuint);
            }
        }

        public void HitDetect()
        {
            if (VitaruPlayfield.vitaruPlayfield != null)
            {
                foreach (Drawable draw in VitaruPlayfield.vitaruPlayfield.Children)
                {
                    if (draw is DrawableBullet)
                    {
                        DrawableBullet = draw as DrawableBullet;
                        if (DrawableBullet.Team != Team)
                        {
                            Vector2 bulletPos = DrawableBullet.ToSpaceOfOtherDrawable(Vector2.Zero, this);
                            float distance = (float)Math.Sqrt(Math.Pow(bulletPos.X, 2) + Math.Pow(bulletPos.Y, 2));
                            float minDist = Hitbox.HitboxWidth + DrawableBullet.BulletWidth;
                            float signDist = ((PlayerSign.Size.Y / 2) - 14) + DrawableBullet.BulletWidth;

                            if (PlayerSign.Alpha > 0f && distance < signDist)
                                DrawableBullet.DeleteBullet();

                            if (distance < minDist && !Dead)
                            {
                                TakeDamage(DrawableBullet.BulletDamage);
                                DrawableBullet.DeleteBullet();
                            }
                        }
                    }
                    /*
                    if (draw is Laser)
                    {
                        Laser laser = draw as Laser;
                        if (laser.Team != Team)
                        {
                            circleDistance.x = abs(circle.x - rect.x);
                            circleDistance.y = abs(circle.y - rect.y);
                        }
                    }
                    */
                }
            }
        }

        /// <summary>
        /// The <see cref="Character"/> gets damaged, with a multiplier of <see cref="DamageMultiplier"/>
        /// </summary>
        /// <param name="damage">Damage without the Resistance applied</param>
        /// <returns>If the Character died</returns>
        public bool TakeDamage(float damage)
        {
            CharacterHealth -= damage;
            if (CharacterHealth <= 0)
            {
                Death();
                return true;
            }
            return false;
        }

        public void Death()
        {

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

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config, TextureStore textures)
        {
            if (!AssetsLoaded)
            {
                AssetsLoaded = true;
                VitaruResources = new ResourceStore<byte[]>();
                VitaruResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Game.Rulesets.Vitaru.dll"), ("Assets")));
                VitaruResources.AddStore(new DllResourceStore("osu.Game.Rulesets.Vitaru.dll"));

                VitaruTextures = new TextureStore(new RawTextureLoaderStore(new NamespacedResourceStore<byte[]>(VitaruResources, @"Textures")));
                VitaruTextures.AddStore(new RawTextureLoaderStore(new OnlineStore()));
            }

            //Drawable stuff loading for each individual Character
            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;
            Children = new Drawable[]
            {
                PlayerSprite = new Sprite()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1,
                },
                PlayerKiaiSprite = new Sprite()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0,
                },
                Hitbox = new Hitbox()
                {
                    Alpha = 0,
                    HitboxWidth = 4,
                    HitboxColor = Color4.Red,
                },
                PlayerSign = new Sprite()
                {
                    Alpha = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Color4.Red,
                },
            };

            PlayerSprite.Texture = VitaruTextures.Get("player");
            PlayerKiaiSprite.Texture = VitaruTextures.Get("playerKiai");
            PlayerSign.Texture = VitaruTextures.Get("playerSign");
        }
    }
}

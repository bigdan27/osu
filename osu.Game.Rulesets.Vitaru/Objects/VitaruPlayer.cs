using OpenTK;
using OpenTK.Input;
using osu.Framework.Graphics;
using osu.Framework.Input;
using System.Collections.Generic;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;
using OpenTK.Graphics;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using osu.Framework.Graphics.Sprites;
using osu.Framework.IO.Stores;
using osu.Framework.Graphics.Textures;
using osu.Framework.Configuration;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Game.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class VitaruPlayer : BeatSyncedContainer
    {
        public static Vector2 PlayerPosition = new Vector2(0);

        public static float PlayerEnergy = 0;
        public static float PlayerHealth = 100;

        protected Sprite PlayerSprite;
        protected Sprite PlayerKiaiSprite;
        protected Sprite PlayerSign;
        protected Hitbox Hitbox;

        public float CharacterHealth { get; set; } = 100;
        public float CharacterEnergy { get; set; } = 0;
        public int Team { get; set; } = 0;
        public int ProjectileDamage { get; set; }
        public bool Kiai { get; set; }

        public static bool AssetsLoaded = false;

        public DrawableBullet DrawableBullet;

        private Dictionary<Key, bool> keys = new Dictionary<Key, bool>();

        private double savedTime = -10000;

        private int healEnergy = 10;
        private int maxEnergy = 100;

        public static ResourceStore<byte[]> VitaruResources;
        public static TextureStore VitaruTextures;
        public static FontStore VitaruFont;
        public static AudioManager VitaruAudio;

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

            if (savedTime <= Time.Current - 1000 && PlayerEnergy < maxEnergy)
                energyAdd();

            if(PlayerSign.Alpha > 0)
                PlayerSign.RotateTo((float)((Clock.CurrentTime / 1000) * 90));

            playerMovement();
        }

        private void energyAdd()
        {
            PlayerEnergy++;
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
            if(PlayerEnergy >= healEnergy && PlayerSign.Alpha <= 0)
            {
                savedTime2 = (float)Time.Current;
                DrawableBullet.BulletSpeedModifier = 0;
                PlayerSign.Colour = Color4.Red;
                PlayerEnergy = PlayerEnergy - healEnergy;
                if((healEnergy + 5) <= maxEnergy)
                healEnergy = healEnergy + 5;
                PlayerHealth = 100;
                PlayerSign.FadeOutFromOne(2500 , EasingTypes.InQuint);
                PlayerSign.Size = new Vector2(300);
                PlayerSign.ResizeTo(new Vector2(50f) , 2500 , EasingTypes.InQuint);
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

                        //yes I may still need this, want to make team battles more interesting ;)
                        if (true)//DrawableBullet.Team != Team)
                        {
                            Vector2 bulletPos = DrawableBullet.ToSpaceOfOtherDrawable(Vector2.Zero, this);
                            float distance = (float)Math.Sqrt(Math.Pow(bulletPos.X, 2) + Math.Pow(bulletPos.Y, 2));
                            float minDist = Hitbox.HitboxWidth + DrawableBullet.BulletWidth;
                            //The -20 is for the blank space around the sprite (transparent pixels)
                            float signDist = ((PlayerSign.Size.Y / 2) - 20) + DrawableBullet.BulletWidth;

                            if (PlayerSign.Alpha > 0f && distance < signDist)
                                DrawableBullet.DeleteBullet();

                            if (distance < minDist)
                            {
                                TakeDamage(DrawableBullet.BulletDamage);
                                DrawableBullet.DeleteBullet();
                            }
                        }
                    }
                    if (draw is DrawableLaser)
                    {
                        DrawableLaser drawableLaser = draw as DrawableLaser;
                        if (true)//drawableLaser.Team != Team)
                        {/*
                            circleDistance.x = abs(circle.x - rect.x);
                            circleDistance.y = abs(circle.y - rect.y);
                        */}
                    }
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
            PlayerHealth -= damage;
            if (PlayerHealth <= 0)
            {
                Death();
                return true;
            }
            return false;
        }

        public void Death()
        {
            Dispose();
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

                VitaruFont = new FontStore(new GlyphStore(VitaruResources, @"Font/vitaruFont"))
                {
                    ScaleAdjust = 100
                };
            }

            //Drawable stuff loading
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

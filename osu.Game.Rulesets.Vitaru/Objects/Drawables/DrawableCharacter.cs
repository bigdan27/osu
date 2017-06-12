using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using System;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Framework.Configuration;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics.Textures;
using osu.Framework.Audio;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public abstract class DrawableCharacter : DrawableVitaruHitObject
    {
        public HitObjectType CharacterType;

        protected Sprite CharacterSprite;
        protected Sprite CharacterKiaiSprite;
        protected Sprite CharacterSign;

        //Sign stuff
        public float DegreesPerSecond = 80;
        public float NormalSize = 200;
        public float SineHeight = 100;
        public float SineSpeed = 0.001f;

        //Generic Character stuff
        public float CharacterHealth { get; set; } = 100;
        public float CharacterEnergy { get; set; } = 0;
        public int Team { get; set; } = 0; // 0 = Player, 1 = Ememies + Boss(s) in Singleplayer
        public int ProjectileDamage { get; set; }

        public int BPM { get; set; } = (200);

        public static ResourceStore<byte[]> VitaruResources;
        public static TextureStore VitaruTextures;

        protected Hitbox Hitbox;

        public bool Dead { get; set; } = false;
        public bool Shooting { get; set; } = false;

        private float timeSaved;
        private double timeSinceLastShoot;

        protected Color4 CharacterColor { get; set; }
        protected float HitboxWidth { get; set; }

        public Action OnDeath { get; set; }
        public Action CharacterShoot { get; set; }

        public static bool AssetsLoaded = false;

        public bool Kiai { get; set; }

        public DrawableCharacter(VitaruHitObject hitObject) : base(hitObject)
        {
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
                OnDeath?.Invoke();
                if(OnDeath == null)
                    Dispose();
                return true;
            }
            return false;
        }

        protected override void Update()
        {
            base.Update();

            if(Kiai && CharacterSprite.Alpha == 1)
            {
                CharacterKiaiSprite.FadeInFromZero(100);
                CharacterSprite.FadeOutFromOne(100);
            }
            if(!Kiai && CharacterSprite.Alpha == 0)
            {
                CharacterSprite.FadeInFromZero(100);
                CharacterKiaiSprite.FadeOutFromOne(100);
            }

            HitDetect();

            if (CharacterSign.Alpha > 0)
            {
                CharacterSign.ResizeTo((float)Math.Abs(Math.Sin(Clock.CurrentTime * SineSpeed)) * SineHeight + NormalSize);
                CharacterSign.RotateTo((float)((Clock.CurrentTime / 1000) * DegreesPerSecond));
            }

            if (Shooting)
            {
                //Shooting every half beat
                timeSinceLastShoot += Clock.ElapsedFrameTime;
                if (timeSinceLastShoot / 1000.0 > 1 / BPM / 30.0)
                {
                    CharacterShoot?.Invoke();
                    timeSinceLastShoot -= 1 / (BPM / 30.0) * 1000.0;
                }
            }
        }

        protected abstract void CharacterHitJudgment();

        public DrawableBullet DrawableBullet;
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
                            float signDist = ((CharacterSign.Size.Y / 2) - 14) + DrawableBullet.BulletWidth;

                            if (CharacterSign.Alpha > 0f && distance < signDist)
                                DrawableBullet.DeleteBullet();

                            if (distance < minDist && !Dead && DrawableBullet.BulletResult == HitResult.None)
                            {
                                TakeDamage(DrawableBullet.BulletDamage);
                                CharacterHitJudgment();
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

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config , TextureStore textures)
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
                CharacterSprite = new Sprite()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1,
                },
                CharacterKiaiSprite = new Sprite()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0,
                },
                Hitbox = new Hitbox()
                {
                    Alpha = 0,
                    HitboxWidth = HitboxWidth,
                    HitboxColor = CharacterColor,
                },
                CharacterSign = new Sprite()
                {
                    Alpha = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
            };

            string characterType = "null";
            switch(CharacterType)
            {
                case HitObjectType.Player:
                    characterType = "player";
                    break;
                case HitObjectType.Enemy:
                    characterType = "enemy";
                    break;
                case HitObjectType.Boss:
                    characterType = "boss";
                    break;
            }

            CharacterSprite.Texture = VitaruTextures.Get(characterType);
            CharacterKiaiSprite.Texture = VitaruTextures.Get(characterType + "Kiai");
            CharacterSign.Texture = VitaruTextures.Get(characterType + "Sign");
        }
    }
}




//Stuff for when lasers are implemented properly, should wait until new hitbox system is ready

/*bool intersects(CircleType circle, RectType rect)
{
    circleDistance.x = abs(circle.x - rect.x);
    circleDistance.y = abs(circle.y - rect.y);

    if (circleDistance.x > (rect.width/2 + circle.r)) { return false; }
    if (circleDistance.y > (rect.height/2 + circle.r)) { return false; }

    if (circleDistance.x <= (rect.width/2)) { return true; } 
    if (circleDistance.y <= (rect.height/2)) { return true; }

    cornerDistance_sq = (circleDistance.x - rect.width/2)^2 +
                         (circleDistance.y - rect.height/2)^2;

    return (cornerDistance_sq <= (circle.r^2));
}*/

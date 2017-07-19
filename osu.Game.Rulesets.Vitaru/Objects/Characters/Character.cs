using OpenTK;
using OpenTK.Input;
using osu.Framework.Graphics;
using osu.Framework.Input;
using System.Collections.Generic;
using OpenTK.Graphics;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Configuration;
using osu.Framework.Allocation;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Framework.Audio.Track;
using osu.Game.Beatmaps.ControlPoints;
using osu.Framework.Graphics.Shapes;
using osu.Framework.IO.Stores;

namespace osu.Game.Rulesets.Vitaru.Objects
{
    public abstract class Character : BeatSyncedContainer
    {
        protected Sprite CharacterSprite;
        protected Sprite CharacterKiaiSprite;
        protected Sprite CharacterSign;
        protected Hitbox Hitbox;
        protected string CharacterName = "null";
        public float CharacterHealth { get; set; } = 100;
        public int Team { get; set; }

        public bool Invincible = false;
        public bool Dead = false;

        public DrawableBullet DrawableBullet;

        public void HitDetect()
        {
            if (VitaruPlayfield.vitaruPlayfield != null && !Dead)
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
                            float minDist = (Hitbox.HitboxWidth - Hitbox.BorderWidth) + (DrawableBullet.BulletWidth - DrawableBullet.BorderWidth);

                            if (distance < minDist && !Invincible)
                            {
                                TakeDamage(DrawableBullet.BulletDamage);
                                DrawableBullet.DeleteBullet(true);
                            }
                        }
                    }
                    if (draw is DrawableLaser)
                    {
                        DrawableLaser drawableLaser = draw as DrawableLaser;
                        if (drawableLaser.Team != Team)
                        {
                         /*
                            circleDistance.x = abs(circle.x - rect.x);
                            circleDistance.y = abs(circle.y - rect.y);
                         */
                        }
                    }
                }
            }
        }

        public bool TakeDamage(float damage)
        {
            CharacterHealth -= damage;

            if (CharacterHealth <= 0)
            {
                CharacterHealth = 0;
                Death();
                return true;
            }
            return false;
        }

        public abstract void Death();

        /// <summary>
        /// Child loading for all Characters (Enemies, Player, Bosses)
        /// </summary>
        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config, TextureStore textures)
        {
            //Drawable stuff loading
            Origin = Anchor.Centre;
            Anchor = Anchor.TopLeft;
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
                    HitboxWidth = 4,
                    HitboxColor = Color4.Red,
                },
                CharacterSign = new Sprite()
                {
                    Alpha = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Color4.Red,
                },
            };

            if (!VitaruRuleset.AssetsLoaded)
            {
                VitaruRuleset.AssetsLoaded = true;
                VitaruRuleset.VitaruResources = new ResourceStore<byte[]>();
                VitaruRuleset.VitaruResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Game.Rulesets.Vitaru.dll"), ("Assets")));
                VitaruRuleset.VitaruResources.AddStore(new DllResourceStore("osu.Game.Rulesets.Vitaru.dll"));

                VitaruRuleset.VitaruTextures = new TextureStore(new RawTextureLoaderStore(new NamespacedResourceStore<byte[]>(VitaruRuleset.VitaruResources, @"Textures")));
                VitaruRuleset.VitaruTextures.AddStore(new RawTextureLoaderStore(new OnlineStore()));

                VitaruRuleset.VitaruFont = new FontStore(new GlyphStore(VitaruRuleset.VitaruResources, @"Font/vitaruFont"))
                {
                    ScaleAdjust = 100
                };
            }

            CharacterSprite.Texture = VitaruRuleset.VitaruTextures.Get(CharacterName);
            CharacterKiaiSprite.Texture = VitaruRuleset.VitaruTextures.Get(CharacterName + "Kiai");
            CharacterSign.Texture = VitaruRuleset.VitaruTextures.Get(CharacterName + "Sign");
        }
    }
}

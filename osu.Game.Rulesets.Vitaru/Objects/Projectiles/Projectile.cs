using System;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.Objects.Projectiles
{
    public class Projectile : VitaruHitObject
    {
        public int Team { get; set;}
        public override HitObjectType Type => HitObjectType.Projectile;
    }
}

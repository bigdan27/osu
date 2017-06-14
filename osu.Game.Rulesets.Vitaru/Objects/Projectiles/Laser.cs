using System;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;

namespace osu.Game.Rulesets.Vitaru.Objects.Projectiles
{
    public class Laser : VitaruHitObject
    {
        public override HitObjectType Type => HitObjectType.Laser;
    }
}

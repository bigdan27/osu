// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

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

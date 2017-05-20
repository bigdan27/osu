// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Graphics.Containers;
using OpenTK;
using OpenTK.Input;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Framework.Graphics;
using osu.Framework.Input;
using System.Collections.Generic;
using osu.Game.Rulesets.Vitaru.Objects.Projectiles;
using OpenTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Objects.Characters
{
    public class VitaruPlayer : Character
    {
        public VitaruPlayer() : base() { }
        public double EndTime { get; set; }
        public object Sample { get; internal set; }

        public static Vector2 PlayerPosition;

        public override HitObjectType Type => HitObjectType.Player;
    }
}
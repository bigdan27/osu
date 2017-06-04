// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Graphics.Containers;
using OpenTK;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Framework.Graphics;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Vitaru.Objects.Characters
{
    public class Boss : Character
    {
        public Boss() { }
        public override HitObjectType Type => HitObjectType.Boss;
    }
}
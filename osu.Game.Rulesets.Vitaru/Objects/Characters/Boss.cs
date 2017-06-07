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
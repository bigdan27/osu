using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using OpenTK.Graphics;
using OpenTK;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Extensions.Color4Extensions;

namespace osu.Game.Rulesets.Vitaru.Objects
{
    public class AccurateHitbox : Container
    {
        //Different stats for Hitboxes
        public Color4 AccurateHitboxColor { get; set; } = Color4.White;
        public float AccurateHitboxHealth { get; set; } = 100;

        //
        public float AccurateHitboxTestWidth { get; set; } = 8f;
        public Sprite HitboxSprite;

        //private Container accurateHitboxContainer;
        
        public AccurateHitbox()
        {
        }

        protected override void LoadComplete()
        {
            Children = new Drawable[]
            {

            };
        }
    }
}

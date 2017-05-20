using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Objects.Characters;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableVitaruBoss : DrawableCharacter
    {
        private readonly Boss boss;

        public DrawableVitaruBoss(Boss boss) : base(boss)
        {
            this.boss = boss;
            Anchor = Anchor.TopCentre;
            Position = boss.Position;
            CharacterType = HitObjectType.Boss;
            CharacterHealth = 1000;
            Team = 1;
            HitboxColor = Color4.Green;
            HitboxWidth = 32;
        }

        protected override void Update()
        {
            base.Update();

            HitDetect();

            float ySpeed = Speed.Y * (float)Clock.ElapsedFrameTime;
            float xSpeed = Speed.X * (float)Clock.ElapsedFrameTime;
        }
    }
}

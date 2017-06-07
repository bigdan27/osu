using System;
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
            CharacterColor = Color4.Green;
            HitboxWidth = 32;
        }

        protected override void CharacterHitJudgment()
        {
            throw new NotImplementedException();
        }

        protected override void Update()
        {
            base.Update();

            HitDetect();
        }
    }
}

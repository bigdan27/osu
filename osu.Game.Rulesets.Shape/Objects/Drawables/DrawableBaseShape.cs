using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.MathUtils;
using osu.Game.Rulesets.Shape.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Shape.Objects.Drawables
{
    public class DrawableBaseShape : DrawableShapeHitObject
    {
        private BaseDial baseDial;
        private ShapeCircle circle;
        private ShapeSquare square;
        private ShapeTriangle triangle;
        private ShapeX x;

        private readonly BaseShape shape;
        public DrawableBaseShape(BaseShape Shape) : base(Shape)
        {
            shape = Shape;
            Position = shape.StartPosition;
            Alpha = 0;
            AlwaysPresent = true;
            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        protected override void CheckJudgement(bool userTriggered)
        {
            base.CheckJudgement(userTriggered);

            if(Time.Current >= shape.StartTime)
            {
                Judgement.Result = HitResult.Hit;
                Judgement.Score = ShapeScoreResult.Hit300;
                Dispose();
            }

        }

        protected override void Update()
        {
            base.Update();
            if(Time.Current >= (shape.StartTime - TIME_PREEMPT) - 500 && !loaded)
            {
                preLoad();
            }

            if(Time.Current >= shape.StartTime - TIME_PREEMPT && !started)
            {
                start();
            }
            if(Time.Current >= shape.StartTime)
            {
                //Dispose();
            }
        }

        private bool loaded = false;
        private bool started = false;

        private void preLoad()
        {
            loaded = true;
            switch (shape.ShapeID)
            {
                case 1:
                    Children = new Drawable[]
                    {
                        baseDial = new BaseDial(shape)
                        {
                            Depth = -1,
                            ShapeID = shape.ShapeID,
                        },
                        circle = new ShapeCircle(shape) { Depth = -2, Colour = Color4.Red, },
                    };
                    break;
                case 2:
                    Children = new Drawable[]
                    {
                        baseDial = new BaseDial(shape)
                        {
                            Depth = -1,
                            ShapeID = shape.ShapeID,
                        },
                        square = new ShapeSquare(shape) { Depth = -2, Colour = Color4.Violet, },
                    };
                    break;
                case 3:
                    Children = new Drawable[]
                    {
                        baseDial = new BaseDial(shape)
                        {
                            Depth = -1,
                            ShapeID = shape.ShapeID,
                        },
                        triangle = new ShapeTriangle(shape) { Depth = -2, Colour = Color4.Green, },
                    };
                    break;
                case 4:
                    Children = new Drawable[]
                    {
                        baseDial = new BaseDial(shape)
                        {
                            Depth = -1,
                            ShapeID = shape.ShapeID,
                        },
                        x = new ShapeX(shape) { Depth = -2, Colour = Color4.Blue, },
                    };
                    break;
            }
        }

        private void start()
        {
            started = true;
            this.FadeIn(TIME_FADEIN);
            baseDial.StartSpinning(TIME_PREEMPT);
            switch (shape.ShapeID)
            {
                case 1:
                    circle.Position = new Vector2(RNG.Next(-200, 200), -400);
                    circle.MoveTo(baseDial.Position, TIME_PREEMPT);
                    break;
                case 2:
                    square.Position = new Vector2(RNG.Next(-200, 200), -400);
                    square.MoveTo(baseDial.Position, TIME_PREEMPT);
                    break;
                case 3:
                    triangle.Position = new Vector2(RNG.Next(-200, 200), -400);
                    triangle.MoveTo(baseDial.Position, TIME_PREEMPT);
                    break;
                case 4:
                    x.Position = new Vector2(RNG.Next(-200, 200), -400);
                    x.MoveTo(baseDial.Position, TIME_PREEMPT);
                    break;
            }
        }
    }
}

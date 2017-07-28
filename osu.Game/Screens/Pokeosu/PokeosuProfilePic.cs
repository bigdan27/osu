using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Game.Graphics;
using osu.Game.Online.API;
using osu.Game.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu.Game.Screens.Pokeosu
{
    public class PokeosuProfilePic : Container, IOnlineComponent
    {
        private Container dragContainer;

        private FillFlowContainer user;
        private UpdateableAvatar avatar;
        private TextAwesome drawableIcon;

        protected override bool OnDragStart(InputState state) => true;

        protected override bool OnDrag(InputState state)
        {
            Trace.Assert(state.Mouse.PositionMouseDown != null, "state.Mouse.PositionMouseDown != null");

            Vector2 change = state.Mouse.Position - state.Mouse.PositionMouseDown.Value;

            // Diminish the drag distance as we go further to simulate "rubber band" feeling.
            change *= change.Length <= 0 ? 0 : (float)Math.Pow(change.Length, 0.7f) / change.Length;

            dragContainer.MoveTo(change);
            return base.OnDrag(state);
        }

        protected override bool OnDragEnd(InputState state)
        {
            dragContainer.MoveTo(Vector2.Zero, 800, EasingTypes.OutElastic);
            return base.OnDragEnd(state);
        }

        [BackgroundDependencyLoader]
        private void load(APIAccess api)
        {
            api.Register(this);

            Children = new Drawable[]
            {
                dragContainer = new Container
                {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                               user = new FillFlowContainer
                               {
                                   Anchor = Anchor.Centre,
                                   Origin = Anchor.Centre,
                                   Size = new Vector2(140),
                                   AutoSizeAxes = Axes.X,
                                   Children = new Drawable[]
                                   {
                                       drawableIcon = new TextAwesome
                                       {
                                           Anchor = Anchor.Centre,
                                           Origin = Anchor.Centre,
                                           TextSize = 100
                                       },
                                   },
                               },
                           }
                      }
            };
            user.Add(avatar = new UpdateableAvatar
            {
                Masking = true,
                Size = new Vector2(140),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                CornerRadius = (16),
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Shadow,
                    Radius = 8,
                    Colour = Color4.Black.Opacity(0.1f),
                }
            });
        }
        public void APIStateChanged(APIAccess api, APIState state)
        {
            switch (state)
            {
                default:
                    avatar.User = new User();
                    break;
                case APIState.Online:
                    avatar.User = api.LocalUser;
                    break;
            }
        }
    }
}

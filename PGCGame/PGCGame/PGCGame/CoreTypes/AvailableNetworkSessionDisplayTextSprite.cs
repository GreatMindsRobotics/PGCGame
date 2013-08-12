using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;

namespace PGCGame.CoreTypes
{
    public class AvailableNetworkSessionDisplayTextSprite : TextSprite
    {
        public AvailableNetworkSession Session;

        public MultiplayerSessionType SessionType;

        public AvailableNetworkSessionDisplayTextSprite(SpriteBatch sb, float prevY, AvailableNetworkSession sessToRepresent)
            : base(sb, new Vector2(0, prevY + GameContent.GameAssets.Fonts.NormalText.LineSpacing * 2 + 5), GameContent.GameAssets.Fonts.NormalText, string.Format("{0}'s {3} session:\n{1} out of {2} gamers", sessToRepresent.HostGamertag, sessToRepresent.CurrentGamerCount, sessToRepresent.CurrentGamerCount + sessToRepresent.OpenPrivateGamerSlots + sessToRepresent.OpenPublicGamerSlots, "{0}"), Color.White)
        {
            SessionType = Enum.Parse(typeof(MultiplayerSessionType), sessToRepresent.SessionProperties[(int)NetworkSessionPropertyType.SessionType].ToString(), true).Cast<MultiplayerSessionType>();
            Text = string.Format(Text, SessionType.ToFriendlyString());
            Session = sessToRepresent;
            HoverColor = Color.MediumAquamarine;
            NonHoverColor = Color.White;
            IsHoverable = true;
            X = this.GetCenterPosition(sb.GraphicsDevice.Viewport).X;
        }
    }
}

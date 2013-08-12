using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;
using Glib.XNA.InputLib;
namespace PGCGame.Screens.Multiplayer
{
    public class WarnScreen : BaseScreen
    {
        public WarnScreen(SpriteBatch sb)
            : base(sb, Color.Black)
        {

        }

        TextSprite WarnLabel;
        TextSprite DetailedWarnLabel;
        Sprite YesButton;
        TextSprite YesLabel;
        Sprite NoButton;
        TextSprite NoLabel;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);

            WarnLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.BoldText, "Not all players are ready", Color.Red);
            WarnLabel.Y = 5;
            WarnLabel.X = WarnLabel.GetCenterPosition(Graphics.Viewport).X;

            DetailedWarnLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "Are you sure you would like to start the session?", Color.White);
            DetailedWarnLabel.Position = DetailedWarnLabel.GetCenterPosition(Graphics.Viewport);

            AdditionalSprites.Add(WarnLabel);
            AdditionalSprites.Add(DetailedWarnLabel);

            YesButton = new Sprite(GameContent.GameAssets.Images.Controls.Button, new Vector2(Graphics.Viewport.Width, Graphics.Viewport.Height), Sprites.SpriteBatch);
            YesLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "Yes") { ParentSprite = YesButton, IsHoverable = true, HoverColor = Color.MediumAquamarine, NonHoverColor = Color.White };
            YesButton.Y -= YesButton.Height + 20;
            YesButton.X -= YesButton.Width + 20;
            YesLabel.Pressed += new EventHandler(YesLabel_Pressed);
            
            Sprites.Add(YesButton);
            AdditionalSprites.Add(YesLabel);

            NoButton = new Sprite(GameContent.GameAssets.Images.Controls.Button, new Vector2(20, Graphics.Viewport.Height), Sprites.SpriteBatch);
            NoLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "No") { ParentSprite = NoButton, IsHoverable = true, HoverColor = Color.MediumAquamarine, NonHoverColor = Color.White };
            NoButton.Y -= NoButton.Height + 20;
            NoLabel.Pressed += new EventHandler(NoLabel_Pressed);

            Sprites.Add(NoButton);
            AdditionalSprites.Add(NoLabel);
        }

        void NoLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.ScreenState = CoreTypes.ScreenType.NetworkLobbyScreen;
        }

        void YesLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.NetworkData.CurrentSession.StartGame();
            //TODO Screen Switch
        }
    }
}

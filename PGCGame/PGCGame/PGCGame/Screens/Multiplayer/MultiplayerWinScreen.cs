using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib.XNA.SpriteLib;
using Glib.XNA;
using Glib;

namespace PGCGame.Screens.Multiplayer
{
    public class MultiplayerWinScreen : BaseScreen
    {
        public MultiplayerWinScreen(SpriteBatch sb) : base(sb, Color.Black) { }

        TextSprite title;
        TextSprite msg;
        Sprite planet;



        public override void InitScreen(ScreenType screenName)
        {
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
            StateManager.ScreenResolutionChanged += new EventHandler<ViewportEventArgs>(Options_ScreenResolutionChanged);

            base.InitScreen(screenName);
            BackgroundSprite = GameContent.Assets.Images.NonPlayingObjects.GlobalBackground;
            title = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.BoldText, "You Won!", Color.White);
            title.X = title.GetCenterPosition(Graphics.Viewport).X;
            title.Y = 12.5f;
            AdditionalSprites.Add(title);
            planet = Sprites.AddNewSprite(new Vector2(-3, Graphics.Viewport.Height - GameContent.Assets.Images.NonPlayingObjects.Planet.Height + 11), GameContent.Assets.Images.NonPlayingObjects.Planet);
            msg = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.NormalText, MessageText, Color.White);
            msg.Y = title.Y + title.Font.LineSpacing;
            AdditionalSprites.Add(msg);
        }

        const string MessageText = "Congratulations, {0}! You won the match!";

        void Options_ScreenResolutionChanged(object sender, ViewportEventArgs e)
        {
            title.X = title.GetCenterPosition(Graphics.Viewport).X;
        }

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                msg.Text = string.Format(msg.Text, StateManager.NetworkData.CurrentSession.LocalGamers[0].Gamertag);
                msg.X = msg.GetCenterPosition(Graphics.Viewport).X;
            }
            else
            {
                msg.Text = MessageText;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}

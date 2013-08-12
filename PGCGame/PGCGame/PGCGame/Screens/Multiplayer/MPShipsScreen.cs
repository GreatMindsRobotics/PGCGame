using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib.XNA.SpriteLib;
using Glib.XNA;

namespace PGCGame.Screens.Multiplayer
{
    public class MPShipsScreen : BaseScreen
    {
        public MPShipsScreen(SpriteBatch sb) : base(sb, Color.Black)
        {
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
        }

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (_firstInit && StateManager.ScreenState == this.ScreenType)
            {
                title.Text = StateManager.NetworkData.SessionMode.ToFriendlyString() + " Lobby";
                char[] allChars = title.Text.ToCharArray();
                allChars[0] = char.ToUpper(allChars[0]);
                title.Text = new string(allChars);
                title.X = title.GetCenterPosition(Graphics.Viewport).X;

                _firstInit = false;
            }
        }

        

        private bool _firstInit = true;
        TextSprite title;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            title = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.BoldText, StateManager.NetworkData.SessionMode.ToFriendlyString()+" Lobby", Color.White);
            title.Y = 5;
            title.X = title.GetCenterPosition(Graphics.Viewport).X;
            AdditionalSprites.Add(title);
        }
    }
}

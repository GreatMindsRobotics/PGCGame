﻿using System;
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

namespace PGCGame.Screens.Multiplayer
{
    public class LobbyScreen : BaseScreen
    {
        public LobbyScreen(SpriteBatch sb)
            : base(sb, Color.Black)
        {

        }

        TextSprite title;
        TextSprite[] allGamerInfos;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            title = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.BoldText, "Lobby", Color.White);
            title.Position = new Vector2(title.GetCenterPosition(Graphics.Viewport).X, 5);
            AdditionalSprites.Add(title);

            
        }

        public void InitScreen()
        {
            StateManager.NetworkData.CurrentSession.GamerJoined += new EventHandler<Microsoft.Xna.Framework.Net.GamerJoinedEventArgs>(CurrentSession_GamerJoined);
            gamersInSession.AddRange(StateManager.NetworkData.CurrentSession.AllGamers);
            
        }

        void gamerInfo_TextChanged(object sender, EventArgs e)
        {
            TextSprite gamerInfo = sender as TextSprite;
            gamerInfo.X = gamerInfo.GetCenterPosition(Graphics.Viewport).X;
        }

        void CurrentSession_GamerJoined(object sender, Microsoft.Xna.Framework.Net.GamerJoinedEventArgs e)
        {
            gamersInSession.Add(e.Gamer);

            float y = title.Y + title.Font.LineSpacing;
            allGamerInfos = new TextSprite[StateManager.NetworkData.CurrentSession.MaxGamers];
            for (int i = 0; i < StateManager.NetworkData.CurrentSession.MaxGamers; i++)
            {
                TextSprite gamerInfo = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "Gamer", Color.White);
                gamerInfo.Visible = allGamerInfos[i] != null;
                gamerInfo.X = gamerInfo.GetCenterPosition(Graphics.Viewport).X;
                gamerInfo.Y = y + 5;
                gamerInfo.TextChanged += new EventHandler(gamerInfo_TextChanged);
                y += gamerInfo.Height + 5;
                allGamerInfos[i] = gamerInfo;
                AdditionalSprites.Add(gamerInfo);
            }

            allGamerInfos[gamersInSession.Count - 1].Text = e.Gamer.Gamertag;
            allGamerInfos[gamersInSession.Count - 1].Visible = true;
        }

        readonly List<Gamer> gamersInSession = new List<Gamer>();

        public void Update(GameTime gameTime)
        {

        }
    }
}
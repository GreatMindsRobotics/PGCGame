﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Glib.XNA.SpriteLib;
using Glib.XNA;
using Glib;
using Microsoft.Xna.Framework.Media;

namespace PGCGame.Screens
{
    public class Credits : Screen
    {
        //TODO FOR MATTHEW:
        //MAKE THE CREDITS END AND GO BACK TO THE MAINMENU
        private TextSprite credits;
        private Song _creditsSong;
        private Vector2 _scrollingSpeed;

        private TimeSpan _timeUntilCreditsFinish = new TimeSpan (0, 0, 0, 45, 0);
        private TimeSpan _elapsedTime;


        public Credits(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            
        }

        public void LoadContent(ContentManager content)
        {
            _scrollingSpeed = new Vector2(0, -1);
            
            credits = new TextSprite(Sprites.SpriteBatch, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "   Plequarius: Galactic Commanders\n\n\n\n\n\nWeek 1 - Functional Spec, GameState Management\n\n\n\n- Kai F.\n\n\n- Michael K.\n\n\n- Alexa L.\n\n\n- Andrea L.\n\n\n- Alexander L.\n\n\n- Matthew P.\n\n\n- Jeremiah T.\n\n\n\n\nWeek 2 - Technical Spec, Class Design, Functionality\n\n\n\n- Glen H.\n\n\n- Michael K.\n\n\n- Alex L.\n\n\n- Matthew P.\n\n\n\n\n\n\n Week 3 - AI's\n\n\n\n\n\n\n\n\n\n\n\nWeek 4 - Xbox Converson\n\n\n\n\nUnderlying Library written by:\nGlen Husman (glen3b)\nGlib is available on github! \n\n\n\n\n\n\n                Music:\n\nFailing Defense - Kevin MacLeod\n\nAll music obtained from Incompetech.com", Color.White);

            credits.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .255f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height);

            _creditsSong = content.Load<Song>("Songs\\Failing Defense");

            AdditionalSprites.Add(credits);
            _elapsedTime = new TimeSpan();
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;
            base.Update(gameTime);
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(_creditsSong);
            }
            credits.Position += _scrollingSpeed;

            if (_elapsedTime >= _timeUntilCreditsFinish)
            {
                MediaPlayer.Stop();
                StateManager.ScreenState = ScreenState.Title;
            }

            //TODO: UPDATE SPRITES
        }
    }
}

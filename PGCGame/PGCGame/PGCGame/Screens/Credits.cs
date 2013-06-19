using System;
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
        private TextSprite credits;

        private Song _creditsSong;

        private Vector2 _scrollingSpeed;


        public Credits(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            
        }

        public void LoadContent(ContentManager content)
        {

            _scrollingSpeed = new Vector2(0, -1);
            //TODO: LOAD CONTENT
            
            //use Sprites to load your sprites
            //EX: Sprites.Add(new Sprite(content.Load<Texture2D>("assetName"), new Vector2(0, 0), Sprites.SpriteBatch));
            //OR
            //EX: Sprites.AddNewSprite(new Vector(0, 0), content.Load<Texture2D("assetName"));
            credits = new TextSprite(Sprites.SpriteBatch, content.Load<SpriteFont>("CreditsFont"), "   Plequarius: Galactic Commanders\n\n\n\n\n\nWeek 1 - Functional Spec, GameState Management\n\n\n\n- Kai F.\n\n\n- Michael K.\n\n\n- Alexa L.\n\n\n- Andrea L.\n\n\n- Alexander L.\n\n\n- Matthew P.\n\n\n- Jeremiah T.\n\n\n\n\nWeek 2 - Technical Spec, Class Design, Functionality\n\n\n\n- Glen H.\n\n\n- Michael K.\n\n\n- Alex L.\n\n\n- Matthew P.\n\n\n\n\n\n\n Week 3 - AI's\n\n\n\n\n\n\n\n\n\n\n\nWeek 4 - Xbox Converson\n\n\n\n\nUnderlying Library written by:\nGlen Husman (glen3b)\nGlib is available on github! \n\n\n\n\n\n\n                Music:\n\nFailing Defense - Kevin MacLeod\n\nAll music obtained from Incompetech.com", Color.White);

            credits.Position = new Vector2(200, 600);

            _creditsSong = content.Load<Song>("Failing Defense");

            AdditionalSprites.Add(credits);
            

            
              

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(_creditsSong);
            }
            credits.Position += _scrollingSpeed;
            //TODO: UPDATE SPRITES
        }
    }
}

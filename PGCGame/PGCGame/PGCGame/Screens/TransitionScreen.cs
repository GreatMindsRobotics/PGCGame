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
using PGCGame.Screens;
namespace PGCGame
{
    public class TransitionScreen : BaseScreen
    {
        Sprite planet;

        public TransitionScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.White)
        {

        }

        public override void InitScreen(ScreenType screenName)
        { 
            base.InitScreen(screenName);




            Sprites.AddNewSprite(Vector2.Zero, GameContent.GameAssets.Images.NonPlayingObjects.Planet3);
            Sprites[0].Scale = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width / (float)Sprites[0].Texture.Width, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / (float)Sprites[0].Texture.Height);
            Sprites[0].Position = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width - (float)Sprites[0].Texture.Width/2, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height /2);


            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            Sprites.AddNewSprite(Vector2.Zero, GameContent.GameAssets.Images.NonPlayingObjects.ShopBackground);
            Sprites[1].Scale = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width / (float)Sprites[1].Texture.Width, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / (float)Sprites[1].Texture.Height);
            Sprites[1].YSpeed = -2;
           

             
       }
    }
}
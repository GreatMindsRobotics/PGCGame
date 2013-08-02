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
   Sprite ship;

   bool isfirstUpdate = true;
        public TransitionScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.White)
        {
            
        }

        private void setupTitleShip()
        {

        }

        public override void InitScreen(ScreenType screenName)
        { 


            base.InitScreen(screenName);


        }

        public override void Update(GameTime gameTime)
        {
            if (!StateManager.IsWindowFocused())
            {
                //Not active window
                return;
            }

            if (isfirstUpdate)
            {
                ship = new Sprite(GameContent.GameAssets.Images.Ships[StateManager.SelectedShip, StateManager.SelectedTier], Vector2.Zero, Sprites.SpriteBatch);



                ship.Position = new Vector2(-ship.Texture.Width / 2, Graphics.Viewport.Height);
                ship.Scale = new Vector2(0.8f);
                ship.XSpeed = 1.5f;
                ship.YSpeed = -ship.XSpeed * .8f;
                ship.Rotation.Degrees = 0;


                Sprites.AddNewSprite(Vector2.Zero, GameContent.GameAssets.Images.NonPlayingObjects.Planet3);
                Sprites[0].Scale = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width / (float)Sprites[0].Texture.Width, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / (float)Sprites[0].Texture.Height);
                Sprites[0].Position = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width - (float)Sprites[0].Texture.Width / 2, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / 2 - 200);


                this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
                Sprites.AddNewSprite(Vector2.Zero, GameContent.GameAssets.Images.NonPlayingObjects.ShopBackground);
                Sprites[1].Scale = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width / (float)Sprites[1].Texture.Width, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / (float)Sprites[1].Texture.Height);
                Sprites[1].YSpeed = -2;

                if (ship.Position.X < Graphics.Viewport.Width * 3)
                {
                    if (ship.Rotation.Degrees <= 90)
                    {
                        ship.Rotation.Radians = (new Vector2(Graphics.Viewport.Width / 2, Graphics.Viewport.Height / 2) - ship.Position).ToAngle();
                        ship.YSpeed -= .0008f;
                        ship.Scale.X -= .001f;
                        ship.Scale.Y -= .001f;
                    }
                    else
                    {
                        ship.XSpeed += .1f;
                        ship.YSpeed = 0;
                        if (ship.Scale.X >= .005f)
                        {
                            ship.Scale.X -= .003f;
                            ship.Scale.Y -= .003f;
                        }
                    }

                }
                Sprites.Add(ship);
                isfirstUpdate = false;
            }


            if (ship.Position.X < Graphics.Viewport.Width * 3)
            {
                if (ship.Rotation.Degrees <= 90)
                {
                    ship.Rotation.Radians = (new Vector2(Graphics.Viewport.Width / 2, Graphics.Viewport.Height / 2) - ship.Position).ToAngle();
                    ship.YSpeed -= .0008f;
                    ship.Scale.X -= .001f;
                    ship.Scale.Y -= .001f;
                }
                else
                {
                    ship.XSpeed += .1f;
                    ship.YSpeed = 0;
                    if (ship.Scale.X >= .005f)
                    {
                        ship.Scale.X -= .003f;
                        ship.Scale.Y -= .003f;
                    }
                }
            }

            if (ship.Position.X > Graphics.Viewport.Width)
            {
                StateManager.ScreenState = CoreTypes.ScreenType.Game;
            }

              


            
#if XBOX
            ButtonManagement.Update(gameTime);
#endif
            base.Update(gameTime);
        }

             
       }
    }

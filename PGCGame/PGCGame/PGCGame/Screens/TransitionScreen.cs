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
using Glib.XNA.InputLib;
using PGCGame.CoreTypes.Utilites;

namespace PGCGame 
{
    public class TransitionScreen : BaseScreen
    {

        public override MusicBehaviour Music
        {
            get { return MusicBehaviour.NoMusic; }
        }

        Ship_Sprite ship;
        bool hasPlayedSound = false;

        public TransitionScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.White)
        {
            DeploySound = GameContent.Assets.Sound[SoundEffectType.SpaceDoorOpening];
            SpaceShipLeaving = GameContent.Assets.Sound[SoundEffectType.SpaceShipLeaving];
            KeyboardManager.KeyDown += new SingleKeyEventHandler(KeyboardManager_KeyDown);

#if WINDOWS
            MouseManager.Updated += new EventHandler(MouseManager_Updated);
#elif XBOX
            GamePadManager.One.Buttons.AButtonPressed += new EventHandler(Buttons_AButtonPressed);
#endif
        }

#if XBOX
        void Buttons_AButtonPressed(object sender, EventArgs e)
        {
            if (StateManager.ScreenState == this.ScreenType)
            {
                Skip();
            }
        }
#endif


#if WINDOWS
        void MouseManager_Updated(object sender, EventArgs e)
        {
            if (StateManager.ScreenState == this.ScreenType && MouseManager.LastMouseState.LeftButton == ButtonState.Released && MouseManager.CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                Skip();
            }
        }
#endif

        private void Skip()
        {
            DeploySound.Stop();
            StateManager.InitializeSingleplayerGameScreen(StateManager.SelectedShip, StateManager.SelectedTier);

            StateManager.ScreenState = CoreTypes.ScreenType.Game;
        }

        void KeyboardManager_KeyDown(object source, SingleKeyEventArgs e)
        {
            if (StateManager.ScreenState == this.ScreenType && (e.Key == Keys.Space || e.Key == Keys.Enter))
            {
                Skip();
            }
        }

        public void Reset()
        {
        
        }
        private void setupTitleShip()
        {

        }
        public static Texture2D planetTexture;

        public override void InitScreen(ScreenType screenName)
        {
            Shop.levelBegin += new EventHandler(Shop_levelBegin);
            
            base.InitScreen(screenName);

        }

        void Shop_levelBegin(object sender, EventArgs e)
        {
            Sprites.Clear();
            hasPlayedSound = false;
            ship = new Ship_Sprite(GameContent.Assets.Images.Ships[StateManager.SelectedShip, StateManager.SelectedTier], Vector2.Zero, Sprites.SpriteBatch);

            ship.Position = new Vector2(-ship.Texture.Width / 2, Graphics.Viewport.Height);
            ship.Scale = new Vector2(0.8f);
            ship.XSpeed = 1.5f;
            ship.YSpeed = -ship.XSpeed * .8f;
            ship.Rotation.Degrees = 0;

            Sprites.Add(ship);

            Sprites.AddNewSprite(Vector2.Zero, planetTexture);
            Sprites[1].Position = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width - (float)Sprites[1].Texture.Width / 2, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / 2 - 70);


            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            Sprites.AddNewSprite(Vector2.Zero, GameContent.Assets.Images.NonPlayingObjects.ShopBackground);
            Sprites[2].Scale = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width / (float)Sprites[2].Texture.Width, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / (float)Sprites[2].Texture.Height);
            Sprites[2].YSpeed = -2;

            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            Sprites.AddNewSprite(Vector2.Zero, GameContent.Assets.Images.NonPlayingObjects.ShopBackground);
            Sprites[3].Scale = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width / (float)Sprites[3].Texture.Width, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / (float)Sprites[3].Texture.Height);
            Sprites[3].YSpeed = -2;

                if (StateManager.Options.SFXEnabled)
                {
                    DeploySound.Play();
                }

            }



        

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            ship.Scale = new Vector2(3);
        }

        public override void Update(GameTime gameTime)
        {
                if (!StateManager.IsWindowFocused())
                {
                    //Not active window
                    return;
                }

                if (ship.Position.X < Graphics.Viewport.Width * 3)
                {
                    if (ship.Rotation.Degrees <= 90)
                    {
                        ship.Rotation.Radians = (new Vector2(Graphics.Viewport.Width / 2, Graphics.Viewport.Height / 2) - ship.Position).ToAngle();
                        ship.YSpeed -= .0008f;
                        ship.Scale -= new Vector2(.001f);
                    }
                    else
                    {
                        ship.XSpeed += .1f;
                        ship.YSpeed = 0;
                        if (ship.Scale.X >= .005f)
                        {
                            ship.Scale -= new Vector2(.003f);
                        }
                    }
                    if (StateManager.Options.SFXEnabled && ship.Rotation.Degrees > 75 && !hasPlayedSound)
                    {
                        
                        SpaceShipLeaving.Play();
                        hasPlayedSound = true;
                    }
                }

                if (ship.Position.X > Graphics.Viewport.Width)
                {
                    StateManager.InitializeSingleplayerGameScreen(StateManager.SelectedShip, StateManager.SelectedTier);

                    StateManager.ScreenState = CoreTypes.ScreenType.Game;
                }

                base.Update(gameTime);
                
            }

        }
    }


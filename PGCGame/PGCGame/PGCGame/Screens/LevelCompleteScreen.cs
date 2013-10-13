using System;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PGCGame.CoreTypes;
using Glib.XNA.InputLib;

namespace PGCGame
{
    public class LevelCompleteScreen : CoreTypes.BaseScreen
    {
        public override MusicBehaviour Music
        {
            get { return _music; }
        }

        private readonly MusicBehaviour _music = new MusicBehaviour();

        public LevelCompleteScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.White)
        {

            ButtonClick = GameContent.Assets.Sound[SoundEffectType.ButtonPressed];

        }
        TextSprite winText;
        Sprite ship;
        bool setWinText = false;

#if XBOX
        GamePadButtonEnumerator allButton;
#endif

        public Texture2D planetTexture;
        public override void InitScreen(ScreenType screenName)
        {
            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;


            StateManager.levelCompleted += new EventHandler(StateManager_levelCompleted);
            ship = new Sprite(GameContent.Assets.Images.Ships[ShipType.FighterCarrier, ShipTier.Tier1], Vector2.Zero, Sprites.SpriteBatch);

            ship.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / 2 - 70);
            ship.Scale = new Vector2(0);
            ship.XSpeed = -3.5f;
            ship.YSpeed = -ship.XSpeed * .8f;
            ship.Rotation.Degrees = -90;



            planetTexture = GameContent.Assets.Images.NonPlayingObjects.Planet;
            Sprites.AddNewSprite(Vector2.Zero, planetTexture);
            Sprites.Add(ship);
            Sprites[0].Position = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width - (float)Sprites[0].Texture.Width / 2, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / 2 - 70);

            Sprite Button;
            Texture2D ButtonImage = GameContent.Assets.Images.Controls.Button;
            Vector2 ButtonPosition = new Vector2(155);
            Button = new Sprite(ButtonImage, ButtonPosition, Sprites.SpriteBatch);
            AdditionalSprites.Add(Button);

            TextSprite Continue = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.NormalText, string.Format("Continue"));
            Continue.Color = Color.Beige;
            Continue.ParentSprite = Button;
            AdditionalSprites.Add(Continue);
            Continue.Pressed += new EventHandler(Continue_Pressed);
            Continue.IsHoverable = true;
            Continue.NonHoverColor = Color.White;
            Continue.HoverColor = Color.MediumAquamarine;
            Button.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .40f, 300);

            winText = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.NormalText, "");
            winText.Color = Color.Beige;
            AdditionalSprites.Add(winText);
            winText.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .30f, 50);

#if XBOX
            allButton = new GamePadButtonEnumerator(new TextSprite[,] { { Continue } }, InputType.LeftJoystick) { FireTextSpritePressed = true };
#endif

            base.InitScreen(screenName);

        }

        void StateManager_levelCompleted(object sender, EventArgs e)
        {
            if (StateManager.CurrentLevel == GameLevel.Level4)
            {
                winText.Text = string.Format("Congratulations you beat the game!!!");

            }
            else
            {
                winText.Text = string.Format("You died {3} times \n\n {2} Completed!\n\nYou earned {1} Points\n\nYou have obtained {0} spacebucks   ", StateManager.AmountOfSpaceBucksRecievedInCurrentLevel, StateManager.AmountOfPointsRecievedInCurrentLevel, StateManager.CurrentLevel-1, StateManager.Deaths);

            }
            Sprites[1].Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width, Sprites[0].Y + Sprites[0].Height / 2);
            Sprites[1].XSpeed = -5;
            // Sprites[1].YSpeed = 0;
            winText.Scale = new Vector2(2);
            winText.Color = Color.White;
            Sprites[1].Texture = GameContent.Assets.Images.Ships[StateManager.SelectedShip, StateManager.SelectedTier];
        }

        void Continue_Pressed(object sender, EventArgs e)
        {
            if (StateManager.Options.SFXEnabled)
            {
                ButtonClick.Play();
            }

            StateManager.AmountOfPointsRecievedInCurrentLevel = 0;
            StateManager.AmountOfSpaceBucksRecievedInCurrentLevel = 0;
            StateManager.Deaths = 0;


            if (StateManager.CurrentLevel < GameLevel.Level4)
            {
                StateManager.ScreenState = CoreTypes.ScreenType.LevelSelect;
            }
            else
            {
                StateManager.ScreenState = CoreTypes.ScreenType.Credits;
            }


        }



        public override void Update(GameTime game)
        {
            if (this.Visible)
            {
                if (this.Visible)
                {
                    if (!setWinText)
                    {
                        if (StateManager.CurrentLevel == GameLevel.Level4)
                        {
                            winText.Text = string.Format("Congratulations you beat the game!!!");

                        }
                        else
                        {
                            winText.Text = string.Format("You died {3} times \n\n {2} Completed!\n\nYou earned {1} Points\n\nYou have obtained {0} spacebucks", StateManager.AmountOfSpaceBucksRecievedInCurrentLevel, StateManager.AmountOfPointsRecievedInCurrentLevel, StateManager.CurrentLevel, StateManager.Deaths);

                        }
                        setWinText = true;
                    }
                    if (ship.X + ship.Width > 0)
                    {
                        if (Sprites[1].Y > Sprites.SpriteBatch.GraphicsDevice.Viewport.Height - Sprites.SpriteBatch.GraphicsDevice.Viewport.Height / 7)
                        {

                            ship.Scale.X += .005f;
                            ship.Scale.Y += .005f;
                            Sprites[1].YSpeed = 0f;
                        }

                    }
#if XBOX
                allButton.Update(game);
#endif
                    base.Update(game);
                }
            }
        }
    }
}

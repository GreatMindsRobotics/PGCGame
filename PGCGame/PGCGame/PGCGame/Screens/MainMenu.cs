using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;

using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

#if XBOX

using Glib.XNA.InputLib;
using Microsoft.Xna.Framework.Input;

#endif

namespace PGCGame.Screens
{
    public class MainMenu : BaseScreen
    {

        public MainMenu(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);

            ButtonClick = GameContent.GameAssets.Sound[SoundEffectType.ButtonPressed];
        }

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                elapsedButtonDelay = TimeSpan.Zero;
                foreach (SignedInGamer sig in Gamer.SignedInGamers)
                {
                    sig.Presence.PresenceMode = GamerPresenceMode.StartingGame;
                }
            }
        }

        Sprite TitleSprite;

        Sprite planet;
        Sprite planettwo;

        Sprite SinglePlayerButton;
        TextSprite SinglePlayerLabel;

        Sprite MultiPlayerButton;
        TextSprite MultiPlayerLabel;

        Sprite BackButton;
        TextSprite BackLabel;

        Sprite OptionsButton;
        TextSprite OptionsLabel;

        Sprite CreditsButton;
        TextSprite CreditsLabel;






        public override void InitScreen(ScreenType screnType)
        {
            base.InitScreen(screnType);


            Texture2D planetTexture = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
            Texture2D altPlanetTexture = GameContent.GameAssets.Images.NonPlayingObjects.AltPlanet;
            Texture2D buttonImage = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;



            StateManager.Options.ScreenResolutionChanged += new EventHandler<ViewportEventArgs>(Options_ScreenResolutionChanged);

            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            TitleSprite = new Sprite(GameContent.GameAssets.Images.Controls.Title, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .05f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .07f), Sprites.SpriteBatch);
            Sprites.Add(TitleSprite);

            planet = new Sprite(altPlanetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planet.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1515f);
            planet.Scale = new Vector2(.7f);
            Sprites.Add(planet);

            planettwo = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.8f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .75f);
            planettwo.Scale = new Vector2(1f);
            Sprites.Add(planettwo);


            SinglePlayerButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f), Sprites.SpriteBatch);

            Sprites.Add(SinglePlayerButton);

            SinglePlayerLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Singleplayer");
            SinglePlayerLabel.IsHoverable = true;
#if WINDOWS
            SinglePlayerLabel.CallKeyboardClickEvent = false;
#endif
            SinglePlayerLabel.NonHoverColor = Color.White;
            SinglePlayerLabel.HoverColor = Color.MediumAquamarine;

            SinglePlayerLabel.ParentSprite = SinglePlayerButton;

            AdditionalSprites.Add(SinglePlayerLabel);


            MultiPlayerButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f), Sprites.SpriteBatch);

            Sprites.Add(MultiPlayerButton);

            MultiPlayerLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Multiplayer");
            MultiPlayerLabel.IsHoverable = true;
#if WINDOWS
            MultiPlayerLabel.CallKeyboardClickEvent = false;
#endif
            MultiPlayerLabel.NonHoverColor = Color.White;
            MultiPlayerLabel.HoverColor = Color.MediumAquamarine;
            MultiPlayerLabel.ParentSprite = MultiPlayerButton;
            AdditionalSprites.Add(MultiPlayerLabel);


            BackButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);

            Sprites.Add(BackButton);

            BackLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Exit");
            BackLabel.IsHoverable = true;

#if WINDOWS
            BackLabel.CallKeyboardClickEvent = false;
#endif
            BackLabel.ParentSprite = BackButton;
            BackLabel.NonHoverColor = Color.White;
            BackLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(BackLabel);


            OptionsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f), Sprites.SpriteBatch);

            Sprites.Add(OptionsButton);

            OptionsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Options");
#if WINDOWS
            OptionsLabel.CallKeyboardClickEvent = false;
#endif
            OptionsLabel.ParentSprite = OptionsButton;
            OptionsLabel.IsHoverable = true;
            OptionsLabel.NonHoverColor = Color.White;
            OptionsLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(OptionsLabel);


            CreditsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f), Sprites.SpriteBatch);

            Sprites.Add(CreditsButton);


            CreditsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Credits");
            CreditsLabel.IsHoverable = true;
#if WINDOWS
            CreditsLabel.CallKeyboardClickEvent = false;
#endif
            CreditsLabel.ParentSprite = CreditsButton;
            CreditsLabel.NonHoverColor = Color.White;
            CreditsLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(CreditsLabel);

#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { { SinglePlayerLabel, OptionsLabel }, { MultiPlayerLabel, CreditsLabel }, { BackLabel, null } }, InputType.LeftJoystick);
            SinglePlayerLabel.IsSelected = true;
            AllButtons.FireTextSpritePressed = true;
#endif
            BackLabel.Pressed += new EventHandler(delegate(object src, EventArgs e) { if (!Guide.IsVisible && this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.Exit(); ButtonClick.Play(); } });
            CreditsLabel.Pressed += new EventHandler(delegate(object src, EventArgs e) { if (!Guide.IsVisible && this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.ScreenState = ScreenType.Credits; if (StateManager.Options.SFXEnabled) ButtonClick.Play(); } });
            OptionsLabel.Pressed += new EventHandler(delegate(object src, EventArgs e) { if (!Guide.IsVisible && this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.ScreenState = ScreenType.Options; if (StateManager.Options.SFXEnabled)  ButtonClick.Play(); } });
            SinglePlayerLabel.Pressed += new EventHandler(SinglePlayerLabel_Pressed);
            MultiPlayerLabel.Pressed += new EventHandler(delegate(object src, EventArgs e) { if (!Guide.IsVisible && this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.ScreenState = ScreenType.NetworkSelectScreen; if (StateManager.Options.SFXEnabled) ButtonClick.Play(); } });
        }

        void SinglePlayerLabel_Pressed(object sender, EventArgs e)
        {
            if (!Guide.IsVisible && Visible && elapsedButtonDelay > totalButtonDelay)
            {

                if (StateManager.Options.SFXEnabled)
                {
                    ButtonClick.Play();
                }

                //the if statements were causing errors when loading the game, starting a level, quitting out, then loading again.
                //Previous stats weren't loaded properly.

                // if (StateManager.SelectedStorage == null)
                // {


                // }
                // else if (StateManager.SelectedStorage != null)
                //{
                lScr = StateManager.AllScreens[ScreenType.LoadingScreen.ToString()] as LoadingScreen;
                lScr.Reset();
                lScr.ScreenFinished += new EventHandler(lScr_ScreenFinished);
                lScr.UserCallbackStartsTask = true;
                lScr.UserCallback = new Delegates.AsyncHandlerMethod(onSelectedStorage);
                lScr.LoadingText = "Loading saved data...";
                StorageDevice.BeginShowSelector(PlayerIndex.One, lScr.Callback, null);
                StateManager.ScreenState = CoreTypes.ScreenType.LoadingScreen;
                //}
            }
        }

        LoadingScreen lScr;

        void onSelectedStorage(object resul)
        {
            IAsyncResult res = resul as IAsyncResult;
            try
            {
                StorageDevice dev = StorageDevice.EndShowSelector(res);
                if (dev == null)
                {
                    return;
                }

                StateManager.SelectedStorage = dev;

                lScr.UserCallback = new PGCGame.CoreTypes.Delegates.AsyncHandlerMethod(loaded);
                dev.BeginOpenContainer("PGCGame", lScr.Callback, null);
                
            }
            catch { };
        }

        void lScr_ScreenFinished(object sender, EventArgs e)
        {
            StateManager.ScreenState = CoreTypes.ScreenType.LevelSelect;
        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            planet.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1515f);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.8f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .75f);
            TitleSprite.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .05f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .07f);
            CreditsButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f);
            BackButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f);
            SinglePlayerButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f);
            MultiPlayerButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f);
            OptionsButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f);

            //to unselect options label when changing to full screens and back
            foreach (ISprite s in AdditionalSprites)
            {
                if (s.GetType() == typeof(TextSprite))
                {
                    //We can deselect
                    s.Cast<TextSprite>().IsSelected = false;
                }
            }
        }

        //Preventing clickthrus
        TimeSpan elapsedButtonDelay = TimeSpan.Zero;
        TimeSpan totalButtonDelay = TimeSpan.FromMilliseconds(250);

        public override void Update(GameTime gameTime)
        {
            elapsedButtonDelay += gameTime.ElapsedGameTime;
#if XBOX

            currentGamePad = GamePad.GetState(PlayerIndex.One);

            lastGamePad = currentGamePad;

#endif
            base.Update(gameTime);
        }

#if XBOX
        GamePadState currentGamePad;
        GamePadState lastGamePad = new GamePadState(Vector2.Zero, Vector2.Zero, 0, 0, Buttons.A);
#endif

        string filename = "PGCGameSave.dat";

        public void loaded(object r)
        {

            IAsyncResult res = r as IAsyncResult;
            StorageContainer strContain = StateManager.SelectedStorage.EndOpenContainer(res);


            // Check to see whether the save exists.
            if (!strContain.FileExists(filename))
            {
                // If not, dispose of the container and return.
                strContain.Dispose();
                lScr.FinishTask();
                return;
            }

            System.ComponentModel.BackgroundWorker br = new System.ComponentModel.BackgroundWorker();

            br.DoWork += new System.ComponentModel.DoWorkEventHandler(br_DoWork);
            br.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(br_RunWorkerCompleted);

            br.RunWorkerAsync(strContain);
        }

        void br_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            lScr.FinishTask();
        }

        void br_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            StorageContainer container = e.Argument as StorageContainer;
            System.IO.Stream stream = container.OpenFile(filename, System.IO.FileMode.Open);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SerializableGameState));
            SerializableGameState savedState = (SerializableGameState)serializer.Deserialize(stream);
            stream.Close();
            container.Dispose();
            StateManager.SpaceBucks = savedState.Cash;
            StateManager.HighestUnlockedLevel = savedState.HighestLevel;
            StateManager.ShipData = savedState.Ship;
            StateManager.SelectedTier = StateManager.ShipData.Tier;
            StateManager.SelectedShip = StateManager.ShipData.Type;
            foreach (System.Collections.Generic.Stack<SecondaryWeapon> s in StateManager.PowerUps)
            {
                s.Clear();
            }
            for (int i = 0; i < savedState.Upgrades.SpaceMineCount; i++)
            {
                StateManager.PowerUps[0].Push(new SpaceMine(GameContent.GameAssets.Images.SecondaryWeapon[SecondaryWeaponType.SpaceMine, TextureDisplayType.InGameUse], Vector2.Zero, Sprites.SpriteBatch));
            }
            for (int i = 0; i < savedState.Upgrades.EMPCount; i++)
            {
                StateManager.PowerUps[2].Push(new EMP(GameContent.GameAssets.Images.SecondaryWeapon[SecondaryWeaponType.EMP, TextureDisplayType.InGameUse], Vector2.Zero, Sprites.SpriteBatch));
            }
            for (int i = 0; i < savedState.Upgrades.ShrinkRayCount; i++)
            {
                StateManager.PowerUps[1].Push(new ShrinkRay(GameContent.GameAssets.Images.SecondaryWeapon[SecondaryWeaponType.ShrinkRay, TextureDisplayType.InGameUse], Vector2.Zero, Sprites.SpriteBatch));
            }
            for (int i = 0; i < savedState.Upgrades.HealthPackCount; i++)
            {
                StateManager.PowerUps[3].Push(new HealthPack(GameContent.GameAssets.Images.Equipment[EquipmentType.HealthPack, TextureDisplayType.InGameUse], Vector2.Zero, Sprites.SpriteBatch));
            }
            StateManager.BoughtScanner = savedState.Upgrades.HasScanner;
        }
    }
}

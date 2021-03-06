﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;

using PGCGame.CoreTypes;
using Glib.XNA.InputLib;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;
using System.ComponentModel;


namespace PGCGame.Screens
{
    public class Options : BaseScreen
    {
        public override MusicBehaviour Music
        {
            get { return MusicBehaviour.NoMusic; }
        }

        public Options(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            ButtonClick = GameContent.Assets.Sound[SoundEffectType.ButtonPressed];
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
        }

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                bool showCheats = StateManager.GamerServicesAreAvailable;
#if WINDOWS
                if (showCheats)
                {
                    showCheats = false;
                    foreach (SignedInGamer gamer in Gamer.SignedInGamers)
                    {
                        if (gamer.IsSignedInToLive && StateManager.GameDevs.Contains(gamer.Gamertag.ToLower()))
                        {
                            showCheats = true;
                            break;
                        }
                    }
                }
#else
                showCheats = false;
#endif
                CheatsButton.Visible = showCheats;
                CheatsLabel.Visible = showCheats;
            }
        }

        Sprite CheatsButton;
        TextSprite CheatsLabel;

        Sprite ControlButton;
        TextSprite ControlLabel;

#if WINDOWS
        bool mouseInBackButton = false;

        TextSprite GFXLabel;
#endif

        TextSprite SFXLabel;
        TextSprite MusicVolumeLabel;
        TextSprite BackLabel;

        public override void InitScreen(ScreenType screenType)
        {
            base.InitScreen(screenType);

            //Add background to this screen     
            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            Texture2D button = GameContent.Assets.Images.Controls.Button;
            SpriteFont font = GameContent.Assets.Fonts.NormalText;

#if WINDOWS
            StateManager.Options.ScreenResolutionChanged += new EventHandler<ViewportEventArgs>(Options_ScreenResolutionChanged);
#endif

            //Move Controls (aka Controls)
            ControlButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f), Sprites.SpriteBatch);

#if WINDOWS
            ControlButton.MouseEnter += new EventHandler(ControlButton_MouseEnter);
            ControlButton.MouseLeave += new EventHandler(ControlButton_MouseLeave);
#endif
            ControlLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, ("Controls"));
            ControlLabel.Position = new Vector2(ControlButton.Position.X + (ControlButton.Width / 2 - ControlLabel.Width / 2), ControlButton.Position.Y + (ControlButton.Height / 2 - ControlLabel.Height / 2));
            ControlLabel.Color = Color.White;
            ControlLabel.IsManuallySelectable = true;
#if WINDOWS
            ControlLabel.CallKeyboardClickEvent = false;
#endif
            ControlLabel.IsHoverable = true;
            ControlLabel.HoverColor = Color.MediumAquamarine;
            ControlLabel.NonHoverColor = Color.White;




#if WINDOWS
            //GFX
            Sprite GraphicsButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);

            GraphicsButton.MouseEnter += new EventHandler(GraphicsButton_MouseEnter);
            GraphicsButton.MouseLeave += new EventHandler(GraphicsButton_MouseLeave);


            GFXLabel = new TextSprite(Sprites.SpriteBatch, font, String.Format("GFX: {0}", StateManager.GraphicsManager.IsFullScreen ? "Full" : "Standard"));
            GFXLabel.Position = new Vector2((GraphicsButton.X + GraphicsButton.Width / 2) - GFXLabel.Width / 2, (GraphicsButton.Y + GraphicsButton.Height / 2) - GFXLabel.Height / 2);
            GFXLabel.Color = Color.White;
            GFXLabel.CallKeyboardClickEvent = false;
            GFXLabel.IsManuallySelectable = true;
            GFXLabel.IsHoverable = true;
            GFXLabel.HoverColor = Color.MediumAquamarine;
            GFXLabel.NonHoverColor = Color.White;
#endif

            //SFX
            Sprite SFXButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .10f), Sprites.SpriteBatch);

#if WINDOWS
            SFXButton.MouseEnter += new EventHandler(SFXButton_MouseEnter);
            SFXButton.MouseLeave += new EventHandler(SFXButton_MouseLeave);
#endif
            SFXLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "SFX: " + (StateManager.Options.SFXEnabled ? "On" : "Off"));
            SFXLabel.Position = new Vector2((SFXButton.X + SFXButton.Width / 2) - SFXLabel.Width / 2, (SFXButton.Y + SFXButton.Height / 2) - SFXLabel.Height / 2);
            SFXLabel.Color = Color.White;
            SFXLabel.IsHoverable = true;
#if WINDOWS
            SFXLabel.CallKeyboardClickEvent = false;
#endif
            SFXLabel.IsManuallySelectable = true;
            SFXLabel.HoverColor = Color.MediumAquamarine;
            SFXLabel.NonHoverColor = Color.White;

            //Back button
#if WINDOWS
            Sprite BackButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
#elif XBOX
            Sprite BackButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);
#endif
            BackLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.Assets.Fonts.NormalText, "Back");
            BackLabel.Position = new Vector2((BackButton.X + BackButton.Width / 2) - BackLabel.Width / 2, (BackButton.Y + BackButton.Height / 2) - BackLabel.Height / 2);
#if WINDOWS
            BackLabel.CallKeyboardClickEvent = false;
#endif
            BackLabel.Color = Color.White;

#if WINDOWS
            BackButton.MouseEnter += new EventHandler(BackButton_MouseEnter);
            BackButton.MouseLeave += new EventHandler(BackButton_MouseLeave);
#endif

            //Music (volume; currently on/off)

            Sprite MusicButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);
            MusicButton.Color = Color.White;

#if WINDOWS
            MusicButton.MouseEnter += new EventHandler(MusicButton_MouseEnter);
            MusicButton.MouseLeave += new EventHandler(MusicButton_MouseLeave);
#endif
            StateManager.Options.MusicStateChanged += new EventHandler(Options_MusicStateChanged);
            MusicVolumeLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "Music: " + (StateManager.Options.MusicEnabled ? "On" : "Off"));
            MusicVolumeLabel.Position = new Vector2((MusicButton.X + MusicButton.Width / 2) - MusicVolumeLabel.Width / 2, (MusicButton.Y + MusicButton.Height / 2) - MusicVolumeLabel.Height / 2);
            MusicVolumeLabel.Color = Color.White;
#if WINDOWS
            MusicVolumeLabel.CallKeyboardClickEvent = false;
#endif
            MusicVolumeLabel.IsHoverable = true;
            MusicVolumeLabel.IsManuallySelectable = true;
            MusicVolumeLabel.HoverColor = Color.MediumAquamarine;
            MusicVolumeLabel.NonHoverColor = Color.White;

            //Add all buttons
            Sprites.Add(ControlButton);
            Sprites.Add(SFXButton);
            Sprites.Add(BackButton);
            Sprites.Add(MusicButton);

            //Add all text sprites
            AdditionalSprites.Add(ControlLabel);
#if WINDOWS
            Sprites.Add(GraphicsButton);
            AdditionalSprites.Add(GFXLabel);
#endif
            AdditionalSprites.Add(SFXLabel);
            AdditionalSprites.Add(BackLabel);
            AdditionalSprites.Add(MusicVolumeLabel);

            CheatsButton = new Sprite(button, new Vector2(MusicButton.X, BackButton.Y), Sprites.SpriteBatch);
            CheatsButton.Visible = StateManager.GamerServicesAreAvailable && Gamer.SignedInGamers.Count > 0;
            Sprites.Add(CheatsButton);

            CheatsLabel = StateManager.CreateButtonTextSprite(false, "Cheats", CheatsButton, this);
            CheatsLabel.Pressed += new EventHandler(CheatsLabel_Pressed);
#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { { ControlLabel,  SFXLabel}, { BackLabel,  MusicVolumeLabel}}, InputType.LeftJoystick);
            AllButtons.ButtonPress += new EventHandler(AllButtons_ButtonPress);
            ControlLabel.IsSelected = true;
#endif

#if WINDOWS
            StateManager.Options.ScreenResolutionChanged += new EventHandler<ViewportEventArgs>(Options_ScreenResolutionChanged);
#endif

        }

        void Options_MusicStateChanged(object sender, EventArgs e)
        {
            MusicVolumeLabel.Text = "Music: " + (StateManager.Options.MusicEnabled ? "On" : "Off");
        }

        void CheatsLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.ScreenState = CoreTypes.ScreenType.CheatModifyScreen;
        }

#if XBOX
        void AllButtons_ButtonPress(object sender, EventArgs e)
        { 
          if (ControlLabel.IsSelected)
          {
              StateManager.ScreenState = ScreenType.ControlScreen;
          }
          else if (SFXLabel.IsSelected)
          {
              StateManager.Options.SFXEnabled = !StateManager.Options.SFXEnabled;
              SFXLabel.Text = String.Format("SFX: {0}", StateManager.Options.SFXEnabled ? "On" : "Off");
          }
#if WINDOWS
          else if (GFXLabel.IsSelected)
          {
              StateManager.Options.ToggleFullscreen();
          }
#endif
          else if (MusicVolumeLabel.IsSelected)
          {
              StateManager.Options.MusicEnabled = !StateManager.Options.MusicEnabled;
              MusicVolumeLabel.Text = String.Format("Music: {0}", StateManager.Options.MusicEnabled ? "On" : "Off");
          }
          else if (BackLabel.IsSelected)
          {
             
          }
        }
#endif

#if WINDOWS
        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            //RESET THE LOCATION OF EVERY SPRITE ON THE SCREEN!
            GFXLabel.Text = String.Format("GFX: {0}", StateManager.GraphicsManager.IsFullScreen ? "Full" : "Standard");

        }
#endif

        //Controls
        void ControlButton_MouseLeave(object sender, EventArgs e)
        {
            ControlLabel.IsSelected = false;
        }
        void ControlButton_MouseEnter(object sender, EventArgs e)
        {
            ControlLabel.IsSelected = true;
        }

        //Sound Effects
        void SFXButton_MouseLeave(object sender, EventArgs e)
        {
            SFXLabel.IsSelected = false;
        }

        void SFXButton_MouseEnter(object sender, EventArgs e)
        {
            SFXLabel.IsSelected = true;
        }

        //standard
        //Full
#if WINDOWS
        void GraphicsButton_MouseLeave(object sender, EventArgs e)
        {
            GFXLabel.IsSelected = false;
        }
        void GraphicsButton_MouseEnter(object sender, EventArgs e)
        {
            GFXLabel.IsSelected = true;
        }

        public bool mouseOnGraphicButton
        {
            get
            {
                return GFXLabel.IsSelected;
            }
        }


        void MusicButton_MouseLeave(object sender, EventArgs e)
        {
            MusicVolumeLabel.IsSelected = false;
        }

        void MusicButton_MouseEnter(object sender, EventArgs e)
        {
            MusicVolumeLabel.IsSelected = true;
        }


        //back button
        void BackButton_MouseLeave(object sender, EventArgs e)
        {
            BackLabel.Color = Color.White;
            mouseInBackButton = false;
        }
        void BackButton_MouseEnter(object sender, EventArgs e)
        {
            BackLabel.Color = Color.MediumAquamarine;
            mouseInBackButton = true;
        }

        MouseState lastMs = new MouseState(0, 0, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);

#endif

        public static readonly string Filename = "PGCPreferences.dat";

        private void storageContainerOpened(object iAsyncRes)
        {
            StorageContainer strContain = StateManager.SelectedStorage.EndOpenContainer(iAsyncRes as IAsyncResult);

            BackgroundWorker fileSaver = new BackgroundWorker();
            fileSaver.DoWork += new DoWorkEventHandler(fileSaver_DoWork);
            fileSaver.RunWorkerCompleted += new RunWorkerCompletedEventHandler(fileSaver_RunWorkerCompleted);
            fileSaver.RunWorkerAsync(strContain);
        }

        void fileSaver_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lScr.FinishTask();
        }

        void fileSaver_DoWork(object sender, DoWorkEventArgs e)
        {
            StorageContainer saveData = e.Argument as StorageContainer;
            

            // Check to see whether the save exists.
            if (saveData.FileExists(Filename))
            {
                // Delete it so that we can create one fresh.
                saveData.DeleteFile(Filename);
            }
            System.IO.Stream stream = saveData.CreateFile(Filename);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SerializableGamePreferences));
            serializer.Serialize(stream, SerializableGamePreferences.Current);
            stream.Close();
            saveData.Dispose();
        }

        private void storageDeviceFound(object iAsyncRes)
        {
            IAsyncResult res = iAsyncRes as IAsyncResult;
            try
            {
                StorageDevice dev = StorageDevice.EndShowSelector(res);
                if (dev == null)
                {
                    //Error occurred.
                    lScr.FinishTask();
                    return;
                }

                StateManager.SelectedStorage = dev;

                lScr.UserCallback = new PGCGame.CoreTypes.Delegates.AsyncHandlerMethod(storageContainerOpened);
                dev.BeginOpenContainer("PGCGame", lScr.Callback, null);

            }
            catch
            {
                //Error occurred.
                lScr.FinishTask();
                return;
            };
        }

        LoadingScreen lScr;

        public override void Update(GameTime gameTime)
        {
#if WINDOWS
            MouseState currentMs = MouseManager.CurrentMouseState;
            if (lastMs.LeftButton == ButtonState.Released && currentMs.LeftButton == ButtonState.Pressed)
            {
                if (mouseInBackButton)
                {
                    if (StateManager.Options.SFXEnabled)
                    {
                        ButtonClick.Play();
                    }

                    if (lScr == null)
                    {
                        lScr = StateManager.AllScreens[ScreenType.LoadingScreen.ToString()] as LoadingScreen;
                    }
                    lScr.Reset();
                    lScr.ScreenFinished += new EventHandler(lScr_ScreenFinished);
                    lScr.UserCallbackStartsTask = true;
                    lScr.UserCallback = new Delegates.AsyncHandlerMethod(storageDeviceFound);
                    lScr.LoadingText = "Saving data...";
                    StorageDevice.BeginShowSelector(PlayerIndex.One, lScr.Callback, null);
                    StateManager.ScreenState = CoreTypes.ScreenType.LoadingScreen;

                    StateManager.ScreenState = ScreenType.LoadingScreen;

                    //StateManager.GoBack();
                }
                if (MusicVolumeLabel.IsSelected)
                {
                    if (StateManager.Options.SFXEnabled)
                    {
                        ButtonClick.Play();
                    }

                    StateManager.Options.MusicEnabled = !StateManager.Options.MusicEnabled;
                    MusicVolumeLabel.Text = String.Format("Music: {0}", StateManager.Options.MusicEnabled ? "On" : "Off");
                }
                if (SFXLabel.IsSelected)
                {
                    if (StateManager.Options.SFXEnabled)
                    {
                        ButtonClick.Play();
                    }

                    StateManager.Options.SFXEnabled = !StateManager.Options.SFXEnabled;
                    SFXLabel.Text = String.Format("SFX: {0}", StateManager.Options.SFXEnabled ? "On" : "Off");

                }
                if (mouseOnGraphicButton)
                {
                    if (StateManager.Options.SFXEnabled)
                    {
                        ButtonClick.Play();
                    }

                    StateManager.Options.ToggleFullscreen();
                }
                if (ControlLabel.IsSelected)
                {
                    if (StateManager.Options.SFXEnabled)
                    {
                        ButtonClick.Play();
                    }

                    StateManager.ScreenState = ScreenType.ControlScreen;
                }
            }
            lastMs = currentMs;
#elif XBOX
            currentGamePad = GamePad.GetState(PlayerIndex.One);

            lastGamePad = currentGamePad;

#endif
            base.Update(gameTime);
        }

        void lScr_ScreenFinished(object sender, EventArgs e)
        {
            StateManager.ScreenState = CoreTypes.ScreenType.MainMenu;
        }
#if XBOX
        GamePadState currentGamePad;
        GamePadState lastGamePad = new GamePadState(Vector2.Zero, Vector2.Zero, 0, 0, Buttons.A);
#endif
    }
}

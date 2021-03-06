﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using Glib.XNA.SpriteLib;
using Glib;
using Glib.XNA;

using PGCGame.CoreTypes;
using Glib.XNA.InputLib;
using Microsoft.Xna.Framework.Storage;
using System.ComponentModel;
using Microsoft.Xna.Framework.GamerServices;

namespace PGCGame.Screens
{
    public class Shop : BaseScreen
    {
        public override MusicBehaviour Music
        {
            get { return MusicBehaviour.NoMusic; }
        }

        public Shop(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            //TODO: BACKGROUND
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
            ButtonClick = GameContent.Assets.Sound[SoundEffectType.ButtonPressed];
        }

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            isSaving = false;
            if (StateManager.ScreenState == ScreenType)
            {
                foreach (SignedInGamer sig in Gamer.SignedInGamers)
                {
                    sig.Presence.PresenceMode = GamerPresenceMode.InGameStore;
                }
            }
        }

        public static event EventHandler levelBegin;

        public static event EventHandler PurchaseScreenSelected;

        Sprite upgradeEquipmentButton;
        TextSprite upgradeEquipmentLabel;

        Sprite shipButton;
        TextSprite shipLabel;

        Sprite weaponsButton;
        TextSprite weaponsLabel;
        TextSprite noShipLabel;

        Sprite PlayButton;
        TextSprite PlayLabel;

        public static Boolean firstShop = true;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);

            StateManager.Options.ScreenResolutionChanged += new EventHandler<ViewportEventArgs>(Options_ScreenResolutionChanged);
            
            Texture2D buttonImage = GameContent.Assets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.Assets.Fonts.NormalText;

            //Configure backgrounds
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            Sprites.AddNewSprite(Vector2.Zero, GameContent.Assets.Images.NonPlayingObjects.ShopBackground);
            Sprites[0].Scale = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width / (float)Sprites[0].Texture.Width, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / (float)Sprites[0].Texture.Height);


            upgradeEquipmentButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .1f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            upgradeEquipmentLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Equipment");
            upgradeEquipmentLabel.Color = Color.White;
            upgradeEquipmentLabel.IsHoverable = true;
            upgradeEquipmentLabel.ParentSprite = upgradeEquipmentButton;
            upgradeEquipmentLabel.NonHoverColor = Color.White;
            upgradeEquipmentLabel.HoverColor = Color.MediumAquamarine;

            Sprites.Add(upgradeEquipmentButton);
            AdditionalSprites.Add(upgradeEquipmentLabel);

            shipButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            shipLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Ship");
            shipLabel.Color = Color.White;
            shipLabel.IsHoverable = true;
            shipLabel.ParentSprite = shipButton;
            shipLabel.NonHoverColor = Color.White;
            shipLabel.HoverColor = Color.MediumAquamarine;


            Sprites.Add(shipButton);
            AdditionalSprites.Add(shipLabel);

            weaponsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .7f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            weaponsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Weapons");
            weaponsLabel.ParentSprite = weaponsButton;
            weaponsLabel.Color = Color.White;
            weaponsLabel.IsHoverable = true;
            weaponsLabel.NonHoverColor = Color.White;
            weaponsLabel.HoverColor = Color.MediumAquamarine;

            Sprites.Add(weaponsButton);
            AdditionalSprites.Add(weaponsLabel);

            PlayButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .65f), Sprites.SpriteBatch);
            PlayLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Play");
            PlayLabel.Color = Color.White;
            PlayLabel.IsHoverable = true;
            PlayLabel.ParentSprite = PlayButton;
            PlayLabel.NonHoverColor = Color.White;
            PlayLabel.HoverColor = Color.MediumAquamarine;

            noShipLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(0, PlayLabel.Y), SegoeUIMono, "You must buy a ship");
            noShipLabel.Color = Color.White;
            noShipLabel.X = noShipLabel.GetCenterPosition(Graphics.Viewport).X;
            noShipLabel.TextChanged += new EventHandler(noShipLabel_TextChanged);

            Sprites.Add(PlayButton);
            AdditionalSprites.Add(noShipLabel);
            AdditionalSprites.Add(PlayLabel);


            PlayLabel.Pressed += new EventHandler(nextLevelLabel_Pressed);
            weaponsLabel.Pressed += new EventHandler(weaponsLabel_Pressed);
            upgradeEquipmentLabel.Pressed += new EventHandler(upgradeEquipmentLabel_Pressed);
            shipLabel.Pressed += new EventHandler(shipLabel_Pressed);

#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { 
            {upgradeEquipmentLabel, shipLabel, weaponsLabel},
            {null, PlayLabel, null}
            }, InputType.LeftJoystick);
            AllButtons.FireTextSpritePressed = true;
#endif
        }

        void noShipLabel_TextChanged(object sender, EventArgs e)
        {
            noShipLabel.X = noShipLabel.GetCenterPosition(Graphics.Viewport).X;
        }


        void shipLabel_Pressed(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }

            if (StateManager.Options.SFXEnabled)
            {
                ButtonClick.Play();
            }

            PurchaseScreenSelected(null, null);
            StateManager.ScreenState = ScreenType.TierSelect;
            
        }

        void upgradeEquipmentLabel_Pressed(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }

            if (StateManager.Options.SFXEnabled)
            {
                ButtonClick.Play();
            }

            PurchaseScreenSelected(null, null);
            StateManager.ScreenState = ScreenType.UpgradeScreen;
        }

        void weaponsLabel_Pressed(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }

            if (StateManager.Options.SFXEnabled)
            {
                ButtonClick.Play();
            }
            PurchaseScreenSelected(null, null);
            StateManager.ScreenState = ScreenType.WeaponSelect;
        }

        void OpenComplete(IAsyncResult res)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            StorageContainer saveData = StateManager.SelectedStorage.EndOpenContainer(res);
            bw.RunWorkerAsync(saveData);
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            levelBegin(null, null);

            StateManager.ScreenState = ScreenType.TransitionScreen;
            isSaving = false;
        }

        private TimeSpan _elapsedSaveTime = TimeSpan.Zero;

        private TimeSpan _reqSaveTime = TimeSpan.FromSeconds(.2);

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            StorageContainer saveData = e.Argument as StorageContainer;
            string filename = "PGCGameSave.dat";

            // Check to see whether the save exists.
            if (saveData.FileExists(filename))
            {
                // Delete it so that we can create one fresh.
                saveData.DeleteFile(filename);
            }
            System.IO.Stream stream = saveData.CreateFile(filename);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SerializableGameState));
            serializer.Serialize(stream, SerializableGameState.Current);
            stream.Close();
            saveData.Dispose();

            while (_elapsedSaveTime < _reqSaveTime)
            {
                //Wait on this variable
            }
        }

        void nextLevelLabel_Pressed(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }

            if (StateManager.Options.SFXEnabled)
            {
                ButtonClick.Play();
            }

            isSaving = true;
            PlayButton.Color = Color.Transparent;
            PlayLabel.Visible = false;
            noShipLabel.Text = "Saving...";
            noShipLabel.Visible = true;
            StateManager.SelectedStorage.BeginOpenContainer("PGCGame", new AsyncCallback(OpenComplete), null);
            
        }

        bool isSaving = false;

        void Options_ScreenResolutionChanged(object sender, ViewportEventArgs e)
        {
            //relocate all the sprites and labels to the correct position
            Sprites[0].Scale = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width / (float)Sprites[0].Texture.Width, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / (float)Sprites[0].Texture.Height);
            shipButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f - (shipButton.Width / 2), Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .5f);
            upgradeEquipmentButton.Position = new Vector2(shipButton.X - (1.5f * shipButton.Width), Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .5f);      
            weaponsButton.Position = new Vector2(shipButton.X + (1.5f * shipButton.Width), Sprites.SpriteBatch.GraphicsDevice.Viewport.Height *.5f);
            PlayButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f - (PlayButton.Width / 2), Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .6f + (.5f * PlayButton.Height));
            noShipLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f - (noShipLabel.Width / 2), Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .6f + (.5f * noShipLabel.Height));
            noShipLabel.Y = Graphics.Viewport.Height / 2 + 100;
            noShipLabel.X = noShipLabel.GetCenterPosition(Graphics.Viewport).X;
        }



        //MouseState lastMs = new MouseState(0, 0, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!isSaving)
            {
                _elapsedSaveTime = TimeSpan.Zero;
                PlayButton.Color = StateManager.SelectedTier == ShipTier.NoShip ? Color.Transparent : Color.White;
                PlayLabel.Visible = PlayButton.Color.A > 0;
                noShipLabel.Visible = !PlayLabel.Visible;
            }
            else
            {
                _elapsedSaveTime += gameTime.ElapsedGameTime;
            }
        }
    }
}

using System;
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

namespace PGCGame.Screens
{
    public class ControlScreen : BaseScreen
    {
        private MusicBehaviour _music = new MusicBehaviour();

        public override MusicBehaviour Music
        {
            get { return _music; }
        }

        public ControlScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            ButtonClick = GameContent.Assets.Sound[SoundEffectType.ButtonPressed];
        }

        TextSprite MoveLabel;
        TextSprite FireLabel;
        TextSprite SecondWeapLabel; //Using the secondary weapon
        TextSprite SwitchSecondWeapLabel; //Switching between the secondary weapons
        TextSprite DeployDronesLabel;
        TextSprite BackLabel;

#if WINDOWS
        Sprite MoveButton;
        Sprite FireButton;
        Sprite SecondWeap;
        Sprite SwitchWeap;
        Sprite DeployDrones;

        MouseState lastMS;
#endif
        Sprite BackButton;


        public override void InitScreen(ScreenType screenType)
        {
            base.InitScreen(screenType);

            //Adding a background
            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            Texture2D button = GameContent.Assets.Images.Controls.Button;
            SpriteFont font = GameContent.Assets.Fonts.NormalText;

#if XBOX
             MoveLabel = new TextSprite(Sprites.SpriteBatch, font, ("Move: Right Joystick"));
             MoveLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f);
             MoveLabel.Color = Color.White;
#endif


#if WINDOWS
            MoveLabel = new TextSprite(Sprites.SpriteBatch, font, String.Format("Move:{0}", StateManager.Options.LeftButtonEnabled ? "Arrow Keys" : "WASD"));
            MoveButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f), Sprites.SpriteBatch);
            MoveLabel.Position = new Vector2((MoveButton.X + MoveButton.Width / 2) - MoveLabel.Width / 2, (MoveButton.Y + MoveButton.Height / 2) - MoveLabel.Height / 2);
            MoveLabel.Color = Color.White;
            MoveLabel.IsManuallySelectable = true;
            MoveLabel.IsHoverable = true;
            MoveLabel.HoverColor = Color.MediumAquamarine;
            MoveLabel.NonHoverColor = Color.White;

            MoveButton.MouseEnter += new EventHandler(MoveButton_MouseEnter);
            MoveButton.MouseLeave += new EventHandler(MoveButton_MouseLeave);
#endif

#if XBOX
             FireLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, ("Fire: RTrigger"));
             FireLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f);
             FireLabel.Color = Color.White;
#endif


#if WINDOWS
            FireLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, String.Format("Fire:{0}", StateManager.Options.ArrowKeysEnabled ? "LClick" : "Space"));
            FireButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);
            FireLabel.Position = new Vector2(FireButton.Position.X + (FireButton.Width / 2 - FireLabel.Width / 2), FireButton.Position.Y + (FireButton.Height / 2 - FireLabel.Height / 2));
            FireLabel.Color = Color.White;
            FireLabel.IsManuallySelectable = true;
            FireLabel.IsHoverable = true;
            FireLabel.HoverColor = Color.MediumAquamarine;
            FireLabel.NonHoverColor = Color.White;

            FireButton.MouseEnter += new EventHandler(FireButton_MouseEnter);
            FireButton.MouseLeave += new EventHandler(FireButton_MouseLeave);
#endif

#if WINDOWS
            SecondWeap = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f), Sprites.SpriteBatch);
            SecondWeapLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, String.Format("Use Weapon:{0}", StateManager.Options.SecondaryButtonEnabled ? "RShift" : "R"));
            SecondWeapLabel.Position = new Vector2(SecondWeap.Position.X + (SecondWeap.Width / 2 - SecondWeapLabel.Width / 2), SecondWeap.Position.Y + (SecondWeap.Height / 2 - SecondWeapLabel.Height / 2));
            SecondWeapLabel.Color = Color.White;
            SecondWeapLabel.IsManuallySelectable = true;
            SecondWeapLabel.IsHoverable = true;
            SecondWeapLabel.HoverColor = Color.MediumAquamarine;
            SecondWeapLabel.NonHoverColor = Color.White;
            SecondWeap.Scale = new Vector2(1.4f, SecondWeap.Scale.Y);

            SecondWeap.MouseEnter += new EventHandler(SecondWeap_MouseEnter);
            SecondWeap.MouseLeave += new EventHandler(SecondWeap_MouseLeave);
#endif

#if XBOX
             SecondWeapLabel = new TextSprite(Sprites.SpriteBatch, font, ("Use Secondary Weapon: LTrigger"));
             SecondWeapLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .10f);
             SecondWeapLabel.Color = Color.White;
#endif



#if WINDOWS
            SwitchWeap = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);
            SwitchSecondWeapLabel = new TextSprite(Sprites.SpriteBatch, font, String.Format("Switch Weapon:{0}", StateManager.Options.LeftButtonEnabled ? "PgUp/PgDn" : "Q/E"));
            SwitchSecondWeapLabel.Color = Color.White;
            SwitchSecondWeapLabel.IsManuallySelectable = true;
            SwitchSecondWeapLabel.IsHoverable = true;
            SwitchSecondWeapLabel.HoverColor = Color.MediumAquamarine;
            SwitchSecondWeapLabel.NonHoverColor = Color.White;
            SwitchWeap.Scale = new Vector2(1.4f, SwitchWeap.Scale.Y);
            SwitchSecondWeapLabel.Position = new Vector2(SwitchWeap.Position.X + (SwitchWeap.Width / 2 - SwitchSecondWeapLabel.Width / 2), SwitchWeap.Position.Y + (SwitchWeap.Height / 2 - SwitchSecondWeapLabel.Height / 2));

            SwitchWeap.MouseEnter += new EventHandler(SwitchWeap_MouseEnter);
            SwitchWeap.MouseLeave += new EventHandler(SwitchWeap_MouseLeave);
#endif

#if XBOX
             SwitchSecondWeapLabel = new TextSprite(Sprites.SpriteBatch, font, ("Switch Secondary Weapons: \n Left and Right Bumpers"));
             SwitchSecondWeapLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f);
             SwitchSecondWeapLabel.Color = Color.White;
#endif


#if WINDOWS
            DeployDrones = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .6f), Sprites.SpriteBatch);
            DeployDronesLabel = new TextSprite(Sprites.SpriteBatch, font, String.Format("Drones:{0}", StateManager.Options.LeftButtonEnabled ? "RControl" : "LShift"));
            DeployDronesLabel.Position = new Vector2(DeployDrones.Position.X + (DeployDrones.Width / 2 - DeployDronesLabel.Width / 2), DeployDrones.Position.Y + (DeployDrones.Height / 2 - DeployDronesLabel.Height / 2));
            DeployDronesLabel.Color = Color.White;
            DeployDronesLabel.IsManuallySelectable = true;
            DeployDronesLabel.IsHoverable = true;
            DeployDronesLabel.HoverColor = Color.MediumAquamarine;
            DeployDronesLabel.NonHoverColor = Color.White;
            DeployDrones.Scale = new Vector2(1.1f, DeployDrones.Scale.Y);

            DeployDrones.MouseEnter += new EventHandler(DeployDrones_MouseEnter);
            DeployDrones.MouseLeave += new EventHandler(DeployDrones_MouseLeave);


#elif XBOX
             DeployDronesLabel = new TextSprite(Sprites.SpriteBatch, font, ("Deploy Fighter Carrier Drones: A Button"));
             DeployDronesLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f);
             DeployDronesLabel.Color = Color.White;
#endif
            BackButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            BackLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .139f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .62f), GameContent.Assets.Fonts.NormalText, "Back");
            BackLabel.Position = new Vector2((BackButton.X + BackButton.Width / 2) - BackLabel.Width / 2, (BackButton.Y + BackButton.Height / 2) - BackLabel.Height / 2);
            BackLabel.Color = Color.White;
            BackLabel.IsManuallySelectable = true;
            BackLabel.IsHoverable = true;
            BackLabel.HoverColor = Color.MediumAquamarine;
            BackLabel.NonHoverColor = Color.White;
#if WINDOWS
            BackButton.MouseEnter += new EventHandler(BackButton_MouseEnter);
            BackButton.MouseLeave += new EventHandler(BackButton_MouseLeave);
#endif
            //Add all buttons
#if WINDOWS
            Sprites.Add(MoveButton);
            Sprites.Add(FireButton);
            Sprites.Add(SecondWeap);
            Sprites.Add(SwitchWeap);
            Sprites.Add(DeployDrones);
#endif
            Sprites.Add(BackButton);

            //Add all text sprites
            AdditionalSprites.Add(BackLabel);
            AdditionalSprites.Add(MoveLabel);
            AdditionalSprites.Add(FireLabel);
            AdditionalSprites.Add(SecondWeapLabel);
            AdditionalSprites.Add(SwitchSecondWeapLabel);
            AdditionalSprites.Add(DeployDronesLabel);


            StateManager.Options.ScreenResolutionChanged += new EventHandler<ViewportEventArgs>(Options_ScreenResolutionChanged);

        }

        public override void Update(GameTime game)
        {
#if WINDOWS
            MouseState currentMouseState = MouseManager.CurrentMouseState;

            if (BackLabel.IsSelected && BackButton.ClickCheck(currentMouseState) && !BackButton.ClickCheck(lastMS))
            {
                if (StateManager.Options.SFXEnabled)
                {
                    ButtonClick.Play();
                }

                StateManager.GoBack();
            }

            if (MoveLabel.IsSelected && currentMouseState.LeftButton == ButtonState.Pressed && lastMS.LeftButton != ButtonState.Pressed)
            {
                if (StateManager.Options.SFXEnabled)
                {
                    ButtonClick.Play();
                }

                StateManager.Options.ArrowKeysEnabled = !StateManager.Options.ArrowKeysEnabled;
                MoveLabel.Text = String.Format("Move:{0}", StateManager.Options.ArrowKeysEnabled ? "Arrow" : "WASD");
            }

            if (FireLabel.IsSelected && currentMouseState.LeftButton == ButtonState.Pressed && lastMS.LeftButton != ButtonState.Pressed)
            {
                if (StateManager.Options.SFXEnabled)
                {
                    ButtonClick.Play();
                }

                StateManager.Options.LeftButtonEnabled = !StateManager.Options.LeftButtonEnabled;
                FireLabel.Text = String.Format("Fire:{0}", StateManager.Options.LeftButtonEnabled ? "LClick" : "Space");
            }

            if (SecondWeapLabel.IsSelected && currentMouseState.LeftButton == ButtonState.Pressed && lastMS.LeftButton != ButtonState.Pressed)
            {
                if (StateManager.Options.SFXEnabled)
                {
                    ButtonClick.Play();
                }

                StateManager.Options.SecondaryButtonEnabled = !StateManager.Options.SecondaryButtonEnabled;
                SecondWeapLabel.Text = String.Format("Use Weapon:{0}", StateManager.Options.SecondaryButtonEnabled ? "RShift" : "R");
            }

            if (SwitchSecondWeapLabel.IsSelected && currentMouseState.LeftButton == ButtonState.Pressed && lastMS.LeftButton != ButtonState.Pressed)
            {
                if (StateManager.Options.SFXEnabled)
                {
                    ButtonClick.Play();
                }

                StateManager.Options.SwitchButtonEnabled = !StateManager.Options.SwitchButtonEnabled;
                SwitchWeap.Scale = new Vector2(1.7f, SwitchWeap.Scale.Y);
                SwitchSecondWeapLabel.Text = String.Format("Switch Weapon:{0}", StateManager.Options.SwitchButtonEnabled ? "PgUp/PgDn" : "Q/E");
            }

            if (DeployDronesLabel.IsSelected && currentMouseState.LeftButton == ButtonState.Pressed && lastMS.LeftButton != ButtonState.Pressed)
            {
                if (StateManager.Options.SFXEnabled)
                {
                    ButtonClick.Play();
                }

                StateManager.Options.DeployDronesEnabled = !StateManager.Options.DeployDronesEnabled;
                DeployDronesLabel.Text = String.Format("Drones:{0}", StateManager.Options.DeployDronesEnabled ? "RControl" : "LShift");
                Drone.DeployKey = StateManager.Options.DeployDronesEnabled ? Keys.RightControl : Keys.LeftShift;
            }
            SwitchWeap.Scale = new Vector2(SwitchSecondWeapLabel.Text == "Switch Weapon:PgUp/PgDn" ? 1.7f : 1.4f, SwitchWeap.Scale.Y);
            SecondWeapLabel.Position = new Vector2(SecondWeap.Position.X + (SecondWeap.Width / 2 - SecondWeapLabel.Width / 2), SecondWeap.Position.Y + (SecondWeap.Height / 2 - SecondWeapLabel.Height / 2));
            SwitchSecondWeapLabel.Position = new Vector2(SwitchWeap.Position.X + (SwitchWeap.Width / 2 - SwitchSecondWeapLabel.Width / 2), SwitchWeap.Position.Y + (SwitchWeap.Height / 2 - SwitchSecondWeapLabel.Height / 2));
            MoveLabel.Position = new Vector2((MoveButton.X + MoveButton.Width / 2) - MoveLabel.Width / 2, (MoveButton.Y + MoveButton.Height / 2) - MoveLabel.Height / 2);

            lastMS = currentMouseState;
#elif XBOX
             
#endif
            base.Update(game);
        }

        public bool FireSelected
        {
            get
            {
                return FireLabel.IsSelected;
            }
        }

        void DeployDrones_MouseEnter(object sender, EventArgs e)
        {
            DeployDronesLabel.IsSelected = true;
        }

        void DeployDrones_MouseLeave(object sender, EventArgs e)
        {
            DeployDronesLabel.IsSelected = false;
        }

        void SwitchWeap_MouseEnter(object sender, EventArgs e)
        {
            SwitchSecondWeapLabel.IsSelected = true;
        }

        void SwitchWeap_MouseLeave(object sender, EventArgs e)
        {
            SwitchSecondWeapLabel.IsSelected = false;
        }

        void SecondWeap_MouseEnter(object sender, EventArgs e)
        {
            SecondWeapLabel.IsSelected = true;
        }

        void SecondWeap_MouseLeave(object sender, EventArgs e)
        {
            SecondWeapLabel.IsSelected = false;
        }

        void FireButton_MouseEnter(object sender, EventArgs e)
        {
            FireLabel.IsSelected = true;
        }

        void FireButton_MouseLeave(object sender, EventArgs e)
        {
            FireLabel.IsSelected = false;
        }

        public bool MoveSelected
        {
            get
            {
                return MoveLabel.IsSelected;
            }
        }

        void MoveButton_MouseEnter(object sender, EventArgs e)
        {
            MoveLabel.IsSelected = true;
        }

        void MoveButton_MouseLeave(object sender, EventArgs e)
        {
            MoveLabel.IsSelected = false;
        }

        public bool BackSelected
        {
            get
            {
                return BackLabel.IsSelected;
            }
        }

        void BackButton_MouseEnter(object sender, EventArgs e)
        {
            BackLabel.IsSelected = true;
        }

        void BackButton_MouseLeave(object sender, EventArgs e)
        {
            BackLabel.IsSelected = false;
        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            //Whatever code you're doing to place your buttons and whatnot, also place it here.
        }
    }
}
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

         public ControlScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
         {
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

         MouseState lastMS;
#endif
         Sprite BackButton;


         public override void InitScreen(ScreenType screenType)
         {
             base.InitScreen(screenType);

             //Adding a background
             this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

             Texture2D button = GameContent.GameAssets.Images.Controls.Button;
             SpriteFont font = GameContent.GameAssets.Fonts.NormalText;

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
             SecondWeapLabel = new TextSprite(Sprites.SpriteBatch, font, ("Use Secondary Weapon: RClick"));
             SecondWeapLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f);
             SecondWeapLabel.Color = Color.White;
#endif

#if XBOX
             SecondWeapLabel = new TextSprite(Sprites.SpriteBatch, font, ("Use Secondary Weapon: LTrigger"));
             SecondWeapLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .10f);
             SecondWeapLabel.Color = Color.White;
#endif 



#if WINDOWS
             SwitchSecondWeapLabel = new TextSprite(Sprites.SpriteBatch, font, ("Switch Secondary Weapons: Scroll Wheel"));
             SwitchSecondWeapLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f);
             SwitchSecondWeapLabel.Color = Color.White;
#endif

#if XBOX
             SwitchSecondWeapLabel = new TextSprite(Sprites.SpriteBatch, font, ("Switch Secondary Weapons: \n Left and Right Bumpers"));
             SwitchSecondWeapLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f);
             SwitchSecondWeapLabel.Color = Color.White;
#endif


#if WINDOWS
             DeployDronesLabel = new TextSprite(Sprites.SpriteBatch, font, ("Deploy Fighter Carrier Drones: LShift"));
             DeployDronesLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .6f);
             DeployDronesLabel.Color = Color.White; 
#endif

#if XBOX
             DeployDronesLabel = new TextSprite(Sprites.SpriteBatch, font, ("Deploy Fighter Carrier Drones: A Button"));
             DeployDronesLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f);
             DeployDronesLabel.Color = Color.White;
#endif
             BackButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
             BackLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .139f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .62f), GameContent.GameAssets.Fonts.NormalText, "Back");
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
#endif
             Sprites.Add(BackButton);

             //Add all text sprites
             AdditionalSprites.Add(BackLabel);
             AdditionalSprites.Add(MoveLabel);
             AdditionalSprites.Add(FireLabel);
             AdditionalSprites.Add(SecondWeapLabel);
             AdditionalSprites.Add(SwitchSecondWeapLabel);
             AdditionalSprites.Add(DeployDronesLabel);


             StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

         }

         public override void Update(GameTime game)
         {
#if WINDOWS            
             MouseState currentMouseState = MouseManager.CurrentMouseState;

             if (BackLabel.IsSelected && BackButton.ClickCheck(currentMouseState) && !BackButton.ClickCheck(lastMS))
             {
                 StateManager.GoBack();
        
             }

             if (MoveLabel.IsSelected && currentMouseState.LeftButton == ButtonState.Pressed && lastMS.LeftButton != ButtonState.Pressed)
             {
                 StateManager.Options.ArrowKeysEnabled = !StateManager.Options.ArrowKeysEnabled;
                 MoveLabel.Text = String.Format("Move: {0}", StateManager.Options.ArrowKeysEnabled ? "Arrow" : "WASD");
             }

             if (FireLabel.IsSelected && currentMouseState.LeftButton == ButtonState.Pressed && lastMS.LeftButton != ButtonState.Pressed)
             {
                 StateManager.Options.LeftButtonEnabled = !StateManager.Options.LeftButtonEnabled;
                 FireLabel.Text = String.Format("Fire:{0}", StateManager.Options.LeftButtonEnabled ? "LClick" : "Space");
             }
            

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

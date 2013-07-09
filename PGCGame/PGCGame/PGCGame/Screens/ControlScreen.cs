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
    class ControlScreen : BaseScreen
    {
         public ControlScreen(SpriteBatch spriteBatch, Delegates.QuitFunction exit)
            : base(spriteBatch, Color.Black)
         {

        }

         TextSprite BackLabel;

         public override void InitScreen(ScreenType screenType)
         {
             base.InitScreen(screenType);

             Texture2D button = GameContent.GameAssets.Images.Controls.Button;
             SpriteFont font = GameContent.GameAssets.Fonts.NormalText;


             Sprite BackButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
             BackLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .139f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .62f), GameContent.GameAssets.Fonts.NormalText, "Back");
             BackLabel.Color = Color.White;

             Sprites.Add(BackButton);
             AdditionalSprites.Add(BackLabel);


             StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

         }

         void Options_ScreenResolutionChanged(object sender, EventArgs e)
         {
             //Whatever code you're doing to place your buttons and whatnot, also place it here.
         }
    }
}

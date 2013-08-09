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
using Glib.XNA.InputLib;

namespace PGCGame.Screens
{
    public abstract class BaseSelectScreen : BaseScreen
    {
        protected bool _firstTimeInit = true;

        public BaseSelectScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            items = new List<KeyValuePair<Sprite, TextSprite>>();
            ButtonClick = GameContent.GameAssets.Sound[SoundEffectType.ButtonPressed];
#if XBOX
            GamePadManager.One.Buttons.LeftShoulderPressed += new EventHandler(Buttons_LeftShoulderPressed);
            GamePadManager.One.Buttons.RightShoulderPressed += new EventHandler(Buttons_RightShoulderPressed);
            GamePadManager.One.Buttons.AButtonPressed += new EventHandler(Buttons_AButtonPressed);
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
#endif 
        }
        

#if XBOX
        TimeSpan totalButtonTime = TimeSpan.FromMilliseconds(250);
        TimeSpan passedButtonTime = TimeSpan.Zero;

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                passedButtonTime = TimeSpan.Zero;
            }
        }

        void Buttons_AButtonPressed(object sender, EventArgs e)
        {
            if (Visible && passedButtonTime > totalButtonTime && nextButtonClicked != null)
            {
                nextButtonClicked(this, EventArgs.Empty);
            }
        }

        void Buttons_LeftShoulderPressed(object sender, EventArgs e)
        {
            if (Visible)
            {
                items[selected].Key.Color = Color.Transparent;
                items[selected].Value.Color = Color.Transparent;

                /*
                    selected++;
                    if (selected == items.Count)
                    {
                        selected -= items.Count;
                    }
                }
                else
                {
                 */
                    selected--;
                    if (selected < 0)
                    {
                        selected += items.Count;
                    }

                items[selected].Key.Color = Color.White;
                items[selected].Value.Color = Color.White;

                if (ChangeItem != null)
                {
                    ChangeItem(this, new EventArgs());
                }
            }
        }

        void Buttons_RightShoulderPressed(object sender, EventArgs e)
        {
            if (Visible)
            {
                items[selected].Key.Color = Color.Transparent;
                items[selected].Value.Color = Color.Transparent;
                selected++;
                if (selected == items.Count)
                {
                    selected -= items.Count;
                }

                items[selected].Key.Color = Color.White;
                items[selected].Value.Color = Color.White;
                if (ChangeItem != null)
                {
                    ChangeItem(this, new EventArgs());
                }
                /*
                selected--;
                if (selected < 0)
                {
                    selected += items.Count;
                }

                items[selected].Key.Color = Color.White;
                items[selected].Value.Color = Color.White;

                if (ChangeItem != null)
                {
                    ChangeItem(this, new EventArgs());
                }
                 */
            }
        }
#endif
        protected TextSprite acceptLabel;
#if WINDOWS
        protected TextSprite backLabel;
#endif
        protected TextSprite leftLabel;
        protected TextSprite rightLabel;
        protected TextSprite nameLabel;
        protected Sprite acceptButton;

        public event EventHandler nextButtonClicked;

        protected int selected = 0;

        protected List<KeyValuePair<Sprite, TextSprite>> items;

        public override void InitScreen(ScreenType screenType)
        {
            base.InitScreen(screenType);

            Texture2D buttonImage = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;

            acceptButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .7f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .8f), Sprites.SpriteBatch);
            acceptLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Play");
            acceptLabel.Position = new Vector2((acceptButton.X + acceptButton.Width / 2) - acceptLabel.Width / 2, (acceptButton.Y + acceptButton.Height / 2) - acceptLabel.Height / 2);
            acceptLabel.Color = Color.White;
            acceptLabel.IsHoverable = true;
            acceptLabel.IsManuallySelectable = true;
            acceptLabel.NonHoverColor = Color.White;
            acceptLabel.HoverColor = Color.MediumAquamarine;
            acceptLabel.ParentSprite = acceptButton;

#if WINDOWS
            acceptButton.MouseEnter += new EventHandler(playButton_MouseEnter);
            acceptButton.MouseLeave += new EventHandler(playButton_MouseLeave);
#endif

            Sprites.Add(acceptButton);
            AdditionalSprites.Add(acceptLabel);

#if WINDOWS
            Sprite backButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .8f), Sprites.SpriteBatch);
            backLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Back");
            backLabel.Position = new Vector2((backButton.X + backButton.Width / 2) - backLabel.Width / 2, (backButton.Y + backButton.Height / 2) - backLabel.Height / 2);
            backLabel.Color = Color.White;
            backLabel.IsHoverable = true;
            backLabel.IsManuallySelectable = true;
            backLabel.NonHoverColor = Color.White;
            backLabel.HoverColor = Color.MediumAquamarine;

            backButton.MouseEnter += new EventHandler(backButton_MouseEnter);
            backButton.MouseLeave += new EventHandler(backButton_MouseLeave);

            Sprites.Add(backButton);
            AdditionalSprites.Add(backLabel);
#endif



            Sprite leftButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .5f), Sprites.SpriteBatch);
            leftButton.Scale = new Vector2(0.5f, 1);
            leftLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(5,5), SegoeUIMono, "<<<");
            leftLabel.Position = new Vector2((leftButton.X + leftButton.Width / 2) - leftLabel.Width / 2, (leftButton.Y + leftButton.Height / 2) - leftLabel.Height / 2);
            leftLabel.Color = Color.White;
            leftLabel.IsHoverable = true;
            leftLabel.IsManuallySelectable = true;
            leftLabel.NonHoverColor = Color.White;
            leftLabel.HoverColor = Color.MediumAquamarine;

#if WINDOWS
            leftButton.MouseEnter += new EventHandler(leftButton_MouseEnter);
            leftButton.MouseLeave += new EventHandler(leftButton_MouseLeave);
#endif

            Sprites.Add(leftButton);
            AdditionalSprites.Add(leftLabel);

            Sprite rightButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .9f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .5f), Sprites.SpriteBatch);
            rightButton.Scale = new Vector2(0.5f, 1);
            rightLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(5,5), SegoeUIMono, ">>>");
            rightLabel.Position = new Vector2((rightButton.X + rightButton.Width / 2) - rightLabel.Width / 2, (rightButton.Y + rightButton.Height / 2) - rightLabel.Height / 2);
            rightLabel.Color = Color.White;
            rightLabel.IsHoverable = true;
            rightLabel.IsManuallySelectable = true;
            rightLabel.NonHoverColor = Color.White;
            rightLabel.HoverColor = Color.MediumAquamarine;

#if WINDOWS
            rightButton.MouseEnter += new EventHandler(rightButton_MouseEnter);
            rightButton.MouseLeave += new EventHandler(rightButton_MouseLeave);
#endif
            Sprites.Add(rightButton);
            AdditionalSprites.Add(rightLabel);

            nameLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Name Of Item");
                                                //right side of left button              //left of right button   //right side of left button 
            nameLabel.Position = new Vector2(((leftButton.X + leftButton.Texture.Width * leftButton.Scale.X) + (rightButton.X - (leftButton.X + leftButton.Texture.Width * leftButton.Scale.X))/2) - nameLabel.Width * .5f, (leftButton.Y + leftButton.Texture.Height/2) - nameLabel.Height/2);
            nameLabel.Color = Color.White;

            AdditionalSprites.Add(nameLabel);

            for (int i = 0; i < items.Count; i++)
            {
                if (i != 0)
                {
                    items[i].Key.Color = Color.Transparent;
                    items[i].Value.Color = Color.Transparent;
                }

                Sprites.Add(items[i].Key);
                AdditionalSprites.Add(items[i].Value);
            }

            
            if (ChangeItem != null)
            {
                ChangeItem(this, new EventArgs());
            }

            _firstTimeInit = false;
        }

        //buybutton
        void buyButton_MouseLeave(object sender, EventArgs e)
        {
            acceptLabel.IsSelected = false;
        }
        void buyButton_MouseEnter(object sender, EventArgs e)
        {
            acceptLabel.IsSelected = true;
        }

        //rightbutton
        void rightButton_MouseLeave(object sender, EventArgs e)
        {
            rightLabel.IsSelected = false;
        }
        void rightButton_MouseEnter(object sender, EventArgs e)
        {
            rightLabel.IsSelected = true;
        }

        public bool mouseOnRightButton
        {
            get
            {
                return rightLabel.IsSelected;
            }
        }

        //leftbutton
        void leftButton_MouseLeave(object sender, EventArgs e)
        {
            leftLabel.IsSelected = false;
        }
        void leftButton_MouseEnter(object sender, EventArgs e)
        {
            leftLabel.IsSelected = true;
        }

        public bool mouseOnLeftButton
        {
            get
            {
                return leftLabel.IsSelected;
            }
        }
#if WINDOWS
        bool mouseInbackButton = false;
        //backbutton
        void backButton_MouseLeave(object sender, EventArgs e)
        {
            backLabel.IsSelected = false;
            mouseInbackButton = false;
        }
        void backButton_MouseEnter(object sender, EventArgs e)
        {
            backLabel.IsSelected = true;
            mouseInbackButton = true;
        }

        //Same length arrays, when you hit the arrow key, i++ (in both of them)

        bool mouseInplayButton = false;
        //playbutton
        void playButton_MouseLeave(object sender, EventArgs e)
        {
            acceptLabel.IsSelected = false;
            mouseInplayButton = false;
        }
        void playButton_MouseEnter(object sender, EventArgs e)
        {
            acceptLabel.IsSelected = true;
            mouseInplayButton = true;
        }

        private MouseState lastMs = new MouseState(0, 0, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
#endif

        public static event EventHandler ChangeItem;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
#if WINDOWS
            MouseState currentMs = MouseManager.CurrentMouseState;
            if (lastMs.LeftButton == ButtonState.Released && currentMs.LeftButton == ButtonState.Pressed)
            {
                if (mouseInplayButton)
                {
                    //if(StateManager.Options.SFXEnabled)
                    //{
                    //  ButtonClick.Play();
                    //}

                    if (nextButtonClicked != null)
                    {
                        nextButtonClicked(this, new EventArgs());
                    }
                }
                else if (mouseInbackButton)
                {
                    if (StateManager.Options.SFXEnabled)
                    {
                        ButtonClick.Play();
                    }

                    StateManager.GoBack();
                }
                else if (mouseOnLeftButton || mouseOnRightButton)
                {
                    items[selected].Key.Color = Color.Transparent;
                    items[selected].Value.Color = Color.Transparent;

                    if (mouseOnRightButton)
                    {
                        if (StateManager.Options.SFXEnabled)
                        {
                            ButtonClick.Play();
                        }

                        selected++;
                        if (selected == items.Count)
                        {
                            selected -= items.Count;
                        }
                    }
                    else
                    {
                        if (StateManager.Options.SFXEnabled)
                        {
                            ButtonClick.Play();
                        }

                        selected--;
                        if (selected < 0)
                        {
                            selected += items.Count;
                        }
                    }

                    items[selected].Key.Color = Color.White;
                    items[selected].Value.Color = Color.White;

                    if (ChangeItem != null)
                    {
                        ChangeItem(this, new EventArgs());
                    }

                }



            }
            lastMs = currentMs;
#elif XBOX
            passedButtonTime += gameTime.ElapsedGameTime;
#endif
        }
    }
}

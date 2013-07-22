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

namespace PGCGame.Screens.SelectScreens
{
    public class WeaponSelectScreen : BaseSelectScreen
    {
        //TODO: Buying a weapon should add that weapon to the ship's secondary weapon list.

        public WeaponSelectScreen(SpriteBatch spriteBatch)
            : base(spriteBatch)
        {

        }

        List<SecondaryWeapon> itemsShown = new List<SecondaryWeapon>();
        TextSprite SpaceBucksAmount;

        public override void InitScreen(ScreenType screenType)
        {
            Texture2D image = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
            Texture2D EMP = GameContent.GameAssets.Images.SecondaryWeapon[SecondaryWeaponType.EMP, TextureDisplayType.ShopDisplay];
            Texture2D RayGun = GameContent.GameAssets.Images.SecondaryWeapon[SecondaryWeaponType.ShrinkRay, TextureDisplayType.ShopDisplay];
            Texture2D Bomb = GameContent.GameAssets.Images.SecondaryWeapon[SecondaryWeaponType.SpaceMine, TextureDisplayType.ShopDisplay];

            SpaceBucksAmount = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, string.Format("You have {0} credits", StateManager.SpaceBucks), Color.White);
            SpaceBucksAmount.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width / 2 - SpaceBucksAmount.Width, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f);
            AdditionalSprites.Add(SpaceBucksAmount);

            SpriteFont font = GameContent.GameAssets.Fonts.NormalText;
            

            //EMP
            EMP weapon1 = new EMP(EMP, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), Sprites.SpriteBatch);
            TextSprite text1 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), font, "\n\nAn electro magnetic pulse. \nThis device disables all \nnearby enemy ships \nwithin your ship's range. \n\ncost: 1000 credits", Color.White);
            weapon1.Scale = new Vector2(0.5f, 0.5f);
           
            
            itemsShown.Add(weapon1);



            //Ray Gun
            ShrinkRay weapon2 = new ShrinkRay(RayGun, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.60f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.2f), Sprites.SpriteBatch);
            weapon2.Scale = new Vector2(0.5f, 0.5f);

            itemsShown.Add(weapon2);

            //Space Mine
            SpaceMine weapon3 = new SpaceMine(Bomb, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.69f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.3f), Sprites.SpriteBatch);
            weapon3.Scale = new Vector2(2f, 2f);

            itemsShown.Add(weapon3);

            TextSprite text2 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), font, "\n\nThis weapon causes the targeted enemy\n to shrink losing 33% of their health. \nThis does not affect bosses.\n\ncost: 2000 credits", Color.White);
            TextSprite text3 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), font, "\n\nYou place this mine anywhere in space \nand when an enemy crashes into it the mine \nexplodes \n\ncost: 500 credits.", Color.White);

            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon2, text2));
            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon3, text3));

            
            ChangeItem +=new EventHandler(WeaponSelectScreen_ChangeItem);

            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon1, text1));
            
            base.InitScreen(screenType);
            
            acceptLabel.Text = "Buy";
            //In clicked code
            this.nextButtonClicked += new EventHandler(WeaponSelectScreen_nextButtonClicked);
        }



        void WeaponSelectScreen_nextButtonClicked(object sender, EventArgs e)
        {
            foreach (SecondaryWeapon item in itemsShown)
            {
                if (item.Texture == items[selected].Key.Texture && item.Cost <= StateManager.SpaceBucks)
                {
                    StateManager.SpaceBucks -= item.Cost;
                    SpaceBucksAmount.Text = string.Format("You have {0} credits", StateManager.SpaceBucks);
                    if (item.GetType() == typeof(SpaceMine))
                    {
                        StateManager.PowerUps[0].Push(new SpaceMine(item.Texture, Vector2.Zero, Sprites.SpriteBatch));
                    }
                    else if (item.GetType() == typeof(EMP))
                    {
                        StateManager.PowerUps[2].Push(new EMP(GameContent.GameAssets.Images.SecondaryWeapon[SecondaryWeaponType.EMP, TextureDisplayType.InGameUse], Vector2.Zero, Sprites.SpriteBatch));
                    }
                    if (item.GetType() == typeof(ShrinkRay))
                    {
                        StateManager.PowerUps[1].Push(new ShrinkRay(item.Texture, Vector2.Zero, Sprites.SpriteBatch));
                    }
                    break;
                }
            }
        }


        void WeaponSelectScreen_ChangeItem(object sender, EventArgs e)
        {
            foreach (SecondaryWeapon item in itemsShown)
            {
                if (item.Texture == items[selected].Key.Texture)
                {
                    nameLabel.Text = item.Name;

                    break;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (StateManager.IsWSFirstUpdate == true)
            {
                SpaceBucksAmount.Text = string.Format("You have {0} credits", StateManager.SpaceBucks);
                StateManager.IsWSFirstUpdate = false;
            }
            base.Update(gameTime);
        }
    }
}

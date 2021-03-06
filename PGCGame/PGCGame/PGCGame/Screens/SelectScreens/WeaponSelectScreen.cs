﻿using System;
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
            ItemBought = GameContent.Assets.Sound[SoundEffectType.BoughtItem];
        }

        List<SecondaryWeapon> itemsShown = new List<SecondaryWeapon>();
        TextSprite SpaceBucksAmount;
        TextSprite ItemAmount;
        //bool bought = false;

        public override void InitScreen(ScreenType screenType)
        {
            Shop.PurchaseScreenSelected += new EventHandler(Shop_PurchaseScreenSelected);
            Texture2D image = GameContent.Assets.Images.NonPlayingObjects.Planet;
            Texture2D EMP = GameContent.Assets.Images.SecondaryWeapon[SecondaryWeaponType.EMP, TextureDisplayType.ShopDisplay];
            Texture2D RayGun = GameContent.Assets.Images.SecondaryWeapon[SecondaryWeaponType.ShrinkRay, TextureDisplayType.ShopDisplay];
            Texture2D Bomb = GameContent.Assets.Images.SecondaryWeapon[SecondaryWeaponType.SpaceMine, TextureDisplayType.ShopDisplay];
            Texture2D HealthPack = GameContent.Assets.Images.Equipment[EquipmentType.HealthPack, TextureDisplayType.ShopDisplay];

            SpaceBucksAmount = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.Assets.Fonts.NormalText, string.Format("You have {0} credits", StateManager.SpaceBucks), Color.White);
            //SpaceBucksAmount.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width / 2 - SpaceBucksAmount.Width, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f);

            ItemAmount = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.Assets.Fonts.NormalText, string.Format("You have x{0} EMPs", StateManager.PowerUps[2].Count), Color.White);
            ItemAmount.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width / 2 - SpaceBucksAmount.Width, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .15f);

            AdditionalSprites.Add(SpaceBucksAmount);
            AdditionalSprites.Add(ItemAmount);

            SpriteFont font = GameContent.Assets.Fonts.NormalText;


            //EMP
            EMP weapon1 = new EMP(EMP, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.75f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.3f), Sprites.SpriteBatch);
            TextSprite text1 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), font, "\n\nAn electro magnetic pulse. \nThis device disables all \nnearby enemy ships \nwithin your ship's range. \n\ncost: 1000 credits", Color.White);


            weapon1.Scale = new Vector2(0.5f, 0.5f);


            itemsShown.Add(weapon1);

            //Ray Gun
            ShrinkRay weapon2 = new ShrinkRay(RayGun, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.64f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.2f), Sprites.SpriteBatch);
            weapon2.Scale = new Vector2(0.5f, 0.5f);

            itemsShown.Add(weapon2);

            //Space Mine
            SpaceMine weapon3 = new SpaceMine(Bomb, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.69f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.3f), Sprites.SpriteBatch);
            weapon3.Scale = new Vector2(2f, 2f);

            itemsShown.Add(weapon3);

            TextSprite text2 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), font, "\n\nThis weapon causes the targeted enemy\nto shrink losing 33% of their health. \nThis does not affect bosses.\n\ncost: 2000 credits", Color.White);
            TextSprite text3 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), font, "\n\nYou place this mine anywhere in space \nand when an enemy crashes into it the mine \nexplodes \n\ncost: 500 credits.", Color.White);

            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon2, text2));
            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon3, text3));


            //HealthPack

            HealthPack weapon4 = new HealthPack(HealthPack, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.74f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.3f), Sprites.SpriteBatch);
            TextSprite text4 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 1.5f), font, "\n\n\nThis power up regenerates\nyour health up\nby 50%\n\nCost: " + weapon4.Cost, Color.White);
            text4.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.05f);
            weapon4.Scale = new Vector2(0.5f, 0.5f);

            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon4, text4));
            itemsShown.Add(weapon4);

            ChangeItem += new EventHandler(WeaponSelectScreen_ChangeItem);

            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon1, text1));

            base.InitScreen(screenType);

            acceptLabel.Text = "Buy";
            //In clicked code
            this.nextButtonClicked += new EventHandler(WeaponSelectScreen_nextButtonClicked);
        }

        void Shop_PurchaseScreenSelected(object sender, EventArgs e)
        {
            SpaceBucksAmount.Text = string.Format("You have {0} credits", StateManager.SpaceBucks);
        }



        void WeaponSelectScreen_nextButtonClicked(object sender, EventArgs e)
        {
            foreach (SecondaryWeapon item in itemsShown)
            {
                if (item.Texture == items[selected].Key.Texture && item.Cost <= StateManager.SpaceBucks)
                {
                    StateManager.SpaceBucks -= item.Cost;

                    if (StateManager.Options.SFXEnabled)
                    {
                        ItemBought.Play();
                    }

                    SpaceBucksAmount.Text = string.Format("You have {0} credits", StateManager.SpaceBucks);
                    if (item.GetType() == typeof(SpaceMine))
                    {
                        StateManager.PowerUps[0].Push(new SpaceMine(item.Texture, Vector2.Zero, Sprites.SpriteBatch));
                    }
                    else if (item.GetType() == typeof(EMP))
                    {
                        StateManager.PowerUps[2].Push(new EMP(GameContent.Assets.Images.SecondaryWeapon[SecondaryWeaponType.EMP, TextureDisplayType.InGameUse], Vector2.Zero, Sprites.SpriteBatch));
                    }
                    else if (item.GetType() == typeof(ShrinkRay))
                    {
                        StateManager.PowerUps[1].Push(new ShrinkRay(item.Texture, Vector2.Zero, Sprites.SpriteBatch));
                    }
                    else if (item.GetType() == typeof(HealthPack))
                    {
                        StateManager.PowerUps[3].Push(new HealthPack(item.Texture, Vector2.Zero, Sprites.SpriteBatch));
                    }
                    break;
                }

            }
        }

        int itemSelected = 0;
        String itemName = "SpaceMine";

        void WeaponSelectScreen_ChangeItem(object sender, EventArgs e)
        {
            foreach (SecondaryWeapon item in itemsShown)
            {
                if (item.Texture == items[selected].Key.Texture)
                {
                    nameLabel.Text = item.Name;
                    itemName = item.Name;
                    if (item.GetType() == typeof(SpaceMine))
                    {
                        itemSelected = 0;
                    }
                    else if (item.GetType() == typeof(EMP))
                    {
                        itemSelected = 2;
                    }
                    else if (item.GetType() == typeof(ShrinkRay))
                    {
                        itemSelected = 1;
                    }
                    else if (item.GetType() == typeof(HealthPack))
                    {
                        itemSelected = 3;
                    }
                    break;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            ItemAmount.Text = "You have x" + StateManager.PowerUps[itemSelected].Count + " " + itemName + "(s)";
            AdditionalSprites.Remove(ItemAmount);
            AdditionalSprites.Add(ItemAmount);

            base.Update(gameTime);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Input;
using Glib;

namespace PGCGame
{
    public class HealthPack : SecondaryWeapon
    {

        public HealthPack(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            UseCenterAsOrigin = true;
            Scale = Vector2.Zero;
            Cost = 2000;
            Name = "Health Pack";
            DeploySound = GameContent.GameAssets.Sound[SoundEffectType.DeployHealthPack];
        }

        public override void Update(GameTime currentGameTime)
        {
            base.Update();
            if (ParentShip.CurrentHealth < ParentShip.InitialHealth)
            {
                DeploySound.Play();
                ParentShip.CurrentHealth += ParentShip.InitialHealth / 2;
                FireKilledEvent();
            }
        }
    }
}

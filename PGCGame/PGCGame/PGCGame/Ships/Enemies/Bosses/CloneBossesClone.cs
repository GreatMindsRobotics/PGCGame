using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using PGCGame.CoreTypes;


namespace PGCGame
{
    class CloneBossesClone : CloneBoss
    {
        public CloneBossesClone(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Scale = new Vector2(.75f);
            BulletTexture = GameContent.GameAssets.Images.Ships.Bullets[ShipType.Drone, ShipTier.Tier1];

            DamagePerShot = 50;
            MovementSpeed = new Vector2(.5f);
            _initHealth = 2000;

            PlayerType = CoreTypes.PlayerType.Enemy;

            killWorth = 100;
        }
    }
}

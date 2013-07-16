using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Glib.XNA;
using PGCGame.CoreTypes;

namespace PGCGame.Ships.Enemies
{
    public class EnemyBattleCruiser : BaseEnemyShip
    {
        public EnemyBattleCruiser(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Scale = new Vector2(.75f);

            DamagePerShot = 10;
            MovementSpeed = new Vector2(.7f);
            _initHealth = 80;

            BulletTexture = GameContent.GameAssets.Images.Ships.Bullets[ShipType.BattleCruiser, ShipTier.Tier1];
        }

        public override ShipType ShipType
        {
            get { return ShipType.EnemyBattleCruiser; }
        }

        static EnemyBattleCruiser()
        {
            _friendlyName = "Enemy Ship";
        }
    }
}






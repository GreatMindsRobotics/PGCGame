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
    public class EnemyDrone : BaseEnemyShip
    {
        public EnemyDrone(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Scale = new Vector2(.75f);

            DistanceToNose = .5f;

            BulletTexture = Ship.DroneBullet;
        }


        public override string TextureFolder
        {
            get { throw new NotImplementedException(); }
        }

        public override string FriendlyName
        {
            get { return "Enemy Ship"; }
        }
    }
}

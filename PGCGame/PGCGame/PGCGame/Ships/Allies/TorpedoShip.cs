using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PGCGame
{
    public class TorpedoShip : Ship
    {
        public TorpedoShip(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Scale = new Vector2(.925f);
            BulletTexture = Ship.Torpedo;
            MovementSpeed = new Vector2(1.333f);
            //MovementSpeed = new Vector2(1f);
            DelayBetweenShots = TimeSpan.FromSeconds(.75);
        }

        public override void Shoot()
        {
            throw new NotImplementedException();
        }

        public override string TextureFolder
        {
            get { return "Torpedo Ship"; }
        }
    }
}

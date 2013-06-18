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
    public class FighterCarrier : Ship
    {
        public FighterCarrier(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
        }

        public Drone[] Drones { get; set; }
        public event EventHandler BulletFired;

        public override void Shoot()
        {
            throw new NotImplementedException();
        }
    }
}

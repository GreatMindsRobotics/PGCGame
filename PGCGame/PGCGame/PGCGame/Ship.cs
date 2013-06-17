using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using Glib.XNA;
using Glib;
using Glib.XNA.SpriteLib;


namespace PGCGame
{
    public abstract class Ship : Sprite
    {
        //TODO: ALEX
        //public abstract void Shoot();

        //override: 
        //Update
        //Draw

        public int DamagePerShot { get; set; }
        public int Cost { get; set; }

        public Vector2 Speed { get; set; }

        public int CurrentHealth { get; set; }

        public int InitialHealth { get; set; }

        public int Shield { get; set; }

        public int Armor { get; set; }

        public List<Bullet> FlyingBullets { get; set; }
    }
}

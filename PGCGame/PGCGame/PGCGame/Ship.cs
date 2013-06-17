using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PGCGame
{
    public abstract class Ship
    {
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

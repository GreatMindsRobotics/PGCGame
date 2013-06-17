using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PGCGame
{
    public class Bullet
    {
        public int Damage { get; set; }
        public Vector2 Speed;
        public float XSpeed
        {
            get
            {
                return Speed.X;
            }
            set
            {
                Speed.X = value;
            }
        }

        public float YSpeed
        {
            get
            {
                return Speed.Y;
            }
            set
            {
                Speed.Y = value;
            }
        }
    }
}

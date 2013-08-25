using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PGCGame.CoreTypes
{
    public class BulletEventArgs : EventArgs
    {
        public Bullet Bullet { get; private set; }

        public Vector2 BulletPosition
        {
            get { return Bullet.Position; }
        }

        public float BulletRotation
        {
            get { return Bullet.Rotation.Radians; }
        }

        public Vector2 BulletSpeed
        {
            get { return Bullet.Speed; }
        }
        

        public BulletEventArgs(Bullet bullet)
        {
            Bullet = bullet;
        }
    }
}

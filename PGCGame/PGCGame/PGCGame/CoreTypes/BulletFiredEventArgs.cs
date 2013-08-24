using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PGCGame.CoreTypes
{
    public class BulletFiredEventArgs : EventArgs
    {
        public Bullet FiredBullet { get; private set; }

        public Vector2 BulletPosition
        {
            get { return FiredBullet.Position; }
        }

        public float BulletRotation
        {
            get { return FiredBullet.Rotation.Radians; }
        }

        public Vector2 BulletSpeed
        {
            get { return FiredBullet.Speed; }
        }
        

        public BulletFiredEventArgs(Bullet fired)
        {
            FiredBullet = fired;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PGCGame.Ships.Enemies
{
    public class EnemyDrone : Ship
    {
        public EnemyDrone(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Scale = new Vector2(.75f);
            RotateTowardsMouse = false;

            BulletTexture = Ship.DroneBullet;
        }

        public override void Shoot()
        {
            Bullet shotBullet = new Bullet(BulletTexture, WorldCoords, SpriteBatch);

            shotBullet.Damage = DamagePerShot;
            shotBullet.Speed = Rotation.AsVector() * 6f;

            shotBullet.UseCenterAsOrigin = true;
            shotBullet.Rotation = Rotation;

            FlyingBullets.Add(shotBullet);
        }


        public override string TextureFolder
        {
            get { throw new NotImplementedException(); }
        }

        public override void DrawNonAuto()
        {
            base.DrawNonAuto();
        }
    }
}

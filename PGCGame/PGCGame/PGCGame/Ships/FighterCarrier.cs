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
        public FighterCarrier(Texture2D texture, Vector2 location, SpriteBatch spriteBatch, Texture2D droneTexture)
            : base(texture, location, spriteBatch)
        {
            //UseCenterAsOrigin = true;
            Drones[0] = new Drone(droneTexture, Vector2.Zero, spriteBatch, this);
            Drones[0].RelativePosition = ShipRelativePosition.TopLeft;
            Drones[1] = new Drone(droneTexture, Vector2.Zero, spriteBatch, this);
            Drones[1].RelativePosition = ShipRelativePosition.BottomRight;
            BulletTexture = Ship.FighterCarrierBullet;
            DelayBetweenShots = TimeSpan.FromSeconds(.5);

        }

        public Drone[] Drones = new Drone[2];
        public event EventHandler BulletFired;

        public override void Shoot()
        {
            //TODO: Fire bullet
            //Ben magic: Targeting
            Bullet bullet = new Bullet(BulletTexture, Position, SpriteBatch);
            MouseState ms = Mouse.GetState();
            bullet.Damage = DamagePerShot;
            Vector2 mousePos = new Vector2(ms.X, ms.Y);
            Vector2 slope = mousePos - Position;
            slope.Normalize();
            bullet.Speed = slope;
            FlyingBullets.Add(bullet);
            if (BulletFired != null)
            {
                BulletFired(this, EventArgs.Empty);
            }
        }

        public override void Update()
        {
            base.Update();
            foreach (Drone d in Drones)
            {
                d.Update();
            }
        }

        public override void DrawNonAuto()
        {
            base.DrawNonAuto();
            foreach (Drone d in Drones)
            {
                d.DrawNonAuto();
            }
        }

        public override string TextureFolder
        {
            get { return "Fighter Carrier"; }
        }
    }
}

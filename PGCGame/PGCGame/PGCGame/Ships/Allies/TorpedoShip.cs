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
using PGCGame.CoreTypes;

namespace PGCGame
{
    public class TorpedoShip : Ship
    {
        //TODO: Scaling logic

        public TorpedoShip(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Scale = new Vector2(.925f);
            BulletTexture = Ship.Torpedo;
            MovementSpeed = new Vector2(1.333f);
            //MovementSpeed = new Vector2(1f);
            DelayBetweenShots = TimeSpan.FromSeconds(.75);
            this.TierChanged += new EventHandler(TorpedoShip_TierChanged);
        }

        void TorpedoShip_TierChanged(object sender, EventArgs e)
        {
            if (Tier == ShipTier.Tier1)
            {
                Scale = new Vector2(.85f);
                DistanceToNose = .5f;
            }
            else if (Tier == ShipTier.Tier2)
            {
                Scale = new Vector2(.85f);
                DistanceToNose = .50f;
            }
            else if (Tier == ShipTier.Tier3)
            {
                Scale = new Vector2(.85f);
                DistanceToNose = .488f;
            }
            else if (Tier == ShipTier.Tier4)
            {
                Scale = new Vector2(.85f);
                DistanceToNose = .5f;
            }
        }
        public event EventHandler BulletFired;

        public override void Shoot()
        {
            //TODO: change image to torpedo
            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, WorldSb);
            
            bullet.Speed = Rotation.Vector * 6f;
            bullet.UseCenterAsOrigin = true;
            bullet.Rotation = Rotation;
            bullet.Damage = DamagePerShot;
            //Vector2 mousePos = new Vector2(ms.X, ms.Y);
            //Vector2 slope = mousePos - Position;
            //slope.Normalize();
            //bullet.Speed = slope;
            FlyingBullets.Add(bullet);
            if (BulletFired != null)
            {
                BulletFired(this, EventArgs.Empty);
            }
        }
        public override string TextureFolder
        {
            get { return "Torpedo Ship"; }
        }
    }
}

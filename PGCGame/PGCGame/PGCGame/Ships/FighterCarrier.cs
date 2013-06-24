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

using Glib;
using PGCGame.CoreTypes;
using Glib.XNA.SpriteLib;

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
            DelayBetweenShots = TimeSpan.FromMilliseconds(55);
            DamagePerShot = 2;
            InitialHealth = 100;
            MovementSpeed = Vector2.One;
            this.TierChanged += new EventHandler(FighterCarrier_TierChanged);

        }

        void FighterCarrier_TierChanged(object sender, EventArgs e)
        {
            if (Tier == ShipTier.Tier1)
            {
                Scale = new Vector2(.7f);
                DistanceToNose = 3f/8f;
            }
            else if (Tier == ShipTier.Tier2)
            {
                //79 to 115
                Scale = new Vector2(.7f);
                DistanceToNose = .15f;
            }
            else if (Tier == ShipTier.Tier3)
            {
                Scale = new Vector2(.7f);
                DistanceToNose = .28f;
            }
            else if (Tier == ShipTier.Tier4)
            {
                Scale = new Vector2(.75f);
                DistanceToNose = .5f;
            }
        }

        private bool _dronesInitialized = false;
        

        public Drone[] Drones = new Drone[2];
        public event EventHandler BulletFired;

        public override void Shoot()
        {
            //TODO: Fire bullet
            //Glen's mom magic: Targeting

            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.AsVector(), WorldSb);
            
            bullet.Speed = Rotation.AsVector()*1.5f;
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

        public List<Bullet> DroneBullets = new List<Bullet>();

        public override void Update()
        {
            if (!_dronesInitialized)
            {
                _dronesInitialized = true;
                Drones[0].Origin = Origin;
                Drones[1].Origin = Origin;
            }
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

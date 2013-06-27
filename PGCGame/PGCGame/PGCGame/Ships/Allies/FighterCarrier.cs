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

            //Init drones
            Drones.Add(new Drone(droneTexture, Vector2.Zero, spriteBatch, this) { DroneState = CoreTypes.DroneState.Stowed });
            Drones[0].Rotation.Radians = MathHelper.Pi;
            Drones.Add(new Drone(droneTexture, Vector2.Zero, spriteBatch, this) { DroneState = CoreTypes.DroneState.Stowed });
            Drones[1].Rotation.Radians = MathHelper.TwoPi;
            
            BulletTexture = Ship.FighterCarrierBullet;
            DelayBetweenShots = TimeSpan.FromMilliseconds(100);
            DamagePerShot = 2;
            InitialHealth = 100;
            MovementSpeed = Vector2.One;
            this.TierChanged += new EventHandler(FighterCarrier_TierChanged);
        }

        void FighterCarrier_TierChanged(object sender, EventArgs e)
        {
            if (Tier == ShipTier.Tier1)
            {
                Scale = new Vector2(.55f);
                DistanceToNose = .4f;
            }
            else if (Tier == ShipTier.Tier2)
            {
                Scale = new Vector2(.55f);
                DistanceToNose = .155f;
            }
            else if (Tier == ShipTier.Tier3)
            {
                Scale = new Vector2(.55f);
                DistanceToNose = .28f;
            }
            else if (Tier == ShipTier.Tier4)
            {
                Scale = new Vector2(.55f);
                DistanceToNose = .5f;
            }
        }
     
        public List<Drone> Drones = new List<Drone>();
        public event EventHandler BulletFired;

        public override void Shoot()
        {
            //TODO: Fire bullet
            //Glen's mom magic: Targeting

            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.AsVector(), WorldSb);
            
            bullet.Speed = Rotation.AsVector()*3f;
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
            base.Update();

            foreach (Drone d in Drones)
            {
                d.Update();
            }
        }

        public override void DrawNonAuto()
        {
            //IMPORTANT: Draw drones first!
            foreach (Drone d in Drones)
            {
                d.DrawNonAuto();
            }

            base.DrawNonAuto();         
        }

        public override string TextureFolder
        {
            get { return "Fighter Carrier"; }
        }
    }
}

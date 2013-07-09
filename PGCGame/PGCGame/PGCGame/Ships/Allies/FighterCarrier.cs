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
using PGCGame.Ships.Allies;

namespace PGCGame
{
    public class FighterCarrier : BaseAllyShip
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

            BulletTexture = GameContent.GameAssets.Images.Ships.Bullets[ShipType.FighterCarrier, ShipTier.Tier1];
            DelayBetweenShots = TimeSpan.FromMilliseconds(100);
            DamagePerShot = 2;
            _initHealth = 100;
            MovementSpeed = Vector2.One;
            this.TierChanged += new EventHandler(FighterCarrier_TierChanged);
            DamagePerShot = 2;
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


        public List<Bullet> DroneBullets = new List<Bullet>();

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            foreach (Drone d in Drones)
            {
                d.Update(gt);
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

        public override ShipType ShipType
        {
            get { return ShipType.FighterCarrier; }
        }

        public override string FriendlyName
        {
            get { return "Fighter Carrier"; }
        }
    }
}

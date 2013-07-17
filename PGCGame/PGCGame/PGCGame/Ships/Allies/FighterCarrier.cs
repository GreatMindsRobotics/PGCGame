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
using Glib.XNA;
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
            Drones.Add(new Drone(droneTexture, location, spriteBatch, this) { DroneState = CoreTypes.DroneState.Stowed });
            Drones[0].Rotation.Radians = MathHelper.Pi;

            Drones.Add(new Drone(droneTexture, location, spriteBatch, this) { DroneState = CoreTypes.DroneState.Stowed });
            Drones[1].Rotation.Radians = MathHelper.TwoPi;

            BulletTexture = GameContent.GameAssets.Images.Ships.Bullets[ShipType.FighterCarrier, ShipTier.Tier1];
            DelayBetweenShots = TimeSpan.FromMilliseconds(100);
            DamagePerShot = 2;
            _initHealth = 100;
            MovementSpeed = Vector2.One;
            this.TierChanged += new EventHandler(FighterCarrier_TierChanged);
            DamagePerShot = 2;
            this.PlayerType = PlayerType.Ally;
        }

        void FighterCarrier_TierChanged(object sender, EventArgs e)
        {
            if (Tier == ShipTier.Tier1)
            {
                Scale = new Vector2(.55f);
                DistanceToNose = .4f;
                InitialHealth = 100;
                DamagePerShot = 2;
            }
            else if (Tier == ShipTier.Tier2)
            {
                Scale = new Vector2(.55f);
                DistanceToNose = .155f;
                InitialHealth = 120;
                DamagePerShot = 3;
            }
            else if (Tier == ShipTier.Tier3)
            {
                Scale = new Vector2(.55f);
                DistanceToNose = .28f;
                InitialHealth = 140;
                DamagePerShot = 4;
            }
            else if (Tier == ShipTier.Tier4)
            {
                Scale = new Vector2(.55f);
                DistanceToNose = .5f;
                InitialHealth = 160;
                DamagePerShot = 5;
            }
        }
     
        public List<Drone> Drones = new List<Drone>();

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            for(int d = 0; d < Drones.Count; d++)
            {
                Drones[d].WorldCoords = WorldCoords + Drones[d].Origin * Drones[d].Rotation.Radians.AngleToVector();
                Drones[d].Update(gt);
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

        static FighterCarrier()
        {
            _friendlyName = "Fighter Carrier";
        }
    }
}

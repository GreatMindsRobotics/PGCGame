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
using PGCGame.Ships.Allies;

namespace PGCGame
{
    public class TorpedoShip : BaseAllyShip
    {
        public static string ShipFriendlyName
        {
            get { return "Torpedo Ship"; }
        }

        //TODO: Scaling logic

        public TorpedoShip(Texture2D texture, Vector2 location, SpriteBatch spriteBatch, bool isAllyShip)
            : base(texture, location, spriteBatch, isAllyShip)
        {
            Scale = new Vector2(.925f);
            BulletTexture = GameContent.Assets.Images.Ships.Bullets[ShipType.TorpedoShip, ShipTier.Tier1];
            MovementSpeed = new Vector2(1.333f);
            //MovementSpeed = new Vector2(1f);
            DelayBetweenShots = TimeSpan.FromSeconds(.75);
            _initHealth = 110;
            this.TierChanged += new EventHandler(TorpedoShip_TierChanged);
            DamagePerShot = 5;
            ShootSound = GameContent.Assets.Sound[SoundEffectType.TorpedoShipFire];
        }

        void TorpedoShip_TierChanged(object sender, EventArgs e)
        {
            if (Tier == ShipTier.Tier1)
            {
                Scale = new Vector2(.85f);
                DistanceToNose = .5f;
                DamagePerShot = 5;
                _initHealth = 110;
            }
            else if (Tier == ShipTier.Tier2)
            {
                Scale = new Vector2(.85f);
                _initHealth = 120;
                DistanceToNose = .50f;
                DamagePerShot = 7;
            }
            else if (Tier == ShipTier.Tier3)
            {
                Scale = new Vector2(.85f);
                DistanceToNose = .488f;
                _initHealth = 130;
                DamagePerShot = 10;
            }
            else if (Tier == ShipTier.Tier4)
            {
                Scale = new Vector2(.85f);
                DistanceToNose = .5f;
                DamagePerShot = 15;
                _initHealth = 140;
            }
        }

        public override void Shoot()
        {
            //TODO: change image to torpedo
            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, WorldSb, this);
            
            bullet.Speed = Rotation.Vector * 6f;
            bullet.Rotation = Rotation;
            bullet.Damage = DamagePerShot;
            if(StateManager.Options.SFXEnabled)
            {
                ShootSound.Play();
            }

            StateManager.AllyBullets.Legit.Add(bullet);
            FireBulletEvent(bullet);
        }

        public override ShipType ShipType
        {
            get { return ShipType.TorpedoShip; }
        }
    }
}

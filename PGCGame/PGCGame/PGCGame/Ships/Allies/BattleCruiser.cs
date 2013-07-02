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
    public class BattleCruiser : Ship
    {

        public BattleCruiser(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            MovementSpeed = Vector2.One / 2;
            BulletTexture = Ship.BattleCruiserBullet;
            DelayBetweenShots = TimeSpan.FromSeconds(1);
            DamagePerShot = 20;
            MovementSpeed = new Vector2(.375f);
            InitialHealth = 120;
            PlayerType = CoreTypes.PlayerType.Ally;
            this.TierChanged += new EventHandler(BattleCruiser_TierChanged);
        }

        void BattleCruiser_TierChanged(object sender, EventArgs e)
        {
            if (Tier == ShipTier.Tier1)
            {
                Scale = new Vector2(.85f);
                Effect = SpriteEffects.FlipVertically;
                DistanceToNose = .5f;
            }
            else if (Tier == ShipTier.Tier2)
            {
                Scale = new Vector2(.85f);
                DistanceToNose = .30f;
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
            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, WorldSb);
            bullet.Speed = Rotation.Vector * 3f;
            bullet.UseCenterAsOrigin = true;
            _initHealth = 120;
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
            get { return "Battle Cruiser"; }
        }

        public override string FriendlyName
        {
            get { return "Battle Cruiser"; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PGCGame.CoreTypes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PGCGame
{
    class CloneBoss : Ship
    {
        public CloneBoss(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Scale = new Vector2(.75f);
            BulletTexture = GameContent.GameAssets.Images.Ships.Bullets[ShipType.Drone, ShipTier.Tier1];

            DamagePerShot = 50;
            MovementSpeed = new Vector2(.5f);
            _initHealth = 2000;

            PlayerType = CoreTypes.PlayerType.Enemy;

            killWorth = 100;
        }

        public override ShipType ShipType
        {
            get { return ShipType.EnemyBoss; }
        }

        public override void Shoot()
        {
            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, WorldSb, this);
            bullet.Speed = Rotation.Vector * 3f;
            bullet.UseCenterAsOrigin = true;
            bullet.Rotation = Rotation;
            bullet.Damage = DamagePerShot;
            bullet.Color = Color.Red;

            FlyingBullets.Add(bullet);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            regenClonesDelay += gt.ElapsedGameTime;
            if (regenClonesDelay >= regenClones)
            {
                for (int i = 0; i < StateManager.ActiveShips.Count; i++)
                {
                    if(StateManager.ActiveShips[i].ShipType == CoreTypes.ShipType.EnemyBossesClones)
                    {
                        StateManager.ActiveShips.RemoveAt(i);
                    }
                }
            }
        }

        public TimeSpan regenClones = new TimeSpan(0, 0, 0, 15);
        public TimeSpan regenClonesDelay = new TimeSpan();


        public int killWorth { get; set; }
    }
}

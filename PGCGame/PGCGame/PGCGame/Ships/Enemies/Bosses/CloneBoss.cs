using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PGCGame.CoreTypes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Glib;
using Glib.XNA;

namespace PGCGame
{
    class CloneBoss : Ships.Enemies.BaseEnemyShip
    {
        public CloneBoss(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Scale = new Vector2(.75f);
            BulletTexture = GameContent.GameAssets.Images.Ships.Bullets[ShipType.Drone, ShipTier.Tier1];

            DistanceToNose = .5f;
            Tier = ShipTier.Tier1;

            _hasHealthBar = false;

            DamagePerShot = 50;
            MovementSpeed = new Vector2(.5f);
            _initHealth = 2000;

            _EMPable = false;
            _isTrackingPlayer = false;

            PlayerType = CoreTypes.PlayerType.Enemy;

            killWorth = 100;
        }

        public Vector2? targetPosition = null;

        Vector2 speedVector = new Vector2();

        public Boolean isClone = true;
        Boolean isFirstUpdate = true;

        public override ShipType ShipType
        {
            get
            {
                if (!isClone)
                {
                    return ShipType.EnemyBoss;
                }
                else
                {
                    return ShipType.EnemyBossesClones;
                }
            }

        }

        public override void Update(GameTime gt)
        {
            if (!isClone)
            {
                regenClonesDelay += gt.ElapsedGameTime;
                if (regenClonesDelay >= regenClones)
                {
                    regenClonesDelay = new TimeSpan(0);
                    StateManager.AllScreens[ScreenType.Game.ToInt()].Cast<Screens.GameScreen>().RegenerateClones();
                }
            }

            if (isClone && isFirstUpdate)
            {
                killWorth /= 10;
                DamagePerShot /= 5;
                _initHealth /= 10;
                CurrentHealth /= 10;
            }
            
            if (closestAllyShipDistance.HasValue && closestAllyShipDistance.Value.LengthSquared() < Math.Pow(400, 2))
            {
                targetPosition = null;
                XSpeed = 0f;
                YSpeed = 0f;
                _isTrackingPlayer = true;
            }
            else if (targetPosition.HasValue)
            {
                speedVector = targetPosition.Value - WorldCoords;
                speedVector.Normalize();
                speedVector *= new Vector2(2);
             
                XSpeed = speedVector.X;
                YSpeed = speedVector.Y;
                if (XSpeed < .01f && YSpeed < .01f)
                {
                    targetPosition = null;
                }
            }
            else
            {
                XSpeed = 0f;
                YSpeed = 0f;
                _isTrackingPlayer = true;
            }
            if (isFirstUpdate)
            {
                isFirstUpdate = false;
            }

            base.Update(gt);
        }

        public TimeSpan regenClones = new TimeSpan(0, 0, 30);
        public TimeSpan regenClonesDelay = new TimeSpan();
    }
}

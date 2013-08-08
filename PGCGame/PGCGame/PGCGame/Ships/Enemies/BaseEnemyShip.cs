using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Glib.XNA;
using Glib.XNA.SpriteLib;

using PGCGame.CoreTypes;

namespace PGCGame.Ships.Enemies
{
    public abstract class BaseEnemyShip : Ship
    {
        public BaseEnemyShip(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            StateManager.EnemyShips.Add(this);

            PlayerType = CoreTypes.PlayerType.Enemy;
            UseCenterAsOrigin = true;
            shipState = ShipState.Alive;

            _EMPable = true;
            _isTrackingPlayer = true;

            killWorth = 200;
            spaceBucksKillAmt = 100;

        }

        public int killWorth;
        public int spaceBucksKillAmt;

        private bool _enemyCounts = true;

        public bool EnemyCounts
        {
            get { return _enemyCounts; }
            set { _enemyCounts = value; }
        }
        

        protected bool _isTrackingPlayer;

        public bool IsTrackingPlayer
        {
            get { return _isTrackingPlayer; }
            set { _isTrackingPlayer = value; }
        }

        protected bool _EMPable;

        public bool EMPable
        {
            get { return _EMPable; }
            set { _EMPable = value; }
        }
        


        public override void Shoot()
        {
            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, WorldSb, this);
            bullet.Speed = Rotation.Vector * 3f;
            bullet.UseCenterAsOrigin = true;
            bullet.Rotation = Rotation;
            bullet.Damage = DamagePerShot;
            bullet.Color = Color.Red;

            StateManager.EnemyBullets.Legit.Add(bullet);

            ExplosionSFX = GameContent.GameAssets.Sound[SoundEffectType.EnemyExplodes];
        }

        Boolean activated = false;

        Boolean firstDeathCheck = true;

        public Boolean isEMPed = false;
        TimeSpan _EMPDelay = new TimeSpan(0, 0, 5);
        TimeSpan _elapsedEMPDelay = TimeSpan.Zero;

        TimeSpan _elapsedRotationDelay = TimeSpan.Zero;
        TimeSpan _rotationDelay = new TimeSpan(0, 0, 0, 0, 5);

        TimeSpan _minFireRate = new TimeSpan(0, 0, 0, 0, 15);
        TimeSpan _maxFireRate = new TimeSpan(0, 0, 0, 0, 300);
        TimeSpan? _delayTillNextFire = null;

        protected Vector2? closestAllyShipDistance = null;

        public override void Update(GameTime gt)
        {
            if (!isEMPed || !_EMPable)
            {
                if (CurrentHealth <= 0)
                {
                    if (StateManager.Options.SFXEnabled && this.shipState == CoreTypes.ShipState.Exploding)
                    {
                        ExplosionSFX.Play();

                    }
                    base.Update(gt);

                    _isDead = true;
                    if (firstDeathCheck)
                    {
                        StateManager.AmountOfPointsRecievedInCurrentLevel += killWorth;
                        StateManager.AmountOfSpaceBucksRecievedInCurrentLevel += spaceBucksKillAmt;
                    }

                    return;
                }

                Ship closestAllyShip = null;
                closestAllyShipDistance = null;

                Vector2? shipDistance = null;

                /*
                //finds the closest ship 
                float bulletDistanceX;
                float bulletDistanceY;
                float? bulletDistance = null;

                 * 
                 * //Bullet avoiding logic
                foreach (Bullet b in StateManager.AllyBullets.Legit)
                {
                    bulletDistanceX = Math.Abs(b.X - this.X);
                    bulletDistanceY = Math.Abs(b.Y - this.Y);
                    bulletDistance = bulletDistanceX + bulletDistanceY;
                    if (Math.Pow(bulletDistance.Value, 2) < Math.Pow(600, 2))
                    {
                        activated = true;
                    }
                }
                */

                foreach (Ship allyShip in StateManager.AllyShips)
                {
                    /*
                    foreach (Bullet b in allyShip.FlyingBullets)
                    {
                        bulletDistanceX = Math.Abs(b.X - this.X);
                        bulletDistanceY = Math.Abs(b.Y - this.Y);
                        bulletDistance = bulletDistanceX + bulletDistanceY;
                        if (Math.Pow(bulletDistance.Value, 2) < Math.Pow(600, 2))
                        {
                            activated = true;
                        }

                    }
                    */
                    if (!shipDistance.HasValue && !closestAllyShipDistance.HasValue)
                    {
                        shipDistance = allyShip.WorldCoords - this.WorldCoords;
                        closestAllyShipDistance = shipDistance;
                        closestAllyShip = allyShip;
                    }
                    else
                    {
                        shipDistance = allyShip.WorldCoords - this.WorldCoords;
                        if (shipDistance.Value.LengthSquared() < closestAllyShipDistance.Value.LengthSquared() && allyShip.CurrentHealth > 0)
                        {
                            closestAllyShip = allyShip;
                        }
                    }
                    if (closestAllyShipDistance.Value.LengthSquared() < Math.Pow(600, 2) && closestAllyShip.CurrentHealth > 0)
                    {
                        activated = true;
                    }
                }


                if (closestAllyShipDistance.HasValue && closestAllyShip != null && activated && _isTrackingPlayer)
                {
                    _elapsedRotationDelay += gt.ElapsedGameTime;

                    if (_elapsedRotationDelay > _rotationDelay)
                    {
                        float angle = closestAllyShipDistance.Value.ToAngle();

                        if (Rotation.Radians > angle)
                        {
                            if (Rotation.Radians + .025f > angle)
                            {
                                Rotation.Radians -= .025f;
                            }
                        }
                        else if (Rotation.Radians < angle)
                        {
                            if (Rotation.Radians + .025f < angle)
                            {
                                Rotation.Radians += .025f;
                            }
                        }
                    }
                    if (closestAllyShipDistance.Value.LengthSquared() > Math.Pow(400, 2))
                    {
                        //Rotation.Vector.Normalize();
                        //Rotation.Vector *= .1f;
                        if ((this.Position + MovementSpeed * Rotation.Vector).X > StateManager.WorldSize.Width ||
                            (this.Position + MovementSpeed * Rotation.Vector).X < 0 ||
                            (this.Position + MovementSpeed * Rotation.Vector).Y > StateManager.WorldSize.Height ||
                            (this.Position + MovementSpeed * Rotation.Vector).Y < 0)
                        {
                            this.Speed = Vector2.Zero;
                        }
                        else
                        {
                            this.Speed = MovementSpeed * Rotation.Vector;
                        }
                    }
                    else
                    {
                        this.Speed = Vector2.Zero;
                        if (!_delayTillNextFire.HasValue)
                        {
                            _delayTillNextFire = StateManager.RandomGenerator.NextTimeSpan(_minFireRate, _maxFireRate);
                        }
                        else if (_delayTillNextFire.Value.Milliseconds <= 0)
                        {
                            Shoot();
                            _delayTillNextFire = null;
                        }
                        else
                        {
                            _delayTillNextFire -= gt.ElapsedGameTime;
                        }
                    }

                }

                _elapsedRotationDelay = TimeSpan.Zero;
            }

            else 
            {
                this.Speed = Vector2.Zero;
                _elapsedEMPDelay += gt.ElapsedGameTime;
                if (_elapsedEMPDelay > _EMPDelay && _EMPable)
                {
                    isEMPed = false;
                    _elapsedEMPDelay = TimeSpan.Zero;
                }

            }
            base.Update(gt);
        }
    }
}


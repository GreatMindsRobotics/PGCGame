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
            PlayerType = CoreTypes.PlayerType.Enemy;
            UseCenterAsOrigin = true;

        }

        public int killWorth;

        public override void Shoot()
        {
            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, WorldSb, this);
            bullet.Speed = Rotation.Vector * 3f;
            bullet.UseCenterAsOrigin = true;
            bullet.Rotation = Rotation;
            bullet.Damage = DamagePerShot;
            bullet.Color = Color.Red;

            StateManager.LegitBullets.Add(bullet);
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

        public override void Update(GameTime gt)
        {
            if (!isEMPed)
            {
                if (CurrentHealth <= 0)
                {
                    base.Update(gt);

                    _isDead = true;
                    if (firstDeathCheck)
                    {
                        StateManager.SpacePoints += killWorth;
                    }

                    return;
                }

                Ship closestAllyShip = null;
                Vector2? closestAllyShipDistance = null;

                //finds the closest ship 
                /*
                float bulletDistanceX;
                float bulletDistanceY;
                float? bulletDistance = null;
                Vector2? shipDistance = null;

                foreach (Ship allyShip in StateManager.ActiveShips)
                {
                    if (allyShip.PlayerType != CoreTypes.PlayerType.Enemy)
                    {
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
                }
                */


                if (closestAllyShipDistance.HasValue && closestAllyShip != null && activated)
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
                if (_elapsedEMPDelay > _EMPDelay)
                {
                    isEMPed = false;
                    _elapsedEMPDelay = TimeSpan.Zero;
                }

            }
            base.Update(gt);
        }
    }
}


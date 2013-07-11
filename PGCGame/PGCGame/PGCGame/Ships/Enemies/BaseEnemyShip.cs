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

        Boolean activated = false;

        TimeSpan _elapsedRotationDelay = TimeSpan.Zero;
        TimeSpan _rotationDelay = new TimeSpan(0, 0, 0, 0, 5);

        TimeSpan _minFireRate = new TimeSpan(0, 0, 0, 0, 15);
        TimeSpan _maxFireRate = new TimeSpan(0, 0, 0, 0, 300);
        TimeSpan? _delayTillNextFire = null;

        public override void Update(GameTime gt)
        {
            if (CurrentHealth <= 0)
            {
                base.Update(gt);

                if (FlyingBullets.Count == 0)
                {
                    _isDead = true;
                }

                return;
            }

            float bulletDistanceX;
            float bulletDistanceY;
            float? bulletDistance = null;

            Ship closestAllyShip = null;
            Vector2? closestAllyShipDistance = null;
            Vector2? shipDistance = null;

            //finds the closes ship 
            foreach (Ship allyShip in StateManager.ActiveShips)
            {
                if (allyShip.PlayerType != CoreTypes.PlayerType.Enemy)
                {
                    foreach (Bullet b in allyShip.FlyingBullets)
                    {
                        bulletDistanceX = Math.Abs(Math.Abs(b.X) - Math.Abs(this.X));
                        bulletDistanceY = Math.Abs(Math.Abs(b.Y) - Math.Abs(this.Y));
                        bulletDistance = bulletDistanceX + bulletDistanceY;
                        if (Math.Pow(bulletDistance.Value, 2) < Math.Pow(300, 2))
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
                        if (shipDistance.Value.LengthSquared() < closestAllyShipDistance.Value.LengthSquared())
                        {
                            closestAllyShip = allyShip;
                        }
                    }
                    if (closestAllyShipDistance.Value.LengthSquared() < Math.Pow(600, 2))
                    {
                        activated = true;
                    }
                }
            }

            _elapsedRotationDelay += gt.ElapsedGameTime;

            if (_elapsedRotationDelay > _rotationDelay)
            {
                if (closestAllyShipDistance.HasValue && closestAllyShip != null && activated)
                {
                    float angle = closestAllyShipDistance.Value.ToAngle();

                    if (Rotation.Radians > angle)
                    {
                        if (Rotation.Radians + .025f > angle)
                        {
                            Rotation.Radians -= .025f;
                        }
                        else
                        {
                            Rotation.Radians = angle;
                        }
                    }
                    else if (Rotation.Radians < angle)
                    {
                        if (Rotation.Radians + .025f < angle)
                        {
                            Rotation.Radians += .025f;
                        }
                        else
                        {
                            Rotation.Radians = angle;
                        }
                    }
                    if (closestAllyShipDistance.Value.LengthSquared() > Math.Pow(400, 2))
                    {
                        //Rotation.Vector.Normalize();
                        //Rotation.Vector *= .1f;
                        this.Speed = new Vector2(Rotation.Vector.X * .5f, Rotation.Vector.Y * .5f);
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

            base.Update(gt);
        }
    }
}

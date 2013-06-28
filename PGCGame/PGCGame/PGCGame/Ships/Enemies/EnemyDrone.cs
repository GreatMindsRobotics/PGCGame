using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Glib.XNA;

namespace PGCGame.Ships.Enemies
{
    public class EnemyDrone : Ship
    {
        public EnemyDrone(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Scale = new Vector2(.75f);
            RotateTowardsMouse = false;

            this.PlayerType = CoreTypes.PlayerType.Enemy;
            UseCenterAsOrigin = true;

            BulletTexture = Ship.DroneBullet;
        }


        public override void Shoot()
        {
            Bullet shotBullet = new Bullet(BulletTexture, WorldCoords, SpriteBatch);
            
            shotBullet.Damage = DamagePerShot;
            shotBullet.Speed = Rotation.Vector * 6f;

            shotBullet.UseCenterAsOrigin = true;
            shotBullet.Rotation = Rotation;

            FlyingBullets.Add(shotBullet);
        }


        public override string TextureFolder
        {
            get { throw new NotImplementedException(); }
        }

        public override void DrawNonAuto()
        {
            base.DrawNonAuto();
        }

        TimeSpan elapsedRotationDelay = new TimeSpan();
        TimeSpan rotationDelay = new TimeSpan(0, 0, 0, 0, 5);

        public override void Update(GameTime gt)
        { 
            //Check if this is your ship: StateManager.ActiveShips[0].PlayerID == StateManager.PlayerID
            //Check if this is another player's ship: StateManager.ActiveShips[0].IsPlayerShip && StateManager.ActiveShips[0].PlayerID != StateManager.PlayerID;
            //Check if this is an enemy ship: StateManager.ActiveShips[0].PlayerType == CoreTypes.PlayerType.Enemy

            Ship closestAllyShip = null;
            Vector2? closestAllyShipDistance = null;
            Vector2? distance = null; 


            foreach (Ship allyShip in StateManager.ActiveShips)
            {
                if (allyShip.PlayerType == CoreTypes.PlayerType.Ally)
                {
                    if (!distance.HasValue && !closestAllyShipDistance.HasValue)
                    {
                        distance = allyShip.WorldCoords - this.WorldCoords;
                        closestAllyShipDistance = distance;
                        closestAllyShip = allyShip;
                    }
                    else
                    {
                        distance = allyShip.WorldCoords - this.WorldCoords;
                        if (distance.Value.LengthSquared() < closestAllyShipDistance.Value.LengthSquared())
                        {
                            closestAllyShip = allyShip;
                        }
                    }
                }
            }

            elapsedRotationDelay += gt.ElapsedGameTime;

            if (elapsedRotationDelay > rotationDelay)
            {
                if (closestAllyShipDistance.HasValue && closestAllyShip != null && closestAllyShipDistance.Value.LengthSquared() < Math.Pow(600, 2))
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
                        this.Position += this.Speed;
                    }
                }

                elapsedRotationDelay = new TimeSpan();
            }
        }
    }
}

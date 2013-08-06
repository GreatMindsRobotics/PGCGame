using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib;
using PGCGame.CoreTypes;

namespace PGCGame
{
    public class ShrinkRay : SecondaryWeapon
    {
        public const int MaxShrinks = 2;

        public ShrinkRay(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Cost = 2000;
            Name = "ShrinkRay";
            DeploySound = GameContent.GameAssets.Sound[SoundEffectType.DeployShrinkRay];
        }

        public List<Bullet> ShrinkRayBullets = new List<Bullet>();

        public Boolean ShotBullet = false;

        public override void Update(GameTime currentGameTime)
        {
            foreach (Ship ship in StateManager.EnemyShips)
            {
                if (ship.ShipType != CoreTypes.ShipType.EnemyBoss && ship.ShipType != CoreTypes.ShipType.EnemyBoss)
                {
                    processShrinkBullets(ship);
                }
            }
            if (ShotBullet && !ShouldDraw)
            {
                if (StateManager.Options.SFXEnabled)
                {
                    DeploySound.Play();
                }
                ShouldDraw = true;
            }
        }

        private void processShrinkBullets(Ship ship)
        {
            foreach (Bullet b in ShrinkRayBullets)
            {
                b.Color = Color.Purple;
                b.DoSpeedPlus();
                b.MaximumDistance = new Vector2(875);
                if (b.MaximumDistance.HasValue)
                {
                    if (b.TraveledDistance.LengthSquared() >= b.MaximumDistance.Value.LengthSquared())
                    {
                        b.IsDead = true;
                        FireKilledEvent();
                    }
                }
                if (!b.IsDead && b.Intersects(ship))
                {
                    if (ship.ShrinkCount < MaxShrinks)
                    {
                        ship.Scale *= .66f;
                        ship.ShrinkCount++;
                    }
                    if (ship.CurrentHealth > 1)
                    {
                        ship.CurrentHealth = (ship.CurrentHealth * .66f).Round();
                    }
                    else
                    {
                        //Ship is at 1 health - MURDER IT!!!!
                        ship.Kill(true);
                    }
                    b.IsDead = true;
                    FireKilledEvent();
                }
            }
        }
    }
}

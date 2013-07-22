using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib;

namespace PGCGame
{
    public class ShrinkRay : SecondaryWeapon
    {
        public ShrinkRay(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Cost = 2000;
            Name = "ShrinkRay";
        }

        public List<Bullet> ShrinkRayBullets = new List<Bullet>();

        public Boolean ShotBullet = false;

        public override void Update(GameTime currentGameTime)
        {
            foreach (Ship ship in StateManager.ActiveShips)
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
                        ship.Scale *= .66f;
                        ship.CurrentHealth = (ship.CurrentHealth * .66f).Round();
                        b.IsDead = true;
                        FireKilledEvent();
                    }
                }
            }
            if (ShotBullet && !ShouldDraw)
            {
                ShouldDraw = true;
            }
        }
    }
}

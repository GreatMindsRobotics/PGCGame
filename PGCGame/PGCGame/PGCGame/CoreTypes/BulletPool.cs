using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PGCGame.Screens;

namespace PGCGame.CoreTypes
{
    /// <summary>
    /// A pool of bullets.
    /// </summary>
    public class BulletPool : Stack<Bullet>
    {
        public BulletPool()
        {
            //Initialize the pool.
            for (int i = 0; i < 5000; i++)
            {
                PushInternal(new Bullet(GameContent.Assets.Images.Ships.Bullets[ShipType.BattleCruiser, ShipTier.Tier1], Vector2.Zero, GameScreen.World, null));
            }
        }

        protected Bullet PopInternal()
        {
            return base.Pop();
        }

        protected void PushInternal(Bullet b)
        {
            base.Push(b);
        }

        public new void Push(Bullet t)
        {
            throw new InvalidOperationException("You cannot manually push bullets to a BulletPool.");
        }

        public new Bullet Pop()
        {
            throw new InvalidOperationException("You cannot pop a bullet off of a BulletPool.");
        }

        public new Bullet Peek()
        {
            throw new InvalidOperationException("You cannot peek at a bullet from a BulletPool.");
        }

        /// <summary>
        /// Returns a used bullet to the pool.
        /// </summary>
        /// <param name="returnBullet">The bullet to return.</param>
        public void ReturnBullet(Bullet returnBullet)
        {
            PushInternal(returnBullet);
        }

        /// <summary>
        /// Gets a bullet from the pool.
        /// </summary>
        /// <returns>A Bullet instance from the pool.</returns>
        public Bullet GetBullet()
        {
            if (Count <= 0)
            {
                throw new InvalidOperationException("The bullet pool is in an invalid state (possibly due to multiple threads accessing it). Please reinitialize the pool.");
            }
            Bullet returnVal = PopInternal();

            if (Count <= 5)
            {
                //Refill the pool
                while (Count <= 500)
                {
                    PushInternal(new Bullet(GameContent.Assets.Images.Ships.Bullets[ShipType.BattleCruiser, ShipTier.Tier1], Vector2.Zero, GameScreen.World, null));
                }
            }

            return returnVal;
        }
    }
}

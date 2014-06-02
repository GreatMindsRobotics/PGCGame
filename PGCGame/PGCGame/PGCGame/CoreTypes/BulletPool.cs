using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PGCGame.Screens;
using PGCGame.CoreTypes.Utilities;

namespace PGCGame.CoreTypes
{
    /// <summary>
    /// A pool of bullets.
    /// </summary>
    public class BulletPool
    {
        /// <summary>
        /// The initial pool size.
        /// </summary>
        public const int POOL_SIZE = 6000;

        /// <summary>
        /// The threshold which determines when to add new bullets to the pool.
        /// </summary>
        public const int ADD_BULLETS_THRESHOLD = 15;

        /// <summary>
        /// The amount of bullets to add when the pool is empty (as determined by ADD_BULLETS_THRESHOLD).
        /// </summary>
        public const int BULLET_ADD_AMOUNT = 15;

        private Deque<Bullet> _bulletDeque;

        public BulletPool()
        {
            _bulletDeque = new Deque<Bullet>(POOL_SIZE);

            //Initialize the pool.
            for (int i = 0; i < POOL_SIZE; i++)
            {
                _bulletDeque.AddFirst(new Bullet(GameContent.Assets.Images.Ships.Bullets[ShipType.BattleCruiser, ShipTier.Tier1], Vector2.Zero, GameScreen.World, null));
            }
        }

        /// <summary>
        /// Returns a used bullet to the pool.
        /// </summary>
        /// <param name="returnBullet">The bullet to return.</param>
        public void ReturnBullet(Bullet returnBullet)
        {
            _bulletDeque.AddLast(returnBullet);
        }

        /// <summary>
        /// Gets a bullet from the pool.
        /// </summary>
        /// <returns>A Bullet instance from the pool.</returns>
        public Bullet GetBullet()
        {
            if (_bulletDeque.Count <= 0)
            {
                throw new InvalidOperationException("The bullet pool is in an invalid state (possibly due to multiple threads accessing it). Please reinitialize the pool.");
            }
            Bullet returnVal = _bulletDeque.PopFirst();

            if (_bulletDeque.Count <= ADD_BULLETS_THRESHOLD)
            {
                //Refill the pool, but we should have bullets returned, so not too much
                while (_bulletDeque.Count <= BULLET_ADD_AMOUNT + ADD_BULLETS_THRESHOLD)
                {
                    _bulletDeque.AddLast(new Bullet(GameContent.Assets.Images.Ships.Bullets[ShipType.BattleCruiser, ShipTier.Tier1], Vector2.Zero, GameScreen.World, null));
                }
            }

            return returnVal;
        }
    }
}

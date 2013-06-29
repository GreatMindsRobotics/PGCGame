using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PGCGame.CoreTypes
{
    public static class Extensions
    {

#region Random Class extensions

        /// <summary>
        /// Generates a new TimeSpan between minValue and maxValue
        /// </summary>
        /// <param name="random">Random number generator to use</param>
        /// <param name="minValue">Inclusive minimum value</param>
        /// <param name="maxValue">Exclusive maximum value</param>
        /// <returns>new TimeSpan object</returns>
        public static TimeSpan NextTimeSpan(this Random random, TimeSpan minValue, TimeSpan maxValue)
        {
            if (minValue >= maxValue)
            {
                throw new Exception("minValue cannot equal or exceed maxValue");
            }

            return TimeSpan.FromMilliseconds(random.Next((int)minValue.TotalMilliseconds, (int)maxValue.TotalMilliseconds));
        }

        /// <summary>
        /// Generates a new Vector2 between minValue and maxValue
        /// </summary>
        /// <param name="random">Random number generator to use</param>
        /// <param name="minValue">Inclusive minimum value</param>
        /// <param name="maxValue">Exclusive maximum value</param>
        /// <returns>new Vector2 object. If minValue.X is equal to maxValue.X, only Y-axis is randomized; if minValue.Y is equal to maxValue.Y, only X-axis is randomized. Both X and Y components cannot be equal in minValue and maxValue.</returns>
        public static Vector2 NextVector2(this Random random, Vector2 minValue, Vector2 maxValue)
        {
            if (minValue.X > maxValue.X)
            {
                throw new Exception("minValue.X cannot exceed maxValue.X");
            }
            else if (minValue.Y > maxValue.Y)
            {
                throw new Exception("minValue.Y cannot exceed maxValue.Y");
            }
            else if(minValue == maxValue)
            {
                throw new Exception("minValue cannot equal maxValue");
            }

            float x = minValue.X == maxValue.X ? minValue.X : (float)random.Next((int)minValue.X, (int)maxValue.X);
            float y = minValue.Y == maxValue.Y ? minValue.Y : (float)random.Next((int)minValue.Y, (int)maxValue.Y);

            return new Vector2(x, y);
        }

#endregion Random Class extensions


    }
}

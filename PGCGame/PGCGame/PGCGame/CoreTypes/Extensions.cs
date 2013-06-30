using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib;
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
        /// <remarks>
        /// Vectors are rounded to integers during randomization... floating point minimums and maximums won't work.
        /// The returned Vector2 will not be floating point, it will have an integer-convertible value (so no return value of .5 anything).
        /// </remarks>
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

            float x = minValue.X == maxValue.X ? minValue.X : random.Next(minValue.X.Round(), maxValue.X.Round()).ToFloat();
            float y = minValue.Y == maxValue.Y ? minValue.Y : random.Next(minValue.Y.Round(), maxValue.Y.Round()).ToFloat();

            return new Vector2(x, y);
        }

#endregion Random Class extensions


    }
}

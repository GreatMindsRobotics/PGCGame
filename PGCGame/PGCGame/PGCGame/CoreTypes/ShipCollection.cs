using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame.CoreTypes
{
    public class ShipCollection : List<Ship>
    {
        public void ClearAllies()
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].PlayerType == PlayerType.Ally || this[i].PlayerType == PlayerType.MyShip)
                {
                    RemoveAt(i);
                    i--;
                }
            }
        }

        public void ClearEnemies()
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].PlayerType == PlayerType.Enemy)
                {
                    RemoveAt(i);
                    i--;
                }
            }
        }

        public Ship this[Guid shipId]
        {
            get
            {
                foreach (Ship s in this)
                {
                    if (s.ShipID.Equals(shipId))
                    {
                        return s;
                    }
                }
                throw new IndexOutOfRangeException("A ship with the specified ID was not found in this collection.");
            }
        }
    }
}

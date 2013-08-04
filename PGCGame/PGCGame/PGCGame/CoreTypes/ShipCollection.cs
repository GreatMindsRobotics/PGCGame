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
            RemoveAll(s => s.PlayerType == PlayerType.Ally || s.PlayerType == PlayerType.MyShip);
        }

        public void ClearEnemies()
        {
            RemoveAll(s => s.PlayerType == PlayerType.Enemy);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;
using PGCGame.Ships.Allies;

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

        public BaseAllyShip this[NetworkGamer shipController]
        {
            get
            {
                foreach (Ship sh in this)
                {
                    BaseAllyShip s = sh as BaseAllyShip;
                    if (s != null && s.Controller != null && s.Controller.Id == shipController.Id)
                    {
                        return s;
                    }
                    
                }
                throw new IndexOutOfRangeException("A ship with the specified controller was not found in this collection.");
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

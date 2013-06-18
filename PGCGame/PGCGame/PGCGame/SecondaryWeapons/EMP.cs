using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame
{
    public class EMP : SecondaryWeapon
    {
        public int Radius { get; set; }
        public int Diameter
        {
            get
            {
                return Radius * 2;
            }
            set
            {
                Radius = value / 2;
            }
        }

        public Ship LaunchingShip { get; set; }
    }
}

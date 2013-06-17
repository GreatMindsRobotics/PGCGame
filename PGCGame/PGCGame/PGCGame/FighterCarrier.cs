using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame
{
    public class FighterCarrier : Ship
    {
        public Drone[] Drones { get; set; }
        public event EventHandler BulletFired;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame.CoreTypes
{
    public class DroneEventArgs : EventArgs
    {
        public int DroneID { get; set; }

        public DroneEventArgs(int drone)
        {
            DroneID = drone;
        }
    }
}

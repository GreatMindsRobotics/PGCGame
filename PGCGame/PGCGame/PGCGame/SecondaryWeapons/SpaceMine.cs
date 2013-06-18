using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame
{
    public class SpaceMine : SecondaryWeapon
    {
        //MICHAEL:
        //override the update function

        public int ExplosionRadius { get; set; }
        public int ExplosionDiameter { get; set; }

        public TimeSpan RemainingArmTime = TimeSpan.FromMilliseconds(500);
        public bool Armed { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Glib.XNA;

namespace PGCGame
{
    public class SpaceMine : SecondaryWeapon
    {
       
       

        public int ExplosionRadius { get; set; }
        public int ExplosionDiameter { get; set; }

        public TimeSpan RemainingArmTime = TimeSpan.FromMilliseconds(500);

        public bool Armed { get; set; }

        public void Update(GameTime gameTime)
        {
            if (Armed == false)
            {
                RemainingArmTime -= gameTime.ElapsedGameTime;
                if (RemainingArmTime.Milliseconds <=  0)
                {
                    Armed = true;
                }
                }
            }
      
        }
    }
 
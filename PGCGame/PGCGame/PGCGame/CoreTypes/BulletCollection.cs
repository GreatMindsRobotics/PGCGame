using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib;
using Glib.XNA;
using Microsoft.Xna.Framework.Graphics;

namespace PGCGame.CoreTypes
{
    public class BulletCollection
    {



        public List<Bullet> Legit = new List<Bullet>();
        public List<Bullet> Dud = new List<Bullet>();

        public void UpdateAll(Microsoft.Xna.Framework.GameTime gt)
        {
            foreach (Bullet b in Legit)
            {
                b.Update();

            }
            foreach (Bullet b in Dud)
            {
                b.Update();
            }
        }

        public void DrawAll(SpriteBatch worldSb)
        {
            foreach (Bullet b in Legit)
            {
                worldSb.Draw(b);
            }
            foreach (Bullet b in Dud)
            {
                worldSb.Draw(b);
            }
        }
    }
}

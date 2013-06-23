using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PGCGame.CoreTypes
{
    class ViewportEventArgs : EventArgs
    {
        public Viewport Viewport { get; set; }
        public bool IsFullScreen { get; set; }
    }
}

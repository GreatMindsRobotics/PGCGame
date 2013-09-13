using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA;
using Glib;
using Microsoft.Xna.Framework;

namespace PGCGame.CoreTypes.Utilites
{
    public class LocationUtility : IPositionable
    {
        private Vector2 _position;

        public LocationUtility(float x, float y)
        {
            _position = new Vector2(x, y);
        }

        public float Y
        {
            get
            {
                return _position.Y;
            }
            set
            {
                _position.Y = value;
            }
        }

        public float X
        {
            get
            {
                return _position.X;
            }
            set
            {
                _position.X = value;
            }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        static public explicit operator Point(LocationUtility loc)
        {
            return new Point(loc.Position.X.Round(), loc.Position.Y.Round());
        }

        static public implicit operator Vector2(LocationUtility loc)
        {
            return new Vector2(loc.X, loc.Y);
        }

        static public implicit operator LocationUtility(Vector2 loc)
        {
            return new LocationUtility(loc.X, loc.Y);
        }

        static public implicit operator LocationUtility(Point loc)
        {
            return new LocationUtility(loc.X, loc.Y);
        }
        
    }
}

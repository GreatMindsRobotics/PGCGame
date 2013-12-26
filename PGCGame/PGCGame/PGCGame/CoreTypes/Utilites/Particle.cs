using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FontEffectsLib.SpriteTypes;

namespace PGCGame.CoreTypes.Utilites
{
    public class Particle : GameSprite
    {
        #region Protected Fields

        protected Vector2 _velocity;
        protected float _angularVelocity;
        protected TimeSpan _ttl;

        #endregion

        #region Public Properties

        public Vector2 Velocity 
        {
            get { return _velocity; }
            set { _velocity = value; }        
        }

        public float AngularVelocity         
        {
            get { return _angularVelocity; }
            set { _angularVelocity = value; }
        }

        public TimeSpan TTL 
        {
            get { return _ttl; }
            set { _ttl = value; }
        }

        #endregion Public Properties

        #region Constructors

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float rotation, float angularVelocity, Color tintColor, float scale, TimeSpan ttl)
            : base(texture, position, tintColor)
        {
            _rotation = rotation;
            _scale = new Vector2(scale);

            _velocity = velocity;
            _angularVelocity = angularVelocity;
            _ttl = ttl;
        }

        #endregion Constructors

        #region Public Methods

        public override void Update(GameTime gameTime)
        {
            _ttl -= gameTime.ElapsedGameTime;
            _position += Velocity;
            _rotation += AngularVelocity;

            base.Update(gameTime);
        }

        #endregion Public Methods
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PGCGame.CoreTypes.Utilites
{
    public class ParticleEngine
    {
        #region Protected Fields

        protected Random _random;
        protected bool _isVisible;
        protected Vector2 _position;

        protected List<Particle> _particles;
        protected List<Texture2D> _textures;

        protected int _newParticlesPerUpdate;

        protected TimeSpan _minTTL;
        protected TimeSpan _maxTTL;

        #endregion Protected Fields

        #region Public Properties

        public bool IsVisible 
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }
        
        public Vector2 Position 
        {
            get { return _position; }
            set { _position = value; }
        }
        
        public int NewParticlesPerUpdate
        {
            get { return _newParticlesPerUpdate; }
            set { _newParticlesPerUpdate = value; }
        }

        public TimeSpan MinTTL
        {
            get { return _minTTL; }
            set { _minTTL = value; }
        }

        public TimeSpan MaxTTL
        {
            get { return _maxTTL; }
            set { _maxTTL = value; }
        }

        #endregion Public Properties

        #region Constructors

        public ParticleEngine(List<Texture2D> textures, Vector2 position)
            : this(textures, position, new Random())                
        {
            
        }

        public ParticleEngine(List<Texture2D> textures, Vector2 position, Random random)
        {
            _position = position;

            _textures = textures;
            _particles = new List<Particle>();
            
            _random = random;

            //Defaults
            _newParticlesPerUpdate = 40;
            _minTTL = TimeSpan.FromMilliseconds(300);
            _maxTTL = TimeSpan.FromMilliseconds(700);
        }

        #endregion Constructors

        #region Public Methods

        public void Update(GameTime gameTime)
        {
            if (!_isVisible)
            {
                return;
            }

            //Add new particles
            for (int i = 0; i < _newParticlesPerUpdate; i++)
            {
                _particles.Add(generateNewParticle());
            }

            //Update particles
            for (int particle = 0; particle < _particles.Count; particle++)
            {
                _particles[particle].Update(gameTime);

                //Remove dead particles
                if (_particles[particle].TTL.TotalMilliseconds <= 0)
                {
                    _particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isVisible)
            {
                return;
            }

            foreach (Particle particle in _particles)
            {
                particle.Draw(spriteBatch);
            }
        }

        #endregion Public Methods

        

        #region Private Helper Functions

        private Particle generateNewParticle()
        {
            Particle particle = new Particle(_textures[_random.Next(_textures.Count)], _position, Vector2.Zero, 0f, 0.1f, Color.White, 0f, _random.NextTimeSpan(_minTTL, _maxTTL));

            particle.Velocity = new Vector2((float)_random.NextDouble() * 2 - 1, (float)_random.NextDouble() * 2 - 1);
            particle.AngularVelocity *= ((float)_random.NextDouble() * 2 - 1);
            particle.TintColor = new Color((float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble());
            particle.Scale = new Vector2((float)_random.NextDouble());

            particle.SetCenterAsOrigin();

            return particle;
        }

        #endregion Private Helper Functions
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib.ParticleEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PGCGame.CoreTypes.Utilites
{
    public class PGCParticleGenerator : RandomParticleGenerator
    {

        private SpriteBatch _spriteBatch;

        public PGCParticleGenerator(SpriteBatch batch, params Texture2D[] textures) : base(batch, textures)
        {
            _spriteBatch = batch;
        }

        public override Particle GenerateParticle(Vector2 pos)
        {
            Particle particle = new Particle(Textures[Random.Next(Textures.Count)], pos, _spriteBatch);

            particle.Speed = new Vector2((float)(Random.NextDouble() * 2 - 1), (float)(Random.NextDouble() * 2 - 1));
            particle.RotationVelocity = MathHelper.ToDegrees(Convert.ToSingle(Random.NextDouble() * 2 - 1) / 10f);
            //particle.Color = new Color(_random.Next(255), _random.Next(255), Random.Next(255), _random.Next(255));
            particle.Scale = new Vector2((float)Random.NextDouble());
            particle.TimeToLive = TimeSpan.FromTicks(Random.Next((int)MinimumTimeToLive.Ticks, (int)MaximumTimeToLive.Ticks));
            particle.TimeToLiveSettings = TTLSettings;

            if (MinimumParticleColorChangeRate != 1)
            {
                float particleColorDegenerationRate = (float)Random.NextDouble();
                while (particleColorDegenerationRate < MinimumParticleColorChangeRate)
                {
                    particleColorDegenerationRate += Convert.ToSingle(Random.NextDouble() % 0.15);
                }
                particle.ColorChange = particleColorDegenerationRate;
            }

            particle.UseCenterAsOrigin = true;

            return particle;
        }
    }
}

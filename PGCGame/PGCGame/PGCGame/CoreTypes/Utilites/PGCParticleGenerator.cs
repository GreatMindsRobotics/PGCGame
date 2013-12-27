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
            TTLSettings = TimeToLiveSettings.AlphaLess100;
        }

        public override Particle GenerateParticle(Vector2 pos)
        {
            Particle particle = base.GenerateParticle(pos);
            particle.Color = Color.White;
            particle.ColorChange = 0.985f;
            return particle;
        }
    }
}

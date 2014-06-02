using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Glib.XNA.SpriteLib.ParticleEngine;
using Glib.XNA;

namespace PGCGame.CoreTypes.Utilities
{
    class Ship_Sprite : Sprite, ITimerSprite
    {
        Vector2 angleVector;
        float toEngineAngle;
        float toEngineLength;

        public Texture2D[] particles = new Texture2D[2];
        public RandomParticleGenerator gen;
        public ParticleEngine engine;

        public Ship_Sprite(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            particles[0] = GameContent.Assets.Images.particles[ParticleType.Circle];
            particles[1] = GameContent.Assets.Images.particles[ParticleType.Square];
            gen = new RandomParticleGenerator(SpriteBatch, particles);
            gen.TTLSettings = TimeToLiveSettings.AlphaLess100;
            gen.RandomProperties = new RandomParticleProperties() { ColorFactor = 0.985f, Tint = Color.White };
            gen.ParticlesToGenerate = 1;
            engine = new Glib.XNA.SpriteLib.ParticleEngine.ParticleEngine(gen);

            Vector2 toEngine = new Vector2(Position.X, Position.Y - Height / 2);
            toEngineLength = -toEngine.Length();
            toEngineAngle = toEngine.ToAngle();

            engine.Tracked = this;
        }

        public void TextureChanged()
        {
            Vector2 toEngine = new Vector2(Texture.Bounds.Left - Texture.Width / 2 * Scale.X, Texture.Bounds.Top - Texture.Height / 2 * Scale.Y);
            toEngineLength = -toEngine.Length();
            toEngineAngle = toEngine.ToAngle();
        }

        public void Update(GameTime gt)
        {
            angleVector = (Rotation.Radians + toEngineAngle).AngleToVector();
            angleVector.Normalize();
            engine.PositionOffset = angleVector * toEngineLength;

            engine.Update(gt);

            base.Update();
        }
        public override void DrawNonAuto()
        {
            engine.Draw();

            base.DrawNonAuto();
        }
    }

}

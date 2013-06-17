using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GitPractice
{
    public class Ship : MovingSprite
    {
        private List<MovingSprite> _flyingBullets;
        private MovingSprite bullet;

        public new void LoadContent(ContentManager content, string assetName)
        {
            base.LoadContent(content, assetName);
            bullet = new MovingSprite();
            bullet.LoadContent(content, "bullet", new Vector2(0, 5));

            _flyingBullets = new List<MovingSprite>();
        }

        public new void Update(KeyboardState keyboard, GameTime gameTime, GameState gameState, Viewport viewport)
        {
            foreach (MovingSprite b in _flyingBullets)
            {
                b.Update(new KeyboardState(b.KeyUp), gameTime, gameState, viewport);
            }

            if (keyboard.IsKeyDown(Keys.Space))
            {
                MovingSprite createdBullet = new MovingSprite();

                createdBullet.Texture = bullet.Texture;
                createdBullet.Location = new Vector2(Location.X + Texture.Width / 2, Location.Y - createdBullet.Texture.Height);
                createdBullet.Speed = new Vector2(0, 5);
                createdBullet.KeyUp = Keys.Up;
                createdBullet.KeyDown = Keys.Down;
                createdBullet.KeyLeft = Keys.Down;
                createdBullet.KeyRight = Keys.Down;
                createdBullet.TintColor = Color.White;

                _flyingBullets.Add(createdBullet);
            }

            base.Update(keyboard, gameTime, gameState, viewport);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (MovingSprite b in _flyingBullets)
            {
                b.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }
}

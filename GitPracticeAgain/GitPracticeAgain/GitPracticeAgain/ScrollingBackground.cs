using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GitPracticeAgain
{
    class ScrollingBackground: BaseSprite
    {
        public void Update()
        {

        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_texture, _position, _tintColor);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib.XNA;

namespace PGCGame
{
    public class HorizontalMenuBGSprite : ISprite
    {
        List<Sprite> _bgList = new List<Sprite>();
        private Viewport vp;

        public HorizontalMenuBGSprite(Texture2D bg, SpriteBatch sb)
        {
            vp = sb.GraphicsDevice.Viewport;
            Sprite spr1 = new Sprite(bg, Vector2.Zero, sb);
            spr1.YSpeed = .15f;
            _bgList.Add(spr1);
            Sprite spr2 = new Sprite(bg, new Vector2(0, -bg.Height), sb);
            spr2.Effect = SpriteEffects.FlipVertically;
            _bgList.Add(spr2);
            spr1.Moved += new EventHandler(spr1_Moved);
        }

        void spr1_Moved(object sender, EventArgs e)
        {
            _bgList[1].Y = _bgList[0].Y - _bgList[1].Height;
        }

        public void Update()
        {
            foreach (Sprite s in _bgList)
            {
                s.Update();
                
            }
            if (_bgList[0].Y >= vp.Height)
            {
                _bgList[0].Position = Vector2.Zero;
                _bgList[0].Effect = _bgList[0].Effect == SpriteEffects.FlipHorizontally ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
        }

        public void Draw()
        {
            foreach (Sprite s in _bgList)
            {
                s.DrawNonAuto();
            }
        }
    }
}

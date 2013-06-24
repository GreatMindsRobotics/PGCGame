using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Glib.XNA.SpriteLib;

using PGCGame.CoreTypes;

namespace PGCGame
{
    public class Drone : Ship
    {
        public ShipRelativePosition RelativePosition;

        public FighterCarrier ParentShip { get; set; }

        public Drone(Texture2D texture, Vector2 location, SpriteBatch spriteBatch, FighterCarrier parent)
            : base(texture, location, spriteBatch)
        {
            ParentShip = parent;
            _performMovement = false;
            //TODO: Change scale w/ actual drone texture
            Scale = Vector2.One;
            _rotateTowardsMouse = false;
            
            BulletTexture = Ship.DroneBullet;
            ParentShip.BulletFired += new EventHandler(ParentShip_BulletFired);
        }

        void ParentShip_BulletFired(object sender, EventArgs e)
        {
            ShotsFromMain++;
            Shoot();
        }
        public int ShotsFromMain { get; set; }
        //must add event from fighter carrier

        public override void Shoot()
        {
            //Every 4th shot of main, shoot
            if (ShotsFromMain % 4 == 0)
            {
                //Glen's mom magic: Targeting
                //TODO: AI Targeting
                Bullet bullet = new Bullet(BulletTexture, WorldCoords, SpriteBatch);
                //MouseState ms = Mouse.GetState();
                bullet.Damage = DamagePerShot;
                bullet.Speed = ParentShip.Rotation.AsVector();
                //Vector2 mousePos = new Vector2(ms.X, ms.Y);
                //Vector2 slope = mousePos - Position;
                //slope.Normalize();
                ParentShip.DroneBullets.Add(bullet);
            }
            //throw new NotImplementedException();
        }

        public override void Update()
        {
            Position = ParentShip.Position; //+ new Vector2(ParentShip.Width / 2, ParentShip.Height / 2);
            Rotation += .5f;
                        
            base.Update();
        }

        public override string TextureFolder
        {
            get { throw new NotImplementedException(); }
        }
    }
}

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

using Glib.XNA;
using Glib;
using Glib.XNA.SpriteLib;

using PGCGame.CoreTypes;

namespace PGCGame
{
    public abstract class Ship : Sprite, ITimerSprite
    {

        protected const bool CanHoldShootKey = false;

        //TODO: ALEX
        public static Texture2D DroneBullet;
        public static Texture2D BattleCruiserBullet;
        public static Texture2D FighterCarrierBullet;
        public static Texture2D Torpedo;

        public abstract string TextureFolder { get; }

        protected bool _rotateTowardsMouse = true;

        public Guid PlayerID
        {
            get { return StateManager.PlayerID; }
        }
        

        public Ship(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            StateManager.ActiveShips.Add(this);
        }

        public PlayerType PlayerType { get; set; }

        public abstract void Shoot();

        //override: 
        //Update
        //DrawNonAuto

        public Texture2D BulletTexture { get; set; }

        public override void Update()
        {
            base.Update();
            if (_rotateTowardsMouse)
            {
                MouseState ms = Mouse.GetState();
                Vector2 mousePos = new Vector2(ms.X, ms.Y);
                Vector2 targetPos = mousePos - Position;              

                //Rotate towards mouse
                Rotation.Radians = Math.Atan2(targetPos.X, -targetPos.Y).ToFloat();
            }           

            foreach (Bullet b in FlyingBullets)
            {
                b.Update();
            }
        }

        public SpriteBatch WorldSb;
        private TimeSpan _elapsedShotTime = new TimeSpan();
        private KeyboardState _lastKs = new KeyboardState();

        public void Update(GameTime gt)
        {
            Update();
            //Shoot
            KeyboardState ks = Keyboard.GetState();
            _elapsedShotTime += gt.ElapsedGameTime;
            //Shoot w/ space key
            if (CanShoot && (_lastKs.IsKeyUp(Keys.Space) || CanHoldShootKey) && ks.IsKeyDown(Keys.Space))
            {
                Shoot();
                _elapsedShotTime = new TimeSpan();
            }
            _lastKs = ks;
        }

        public Vector2 WorldCoords = Vector2.Zero;

        public bool CanShoot
        {
            get
            {
                return _elapsedShotTime >= DelayBetweenShots;
            }
        }

        public override void DrawNonAuto()
        {
            base.DrawNonAuto();

            //SpriteBatch.Draw(Texture, Position, DrawRegion, Color.White, Rotation.Radians, Origin, Scale, Effect, 0);

            
            
            //TODO: Draw Bullets
        }

        public int DamagePerShot { get; set; }
        public int Cost { get; set; }

        public TimeSpan DelayBetweenShots { get; set; }

        private Vector2 _movementSpeed = Vector2.One;


        public Vector2 MovementSpeed
        {
            get { return _movementSpeed; }
            set { _movementSpeed = value; }
        }

        public int CurrentHealth { get; set; }

        public int InitialHealth { get; set; }

        public int Shield { get; set; }

        public int Armor { get; set; }

        private List<Bullet> _flyingBullets = new List<Bullet>();


        private ShipTier _shipTier = ShipTier.Tier1;

        public event EventHandler TierChanged;

        /// <summary>
        /// Gets or sets the tier of the ship.
        /// </summary>
        public ShipTier Tier
        {
            get { return _shipTier; }
            set {
                _shipTier = value;
                if (TierChanged != null)
                {
                    TierChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// The percentage of the total height of the ship that the nose is from the center.
        /// </summary>
        public float DistanceToNose;

        public List<Bullet> FlyingBullets
        {
            get { return _flyingBullets; }
            set { _flyingBullets = value; }
        }
    }
}

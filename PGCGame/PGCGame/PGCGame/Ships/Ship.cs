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
using PGCGame.Ships.Allies;

namespace PGCGame
{
    public abstract class Ship : Sprite, ITimerSprite
    {
        protected const bool CanHoldShootKey = true;

        #region StaticProperties
        
        public static Texture2D DroneBullet;
        public static Texture2D BattleCruiserBullet;
        public static Texture2D FighterCarrierBullet;
        public static Texture2D Torpedo;
        public static Texture2D SpaceMine;

        #endregion StaticProperties

        
        public abstract string TextureFolder { get; }
        

        #region Private Fields

        //current tier of the ship
        private ShipTier _shipTier = ShipTier.Tier1;

        //position of the ship in the world
        private List<Bullet> _flyingBullets = new List<Bullet>();

        private ProgressBar _healthBar;

        private Vector2 _movementSpeed = Vector2.One;

        private Guid _shipID;
        private bool _isFirstUpdate = true;

        #endregion Private Fields

        protected int _initHealth;

        public event EventHandler TierChanged;
        public event EventHandler BulletFired;

        public abstract string FriendlyName { get; }

        #region PublicProperties

        public Guid PlayerID
        {
            get
            {
                return StateManager.PlayerID;
            }
        }

        public Guid ShipID
        {
             get
             {
                 return _shipID;
             }
        }

        public PlayerType PlayerType { get; set; }

        public Texture2D BulletTexture { get; set; }

        public SpriteBatch WorldSb;

        public virtual Vector2 WorldCoords
        {
            get
            {
                return Position;
            }
            set
            {
                Position = value;
            }
        }

        public int DamagePerShot { get; set; }

        public List<Bullet> FlyingBullets
        {
            get { return _flyingBullets; }
            set { _flyingBullets = value; }

        }

        public int Cost { get; set; }

        public TimeSpan DelayBetweenShots { get; set; }

        public Vector2 MovementSpeed
        {
            get { return _movementSpeed; }
            set { _movementSpeed = value; }
        }

        public int CurrentHealth { get; set; }

        

        public int InitialHealth
        {
            get { return _initHealth; }
            set { _initHealth = value; }
        }
        

        public int Shield { get; set; }

        public int Armor { get; set; }

        /// <summary>
        /// Gets or sets the tier of the ship.
        /// </summary>
        public ShipTier Tier
        {
            get { return _shipTier; }
            set
            {
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

        #endregion PublicProperties

        #region CTOR

        public Ship(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            StateManager.ActiveShips.Add(this);
            _shipID = Guid.NewGuid();
            _initHealth = 100;
            _healthBar = new ProgressBar(new Vector2(X, Y), Color.DarkGreen, Color.Red, spriteBatch);
            _healthBar.WidthScale = 1;
            _healthBar.HeightScale = 10;
        }

        #endregion CTOR

        #region PublicMethod
        protected void FireBulletEvent()
        {
            if (BulletFired != null)
            {
                BulletFired(this, EventArgs.Empty);
            }
        }

        public virtual void Shoot()
        {
            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, WorldSb);
            bullet.Speed = Rotation.Vector * 3f;
            bullet.UseCenterAsOrigin = true;
            bullet.Rotation = Rotation;
            bullet.Damage = DamagePerShot;

            FlyingBullets.Add(bullet);

            FireBulletEvent();
        }

        public virtual void Update(GameTime gt)
        {
            base.Update();

            if (_isFirstUpdate)
            {
                _healthBar.Position = new Vector2(X - (_healthBar.Width / 2), Y - (Height / 1.5f));
                CurrentHealth = InitialHealth;
                
            }

            _healthBar.Denominator = InitialHealth;
            _healthBar.Value = CurrentHealth;

            foreach (Bullet b in FlyingBullets)
            {
                b.Update();
            }
        }

        public override void DrawNonAuto()
        {
            base.DrawNonAuto();

            if (InitialHealth > 1 && (StateManager.HasBoughtScanner || (this is BaseAllyShip && this.Cast<BaseAllyShip>().IsPlayerShip)))
            {
                _healthBar.DrawNonAuto();
            }
        }
        #endregion PublicMethod
    }
}

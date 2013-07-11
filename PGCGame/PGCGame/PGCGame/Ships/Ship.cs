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

        private static KeyValuePair<int, ShipTier>[] _cost;
        public static KeyValuePair<int, ShipTier>[] Cost { get { return _cost; } set { _cost = value; } }

        public abstract ShipType ShipType { get; }

        #endregion StaticProperties
        #region Private Fields

        public Ship(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            StateManager.ActiveShips.Add(this);
            _healthBar = new ProgressBar(new Vector2(X, Y), Color.DarkGreen, Color.Red, spriteBatch);
            _healthBar.WidthScale = 1;
            _healthBar.HeightScale = 10;
            Moved += new EventHandler(Ship_Moved);
            _shipID = Guid.NewGuid();
            _initHealth = 100;
            _isDead = false;

            _currentHealth = _initHealth;
            _cost = new KeyValuePair<int, ShipTier>[4];
        }

        void Ship_Moved(object sender, EventArgs e)
        {
            _healthBar.Position = new Vector2(X - (_healthBar.Width / 2), Y - (Height / 1.5f));
        }

        public PlayerType PlayerType { get; set; }


        //override: 
        //Update
        //DrawNonAuto

        public Texture2D BulletTexture { get; set; }

        public SpriteBatch WorldSb;
        //private TimeSpan _elapsedShotTime = new TimeSpan();
        protected KeyboardState _lastKs = new KeyboardState();

        public abstract string FriendlyName { get; }

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

        protected bool _isDead;

        public bool IsDead
        {
            get { return _isDead; }
            set { _isDead = value; }
        }


        #region PublicProperties

        public Guid PlayerID
        {
            get
            {
                return this is BaseAllyShip ? StateManager.PlayerID : StateManager.EnemyID;
            }
        }

        public Guid ShipID
        {
            get
            {
                return _shipID;
            }
        }

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

        public TimeSpan DelayBetweenShots { get; set; }

        public Vector2 MovementSpeed
        {
            get { return _movementSpeed; }
            set { _movementSpeed = value; }
        }

        public int CurrentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = value; }
        }



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

        private int _currentHealth;

        #endregion PublicProperties

        #region PublicMethod

        public virtual void Shoot()
        {
            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, WorldSb);
            bullet.Speed = Rotation.Vector * 3f;
            bullet.UseCenterAsOrigin = true;
            bullet.Rotation = Rotation;
            bullet.Damage = DamagePerShot;

            FlyingBullets.Add(bullet);
        }



        public virtual void Update(GameTime gt)
        {
            base.Update();

            if (_isFirstUpdate)
            {
                CurrentHealth = InitialHealth;
                _isFirstUpdate = false;

            }

           

            if (CurrentHealth <= 0)
            {
                StateManager.ActiveShips.Remove(this);
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
            if (CurrentHealth <= 0)
            {
                return;
            }

            base.DrawNonAuto();

            if (InitialHealth > 1 && (StateManager.HasBoughtScanner || (this is BaseAllyShip && this.Cast<BaseAllyShip>().IsPlayerShip)))
            {
                _healthBar.DrawNonAuto();
            }
        }
        #endregion PublicMethod
    }
}

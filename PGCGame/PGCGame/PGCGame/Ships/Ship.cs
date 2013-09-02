using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Glib.XNA;
using Glib;
using Glib.XNA.SpriteLib;

using PGCGame.CoreTypes;
using PGCGame.Ships.Allies;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;

namespace PGCGame
{
    [DebuggerDisplay("ShipType = {ShipType}, ShipID = {ShipID}")]
    public abstract class Ship : Sprite, ITimerSprite
    {
        protected const bool CanHoldShootKey = true;

        #region StaticProperties

        public static Texture2D DroneBullet;
        public static Texture2D BattleCruiserBullet;
        public static Texture2D FighterCarrierBullet;
        public static Texture2D Torpedo;
        public static Texture2D SpaceMine;
        public static Texture2D Explosion;
        /// <summary>
        /// Called when the ship dies.
        /// </summary>
        /// <remarks>
        /// THE "THE" IMPLIES THAT SHIP IS A SINGLETON!!!!!!!!!
        /// </remarks>
        public static event EventHandler Dead;

        public abstract ShipType ShipType { get; }

        #endregion StaticProperties
        #region Private Fields

        public Ship(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            StateManager.Options.ScreenResolutionChanged += new EventHandler<ViewportEventArgs>(Options_ScreenResolutionChanged);
            _healthBar = new ProgressBar(new Vector2(X, Y), Color.DarkGreen, Color.Red, spriteBatch);
            _healthBar.WidthScale = 1;
            _healthBar.HeightScale = 10;
            Moved += new EventHandler(Ship_Moved);
            _shipID = Guid.NewGuid();
            _initHealth = 100;
            _healthBar.X -= (_healthBar.Width / 2);
            _healthBar.Y -= (_healthBar.Height / 1.5f);

            _isDead = false;

            Explosion = GameContent.GameAssets.Images.SpriteSheets[SpriteSheetType.Explosion];
            _explosionSheet = new SpriteSheet(GameContent.GameAssets.Images.SpriteSheets[SpriteSheetType.Explosion], new Rectangle(0, 0, 50, 50), this.Position, spriteBatch, 8, 9);

            _explosionSheet.IsAnimated = true;
            _explosionSheet.Scale = new Vector2(1.5f);
            _explosionSheet.RestartAnimation = false;
            _currentHealth = _initHealth;

            ExplosionSFX = GameContent.GameAssets.Sound[SoundEffectType.EnemyExplodes];
            FriendlyName = ShipType.ToFriendlyString();
        }

        internal ISizedScreenObject Healthbar
        {
            get
            {
                return _healthBar;
            }
        }

        void Options_ScreenResolutionChanged(object sender, ViewportEventArgs e)
        {
            _healthBar.Position = new Vector2(X - (_healthBar.Width / 2), Y - (Height / 1.5f));
        }

        void Ship_Moved(object sender, EventArgs e)
        {
            BaseAllyShip meAsAlly = this as BaseAllyShip;
            if (meAsAlly != null && meAsAlly.IsPlayerShip)
            {
                return;
            }
            UpdateWcPos();
        }

        public PlayerType PlayerType { get; set; }


        //override: 
        //Update
        //DrawNonAuto

        public Texture2D BulletTexture { get; set; }

        public SpriteBatch WorldSb;
        //private TimeSpan _elapsedShotTime = new TimeSpan();
        protected KeyboardState _lastKs = new KeyboardState();

        protected ShipState shipState;
        public ShipState ShipState { get { return shipState; } set { shipState = value; } }

        protected bool _hasHealthBar = true;

        public bool HasHealthBar
        {
            get { return _hasHealthBar = true; }
            set { _hasHealthBar = value; }
        }

        private string _instanceName = null;

        public string FriendlyName
        {
            get { return _instanceName == null ? (PlayerType == CoreTypes.PlayerType.Enemy || PlayerType == CoreTypes.PlayerType.Solo ? "Enemy Ship" : "Ally Ship") : _instanceName; }
            private set { _instanceName = value; }
        }

        //current tier of the ship
        private ShipTier _shipTier = ShipTier.Tier1;

        //position of the ship in the world
        private List<Bullet> _flyingBullets = new List<Bullet>();

        protected ProgressBar _healthBar;

        private Vector2 _movementSpeed = Vector2.One;

        protected SpriteSheet _explosionSheet;

        private Guid _shipID;
        private bool _isFirstUpdate = true;

        #endregion Private Fields

        public Rectangle WCrectangle
        {
            get
            {
                return _wcRect;
            }
        }
        public SoundEffectInstance ShootSound { get; set; }

        public SoundEffectInstance ExplosionSFX { get; set; }

        public SoundEffectInstance EnemyShoots { get; set; }

        public SoundEffectInstance DroneDeploy { get; set; }

        public SoundEffectInstance DroneShoot { get; set; }

        private Rectangle _wcRect;

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

        public event EventHandler WCMoved;

        protected void UpdateWcPos()
        {
            _wcRect = new Rectangle((WorldCoords.X - Origin.X * Scale.X).ToInt(), (WorldCoords.Y - Origin.Y * Scale.Y).ToInt(), Width.ToInt(), Height.ToInt());
            _healthBar.Position = new Vector2(X - (_healthBar.Width / 2), Y - (Height / 1.5f));
            if (WCMoved != null)
            {
                WCMoved(this, EventArgs.Empty);
            }
            /*
            if (++WCMoveCt >= 750 && this is BaseAllyShip && (this as BaseAllyShip).IsPlayerShip)
            {
                System.Diagnostics.Debugger.Break();
            }
            */
        }

        public virtual Vector2 WorldCoords
        {
            get
            {
                return Position;
            }
            set
            {
                if (value != Position)
                {
                    Position = value;
                }
            }
        }

        public int DamagePerShot { get; set; }

        public void Kill(bool brutally)
        {
            this.CurrentHealth = brutally ? int.MinValue : 0;
        }

        /*
        public List<Bullet> FlyingBullets
        {
            get { return _flyingBullets; }
            set { _flyingBullets = value; }

        }
        */

        public TimeSpan DelayBetweenShots { get; set; }

        public Vector2 MovementSpeed
        {
            get
            {
                return _movementSpeed * (StateManager.DebugData.ShipSpeedIncrease && this is BaseAllyShip ? 10 : 1);
            }
            set
            {
                _movementSpeed = value;
            }
        }

        public int ShrinkCount = 0;

        public virtual int CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                if (value != _currentHealth)
                {
                    _currentHealth = MathHelper.Clamp(value, 0, InitialHealth).ToInt();
                    if (HealthChanged != null)
                    {
                        HealthChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler HealthChanged;

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


        public event EventHandler<BulletEventArgs> BulletFired;

        #endregion PublicProperties

        protected void FireBulletEvent(Bullet fired)
        {
            if (fired == null)
            {
                throw new ArgumentNullException("fired");
            }
            if (BulletFired != null)
            {
                BulletFired(this, new BulletEventArgs(fired));
            }
        }

        #region PublicMethod



        public virtual void Shoot()
        {
            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, WorldSb, this);
            bullet.Speed = Rotation.Vector * 3f;
            bullet.Rotation = Rotation;
            bullet.Damage = DamagePerShot;


            //BaseEnemyShip overrides this
            StateManager.AllyBullets.Legit.Add(bullet);

            if (StateManager.Options.SFXEnabled)
            {
                ShootSound.Play();
            }
            FireBulletEvent(bullet);
        }



        public virtual void Update(GameTime gt)
        {
            if (shipState != ShipState.Dead && shipState != ShipState.Exploding)
            {
                base.Update();

                if (WorldCoords.X < 0 || WorldCoords.X >= StateManager.WorldSize.Width || WorldCoords.Y <= 0 || WorldCoords.Y >= StateManager.WorldSize.Height)
                {
                    //"Void" effect
                    CurrentHealth--;
                }

                if (_isFirstUpdate)
                {
                    CurrentHealth = InitialHealth;
                    UpdateWcPos();
                    _isFirstUpdate = false;
                }



                if (CurrentHealth <= 0)
                {
                    shipState = ShipState.Dead;
                    if (StateManager.Options.SFXEnabled && ShipState == CoreTypes.ShipState.Exploding)
                    {
                        ExplosionSFX.Play();
                    }
                    if (PlayerType == CoreTypes.PlayerType.Enemy || PlayerType == CoreTypes.PlayerType.Solo)
                    {
                        //Done in DrawNonAuto() ?!?!?!?
                        //StateManager.EnemyShips.Remove(this);
                    }
                    else
                    {
                        //This code isn't called consistently
                        if (PlayerType == CoreTypes.PlayerType.MyShip)
                        {
                            StateManager.AllyBullets.Legit.Clear();
                            StateManager.AllyBullets.Dud.Clear();
                            StateManager.EnemyBullets.Legit.Clear();
                            StateManager.EnemyBullets.Dud.Clear();
                        }
                        //Done in DrawNonAuto() ?!?!?!?
                        //StateManager.AllyShips.Remove(this);    
                    }
                }

                _healthBar.Denominator = InitialHealth;
                _healthBar.Value = CurrentHealth;
            }
        }

        public bool IsAllyWith(PlayerType pt)
        {
            switch (this.PlayerType)
            {
                case CoreTypes.PlayerType.Enemy:
                    return pt == CoreTypes.PlayerType.Enemy;
                case CoreTypes.PlayerType.Ally:
                    return pt == CoreTypes.PlayerType.Ally || pt == CoreTypes.PlayerType.MyShip;
                case CoreTypes.PlayerType.Solo:
                    return false;
                case CoreTypes.PlayerType.MyShip:
                    return pt == CoreTypes.PlayerType.Ally || pt == CoreTypes.PlayerType.MyShip;
            }
            return false;
        }

        public override void DrawNonAuto()
        {
            if (shipState != ShipState.Dead)
            {
                if (CurrentHealth <= 0 && shipState != ShipState.Exploding)
                {
                    shipState = ShipState.Exploding;
                    // ExplosionSFX.Play();
                    return;
                }
                else if (shipState == ShipState.Exploding)
                {
                    if (StateManager.Options.SFXEnabled)
                    {
                        ExplosionSFX.Play();
                    }
                    _explosionSheet.Update();
                    Dead(this, EventArgs.Empty);
                    _explosionSheet.Position = this.Position;
                    _explosionSheet.DrawNonAuto();
                    if (_explosionSheet.IsComplete)
                    {
                        shipState = ShipState.Dead;

                        if (PlayerType == CoreTypes.PlayerType.Enemy || PlayerType == CoreTypes.PlayerType.Solo)
                        {
                            StateManager.EnemyShips.Remove(this);
                        }
                        else
                        {
                            StateManager.AllyShips.Remove(this);
                        }
                    }
                    return;
                }



                base.DrawNonAuto();

                if (_hasHealthBar && (StateManager.ShowShipData || (this is BaseAllyShip && this.Cast<BaseAllyShip>().IsPlayerShip)))
                {
                    _healthBar.DrawNonAuto();
                }
            }
        }
        #endregion PublicMethod
    }
}

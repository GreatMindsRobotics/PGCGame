using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PGCGame.Ships;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;
using PGCGame.CoreTypes;
using Glib.XNA.InputLib;

namespace PGCGame.Ships.Allies
{
    public abstract class BaseAllyShip : Ship
    {
        private TimeSpan _elapsedShotTime = TimeSpan.Zero;

#if WINDOWS
        private MouseState ms;
        private MouseState lastms;
#endif

        protected Vector2 _worldPos;
        protected bool _isPlayerShip;
        protected bool _rotateTowardMouse;

        public SecondaryWeaponType _SecondaryWeaponType;
        SecondaryWeapon previousSecondaryWeapon;

        public bool IsPlayerShip
        {
            get
            {
                return _isPlayerShip;
            }
            set
            {
                _isPlayerShip = value;
            }
        }


        public virtual void ShrinkRayShoot()
        {
            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, WorldSb);
            bullet.Speed = Rotation.Vector * 3f;
            bullet.UseCenterAsOrigin = true;
            bullet.Rotation = Rotation;
            bullet.Damage = 0;
            bullet.Color = Color.White;
            bullet.MaximumDistance = new Vector2(100f);
            ActiveSecondaryWeapon.Cast<ShrinkRay>().ShotBullet = true;

            ActiveSecondaryWeapon.Cast<ShrinkRay>().ShrinkRayBullets.Add(bullet);
        }

        public override Vector2 WorldCoords
        {
            get
            {
                return IsPlayerShip ? _worldPos : Position;
            }
            set
            {
                if (!IsPlayerShip)
                {
                    Position = value;
                }
                else
                {
                    _worldPos = value;
                }
            }
        }

        public bool RotateTowardsMouse
        {
            get
            {
                return _rotateTowardMouse;
            }
            set
            {
                _rotateTowardMouse = value;
            }
        }

        public bool CanShoot
        {
            get
            {
                return _elapsedShotTime >= DelayBetweenShots && IsPlayerShip;
            }
        }

        public SecondaryWeapon ActiveSecondaryWeapon;


        public BaseAllyShip(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            PlayerType = CoreTypes.PlayerType.Ally;
        }

        protected static Dictionary<ShipTier, int> _cost;
        public static Dictionary<ShipTier, int> Cost { get { return _cost; } set { _cost = value; } }

        static BaseAllyShip()
        {
            _cost = new Dictionary<ShipTier, int>();

            _cost.Add(ShipTier.Tier1, 100000);
            _cost.Add(ShipTier.Tier2, 200000);
            _cost.Add(ShipTier.Tier3, 300000);
            _cost.Add(ShipTier.Tier4, 400000);
        }

        public override void Update(GameTime gt)
        {
            if (CurrentHealth <= 0)
            {
                base.Update(gt);

                if (FlyingBullets.Count == 0)
                {
                    _isDead = true;
                }

                return;
            }

            if (RotateTowardsMouse)
            {
#if WINDOWS
                ms = MouseManager.CurrentMouseState;
                Vector2 mousePos = new Vector2(ms.X, ms.Y);
                Vector2 targetPos = mousePos - Position;

                //Rotate towards mouse
                Rotation.Radians = Math.Atan2(targetPos.X, -targetPos.Y).ToFloat();

                lastms = ms;
#elif XBOX
                if (new Vector2(Math.Round(GamePadManager.One.Current.ThumbSticks.Right.X * 10).ToFloat() / 10.ToFloat(), Math.Round(GamePadManager.One.Current.ThumbSticks.Right.Y * 10).ToFloat() / 10.ToFloat()) != Vector2.Zero)
                {
                    Rotation.Vector = new Vector2(GamePadManager.One.Current.ThumbSticks.Right.X, -GamePadManager.One.Current.ThumbSticks.Right.Y);
                }
#endif
            }

            //Shoot
            KeyboardState ks = Keyboard.GetState();
            _elapsedShotTime += gt.ElapsedGameTime;
            //Shoot w/ space key

            if (CanShoot)
            {
#if WINDOWS
                if ((StateManager.Options.LeftButtonEnabled && ms.LeftButton == ButtonState.Pressed) || (!StateManager.Options.LeftButtonEnabled && ks.IsKeyDown(Keys.Space)))
                {
                    Shoot();
                    _elapsedShotTime = TimeSpan.Zero;
                }
#elif XBOX
                if (GamePadManager.One.Current.Triggers.Right > 0.875f)
                {
                    Shoot();
                    _elapsedShotTime = TimeSpan.Zero;
                }
#endif
            }

            if (StateManager.PowerUps.Count > 0 && ks.IsKeyDown(Keys.Q) && _lastKs != null && !_lastKs.IsKeyDown(Keys.Q) && ActiveSecondaryWeapon.fired == false)
            {
                foreach (var secondaryWeapon in StateManager.PowerUps)
                {
                    if (ActiveSecondaryWeapon.GetType() != secondaryWeapon.GetType())
                    {
                        previousSecondaryWeapon = ActiveSecondaryWeapon;
                        ActiveSecondaryWeapon = secondaryWeapon;
                        break;
                    }
                }
            }
            else if (StateManager.PowerUps.Count > 0 && ks.IsKeyDown(Keys.Q) && _lastKs != null && !_lastKs.IsKeyDown(Keys.E) && ActiveSecondaryWeapon.fired == false)
            {
                if (previousSecondaryWeapon != null)
                {
                    ActiveSecondaryWeapon = previousSecondaryWeapon;
                }
                else
                {
                    foreach (var secondaryWeapon in StateManager.PowerUps)
                    {
                        if (ActiveSecondaryWeapon.GetType() != secondaryWeapon.GetType())
                        {
                            previousSecondaryWeapon = ActiveSecondaryWeapon;
                            ActiveSecondaryWeapon = secondaryWeapon;
                            break;
                        }
                    }
                }
            }
            if (ActiveSecondaryWeapon == null && StateManager.PowerUps.Count > 0)
            {
                ActiveSecondaryWeapon = StateManager.PowerUps.First();
            }

            //Deploy secondary weapon
            else if (ActiveSecondaryWeapon != null && ks.IsKeyDown(Keys.RightShift) && _lastKs != null && !_lastKs.IsKeyDown(Keys.RightShift) && ActiveSecondaryWeapon.fired == false)
            {
                ActiveSecondaryWeapon.fired = true;
                StateManager.PowerUps.Remove(ActiveSecondaryWeapon);
                ActiveSecondaryWeapon.ParentShip = this;
                ActiveSecondaryWeapon.Killed += new EventHandler(ActiveSecondaryWeapon_Killed);
                //Specifics of certain secondary weapons 
                switch (ActiveSecondaryWeapon.GetType().FullName)
                {

                    case "PGCGame.SpaceMine":
                        ActiveSecondaryWeapon.Cast<SpaceMine>().SpaceMineState = SpaceMineState.Deploying;
                        break;
                    case "PGCGame.EMP":
                        ActiveSecondaryWeapon.Cast<EMP>().PublicEMPState = EMPState.Deployed;
                        break;
                    case "PGCGame.ShrinkRay":
                        if (ActiveSecondaryWeapon.Cast<ShrinkRay>().ShotBullet == false)
                        {

                            ActiveSecondaryWeapon.Position = ActiveSecondaryWeapon.ParentShip.WorldCoords;
                            ShrinkRayShoot();
                        }
                        break;
                }

                ActiveSecondaryWeapon.Update(gt);
            }

            if (ActiveSecondaryWeapon != null)
            {
                ActiveSecondaryWeapon.Update(gt);
            }

            _lastKs = ks;

            base.Update(gt);

        }

        void ActiveSecondaryWeapon_Killed(object sender, EventArgs e)
        {
            StateManager.PowerUps.Remove(ActiveSecondaryWeapon);
            ActiveSecondaryWeapon = null;
        }
    }
}

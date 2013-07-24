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
        //SecondaryWeapon previousSecondaryWeapon;

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


        //int weaponIndex = 0;

        public virtual void ShrinkRayShoot(ShrinkRay bulletShooter)
        {
            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, WorldSb, this);
            bullet.Speed = Rotation.Vector * 3f;
            bullet.UseCenterAsOrigin = true;
            bullet.Rotation = Rotation;
            bullet.Damage = 0;
            bullet.Color = Color.White;
            bullet.MaximumDistance = new Vector2(100f);
            bulletShooter.ShotBullet = true;
            bulletShooter.ShrinkRayBullets.Add(bullet);
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

        public List<SecondaryWeapon> ActiveSecondaryWeapons = new List<SecondaryWeapon>();

        private int _updateI = 0;

        public BaseAllyShip(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            PlayerType = CoreTypes.PlayerType.Ally;
        }

        protected static Dictionary<ShipTier, int> _cost;
        public static Dictionary<ShipTier, int> Cost { get { return _cost; } set { _cost = value; } }

        public int SecondaryWeaponIndex = 0;

        static BaseAllyShip()
        {
            _cost = new Dictionary<ShipTier, int>();

            _cost.Add(ShipTier.Tier1, 100000);
            _cost.Add(ShipTier.Tier2, 200000);
            _cost.Add(ShipTier.Tier3, 300000);
            _cost.Add(ShipTier.Tier4, 400000);
        }

        public string CurrentWeaponName
        {
            get
            {
                return StateManager.PowerUps[SecondaryWeaponIndex].Count == 0 ? null : StateManager.PowerUps[SecondaryWeaponIndex].Peek().Name;
            }
        }

        public override int CurrentHealth
        {
            get
            {
                return base.CurrentHealth;
            }
            set
            {
                if (StateManager.DebugData.Invincible && value < CurrentHealth)
                {
                    return;
                }

                base.CurrentHealth = value;
            }
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

            if (StateManager.DebugData.EmergencyHeal && ks.IsKeyDown(Keys.F4))
            {
                CurrentHealth = InitialHealth;
            }

            if (StateManager.PowerUps[0].Count > 0 || StateManager.PowerUps[1].Count > 0 || StateManager.PowerUps[2].Count > 0)
            {

                if ( StateManager.PowerUps[SecondaryWeaponIndex].Count == 0 || (ks.IsKeyDown(Keys.E) && _lastKs.IsKeyUp(Keys.E)) )
                {
                    int selCount = 0;
                    do
                    {
                        SecondaryWeaponIndex++;
                        SecondaryWeaponIndex %= StateManager.PowerUps.Length;
                    } while (StateManager.PowerUps[SecondaryWeaponIndex].Count == 0 && selCount < StateManager.PowerUps.Length);
                }
                if (ks.IsKeyDown(Keys.Q) && _lastKs.IsKeyUp(Keys.Q))
                {
                    int selCount = 0;
                    do
                    {
                        SecondaryWeaponIndex--;
                        if (SecondaryWeaponIndex < 0)
                        {
                            SecondaryWeaponIndex = StateManager.PowerUps.Length + SecondaryWeaponIndex;
                        }
                    } while (StateManager.PowerUps[SecondaryWeaponIndex].Count == 0 && selCount < StateManager.PowerUps.Length);
                }
            }

            /*
            
            //Andrew's horrible implementation of powerup switching
            
            if (StateManager.PowerUps.Count > 0 && ks.IsKeyDown(Keys.Q) && _lastKs != null && _lastKs.IsKeyUp(Keys.Q) && ActiveSecondaryWeapon.fired == false)
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
            else if (StateManager.PowerUps.Count > 0 && ks.IsKeyDown(Keys.E) && _lastKs != null && _lastKs.IsKeyUp(Keys.E) && ActiveSecondaryWeapon.fired == false)
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
            */

            //Deploy secondary weapon
            if (StateManager.PowerUps[SecondaryWeaponIndex].Count > 0 && ks.IsKeyDown(Keys.RightShift) && _lastKs != null && _lastKs.IsKeyUp(Keys.RightShift))
            {
                SecondaryWeapon fired = StateManager.DebugData.InfiniteSecondaryWeapons ? StateManager.PowerUps[SecondaryWeaponIndex].Peek() : StateManager.PowerUps[SecondaryWeaponIndex].Pop();
                fired.fired = true;
                //canDeploySecWeap = false;
                //StateManager.PowerUps.Remove(ActiveSecondaryWeapon);

                fired.ParentShip = this;
                fired.Killed += new EventHandler(ActiveSecondaryWeapon_Killed);


                //Specifics of certain secondary weapons 
                switch (fired.GetType().FullName)
                {

                    case "PGCGame.SpaceMine":
                        fired.Cast<SpaceMine>().SpaceMineState = SpaceMineState.Deploying;
                        break;
                    case "PGCGame.EMP":
                        fired.Cast<EMP>().PublicEMPState = EMPState.Deployed;
                        break;
                    case "PGCGame.ShrinkRay":
                        if (!fired.Cast<ShrinkRay>().ShotBullet)
                        {

                            fired.Position = fired.ParentShip.WorldCoords;
                            ShrinkRayShoot(fired.Cast<ShrinkRay>());
                        }
                        break;
                }
                ActiveSecondaryWeapons.Add(fired);

            }


            if (ActiveSecondaryWeapons.Count > 0)
            {
                _updateI = 0;
                for (; _updateI < ActiveSecondaryWeapons.Count; _updateI++)
                {
                    if (_updateI < 0 && ActiveSecondaryWeapons.Count <= 0)
                    {
                        break;
                    }
                    else if (_updateI < 0)
                    {
                        _updateI = 0;
                    }
                    ActiveSecondaryWeapons[_updateI].Update(gt);
                }
            }

            _lastKs = ks;

            base.Update(gt);

        }

        void ActiveSecondaryWeapon_Killed(object sender, EventArgs e)
        {
            ActiveSecondaryWeapons.Remove(sender.Cast<SecondaryWeapon>());
            _updateI--;
        }
    }
}

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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Net;

namespace PGCGame.Ships.Allies
{
    public abstract class BaseAllyShip : Ship
    {
        public NetworkGamer Controller;

        public static BaseAllyShip CreateShip(ShipType type, ShipTier tier, SpriteBatch spawnSpriteBatch, Boolean isAllyShip)
        {
            BaseAllyShip bas = null;
            switch (type)
            {
                case ShipType.BattleCruiser:
                    bas = new BattleCruiser(GameContent.Assets.Images.Ships[type, tier], Vector2.Zero, spawnSpriteBatch, isAllyShip);
                    break;
                case ShipType.FighterCarrier:
                    bas = new FighterCarrier(GameContent.Assets.Images.Ships[type, tier], Vector2.Zero, spawnSpriteBatch, GameContent.Assets.Images.Ships[ShipType.Drone, ShipTier.Tier1], isAllyShip);
                    break;
                case ShipType.TorpedoShip:
                    bas = new TorpedoShip(GameContent.Assets.Images.Ships[type, tier], Vector2.Zero, spawnSpriteBatch, isAllyShip);
                    break;

                default:
                    throw new NotImplementedException("Cannot create the specified ship type.");
            }

            bas.Tier = tier;
            bas.UseCenterAsOrigin = true;
            //bas.FriendlyName = type.ToFriendlyString();

            return bas;
        }

        public static BaseAllyShip CreateShip(ShipType type, SpriteBatch sb)
        {
            return CreateShip(type, ShipTier.Tier1, sb, true);
        }

        public static BaseAllyShip CreateShip(ShipType type, SpriteBatch sb, bool isAlly)
        {
            return CreateShip(type, ShipTier.Tier1, sb, isAlly);
        }

        public static BaseAllyShip CreateShip(ShipStats stats, SpriteBatch sb, bool isAlly)
        {
            return CreateShip(stats.Type, stats.Tier, sb, isAlly);
        }

        public static BaseAllyShip CreateShip(ShipStats stats, SpriteBatch sb)
        {
            return CreateShip(stats.Type, stats.Tier, sb, true);
        }


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
            Bullet bullet = StateManager.BulletPool.GetBullet();
            bullet.InitializePooledBullet(WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, this);
            bullet.SpriteBatch = WorldSb;

            //Shrink ray is always laser
            bullet.Texture = GameContent.Assets.Images.Ships.Bullets.Laser;

            
            bullet.Speed = Rotation.Vector * 3f;
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
                if (value != WorldCoords)
                {
                    if (!IsPlayerShip)
                    {
                        Position = value;
                    }
                    else
                    {
                        _worldPos = value;
                        UpdateWcPos();
                    }
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

        protected BaseAllyShip(Texture2D texture, Vector2 location, SpriteBatch spriteBatch, bool isAlly)
            : base(texture, location, spriteBatch)
        {
            StateManager.InputManager.DebugControlManager.ShipHeal += new EventHandler(DebugControlManager_ShipHeal);
            StateManager.InputManager.DebugControlManager.ShipSuicide += new EventHandler(DebugControlManager_ShipSuicide);
            if (isAlly)
            {
                StateManager.AllyShips.Add(this);
            }

            PlayerType = CoreTypes.PlayerType.Ally;
            shipState = ShipState.Alive;
        }

        void DebugControlManager_ShipHeal(object sender, EventArgs e)
        {
            CurrentHealth = InitialHealth;
        }

        void DebugControlManager_ShipSuicide(object sender, EventArgs e)
        {
            if (shipState != CoreTypes.ShipState.Exploding && shipState != CoreTypes.ShipState.Dead)
            {
                this.shipState = CoreTypes.ShipState.Exploding;
            }
        }


        protected static Dictionary<ShipTier, int> _cost;
        public static Dictionary<ShipTier, int> Cost { get { return _cost; } set { _cost = value; } }

        public SecondaryWeaponType SecondaryWeaponIndex = SecondaryWeaponType.SpaceMine;

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
                string returnVal = StateManager.PowerUps[SecondaryWeaponIndex.ToInt()].Count == 0 ? null : StateManager.PowerUps[SecondaryWeaponIndex.ToInt()].Peek().Name;
                return returnVal == null ? returnVal : returnVal + " X" + StateManager.PowerUps[SecondaryWeaponIndex.ToInt()].Count;
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
                if (StateManager.DebugData.Invincible && value < InitialHealth)
                {
                    return;
                }

                base.CurrentHealth = value;
            }
        }

        public override void Update(GameTime gt)
        {
            if (shipState != CoreTypes.ShipState.Alive)
            {
                return;
            }

            if (CurrentHealth <= 0)
            {
                if (StateManager.Options.SFXEnabled)
                {
                   ExplosionSFX.Play();
                }
                base.Update(gt);

                _isDead = true;
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
            _elapsedShotTime += gt.ElapsedGameTime;
            if (CanShoot)
            {
                if (StateManager.InputManager.ShootControlDown)
                {
                    Shoot();
                    _elapsedShotTime = TimeSpan.Zero;
                }
            }
         

            if (StateManager.PowerUps[0].Count > 0 || StateManager.PowerUps[1].Count > 0 || StateManager.PowerUps[2].Count > 0 || StateManager.PowerUps[3].Count > 0)
            {

                if ( StateManager.PowerUps[SecondaryWeaponIndex.ToInt()].Count == 0 || StateManager.InputManager.ShouldMoveSecondaryWeaponSelection(1))
                {
                    int selCount = 0;
                    do
                    {
                        SecondaryWeaponIndex++;
                        SecondaryWeaponIndex = (SecondaryWeaponType)(SecondaryWeaponIndex.ToInt() % StateManager.PowerUps.Length);
                    } while (StateManager.PowerUps[SecondaryWeaponIndex.ToInt()].Count == 0 && selCount < StateManager.PowerUps.Length);
                }
                if (StateManager.PowerUps[SecondaryWeaponIndex.ToInt()].Count == 0 || StateManager.InputManager.ShouldMoveSecondaryWeaponSelection(-1))
                {
                    int selCount = 0;
                    do
                    {
                        SecondaryWeaponIndex--;
                        if (SecondaryWeaponIndex < 0)
                        {
                            SecondaryWeaponIndex = StateManager.PowerUps.Length + SecondaryWeaponIndex;
                        }
                    } while (StateManager.PowerUps[SecondaryWeaponIndex.ToInt()].Count == 0 && selCount < StateManager.PowerUps.Length);
                }
            }

            if (StateManager.InputManager.DeploySecondaryWeapon(SecondaryWeaponIndex))
            {
                if ((CurrentHealth < InitialHealth && StateManager.PowerUps[SecondaryWeaponIndex.ToInt()].Peek().GetType() == typeof(HealthPack)) || StateManager.PowerUps[SecondaryWeaponIndex.ToInt()].Peek().GetType() != typeof(HealthPack))
                {
                    SecondaryWeapon fired = StateManager.DebugData.InfiniteSecondaryWeapons ? StateManager.PowerUps[SecondaryWeaponIndex.ToInt()].Peek() : StateManager.PowerUps[SecondaryWeaponIndex.ToInt()].Pop();
                    fired.fired = true;
                    //canDeploySecWeap = false;
                    //StateManager.PowerUps.Remove(ActiveSecondaryWeapon);

                    fired.ParentShip = this;
                    fired.Killed += new EventHandler(ActiveSecondaryWeapon_Killed);


                    //Specifics of certain secondary weapons 
                    ActiveSecondaryWeapons.Add(fired); ;
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
                }
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

            base.Update(gt);

        }

        void ActiveSecondaryWeapon_Killed(object sender, EventArgs e)
        {
            ActiveSecondaryWeapons.Remove(sender.Cast<SecondaryWeapon>());
            _updateI--;
        }
    }
}

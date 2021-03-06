﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Glib.XNA;
using Microsoft.Xna.Framework.Audio;
using Glib;

namespace PGCGame.CoreTypes
{

    /// <summary>
    /// Singleton class; create in LoadContent, and use GameContent.GameAssets
    /// </summary>
    public class GameContent : IDisposable
    {
        private static GameContent _gameAssets = null;
        public static GameContent Assets
        {
            get
            {
                if (_gameAssets == null)
                {
                    throw new NullReferenceException("GameContent is not initialized. Please create new GameContent instance");
                }

                return _gameAssets;
            }
        }

        public static GameContent InitializeAssets(ContentManager content)
        {
            return new GameContent(content);
        }

        private static TextureFactory _textureFactory;

        /// <summary>
        /// Thread safety.
        /// </summary>
        private static bool _isInitializing = false;

        private GameContent(ContentManager content)
        {
            if (_isInitializing)
            {
                throw new InvalidOperationException("Another thread is initializing this object. Please access GameContent.Assets after the initialization is completed.");
            }

            _isInitializing = true;

            if (_gameAssets != null)
            {
                throw new InvalidOperationException("This singleton class has already been initialized, please use GameContent.Assets.");
            }

            _textureFactory = new TextureFactory(StateManager.GraphicsManager.GraphicsDevice);

            Fonts = new GameFonts(content);
            Images = new GameImages(content);
            Music = new GameMusic(content);
            Sound = new GameSound(content);

            _gameAssets = this;

            _isInitializing = false;
        }

        public readonly GameMusic Music;
        public class GameMusic
        {
            private readonly Dictionary<ScreenMusic, Song> _gameMusic;

            public Song this[ScreenMusic index]
            {
                get { return _gameMusic[index == ScreenMusic.MainMenu ? ScreenMusic.Level1 : index]; }
            }

            internal GameMusic(ContentManager content)
            {
                _gameMusic = new Dictionary<ScreenMusic, Song>();
                _gameMusic.Add(ScreenMusic.Level1, content.Load<Song>("Songs\\Movement Proposition"));
                _gameMusic.Add(ScreenMusic.Credits, content.Load<Song>("Songs\\Failing Defense"));
            }
        }
        public readonly GameSound Sound;
        public class GameSound : IEnumerable<SoundEffectInstance>
        {


            private readonly Dictionary<SoundEffectType, SoundEffectInstance> _gameSFX;

            public SoundEffectInstance this[SoundEffectType index]
            {
                get { return _gameSFX[index]; }
            }

            internal GameSound(ContentManager content)
            {
                _gameSFX = new Dictionary<SoundEffectType, SoundEffectInstance>();
                _gameSFX.Add(SoundEffectType.DeployEMP, content.Load<SoundEffect>("SFX\\SecondaryWeapons\\Emp\\EMPSound").CreateInstance());
                _gameSFX.Add(SoundEffectType.DeployShrinkRay, content.Load<SoundEffect>("SFX\\SecondaryWeapons\\ShrinkRay\\ShrinkRayBullet").CreateInstance());
                _gameSFX.Add(SoundEffectType.ExplodeSpaceMine, content.Load<SoundEffect>("SFX\\SecondaryWeapons\\SpaceMine\\explosionSpaceMine").CreateInstance());
                _gameSFX.Add(SoundEffectType.DeploySpaceMine, content.Load<SoundEffect>("SFX\\SecondaryWeapons\\SpaceMine\\countdown").CreateInstance());
                _gameSFX.Add(SoundEffectType.DeployHealthPack, content.Load<SoundEffect>("SFX\\SecondaryWeapons\\HealthPack\\heathPack").CreateInstance());
                _gameSFX.Add(SoundEffectType.SpaceDoorOpening, content.Load<SoundEffect>("SFX\\SpaceDoor\\Alarm").CreateInstance());
                _gameSFX.Add(SoundEffectType.SpaceShipLeaving, content.Load<SoundEffect>("SFX\\SpaceDoor\\SpaceShipLeaving").CreateInstance());
                _gameSFX.Add(SoundEffectType.BattleCruiserFire, content.Load<SoundEffect>("SFX\\Ships\\BattleCruiser\\BattleCruiserBulletSFX").CreateInstance());
                _gameSFX.Add(SoundEffectType.FighterCarrierFire, content.Load<SoundEffect>("SFX\\Ships\\FighterCarrier\\fighterCarrier").CreateInstance());
                _gameSFX.Add(SoundEffectType.TorpedoShipFire, content.Load<SoundEffect>("SFX\\Ships\\TorpedoShip\\torpedoShip").CreateInstance());
                _gameSFX.Add(SoundEffectType.EnemyExplodes, content.Load<SoundEffect>("SFX\\Explosions\\enemyExplosion").CreateInstance());
                _gameSFX.Add(SoundEffectType.BoughtItem, content.Load<SoundEffect>("SFX\\NonPlayingObjects\\Money\\itemBought").CreateInstance());
                _gameSFX.Add(SoundEffectType.EnemyShoots, content.Load<SoundEffect>("SFX\\Ships\\EnemyBattleCruiser\\enemyBattleCruiser").CreateInstance());
                _gameSFX.Add(SoundEffectType.ButtonPressed, content.Load<SoundEffect>("SFX\\NonPlayingObjects\\ButtonPressed\\buttonPressed").CreateInstance());
                _gameSFX.Add(SoundEffectType.DronesDeploy, content.Load<SoundEffect>("SFX\\Ships\\Drones\\Deploy").CreateInstance());
                _gameSFX.Add(SoundEffectType.DronesShoot, content.Load<SoundEffect>("SFX\\Ships\\Drones\\droneBullet").CreateInstance());
                _gameSFX.Add(SoundEffectType.CloneMade, content.Load<SoundEffect>("SFX\\Ships\\Boss\\clonesMade").CreateInstance());
            }

            public IEnumerator<SoundEffectInstance> GetEnumerator()
            {
                return _gameSFX.Values.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return _gameSFX.Values.GetEnumerator();
            }
        }

        public readonly GameFonts Fonts;
        public class GameFonts
        {
            public readonly SpriteFont NormalText;
            public readonly SpriteFont BoldText;

            internal GameFonts(ContentManager content)
            {
                NormalText = content.Load<SpriteFont>("Fonts\\SegoeUIMono");
                BoldText = content.Load<SpriteFont>("Fonts\\SegoeUIMonoBold");
            }
        }

        public readonly GameImages Images;
        public class GameImages
        {
            internal GameImages(ContentManager content)
            {
                Backgrounds = new GameBackgrounds(content);
                Controls = new GameControls(content);
                Ships = new GameShips(content);
                MiniShips = new GameMiniShips(content);
                NonPlayingObjects = new NonPlayingGameObjects(content);
                SecondaryWeapon = new GameSecondaryWeapon(content);
                Equipment = new GameEquipment(content);
                SpriteSheets = new SpriteSheetLoader(content);
                particles = new Particles(content);
            }

            public readonly GameBackgrounds Backgrounds;
            public class GameBackgrounds
            {
                internal GameBackgrounds(ContentManager content)
                {
                    Screens = new GameScreens(content);
                    Levels = new GameLevels(content);
                }

                public readonly GameScreens Screens;
                public class GameScreens
                {
                    private Dictionary<ScreenBackgrounds, Texture2D> _screenTextures;

                    public Texture2D this[ScreenBackgrounds index]
                    {
                        get { return _screenTextures[index]; }
                    }

                    internal GameScreens(ContentManager content)
                    {
                        _screenTextures = new Dictionary<ScreenBackgrounds, Texture2D>();
                        _screenTextures.Add(ScreenBackgrounds.GlobalScrollingBg, content.Load<Texture2D>("Images\\Backgrounds\\1920by1080SkyStar"));
                        _screenTextures.Add(ScreenBackgrounds.Credits, content.Load<Texture2D>("Images\\Backgrounds\\1920by1080SkyStar"));
                    }
                }

                public readonly GameLevels Levels;
                public class GameLevels
                {
                    private Dictionary<GameLevel, Texture2D> _gameLevelTextures;

                    public Texture2D this[GameLevel index]
                    {
                        get { return _gameLevelTextures[index]; }
                    }

                    internal GameLevels(ContentManager content)
                    {
                        _gameLevelTextures = new Dictionary<GameLevel, Texture2D>();
                        _gameLevelTextures.Add(GameLevel.Level1, content.Load<Texture2D>("Images\\Backgrounds\\NebulaSky"));
                    }
                }
            }

            public readonly Particles particles;
            public class Particles
            {                
                private Dictionary<ParticleType, Texture2D> _particles;

                public Texture2D this[ParticleType index]
                {
                    get { return _particles[index]; }
                }

                internal Particles(ContentManager content)
                {
                    _particles = new Dictionary<ParticleType, Texture2D>();
                    _particles.Add(ParticleType.Square, content.Load<Texture2D>("Images\\Equipment\\Particles\\Orange Square"));
                    _particles.Add(ParticleType.Circle, content.Load<Texture2D>("Images\\Equipment\\Particles\\Orange Circle"));
                }
            }

            public readonly GameShips Ships;
            public class GameShips
            {
                private readonly Dictionary<KeyValuePair<ShipType, ShipTier>, Texture2D> _shipTextures;

                public Texture2D this[ShipType ship, ShipTier tier]
                {
                    get { return _shipTextures[new KeyValuePair<ShipType, ShipTier>(ship, tier)]; }
                }

                internal GameShips(ContentManager content)
                {
                    _shipTextures = new Dictionary<KeyValuePair<ShipType, ShipTier>, Texture2D>();

                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.NoShip, ShipTier.NoShip), _textureFactory.CreateSquare(15, Color.Transparent));

                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.BattleCruiser, ShipTier.Tier1), content.Load<Texture2D>("Images\\Ships\\Allies\\BattleCruiser\\Tier1"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.BattleCruiser, ShipTier.Tier2), content.Load<Texture2D>("Images\\Ships\\Allies\\BattleCruiser\\Tier2"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.BattleCruiser, ShipTier.Tier3), content.Load<Texture2D>("Images\\Ships\\Allies\\BattleCruiser\\Tier3"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.BattleCruiser, ShipTier.Tier4), content.Load<Texture2D>("Images\\Ships\\Allies\\BattleCruiser\\Tier4"));

                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.FighterCarrier, ShipTier.Tier1), content.Load<Texture2D>("Images\\Ships\\Allies\\FighterCarrier\\Tier1"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.FighterCarrier, ShipTier.Tier2), content.Load<Texture2D>("Images\\Ships\\Allies\\FighterCarrier\\Tier2"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.FighterCarrier, ShipTier.Tier3), content.Load<Texture2D>("Images\\Ships\\Allies\\FighterCarrier\\Tier3"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.FighterCarrier, ShipTier.Tier4), content.Load<Texture2D>("Images\\Ships\\Allies\\FighterCarrier\\Tier4"));

                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.TorpedoShip, ShipTier.Tier1), content.Load<Texture2D>("Images\\Ships\\Allies\\TorpedoShip\\Tier1"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.TorpedoShip, ShipTier.Tier2), content.Load<Texture2D>("Images\\Ships\\Allies\\TorpedoShip\\Tier2"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.TorpedoShip, ShipTier.Tier3), content.Load<Texture2D>("Images\\Ships\\Allies\\TorpedoShip\\Tier3"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.TorpedoShip, ShipTier.Tier4), content.Load<Texture2D>("Images\\Ships\\Allies\\TorpedoShip\\Tier4"));

                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.Drone, ShipTier.Tier1), content.Load<Texture2D>("Images\\Ships\\Allies\\FighterCarrier\\Drones\\Tier1"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.Drone, ShipTier.Tier2), content.Load<Texture2D>("Images\\Ships\\Allies\\FighterCarrier\\Drones\\Tier2"));


                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.EnemyBattleCruiser, ShipTier.Tier1), content.Load<Texture2D>("Images\\Ships\\Enemies\\BattleCruiser1"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.EnemyBattleCruiser, ShipTier.Tier2), content.Load<Texture2D>("Images\\Ships\\Enemies\\Battlecruiser2"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.EnemyBattleCruiser, ShipTier.Tier3), content.Load<Texture2D>("Images\\Ships\\Enemies\\Battlecruiser3"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.EnemyBattleCruiser, ShipTier.Tier4), content.Load<Texture2D>("Images\\Ships\\Enemies\\Battlecruiser4"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.EnemyFighterCarrier, ShipTier.Tier4), content.Load<Texture2D>("Images\\Ships\\Enemies\\Mothership"));
                    _shipTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.EnemyDrone, ShipTier.Tier1), content.Load<Texture2D>("Images\\Ships\\Enemies\\Scout"));


                    Bullets = new ShipBullets(content);
                }

                public readonly ShipBullets Bullets;
                public class ShipBullets
                {
                    private readonly Dictionary<KeyValuePair<ShipType, ShipTier>, Texture2D> _bulletTextures;

                    public Texture2D this[ShipType ship, ShipTier tier]
                    {
                        get { return _bulletTextures[new KeyValuePair<ShipType, ShipTier>(ship, tier)]; }
                    }

                    public readonly Texture2D Torpedo;
                    public readonly Texture2D Laser;

                    internal ShipBullets(ContentManager content)
                    {
                        _bulletTextures = new Dictionary<KeyValuePair<ShipType, ShipTier>, Texture2D>();
                        
                        //The laser texture
                        Laser = content.Load<Texture2D>("Images\\TempBullets\\Laser");
                        //TODO: Actual torpedo texture
                        Torpedo = _textureFactory.CreateRectangle(5, 3, Color.Red);

                        for (int i = ShipTier.Tier1.ToInt(); i <= ShipTier.Tier4.ToInt(); i++)
                        {
                            _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.EnemyBattleCruiser, Enum.Parse(typeof(ShipTier), i.ToString(), true).Cast<ShipTier>()), Laser);
                            _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.EnemyFighterCarrier, Enum.Parse(typeof(ShipTier), i.ToString(), true).Cast<ShipTier>()), Laser);
                            _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.EnemyTorpedoShip, Enum.Parse(typeof(ShipTier), i.ToString(), true).Cast<ShipTier>()), Torpedo);
                            _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.EnemyBoss, Enum.Parse(typeof(ShipTier), i.ToString(), true).Cast<ShipTier>()), Laser);
                            _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.EnemyBossesClones, Enum.Parse(typeof(ShipTier), i.ToString(), true).Cast<ShipTier>()), Laser);
                            _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.EnemyDrone, Enum.Parse(typeof(ShipTier), i.ToString(), true).Cast<ShipTier>()), Laser);

                            _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.BattleCruiser, Enum.Parse(typeof(ShipTier), i.ToString(), true).Cast<ShipTier>()), Laser);
                            _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.FighterCarrier, Enum.Parse(typeof(ShipTier), i.ToString(), true).Cast<ShipTier>()), Laser);
                            _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.TorpedoShip, Enum.Parse(typeof(ShipTier), i.ToString(), true).Cast<ShipTier>()), Torpedo);
                            _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.Drone, Enum.Parse(typeof(ShipTier), i.ToString(), true).Cast<ShipTier>()), Laser);
                        }

                        /*
                         * TODO: Create correct bullets for each ship / tier
                         * 
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.BattleCruiser, ShipTier.Tier1), content.Load<Texture2D>("Images\\Ships\\Allies\\BattleCruiser\\Bullets\\Tier1"));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.BattleCruiser, ShipTier.Tier2), content.Load<Texture2D>("Images\\Ships\\Allies\\BattleCruiser\\Bullets\\Tier2"));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.BattleCruiser, ShipTier.Tier3), content.Load<Texture2D>("Images\\Ships\\Allies\\BattleCruiser\\Bullets\\Tier3"));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.BattleCruiser, ShipTier.Tier4), content.Load<Texture2D>("Images\\Ships\\Allies\\BattleCruiser\\Bullets\\Tier4"));

                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.FighterCarrier, ShipTier.Tier1), content.Load<Texture2D>("Images\\Ships\\Allies\\FighterCarrier\\Bullets\\Tier1"));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.FighterCarrier, ShipTier.Tier2), content.Load<Texture2D>("Images\\Ships\\Allies\\FighterCarrier\\Bullets\\Tier2"));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.FighterCarrier, ShipTier.Tier3), content.Load<Texture2D>("Images\\Ships\\Allies\\FighterCarrier\\Bullets\\Tier3"));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.FighterCarrier, ShipTier.Tier4), content.Load<Texture2D>("Images\\Ships\\Allies\\FighterCarrier\\Bullets\\Tier4"));

                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.TorpedoShip, ShipTier.Tier1), content.Load<Texture2D>("Images\\Ships\\Allies\\TorpedoShip\\Bullets\\Tier1"));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.TorpedoShip, ShipTier.Tier2), content.Load<Texture2D>("Images\\Ships\\Allies\\TorpedoShip\\Bullets\\Tier2"));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.TorpedoShip, ShipTier.Tier3), content.Load<Texture2D>("Images\\Ships\\Allies\\TorpedoShip\\Bullets\\Tier3"));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.TorpedoShip, ShipTier.Tier4), content.Load<Texture2D>("Images\\Ships\\Allies\\TorpedoShip\\Bullets\\Tier4"));

                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.Drone, ShipTier.Tier1), content.Load<Texture2D>("Images\\Ships\\Allies\\FighterCarrier\\Drones\\Bullets\\Tier1"));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.Drone, ShipTier.Tier2), content.Load<Texture2D>("Images\\Ships\\Allies\\FighterCarrier\\Drones\\Bullets\\Tier2"));
                         */
                    }

                }
            }

            public readonly GameMiniShips MiniShips;
            public class GameMiniShips
            {
                private readonly Dictionary<PlayerType, Texture2D> _miniShipTextures;
                private readonly Dictionary<ShipType, Texture2D> _miniShipTexturesByShipType;

                public Texture2D this[PlayerType index]
                {
                    get { return _miniShipTextures[index]; }
                }

                public Texture2D this[ShipType index]
                {
                    get { return _miniShipTexturesByShipType[index]; }
                }

                internal GameMiniShips(ContentManager content)
                {
                    _miniShipTextures = new Dictionary<PlayerType, Texture2D>();
                    _miniShipTexturesByShipType = new Dictionary<ShipType, Texture2D>();

                    

                    _miniShipTextures.Add(PlayerType.MyShip, content.Load<Texture2D>("Images\\MiniMap\\WhiteCircle100x100"));
                    _miniShipTextures.Add(PlayerType.Enemy, content.Load<Texture2D>("Images\\MiniMap\\WhiteTriangle100x100"));
                    _miniShipTextures.Add(PlayerType.Ally, content.Load<Texture2D>("Images\\MiniMap\\WhiteCircle100x100"));
                    _miniShipTextures.Add(PlayerType.Solo, content.Load<Texture2D>("Images\\MiniMap\\WhiteTriangle100x100"));

                    _miniShipTexturesByShipType.Add(ShipType.FighterCarrier, content.Load<Texture2D>("Images\\MiniMap\\WhiteCircle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.BattleCruiser, content.Load<Texture2D>("Images\\MiniMap\\WhiteCircle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.TorpedoShip, content.Load<Texture2D>("Images\\MiniMap\\WhiteCircle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.EnemyDrone, content.Load<Texture2D>("Images\\MiniMap\\WhiteTriangle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.EnemyBattleCruiser, content.Load<Texture2D>("Images\\MiniMap\\WhiteTriangle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.EnemyFighterCarrier, content.Load<Texture2D>("Images\\MiniMap\\WhiteTriangle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.EnemyTorpedoShip, content.Load<Texture2D>("Images\\MiniMap\\WhiteTriangle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.EnemyBoss, content.Load<Texture2D>("Images\\MiniMap\\WhiteTriangle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.EnemyBossesClones, content.Load<Texture2D>("Images\\MiniMap\\WhiteTriangle100x100"));
                }
            }

            public readonly GameControls Controls;
            public class GameControls
            {
                public readonly Texture2D Title;
                public readonly Texture2D Button;

                public readonly Texture2D AButton;
                public readonly Texture2D BButton;
                public readonly Texture2D YButton;
                public readonly Texture2D XButton;

                public readonly Texture2D LeftTrigger;
                public readonly Texture2D RightTrigger;
                public readonly Texture2D LeftBumper;
                public readonly Texture2D RightBumper;

                public readonly Texture2D StartButton;
                public readonly Texture2D BackButton;

                public readonly Texture2D DPad;


                internal GameControls(ContentManager content)
                {
                    Title = content.Load<Texture2D>("Images\\Controls\\Gametitle");
                    Button = content.Load<Texture2D>("Images\\Controls\\Button");
                    AButton = content.Load<Texture2D>("Images\\Controls\\XBOXButtons\\AButton");
                    BButton = content.Load<Texture2D>("Images\\Controls\\XBOXButtons\\BButton");
                    YButton = content.Load<Texture2D>("Images\\Controls\\XBOXButtons\\YButton");
                    XButton = content.Load<Texture2D>("Images\\Controls\\XBOXButtons\\XButton");
                    LeftTrigger = content.Load<Texture2D>("Images\\Controls\\XBOXButtons\\LeftTrigger");
                    RightTrigger = content.Load<Texture2D>("Images\\Controls\\XBOXButtons\\RightTrigger");
                    LeftBumper = content.Load<Texture2D>("Images\\Controls\\XBOXButtons\\LeftBumper");
                    RightBumper = content.Load<Texture2D>("Images\\Controls\\XBOXButtons\\RightBumper");
                    StartButton = content.Load<Texture2D>("Images\\Controls\\XBOXButtons\\StartButton");
                    BackButton = content.Load<Texture2D>("Images\\Controls\\XBOXButtons\\BackButton");
                    DPad = content.Load<Texture2D>("Images\\Controls\\XBOXButtons\\DPad");
                }
            }



            public readonly NonPlayingGameObjects NonPlayingObjects;
            public class NonPlayingGameObjects
            {
                public readonly Texture2D Planet;
                public readonly Texture2D AltPlanet;
                public readonly Texture2D Planet4;
                public readonly Texture2D Planet3;
                public readonly Texture2D ShopBackground;

                /// <summary>
                /// Gets the instance of the singleton global scrolling background class.
                /// </summary>
                public HorizontalMenuBGSprite GlobalBackground
                {
                    get
                    {
                        return HorizontalMenuBGSprite.CurrentBG;
                    }
                }

                internal NonPlayingGameObjects(ContentManager content)
                {
                    Planet = content.Load<Texture2D>("Images\\NonPlayingObjects\\Planet");
                    AltPlanet = content.Load<Texture2D>("Images\\NonPlayingObjects\\AlternativePlanet");
                    Planet3 = content.Load<Texture2D>("Images\\NonPlayingObjects\\Planet3");
                    Planet4 = content.Load<Texture2D>("Images\\NonPlayingObjects\\Planet4");
                    ShopBackground = content.Load<Texture2D>("Images\\NonPlayingObjects\\SpaceShopBackground");
                }
            }


            public readonly GameSecondaryWeapon SecondaryWeapon;
            public class GameSecondaryWeapon
            {
                private readonly Dictionary<KeyValuePair<SecondaryWeaponType, TextureDisplayType>, Texture2D> _secondaryWeaponTextures;

                public Texture2D this[SecondaryWeaponType secondaryWeapon, TextureDisplayType textureType]
                {
                    get { return _secondaryWeaponTextures[new KeyValuePair<SecondaryWeaponType, TextureDisplayType>(secondaryWeapon, textureType)]; }
                }

                internal GameSecondaryWeapon(ContentManager content)
                {
                    _secondaryWeaponTextures = new Dictionary<KeyValuePair<SecondaryWeaponType, TextureDisplayType>, Texture2D>();

                    _secondaryWeaponTextures.Add(new KeyValuePair<SecondaryWeaponType, TextureDisplayType>(SecondaryWeaponType.EMP, TextureDisplayType.ShopDisplay), content.Load<Texture2D>("Images\\SecondaryWeapons\\EMP\\EMP"));
                    _secondaryWeaponTextures.Add(new KeyValuePair<SecondaryWeaponType, TextureDisplayType>(SecondaryWeaponType.EMP, TextureDisplayType.InGameUse), content.Load<Texture2D>("Images\\SecondaryWeapons\\EMP\\EMPInGame"));

                    _secondaryWeaponTextures.Add(new KeyValuePair<SecondaryWeaponType, TextureDisplayType>(SecondaryWeaponType.ShrinkRay, TextureDisplayType.ShopDisplay), content.Load<Texture2D>("Images\\SecondaryWeapons\\ShrinkRay\\ShrinkRay"));
                    _secondaryWeaponTextures.Add(new KeyValuePair<SecondaryWeaponType, TextureDisplayType>(SecondaryWeaponType.ShrinkRay, TextureDisplayType.InGameUse), content.Load<Texture2D>("Images\\SecondaryWeapons\\ShrinkRay\\ShrinkRay"));

                    _secondaryWeaponTextures.Add(new KeyValuePair<SecondaryWeaponType, TextureDisplayType>(SecondaryWeaponType.SpaceMine, TextureDisplayType.ShopDisplay), content.Load<Texture2D>("Images\\SecondaryWeapons\\SpaceMine\\SpaceMine"));
                    _secondaryWeaponTextures.Add(new KeyValuePair<SecondaryWeaponType, TextureDisplayType>(SecondaryWeaponType.SpaceMine, TextureDisplayType.InGameUse), content.Load<Texture2D>("Images\\SecondaryWeapons\\SpaceMine\\SpaceMine"));
                }
            }


            public readonly GameEquipment Equipment;
            public class GameEquipment
            {
                private readonly Dictionary<KeyValuePair<EquipmentType, TextureDisplayType>, Texture2D> _EquipmentTextures;

                public Texture2D this[EquipmentType Equipment, TextureDisplayType textureType]
                {
                    get { return _EquipmentTextures[new KeyValuePair<EquipmentType, TextureDisplayType>(Equipment, textureType)]; }
                }

                internal GameEquipment(ContentManager content)
                {
                    _EquipmentTextures = new Dictionary<KeyValuePair<EquipmentType, TextureDisplayType>, Texture2D>();

                    _EquipmentTextures.Add(new KeyValuePair<EquipmentType, TextureDisplayType>(EquipmentType.Scanner, TextureDisplayType.ShopDisplay), content.Load<Texture2D>("Images\\Equipment\\Scanner\\Scanner"));
                    _EquipmentTextures.Add(new KeyValuePair<EquipmentType, TextureDisplayType>(EquipmentType.Scanner, TextureDisplayType.InGameUse), content.Load<Texture2D>("Images\\Equipment\\Scanner\\Scanner"));
                    _EquipmentTextures.Add(new KeyValuePair<EquipmentType, TextureDisplayType>(EquipmentType.HealthPack, TextureDisplayType.InGameUse), content.Load<Texture2D>("Images\\Equipment\\HealthPack\\HealthPack"));
                    _EquipmentTextures.Add(new KeyValuePair<EquipmentType, TextureDisplayType>(EquipmentType.HealthPack, TextureDisplayType.ShopDisplay), content.Load<Texture2D>("Images\\Equipment\\HealthPack\\HealthPack"));
                }
            }

            public readonly SpriteSheetLoader SpriteSheets;
            public class SpriteSheetLoader
            {
                private readonly Dictionary<SpriteSheetType, Texture2D> _spriteSheetTextures;

                public Texture2D this[SpriteSheetType sheet]
                {
                    get { return _spriteSheetTextures[sheet]; }
                }

                internal SpriteSheetLoader(ContentManager content)
                {
                    _spriteSheetTextures = new Dictionary<SpriteSheetType, Texture2D>();
                    _spriteSheetTextures.Add(SpriteSheetType.Explosion, content.Load<Texture2D>("Images\\SpriteSheets\\explosion2"));

                }
            }

        }

        public void Dispose()
        {
            //Everything else is auto disposed by content pipeline
            foreach (SoundEffectInstance sei in Sound)
            {
                if (sei != null && !sei.IsDisposed)
                {
                    if (sei.State != SoundState.Stopped)
                    {
                        sei.Stop(true);
                    }
                    sei.Dispose();
                }
            }
        }
    }
}

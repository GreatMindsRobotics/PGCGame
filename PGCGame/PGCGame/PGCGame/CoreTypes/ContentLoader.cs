using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Glib.XNA;

namespace PGCGame.CoreTypes
{

    /// <summary>
    /// Singleton class; create in LoadContent, and use GameContent.GameAssets
    /// </summary>
    public class GameContent
    {
        private static GameContent _gameAssets = null;
        public static GameContent GameAssets
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

        public GameContent(ContentManager content)
        {
            if (_gameAssets != null)
            {
                throw new Exception("This class is a singleton; use GameContent.GameAssets.");
            }

            Fonts = new GameFonts(content);
            Images = new GameImages(content);
            Music = new GameMusic(content);

            _gameAssets = this;
        }

        public readonly GameMusic Music;
        public class GameMusic
        {
            private readonly Dictionary<ScreenMusic, Song> _gameMusic;

            public Song this[ScreenMusic index]
            {
                get { return _gameMusic[index]; }
            }

            internal GameMusic(ContentManager content)
            {
                _gameMusic = new Dictionary<ScreenMusic, Song>();
                _gameMusic.Add(ScreenMusic.Level1, content.Load<Song>("Songs\\Movement Proposition"));
                _gameMusic.Add(ScreenMusic.Credits, content.Load<Song>("Songs\\Failing Defense"));
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
                SpriteSheets = new SpriteSheet(content);
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

                    internal ShipBullets(ContentManager content)
                    {
                        _bulletTextures = new Dictionary<KeyValuePair<ShipType, ShipTier>, Texture2D>();

                        //TEMP
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.BattleCruiser, ShipTier.Tier1), content.Load<Texture2D>("Images\\TempBullets\\Laser"));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.FighterCarrier, ShipTier.Tier1), content.Load<Texture2D>("Images\\TempBullets\\Laser"));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.TorpedoShip, ShipTier.Tier1), new PlainTexture2D(StateManager.GraphicsManager.GraphicsDevice, 5, 3, Color.Red));
                        _bulletTextures.Add(new KeyValuePair<ShipType, ShipTier>(ShipType.Drone, ShipTier.Tier1), content.Load<Texture2D>("Images\\TempBullets\\Laser"));

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

                    _miniShipTextures.Add(PlayerType.MyShip, new PlainTexture2D(StateManager.GraphicsManager.GraphicsDevice, 3, 3, Color.Lime));
                    _miniShipTextures.Add(PlayerType.Enemy, new PlainTexture2D(StateManager.GraphicsManager.GraphicsDevice, 3, 3, Color.Red));
                    _miniShipTextures.Add(PlayerType.Ally, new PlainTexture2D(StateManager.GraphicsManager.GraphicsDevice, 3, 3, Color.CornflowerBlue));
                    _miniShipTextures.Add(PlayerType.Solo, new PlainTexture2D(StateManager.GraphicsManager.GraphicsDevice, 3, 3, Color.Crimson));

                    _miniShipTexturesByShipType.Add(ShipType.FighterCarrier, content.Load<Texture2D>("Images\\MiniMap\\WhiteCircle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.BattleCruiser, content.Load<Texture2D>("Images\\MiniMap\\WhiteCircle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.TorpedoShip, content.Load<Texture2D>("Images\\MiniMap\\WhiteCircle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.EnemyDrone, content.Load<Texture2D>("Images\\MiniMap\\WhiteTriangle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.EnemyBattleCruiser, content.Load<Texture2D>("Images\\MiniMap\\WhiteTriangle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.EnemyFighterCarrier, content.Load<Texture2D>("Images\\MiniMap\\WhiteTriangle100x100"));
                    _miniShipTexturesByShipType.Add(ShipType.EnemyTorpedoShip, content.Load<Texture2D>("Images\\MiniMap\\WhiteTriangle100x100"));
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

            public readonly SpriteSheet SpriteSheets;
            public class SpriteSheet
            {
                private readonly Dictionary<SpriteSheetType, Texture2D> _spriteSheetTextures;

                public Texture2D this[SpriteSheetType sheet]
                {
                    get { return _spriteSheetTextures[sheet]; }
                }

                internal SpriteSheet(ContentManager content)
                {
                    _spriteSheetTextures = new Dictionary<SpriteSheetType, Texture2D>();
                    _spriteSheetTextures.Add(SpriteSheetType.Explosion, content.Load<Texture2D>("Images\\SpriteSheets\\Explosion"));

                }
            }
        
        }
    }
}

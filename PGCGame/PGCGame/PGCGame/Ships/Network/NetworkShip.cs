using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PGCGame.Ships
{
    public abstract class NetworkShip : Ship
    {
        public NetworkGamer ControllingGamer;

        public NetworkShip(ShipType type, ShipTier tier, SpriteBatch worldsb)
            : base(GameContent.GameAssets.Images.Ships[type, tier], StateManager.RandomGenerator.NextVector2(new Vector2(500), new Vector2(StateManager.SpawnArea.X + StateManager.SpawnArea.Width, StateManager.SpawnArea.Y + StateManager.SpawnArea.Height)), worldsb)
        {
            PlayerType = CoreTypes.PlayerType.Ally;
            Tier = tier;
            ship = type;
            switch (type)
            {
                case CoreTypes.ShipType.FighterCarrier:
                    BulletTexture = GameContent.GameAssets.Images.Ships.Bullets[ShipType.FighterCarrier, ShipTier.Tier1];
                    DelayBetweenShots = TimeSpan.FromMilliseconds(100);
                    DamagePerShot = 2;
                    _initHealth = 100;
                    MovementSpeed = Vector2.One * 2f;
                    DamagePerShot = 2;
                    ShootSound = GameContent.GameAssets.Sound[SoundEffectType.FighterCarrierFire];
                    if (Tier == ShipTier.Tier1)
                    {
                        Scale = new Vector2(.55f);
                        DistanceToNose = .4f;
                        InitialHealth = 100;
                        DamagePerShot = 2;
                    }
                    else if (Tier == ShipTier.Tier2)
                    {
                        Scale = new Vector2(.55f);
                        DistanceToNose = .155f;
                        InitialHealth = 120;
                        DamagePerShot = 3;
                    }
                    else if (Tier == ShipTier.Tier3)
                    {
                        Scale = new Vector2(.55f);
                        DistanceToNose = .28f;
                        InitialHealth = 140;
                        DamagePerShot = 4;
                    }
                    else if (Tier == ShipTier.Tier4)
                    {
                        Scale = new Vector2(.55f);
                        DistanceToNose = .5f;
                        InitialHealth = 160;
                        DamagePerShot = 5;
                    }
                    break;
                case CoreTypes.ShipType.TorpedoShip:
                    BulletTexture = GameContent.GameAssets.Images.Ships.Bullets[ShipType.TorpedoShip, ShipTier.Tier1];
                    MovementSpeed = new Vector2(1.333f);
                    //MovementSpeed = new Vector2(1f);
                    DelayBetweenShots = TimeSpan.FromSeconds(.75);
                    _initHealth = 110;
                    DamagePerShot = 5;
                    ShootSound = GameContent.GameAssets.Sound[SoundEffectType.TorpedoShipFire];
                    if (Tier == ShipTier.Tier1)
                    {
                        Scale = new Vector2(.85f);
                        DistanceToNose = .5f;
                        DamagePerShot = 5;
                    }
                    else if (Tier == ShipTier.Tier2)
                    {
                        Scale = new Vector2(.85f);
                        DistanceToNose = .50f;
                        DamagePerShot = 7;
                    }
                    else if (Tier == ShipTier.Tier3)
                    {
                        Scale = new Vector2(.85f);
                        DistanceToNose = .488f;
                        DamagePerShot = 10;
                    }
                    else if (Tier == ShipTier.Tier4)
                    {
                        Scale = new Vector2(.85f);
                        DistanceToNose = .5f;
                        DamagePerShot = 15;
                    }
                    break;
                case CoreTypes.ShipType.BattleCruiser:
                    BulletTexture = GameContent.GameAssets.Images.Ships.Bullets[ShipType.BattleCruiser, ShipTier.Tier1];
                    DelayBetweenShots = TimeSpan.FromSeconds(1);
                    DamagePerShot = 20;
                    MovementSpeed = new Vector2(.7f);
                    InitialHealth = 120;
                    DamagePerShot = 20;
                    ShootSound = GameContent.GameAssets.Sound[SoundEffectType.BattleCruiserFire];
                    if (Tier == ShipTier.Tier1)
                    {

                        Scale = new Vector2(.85f);
                        Effect = SpriteEffects.FlipVertically;
                        DistanceToNose = .5f;
                        DamagePerShot = 20;
                    }
                    else if (Tier == ShipTier.Tier2)
                    {

                        Scale = new Vector2(.85f);
                        DistanceToNose = .30f;
                        DamagePerShot = 30;
                    }
                    else if (Tier == ShipTier.Tier3)
                    {

                        Scale = new Vector2(.85f);
                        DistanceToNose = .488f;
                        DamagePerShot = 40;
                    }
                    else if (Tier == ShipTier.Tier4)
                    {

                        Scale = new Vector2(.85f);
                        DistanceToNose = .5f;
                        DamagePerShot = 50;
                    }
                    break;
            }

            StateManager.NetworkData.DataWriter.Write(new Vector4(X, Y, Rotation.Radians, CurrentHealth));
        }

        protected ShipType ship;

        public override ShipType ShipType
        {
            get { return ship; }
        }
    }
}

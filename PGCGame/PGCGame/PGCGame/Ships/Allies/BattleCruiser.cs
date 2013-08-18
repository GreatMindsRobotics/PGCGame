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

using Glib;
using PGCGame.CoreTypes;
using Glib.XNA.SpriteLib;

using PGCGame.Ships.Allies;

namespace PGCGame
{
    public class BattleCruiser : BaseAllyShip
    {
        public static string ShipFriendlyName
        {
            get { return "Battle Cruiser"; }
        }
        public BattleCruiser(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            MovementSpeed = Vector2.One;
            BulletTexture = GameContent.GameAssets.Images.Ships.Bullets[ShipType.BattleCruiser, ShipTier.Tier1];
            DelayBetweenShots = TimeSpan.FromSeconds(1);
            DamagePerShot = 20;
            MovementSpeed = new Vector2(.7f);
            InitialHealth = 120;
            PlayerType = CoreTypes.PlayerType.Ally;
            this.TierChanged += new EventHandler(BattleCruiser_TierChanged);
            DamagePerShot = 20;
            ShootSound = GameContent.GameAssets.Sound[SoundEffectType.BattleCruiserFire];
        }

        void BattleCruiser_TierChanged(object sender, EventArgs e)
        {
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
        }
       

        public override ShipType ShipType
        {
            get { return ShipType.BattleCruiser; }
        }
    }
}

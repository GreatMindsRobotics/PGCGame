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

        public NetworkShip(ShipType type, ShipTier tier, SpriteBatch worldsb) : base(GameContent.GameAssets.Images.Ships[type, tier], StateManager.RandomGenerator.NextVector2(new Vector2(500), new Vector2(StateManager.SpawnArea.X+StateManager.SpawnArea.Width, StateManager.SpawnArea.Y+StateManager.SpawnArea.Height)), worldsb)
        {
            PlayerType = CoreTypes.PlayerType.Ally;
            Tier = tier;
            ship = type;
        }

        protected ShipType ship;

        public override ShipType ShipType
        {
            get { return ship; }
        }
    }
}

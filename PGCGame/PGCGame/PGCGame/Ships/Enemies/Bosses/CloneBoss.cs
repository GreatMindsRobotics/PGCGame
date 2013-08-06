using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PGCGame.CoreTypes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PGCGame
{
    class CloneBoss : Ships.Enemies.BaseEnemyShip
    {
        public CloneBoss(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Scale = new Vector2(.75f);
            BulletTexture = GameContent.GameAssets.Images.Ships.Bullets[ShipType.Drone, ShipTier.Tier1];

            DamagePerShot = 50;
            MovementSpeed = new Vector2(.5f);
            _initHealth = 2000;

            PlayerType = CoreTypes.PlayerType.Enemy;

            killWorth = 100;
        }

        int Pos;
        Random PosGenerator = new Random();

        CloneBoss[] Clones = new CloneBoss[2];

        Boolean isClone = false;
        Boolean isFirstUpdate = true;

        public CloneBoss Parent { get; set; }

        public override ShipType ShipType
        {
            get
            {
                if (!isClone)
                {
                    return ShipType.EnemyBoss;
                }
                else
                {
                    return ShipType.EnemyBossesClones;
                }
            }

        }

        public override void Shoot()
        {
            Bullet bullet = new Bullet(BulletTexture, WorldCoords - new Vector2(Height * -DistanceToNose, Height * -DistanceToNose) * Rotation.Vector, WorldSb, this);
            bullet.Speed = Rotation.Vector * 3f;
            bullet.UseCenterAsOrigin = true;
            bullet.Rotation = Rotation;
            bullet.Damage = DamagePerShot;
            bullet.Color = Color.Red;

            StateManager.EnemyBullets.Legit.Add(bullet);
        }

        public override void Update(GameTime gt)
        {
            if (!isClone && isFirstUpdate)
            {
                Clones[0] = new CloneBoss(this.Texture, this.WorldCoords, this.SpriteBatch);
                Clones[0].isClone = true;
                Clones[0].Parent = this;
                Clones[0].Color = Color.White;

                Clones[1] = new CloneBoss(this.Texture, this.WorldCoords, this.SpriteBatch);
                Clones[1].isClone = true;
                Clones[1].Parent = this;
                Clones[1].Color = Color.White;

                StateManager.EnemyShips.Add(Clones[0]);
                StateManager.EnemyShips.Add(Clones[1]);

                StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(Clones[0]);
                StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(Clones[1]);

                while (Clones[0].Pos == Clones[1].Pos || Clones[0].Pos == this.Pos || Clones[1].Pos == this.Pos)
                {
                    Clones[0].Pos = PosGenerator.Next(3);
                    Clones[1].Pos = PosGenerator.Next(3);
                    this.Pos = PosGenerator.Next(3);
                }
            }
            if (Pos == 0)
            {
                if (!isClone)
                {
                    foreach (CloneBoss clone in Clones)
                    {
                        if (clone.Pos == 1)
                        {
                            if (this.WorldCoords.Y > clone.WorldCoords.Y - 100)
                            {
                                this.YSpeed = -1;
                            }
                            else
                            {
                                this.YSpeed = 0;
                            }
                        }
                    }
                }
                else
                {
                    foreach (CloneBoss clone in Parent.Clones)
                    {
                        if (clone.Pos == 1)
                        {
                            if (this.WorldCoords.Y > clone.WorldCoords.Y - 100)
                            {
                                this.YSpeed = -1;
                            }
                            else
                            {
                                this.YSpeed = 0;
                            }
                        }
                    }
                }
            }
            else if (Pos == 1)
            {

            }
            else
            {
                if (!isClone)
                {
                    foreach (CloneBoss clone in Clones)
                    {
                        if (clone.Pos == 1)
                        {
                            if (this.WorldCoords.Y < clone.WorldCoords.Y + 100)
                            {
                                this.YSpeed = +1;
                            }
                            else
                            {
                                this.YSpeed = 0;
                            }
                        }
                    }
                }
                else
                {
                    foreach (CloneBoss clone in Parent.Clones)
                    {
                        if (clone.Pos == 1)
                        {
                            if (this.WorldCoords.Y < clone.WorldCoords.Y + 100)
                            {
                                this.YSpeed = +1;
                            }
                            else
                            {
                                this.YSpeed = 0;
                            }
                        }
                    }
                }
            }
            regenClonesDelay += gt.ElapsedGameTime;
            if (regenClonesDelay >= regenClones)
            {
                for (int i = 0; i < StateManager.EnemyShips.Count; i++)
                {
                    if (StateManager.EnemyShips[i].ShipType == CoreTypes.ShipType.EnemyBossesClones)
                    {
                        StateManager.EnemyShips[i].CurrentHealth = 0;
                    }
                }
                Clones[0] = new CloneBoss(this.Texture, this.WorldCoords, this.SpriteBatch);
                Clones[0].isClone = true;
                Clones[0].Parent = this;
                Clones[0].Color = Color.White;

                Clones[1] = new CloneBoss(this.Texture, this.WorldCoords, this.SpriteBatch);
                Clones[1].isClone = true;
                Clones[1].Parent = this;
                Clones[1].Color = Color.White;

                StateManager.EnemyShips.Add(Clones[0]);
                StateManager.EnemyShips.Add(Clones[1]);

                StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(Clones[0]);
                StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(Clones[1]);

                while (Clones[0].Pos == Clones[1].Pos || Clones[0].Pos == this.Pos || Clones[1].Pos == this.Pos)
                {
                    Clones[0].Pos = PosGenerator.Next(3);
                    Clones[1].Pos = PosGenerator.Next(3);
                    this.Pos = PosGenerator.Next(3);
                }
            }
            if (isClone && isFirstUpdate)
            {
                killWorth /= 10;
                DamagePerShot /= 5;
                _initHealth /= 20;
                CurrentHealth /= 20;
            }
            if (isFirstUpdate)
            {
                isFirstUpdate = false;
            }
            if (!isClone)
            {
                Clones[0].Update();
                Clones[1].Update();
            }
        }

        public TimeSpan regenClones = new TimeSpan(0, 0, 30);
        public TimeSpan regenClonesDelay = new TimeSpan();


        public int killWorth { get; set; }
    }
}

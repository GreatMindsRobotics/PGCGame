using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.GamerServices;
using Glib.XNA.InputLib;

namespace PGCGame.Screens
{
    public class CheatEditScreen : BaseScreen
    {
        public CheatEditScreen(SpriteBatch sb) : base(sb, Color.Black) {
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
        }

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                UpdateTextSprites();
                bool showCheats = StateManager.GamerServicesAreAvailable;
                if (showCheats)
                {
                    showCheats = false;
#if WINDOWS
                    foreach (SignedInGamer gamer in Gamer.SignedInGamers)
                    {
                        if (gamer.IsSignedInToLive && StateManager.GameDevs.Contains(gamer.Gamertag.ToLower()))
                        {
                            showCheats = true;
                            break;
                        }
                    }
#endif
                }
                if (!showCheats)
                {
                    StateManager.ScreenState = CoreTypes.ScreenType.Options;
                }
            }
        }

        public override MusicBehaviour Music
        {
            get { return MusicBehaviour.NoMusic; }
        }

        private void UpdateTextSprites()
        {
            invincible.Text = string.Format("Invincible: {0}", StateManager.DebugData.Invincible);
            infimoney.Text = string.Format("InfiniteMoney: {0}", StateManager.DebugData.InfiniteMoney);
            shipSpeed.Text = string.Format("ShipSpeedIncrease: {0}", StateManager.DebugData.ShipSpeedIncrease);
            killall.Text = string.Format("KillAll: {0}", StateManager.DebugData.KillAll);
            emergencyheal.Text = string.Format("EmergencyHeal: {0}", StateManager.DebugData.EmergencyHeal);
            fowEnabled.Text = string.Format("FogOfWarEnabled: {0}", StateManager.DebugData.FogOfWarEnabled);
        }

        private void event_OnPressGeneral(object src, EventArgs ea)
        {
            UpdateTextSprites();
        }

        TextSprite invincible;
        TextSprite infimoney;
        TextSprite shipSpeed;
        TextSprite killall;
        TextSprite emergencyheal;
        TextSprite fowEnabled;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);

            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            invincible = StateManager.CreateButtonTextSprite(false, null, null, this);
            invincible.Pressed += new EventHandler(invincible_Pressed);
            invincible.Pressed += new EventHandler(event_OnPressGeneral);

            infimoney = StateManager.CreateButtonTextSprite(false, null, null, this);
            infimoney.Y = invincible.Height;
            infimoney.Pressed += new EventHandler(infimoney_Pressed);
            infimoney.Pressed += new EventHandler(event_OnPressGeneral);

            shipSpeed = StateManager.CreateButtonTextSprite(false, null, null, this);
            shipSpeed.Y = infimoney.Height + infimoney.Y;
            shipSpeed.Pressed += new EventHandler(shipSpeed_Pressed);
            shipSpeed.Pressed += new EventHandler(event_OnPressGeneral);

            killall = StateManager.CreateButtonTextSprite(false, null, null, this);
            killall.Y = shipSpeed.Height + shipSpeed.Y;
            killall.Pressed += new EventHandler(killall_Pressed);
            killall.Pressed += new EventHandler(event_OnPressGeneral);

            emergencyheal = StateManager.CreateButtonTextSprite(false, null, null, this);
            emergencyheal.Y = killall.Height + killall.Y;
            emergencyheal.Pressed += new EventHandler(emergencyheal_Pressed);
            emergencyheal.Pressed += new EventHandler(event_OnPressGeneral);

            fowEnabled = StateManager.CreateButtonTextSprite(false, null, null, this);
            fowEnabled.Y = emergencyheal.Height + emergencyheal.Y;
            fowEnabled.Pressed += new EventHandler(fowEnabled_Pressed);
            fowEnabled.Pressed += new EventHandler(event_OnPressGeneral);

            UpdateTextSprites();
        }

        void fowEnabled_Pressed(object sender, EventArgs e)
        {
            StateManager.DebugData.FogOfWarEnabled = !StateManager.DebugData.FogOfWarEnabled;
        }

        void emergencyheal_Pressed(object sender, EventArgs e)
        {
            StateManager.DebugData.EmergencyHeal = !StateManager.DebugData.EmergencyHeal;
        }

        void killall_Pressed(object sender, EventArgs e)
        {
            StateManager.DebugData.KillAll = !StateManager.DebugData.KillAll;
        }

        void shipSpeed_Pressed(object sender, EventArgs e)
        {
            StateManager.DebugData.ShipSpeedIncrease = !StateManager.DebugData.ShipSpeedIncrease;
        }

        void infimoney_Pressed(object sender, EventArgs e)
        {
            StateManager.DebugData.InfiniteMoney = !StateManager.DebugData.InfiniteMoney;
        }

        void invincible_Pressed(object sender, EventArgs e)
        {
            StateManager.DebugData.Invincible = !StateManager.DebugData.Invincible;
        }




        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (KeyboardManager.State.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) || GamePadManager.One.Current.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.B))
            {
                StateManager.GoBack();
            }
        }
    }
}

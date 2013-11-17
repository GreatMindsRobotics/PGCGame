using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame.CoreTypes
{
    public struct SerializableGamePreferences
    {

        public static SerializableGamePreferences Current
        {
            get
            {
                SerializableGamePreferences retVal = new SerializableGamePreferences();
                retVal.ArrowKeys = StateManager.Options.ArrowKeysEnabled;
                retVal.DroneDeploy = StateManager.Options.DeployDronesEnabled;
                retVal.LeftButtonEnabled = StateManager.Options.LeftButtonEnabled;
                retVal.MusicOn = StateManager.Options.MusicEnabled;
                retVal.SecondaryButtonEnabled = StateManager.Options.SecondaryButtonEnabled;
                retVal.SFXOn = StateManager.Options.SFXEnabled;
                retVal.SwitchButtonEnabled = StateManager.Options.SFXEnabled;
                return retVal;
            }
        }

        public bool ArrowKeys;
        public bool MusicOn;
        public bool SFXOn;
        public bool DroneDeploy;
        public bool LeftButtonEnabled;
        public bool SecondaryButtonEnabled;
        public bool SwitchButtonEnabled;
    }
}

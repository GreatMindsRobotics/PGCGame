using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame.CoreTypes
{
    public struct MusicBehaviour
    {
        private static MusicBehaviour _noMusic = new MusicBehaviour();

        public static MusicBehaviour NoMusic
        {
            get { return _noMusic; }
        }
        

        /// <summary>
        /// Should we stop or pause music on screen switch?
        /// </summary>
        public bool PauseMusic;

        public ScreenMusic? DesiredMusic;

        public override bool Equals(object obj)
        {
            MusicBehaviour other = new MusicBehaviour();
            try
            {
                other = (MusicBehaviour)obj;
            }
            catch { return false; }

            return other.DesiredMusic == DesiredMusic && PauseMusic == other.PauseMusic;
        }

        public override int GetHashCode()
        {
            int hashCode = 18;
            hashCode = hashCode * 23 + (DesiredMusic.HasValue ? Convert.ToInt32(DesiredMusic.Value) * 10 : -1);
            hashCode = hashCode * 23 + (PauseMusic ? 1 : 0);

            return hashCode;
        }

        public static bool operator !=(MusicBehaviour a, MusicBehaviour b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(MusicBehaviour a, MusicBehaviour b)
        {
            return a.Equals(b);
        }

        public MusicBehaviour(ScreenMusic desired) : this(desired, false) { }

        public MusicBehaviour(ScreenMusic desired, bool pauseOnScreenChange)
        {
            PauseMusic = pauseOnScreenChange;
            DesiredMusic = desired;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChameleonMod
{
    public static class CustomGameOptions
    {
        public enum customGameOptionsRpc : byte
        {
            syncCustomSettings = 60,
            setChameleon = 61,
            setChameleonInvisible = 62,
        }

        public static float invisibilityDuration = 15f;
        public static float invisibilityCooldown = 25f;
    }
}

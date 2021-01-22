using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChameleonMod.API;
using UnityEngine;

namespace ChameleonMod
{
    public static class ChameleonPlayer
    {
        public static PlayerController player;
        public static DateTime lastUsed;
        public static Boolean isInvisible;
        public static Color color = new Color(0.25f, 1f, 0.32f, 1f);

        public static Boolean isChameleon(PlayerController otherPlayer)
        {
            return otherPlayer.Equals(player);
        }

        public static Boolean isLocalPlayerChameleon()
        {
            if (player == null)
                return false;
            return PlayerController.GetLocalPlayer().Equals(player);
        }
    }
}

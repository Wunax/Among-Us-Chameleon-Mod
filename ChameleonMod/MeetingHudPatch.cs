using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

using MeetingHud = OOCJALPKPEP;

namespace ChameleonMod
{
    public static class MeetingHudPatch
    {
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        public static class MeetingHudStartPatch
        {
            public static void Postfix()
            {
                if (ChameleonPlayer.isInvisible)
                {
                    HudManagerPatch.setPlayerVisibility(ChameleonPlayer.player, 1f);
                    ChameleonPlayer.player.PlayerControl.nameText.Color = new Color(ChameleonPlayer.player.PlayerControl.nameText.Color.r, ChameleonPlayer.player.PlayerControl.nameText.Color.g, ChameleonPlayer.player.PlayerControl.nameText.Color.b, 1f);
                }
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
        public static class MeetingHudClosePatch
        {
            public static void Postfix()
            {
                if (ChameleonPlayer.isLocalPlayerChameleon())
                {
                    HudManagerPatch.invisibleButton.lastUsed = DateTime.UtcNow.AddSeconds(-HudManagerPatch.invisibleButton.duration - HudManagerPatch.invisibleButton.cooldown + 15);
                }
            }
        }
    }
}

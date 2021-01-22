using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

using IntroClass = PENEIDJGGAF.CKACLKCOJFO;

namespace ChameleonMod
{
    [HarmonyPatch]
    public static class IntroCutScenePatch
    {
        [HarmonyPatch(typeof(IntroClass), nameof(IntroClass.MoveNext))]
        public static void Postfix(IntroClass __instance)
        {
            if (ChameleonPlayer.isLocalPlayerChameleon())
            {
                var color = new Color(1, 1, 1, 1);
                __instance.field_Public_PENEIDJGGAF_0.Title.Text = "Chameleon";
                __instance.field_Public_PENEIDJGGAF_0.Title.Color = ChameleonPlayer.color;
                __instance.field_Public_PENEIDJGGAF_0.ImpostorText.Text = "Use your [40ff52ff]invisibility[ffffffff] to find [ff0000ff]impostors";
                __instance.field_Public_PENEIDJGGAF_0.BackgroundBar.material.color = ChameleonPlayer.color;
            }
        }
    }
}

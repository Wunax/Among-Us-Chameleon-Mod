using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

using NumberOption = PCGDGFIAJJI;
using PlayerControl = FFGALNAPKCD;

namespace ChameleonMod
{
    public static class NumberOptionPatch
    {
        [HarmonyPatch(typeof(NumberOption), nameof(NumberOption.Increase))]
        public static class NumberOptionIncreasePatch
        {
            public static bool Prefix(NumberOption __instance)
            {
                if (__instance.TitleText.Text == "Chameleon Duration")
                {
                    CustomGameOptions.invisibilityDuration = Math.Min(CustomGameOptions.invisibilityDuration + 2.5f, 60);
                    PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                    __instance.NHLMDAOEOAE = CustomGameOptions.invisibilityDuration;
                    __instance.Value = CustomGameOptions.invisibilityDuration;
                    __instance.ValueText.Text = $"{CustomGameOptions.invisibilityDuration.ToString()}s";
                    return false;
                } else if (__instance.TitleText.Text == "Chameleon Cooldown")
                {
                    CustomGameOptions.invisibilityCooldown = Math.Min(CustomGameOptions.invisibilityCooldown + 2.5f, 60);
                    PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                    __instance.NHLMDAOEOAE = CustomGameOptions.invisibilityCooldown;
                    __instance.Value = CustomGameOptions.invisibilityCooldown;
                    __instance.ValueText.Text = $"{CustomGameOptions.invisibilityCooldown.ToString()}s";
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(NumberOption), nameof(NumberOption.Decrease))]
        public static class NumberOptionDecreasePatch
        {
            public static bool Prefix(NumberOption __instance)
            {
                if (__instance.TitleText.Text == "Chameleon Duration")
                {
                    CustomGameOptions.invisibilityDuration = Math.Max(CustomGameOptions.invisibilityDuration - 2.5f, 10);
                    PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                    __instance.NHLMDAOEOAE = CustomGameOptions.invisibilityDuration;
                    __instance.Value = CustomGameOptions.invisibilityDuration;
                    __instance.ValueText.Text = $"{CustomGameOptions.invisibilityDuration.ToString()}s";
                    return false;
                }
                else if (__instance.TitleText.Text == "Chameleon Cooldown")
                {
                    CustomGameOptions.invisibilityCooldown = Math.Max(CustomGameOptions.invisibilityCooldown - 2.5f, 10);
                    PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                    __instance.NHLMDAOEOAE = CustomGameOptions.invisibilityCooldown;
                    __instance.Value = CustomGameOptions.invisibilityCooldown;
                    __instance.ValueText.Text = $"{CustomGameOptions.invisibilityCooldown.ToString()}s";
                    return false;
                }
                return true;
            }
        }
    }
}

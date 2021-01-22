using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;

using GameOptionsMenu = PHCKLDDNJNP;
using OptionBehaviour = LLKOLCLGCBD;
using NumberOption = PCGDGFIAJJI;

namespace ChameleonMod
{
    public static class GameOptionsMenuPatch
    {
        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
        public static class GameOptionsMenuStartPatch
        {
            public static void Postfix(GameOptionsMenu __instance)
            {
                if (UnityEngine.Object.FindObjectsOfType<BCLDBBKFJPK>().Count == 4)
                {
                    NumberOption killCooldown = GameObject.FindObjectsOfType<PCGDGFIAJJI>().ToList().Where(x => x.TitleText.Text == "Kill Cooldown").First();

                    GameOptionsMenuUpdatePatch.chameleonDuration = GameObject.Instantiate(killCooldown);
                    GameOptionsMenuUpdatePatch.chameleonDuration.gameObject.name = "ChameleonDurationText";
                    GameOptionsMenuUpdatePatch.chameleonDuration.TitleText.Text = "Chameleon Duration";
                    GameOptionsMenuUpdatePatch.chameleonDuration.Value = CustomGameOptions.invisibilityDuration;
                    GameOptionsMenuUpdatePatch.chameleonDuration.ValueText.Text = CustomGameOptions.invisibilityDuration.ToString();

                    GameOptionsMenuUpdatePatch.chameleonCooldown = GameObject.Instantiate(killCooldown);
                    GameOptionsMenuUpdatePatch.chameleonCooldown.gameObject.name = "ChameleonCooldownText";
                    GameOptionsMenuUpdatePatch.chameleonCooldown.TitleText.Text = "Chameleon Cooldown";
                    GameOptionsMenuUpdatePatch.chameleonCooldown.Value = CustomGameOptions.invisibilityCooldown;
                    GameOptionsMenuUpdatePatch.chameleonCooldown.ValueText.Text = CustomGameOptions.invisibilityCooldown.ToString();

                    OptionBehaviour[] options = new OptionBehaviour[__instance.KJFHAPEDEBH.Count + 2];
                    __instance.KJFHAPEDEBH.ToArray().CopyTo(options, 0);
                    options[options.Length - 2] = GameOptionsMenuUpdatePatch.chameleonDuration;
                    options[options.Length - 1] = GameOptionsMenuUpdatePatch.chameleonCooldown;
                    __instance.KJFHAPEDEBH = new Il2CppReferenceArray<OptionBehaviour>(options);
                }
            }
        }

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
        public static class GameOptionsMenuUpdatePatch
        {
            public static NumberOption chameleonDuration;
            public static NumberOption chameleonCooldown;

            public static void Postfix()
            {
                BCLDBBKFJPK option = GameObject.FindObjectsOfType<BCLDBBKFJPK>().ToList().Where(x => x.TitleText.Text == "Anonymous Votes").First();
                if (chameleonDuration != null && chameleonCooldown != null)
                {
                    chameleonDuration.transform.position = option.transform.position - new Vector3(0f, 5.5f, 0f);
                    chameleonCooldown.transform.position = option.transform.position - new Vector3(0f, 6f, 0f);
                }
            }
        }
    }
}
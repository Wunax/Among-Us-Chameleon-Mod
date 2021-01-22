using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

using GameDataClass = KHNHJFFECBP;

namespace ChameleonMod
{
    public static class GameData
    {
        public static GameDataClass currentGame = null;
    }

    [HarmonyPatch(typeof(GameDataClass), nameof(GameDataClass.Connect))]
    public static class GameDataConnectPatch
    {
        public static void Postfix(GameDataClass __instance)
        {
            GameData.currentGame = __instance;
        }
    }
}
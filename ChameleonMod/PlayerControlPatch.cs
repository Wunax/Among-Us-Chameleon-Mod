using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hazel;
using HarmonyLib;
using UnhollowerBaseLib;
using ChameleonMod.API;

using PlayerControl = FFGALNAPKCD;
using GameDataPlayerInfo = EGLJNOMOGNP.DCJMABDDJCF;
using NetClient = FMLLKEACGIO;

namespace ChameleonMod
{
    public static class PlayerControlPatch
    {
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OAFBLFBAOJJ))]
        public static class PlayerControlFindClosestPlayerPatch
        {
            public static void Prefix()
            {
                if (ChameleonPlayer.isInvisible)
                {
                    ChameleonPlayer.player.PlayerData.IsImpostor = true;
                }
            }

            public static void Postfix()
            {
                ChameleonPlayer.player.PlayerData.IsImpostor = false;
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        public static class PlayerControlHandleRpcPatch
        {
            public static void Postfix(byte HKHMBLJFLMC, MessageReader ALMCIJKELCP)
            {
                switch (HKHMBLJFLMC)
                {
                    case (byte) CustomGameOptions.customGameOptionsRpc.syncCustomSettings:
                        {
                            CustomGameOptions.invisibilityDuration = BitConverter.ToSingle(ALMCIJKELCP.ReadBytes(4).ToArray(), 0);
                            CustomGameOptions.invisibilityCooldown = BitConverter.ToSingle(ALMCIJKELCP.ReadBytes(4).ToArray(), 0);
                            break;
                        }
                    case (byte) CustomGameOptions.customGameOptionsRpc.setChameleon:
                        {
                            ChameleonPlayer.player = PlayerController.getPlayerById(ALMCIJKELCP.ReadByte());
                            break;
                        }
                    case (byte) CustomGameOptions.customGameOptionsRpc.setChameleonInvisible:
                        {
                            ChameleonPlayer.isInvisible = ALMCIJKELCP.ReadBoolean();
                            break;
                        }

                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
        public static class PlayerControlRpcSyncSettingsPatch
        {
            public static void Postfix()
            {
                if (PlayerControl.AllPlayerControls.Count > 1)
                {
                    MessageWriter messageWriter = NetClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomGameOptions.customGameOptionsRpc.syncCustomSettings, SendOption.None, -1);
                    messageWriter.Write(CustomGameOptions.invisibilityDuration);
                    messageWriter.Write(CustomGameOptions.invisibilityCooldown);
                    NetClient.Instance.FinishRpcImmediately(messageWriter);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
        public static class PlayerControlSetInfectedPatch
        {
            public static void Postfix(Il2CppReferenceArray<GameDataPlayerInfo> JPGEIBIBJPJ)
            {
                MessageWriter writer = NetClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomGameOptions.customGameOptionsRpc.setChameleon, Hazel.SendOption.None, -1);
                List<PlayerController> allPlayers = PlayerController.GetAllPlayers();
                List<PlayerController> crewmates = new List<PlayerController>();

                foreach (var player in allPlayers)
                {
                    if (!Array.Exists<GameDataPlayerInfo>(JPGEIBIBJPJ, x => x.LAOEJKHLKAI.PlayerId == player.PlayerControl.PlayerId))
                        crewmates.Add(player);
                }
                Random rnd = new Random();
                ChameleonPlayer.player = crewmates[rnd.Next(0, crewmates.Count)];
                writer.Write(ChameleonPlayer.player.PlayerControl.PlayerId);
                NetClient.Instance.FinishRpcImmediately(writer);
            }
        }
    }
}

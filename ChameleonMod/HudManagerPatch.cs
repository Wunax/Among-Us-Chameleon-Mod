using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Hazel;
using UnityEngine;
using ChameleonMod.API;

using GameOptionsMenu = PHCKLDDNJNP;
using Hud = PIEFJFEOGOL;
using HudManager = PPAEIPHJPDH<PIEFJFEOGOL>;
using PlayerMeeting = HDJGDMFCHDN;
using MeetingHud = OOCJALPKPEP;
using PlayerControl = FFGALNAPKCD;
using GameStates = KHNHJFFECBP.KGEKNMMAKKN;
using NetClient = FMLLKEACGIO;

namespace ChameleonMod
{
    [HarmonyPatch]
    public static class HudManagerPatch
    {
        public static string GameSettingsText = null;
        public static CustomButton invisibleButton = null;
        public static int frame = 0;

        public static void updateMeetingHud(MeetingHud __instance, PlayerController localPlayer)
        {
            foreach (PlayerMeeting player in __instance.HBDFFAHBIGI)
            {
                if (player.NameText.Text == localPlayer.PlayerControl.nameText.Text)
                {
                    player.NameText.Color = ChameleonPlayer.color;
                    return;
                }
            }
        }

        public static void updateGameSettingsText(Hud __instance)
        {
            if (__instance.GameSettings.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Count() == 19)
                GameSettingsText = __instance.GameSettings.Text;
            if (GameSettingsText != null)
            {
                string append = String.Empty;
                append += $"Chameleon Duration: {CustomGameOptions.invisibilityDuration.ToString()}s\n";
                append += $"Chameleon Cooldown: {CustomGameOptions.invisibilityCooldown.ToString()}s\n";
                __instance.GameSettings.Text = GameSettingsText + append;
            }
        }

        public static void hideGameMenuSettings()
        {
            if (GameOptionsMenuPatch.GameOptionsMenuUpdatePatch.chameleonCooldown != null && GameOptionsMenuPatch.GameOptionsMenuUpdatePatch.chameleonDuration != null)
            {
                bool isActive = GameObject.FindObjectsOfType<GameOptionsMenu>().Count != 0;
                GameOptionsMenuPatch.GameOptionsMenuUpdatePatch.chameleonDuration.gameObject.SetActive(isActive);
                GameOptionsMenuPatch.GameOptionsMenuUpdatePatch.chameleonCooldown.gameObject.SetActive(isActive);
            }
        }
        public static void setPlayerVisibility(PlayerController player, float alpha)
        {
            player.PlayerControl.MKIDFJAEBGH.color = new Color(player.PlayerControl.MKIDFJAEBGH.color.r, player.PlayerControl.MKIDFJAEBGH.color.g, player.PlayerControl.MKIDFJAEBGH.color.b, alpha);
            if (player.PlayerControl.CurrentPet != null && player.PlayerControl.CurrentPet.rend != null && player.PlayerControl.CurrentPet.shadowRend != null)
            {
                player.PlayerControl.CurrentPet.rend.color = new Color(player.PlayerControl.CurrentPet.rend.color.r, player.PlayerControl.CurrentPet.rend.color.g, player.PlayerControl.CurrentPet.rend.color.b, alpha);
                player.PlayerControl.CurrentPet.shadowRend.color = new Color(player.PlayerControl.CurrentPet.shadowRend.color.r, player.PlayerControl.CurrentPet.shadowRend.color.g, player.PlayerControl.CurrentPet.shadowRend.color.b, alpha);
            }
            if (player.PlayerControl.HatRenderer != null)
            {
                player.PlayerControl.HatRenderer.Parent.color = new Color(player.PlayerControl.HatRenderer.Parent.color.r, player.PlayerControl.HatRenderer.Parent.color.g, player.PlayerControl.HatRenderer.Parent.color.b, alpha);
                player.PlayerControl.HatRenderer.BackLayer.color = new Color(player.PlayerControl.HatRenderer.BackLayer.color.r, player.PlayerControl.HatRenderer.BackLayer.color.g, player.PlayerControl.HatRenderer.BackLayer.color.b, alpha);
                player.PlayerControl.HatRenderer.FrontLayer.color = new Color(player.PlayerControl.HatRenderer.FrontLayer.color.r, player.PlayerControl.HatRenderer.FrontLayer.color.g, player.PlayerControl.HatRenderer.FrontLayer.color.b, alpha);
            }
            player.PlayerControl.MyPhysics.Skin.layer.color = new Color(player.PlayerControl.MyPhysics.Skin.layer.color.r, player.PlayerControl.MyPhysics.Skin.layer.color.g, player.PlayerControl.MyPhysics.Skin.layer.color.b, alpha);
        }

        public static void sendRpcInvisible()
        {
            MessageWriter messageWriter = NetClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomGameOptions.customGameOptionsRpc.setChameleonInvisible, SendOption.None, -1);
            messageWriter.Write(ChameleonPlayer.isInvisible);
            NetClient.Instance.FinishRpcImmediately(messageWriter);
        }

        public static void setPositionInvisibleButton()
        {
            if (Math.Abs((((float)Screen.width / (float)Screen.height) - (4f / 3f))) < 0.01f)
            {
                invisibleButton.gameObject.transform.localPosition = new Vector3(-3.25f, -2.25f);
            } else
            {
                invisibleButton.gameObject.transform.localPosition = new Vector3(-4.5f, -2.25f);
            }
        }

        [HarmonyPatch(typeof(Hud), nameof(Hud.Update))]
        public static void Postfix(Hud __instance)
        {
            if (PlayerControl.LocalPlayer == null)
                return;
            if (GameData.currentGame.GameState != GameStates.Started)
                updateGameSettingsText(__instance);
            if (GameData.currentGame.GameState == GameStates.Started)
            {
                var localPlayer = PlayerController.GetLocalPlayer();
                if (ChameleonPlayer.isLocalPlayerChameleon())
                {
                    if (MeetingHud.Instance != null)
                        updateMeetingHud(MeetingHud.Instance, localPlayer);
                    if (invisibleButton == null)
                    {
                        invisibleButton = new CustomButton(__instance, CustomGameOptions.invisibilityCooldown, CustomGameOptions.invisibilityDuration, "ChameleonMod.Assets.invisible.png");
                    }
                    localPlayer.PlayerControl.nameText.Color = ChameleonPlayer.color;
                    if (!localPlayer.PlayerData.IsDead)
                    {
                        setPositionInvisibleButton();
                        if (invisibleButton.isActive)
                        {
                            var activeTime = invisibleButton.ActiveTime();
                            if (activeTime == 0)
                            {
                                ChameleonPlayer.isInvisible = false;
                                sendRpcInvisible();
                                invisibleButton.isActive = false;
                                invisibleButton.spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                            }
                            else
                            {
                                invisibleButton.textDuration.textRenderer.Text = ((int)Math.Ceiling(activeTime)).ToString();
                                invisibleButton.textDuration.gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            var useTime = invisibleButton.UseTime();
                            if (useTime == 0)
                            {
                                if (invisibleButton.Clicked())
                                {
                                    ChameleonPlayer.isInvisible = true;
                                    sendRpcInvisible();
                                    invisibleButton.lastUsed = DateTime.UtcNow;
                                    invisibleButton.isActive = true;
                                    invisibleButton.spriteRenderer.color = new Color(0.75f, 0.75f, 0.75f, 0.75f);
                                    invisibleButton.textDuration.gameObject.SetActive(true);
                                }
                                else
                                {
                                    invisibleButton.textDuration.gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                invisibleButton.textDuration.textRenderer.Text = ((int)Math.Ceiling(useTime)).ToString();
                                invisibleButton.textDuration.gameObject.SetActive(true);
                            }
                        }
                        if (ChameleonPlayer.isInvisible)
                        {
                            setPlayerVisibility(localPlayer, 0.5f);
                        }
                        else
                        {
                            setPlayerVisibility(localPlayer, 1f);
                        }
                    } else
                    {
                        invisibleButton.gameObject.SetActive(false);
                        invisibleButton.textDuration.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (ChameleonPlayer.player != null && ChameleonPlayer.player.PlayerControl != null && !ChameleonPlayer.player.PlayerData.IsDead)
                    {
                        if (ChameleonPlayer.isInvisible)
                        {
                            setPlayerVisibility(ChameleonPlayer.player, 0f);
                            ChameleonPlayer.player.PlayerControl.nameText.Color = new Color(ChameleonPlayer.player.PlayerControl.nameText.Color.r, ChameleonPlayer.player.PlayerControl.nameText.Color.g, ChameleonPlayer.player.PlayerControl.nameText.Color.b, 0f);
                        }
                        else
                        {
                            setPlayerVisibility(ChameleonPlayer.player, 1f);
                            ChameleonPlayer.player.PlayerControl.nameText.Color = new Color(ChameleonPlayer.player.PlayerControl.nameText.Color.r, ChameleonPlayer.player.PlayerControl.nameText.Color.g, ChameleonPlayer.player.PlayerControl.nameText.Color.b, 1f);
                        }
                    }
                }
            }
            if (GameData.currentGame.GameState != GameStates.Started)
            {
                if (frame < 60)
                {
                    frame++;
                }
                else
                {
                    frame = 0;
                    hideGameMenuSettings();
                }
            }
        }
    }
}

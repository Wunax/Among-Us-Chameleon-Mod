using System;
using System.Collections.Generic;
using UnityEngine;

using PlayerControl = FFGALNAPKCD;

namespace ChameleonMod.API
{
    public class PlayerController
    {
        public PlayerControl PlayerControl { get; }
        public PlayerController(PlayerControl playerControl)
        {
            PlayerControl = playerControl;
        }

        public PlayerData PlayerData
        {
            get
            {
                return new PlayerData(this);
            }
        }
        public uint NetId
        {
            get
            {
                return PlayerControl.NetId;
            }
            set
            {
                PlayerControl.NetId = value;
            }
        }
        public Vector2 Position
        {
            get
            {
                return PlayerControl.GetTruePosition();
            }
        }

        public bool Equals(PlayerController other)
        {
            return NetId == other.NetId;
        }
        public static PlayerController GetLocalPlayer()
        {
            return new PlayerController(PlayerControl.LocalPlayer);
        }

        public static List<PlayerController> GetAllPlayers()
        {
            List<PlayerController> allPlayers = new List<PlayerController>();

            foreach (var playerControl in PlayerControl.AllPlayerControls)
                allPlayers.Add(new PlayerController(playerControl));

            return allPlayers;
        }

        public static List<PlayerController> GetAllPlayersAlive()
        {
            List<PlayerController> alives = new List<PlayerController>();
            List<PlayerController> allPlayers = GetAllPlayers();

            foreach (var player in allPlayers)
            {
                if (!player.PlayerData.IsDead)
                    alives.Add(player);
            }
            return alives;
        }

        public static List<PlayerController> GetCrewmates()
        {
            List<PlayerController> crewmates = new List<PlayerController>();
            List<PlayerController> allPlayers = GetAllPlayers();

            foreach (var player in allPlayers)
            {
                if (!player.PlayerData.IsImpostor)
                    crewmates.Add(player);
            }
            return crewmates;
        }

        public static List<PlayerController> GetImpostors()
        {
            List<PlayerController> impostors = new List<PlayerController>();
            List<PlayerController> allPlayers = GetAllPlayers();

            foreach (var player in allPlayers)
            {
                if (player.PlayerData.IsImpostor)
                    impostors.Add(player);
            }
            return impostors;
        }

        public static List<PlayerController> GetImpostorsAlive()
        {
            List<PlayerController> impostorsAlive = new List<PlayerController>();
            List<PlayerController> impostors = GetImpostors();

            foreach (var player in impostors)
            {
                if (!player.PlayerData.IsDead)
                    impostorsAlive.Add(player);
            }
            return impostorsAlive;
        }

        public static PlayerController getPlayerById(byte id)
        {
            var allPlayers = GetAllPlayers();
            foreach (var player in allPlayers)
            {
                if (player.PlayerControl.PlayerId == id)
                    return player;
            }
            return null;
        }

        public static PlayerController getPlayerByName(string name)
        {
            var allPlayers = GetAllPlayers();
            foreach (var player in allPlayers)
            {
                if (player.PlayerControl.nameText.Text == name)
                    return player;
            }
            return null;
        }

        public static double distBeetweenPlayers(PlayerController first, PlayerController second)
        {
            return Math.Sqrt(Math.Pow(second.Position.x - first.Position.x, 2) + Math.Pow(second.Position.y - first.Position.y, 2));
        }

        public static PlayerController getHost()
        {
            var allPlayers = PlayerController.GetAllPlayers();
            PlayerController host = allPlayers[0];

            foreach (var player in allPlayers)
                if (player.NetId < host.NetId)
                    host = player;
            return host;
        }
    }
}
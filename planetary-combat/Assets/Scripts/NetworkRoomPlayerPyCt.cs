using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    public class NetworkRoomPlayerPyCt : NetworkRoomPlayer
    {
        public string username;

        static readonly List<NetworkRoomPlayerPyCt> playersList = new List<NetworkRoomPlayerPyCt>();

        [Header("Player UI")]
        public GameObject playerUIPrefab;

        GameObject playerUIObject;

        RoomPlayerUI playerUI = null;

      
        [SyncVar]
        public int playerNumber = 0;


        public override void OnStartClient()
        {
            base.OnStartClient();
            playerUIObject = Instantiate(playerUIPrefab, RoomCanvasUI.instance.playersPanel);
            playerUI = playerUIObject.GetComponent<RoomPlayerUI>();

            playerUI.OnPlayerNumberChanged(playerNumber);
            playerUI.SetRoomPlayer(this);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            // Add this to the static Players List
            playersList.Add(this);
        }

        [ServerCallback]
        internal static void ResetPlayerNumbers()
        {
            int playerNumber = 0;
            foreach (var player in playersList)
                player.playerNumber = ++playerNumber;
        }

        public override void OnStopServer()
        {
            playersList.Remove(this);
        }
    }
}
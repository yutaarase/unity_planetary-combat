using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.PlanetaryCombat
{
    public class RoomPlayerUI : MonoBehaviour
    {
        NetworkRoomPlayerPyCt roomPlayer;
        public Image image;

        public Text playerNumText;

        public Text playerNameText;

        private void Start()
        {

        }

        private void Update()
        {
            if (roomPlayer == null) return;
            if(playerNameText.text != roomPlayer.username)
                playerNameText.text = roomPlayer.username;

        }

        // Sets a highlight color for the local player
        public void SetLocalPlayer()
        {
            // add a visual background for the local player in the UI
            image.color = new Color(1f, 1f, 1f, 0.1f);
        }

        // This value can change as clients leave and join
        public void OnPlayerNumberChanged(int newPlayerNumber)
        {
            playerNumText.text = newPlayerNumber.ToString();
        }

        public void SetRoomPlayer(NetworkRoomPlayerPyCt player)
        {
            roomPlayer = player;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    [AddComponentMenu("")]
    public class NetworkRoomManagerPyCt : NetworkRoomManager
    {
        [Header("Spawner Setup")]
        [Tooltip("Reward Prefab for the Spawner")]
        public GameObject rewardPrefab;

        public override void OnRoomServerSceneChanged(string sceneName)
        {
            // spawn the initial batch of Rewards
            if (sceneName == GameplayScene)
                PlayerSetup.InitialSpawn();
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
            Player player = gamePlayer.GetComponent<Player>();
            playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayerPyCt>().index;
            player.username = roomPlayer.GetComponent<NetworkRoomPlayerPyCt>().username;
            return true;
        }

        public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnRoomServerAddPlayer(conn);
            NetworkRoomPlayerPyCt.ResetPlayerNumbers();
        }

        public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnRoomServerDisconnect(conn);
            NetworkRoomPlayerPyCt.ResetPlayerNumbers();
        }

        public override void OnRoomStopClient()
        {
            base.OnRoomStopClient();
        }

        public override void OnRoomStopServer()
        {
            base.OnRoomStopServer();
        }

        bool showStartButton;

        public override void OnRoomServerPlayersReady()
        {
            
        #if UNITY_SERVER
            base.OnRoomServerPlayersReady();
        #else
            showStartButton = true;
        #endif
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if (allPlayersReady && showStartButton && numPlayers >= minPlayers)
            {
                //GUI.Button(new Rect(150, 300, 120, 20), "START GAME")
                showStartButton = false;

                ServerChangeScene(GameplayScene);
            }
        }
    }
}
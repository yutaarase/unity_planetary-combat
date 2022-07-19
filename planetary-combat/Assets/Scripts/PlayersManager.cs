using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    public class PlayersManager : NetworkBehaviour
    {

        public static PlayersManager instatnce;

        public MatchSettings matchSettings;

        private const string PLAYER_ID_PREFIX = "Player ";

        public SyncDictionary<string, Player> players = new SyncDictionary<string, Player>();


        private void Start()
        {
            if (instatnce != null) return;
            instatnce = GetComponent<PlayersManager>();
        }

        [Command]
        public void RegisterPlayer(string netID, Player player)
        {
            string playerID = PLAYER_ID_PREFIX + netID;
            instatnce.players.Add(playerID, player);
            player.transform.name = playerID;
            Test(netID);
        }

        public void Test(string id)
        {
            Debug.Log(id);
        }

        [Command]
        public void UnRegisterPlayer(string playerID)
        {
            players.Remove(playerID);
        }

        public Player GetPlayer(string playerID)
        {
            return players[playerID];
        }

        public Player[] GetAllPlayers()
        {
            return players.Values.ToArray();
        }

        public void OnPlayerKilledCallback(string playerID, string sourceID)
        {
            StartCoroutine(Respawn(instatnce.GetPlayer(playerID).transform));
            instatnce.GetPlayer(playerID).gameObject.SetActive(false);
        }

        private IEnumerator Respawn(Transform player)
        {
            yield return new WaitForSeconds(matchSettings.respawnTime);

            player.gameObject.SetActive(true);

            Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
            player.position = _spawnPoint.position;
            player.rotation = _spawnPoint.rotation;

            yield return new WaitForSeconds(0.1f);


            player.GetComponent<Player>().SetupPlayer();
            Debug.Log("Player Respawn");
        }
    }
}
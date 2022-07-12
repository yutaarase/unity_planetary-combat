using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using Mirror.PlanetaryCombat;

	public class GameManager : MonoBehaviour
	{

		public static GameManager instance;

	    public MatchSettings matchSettings;

		public void OnPlayerKilledCallback(string playerID, string sourceID)
		{
			StartCoroutine(Respawn(GetPlayer(playerID).transform));
			GetPlayer(playerID).gameObject.SetActive(false);
			
		}


		void Awake()
		{
			if (instance == null)
			{
				instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
			else
			{
				Destroy(this.gameObject);
			}
		}

		private const string PLAYER_ID_PREFIX = "Player ";

		private static Dictionary<string, Player> players = new Dictionary<string, Player>();

		public IEnumerator RegisterPlayer(string _netID, Player _player)
		{
			string _playerID = PLAYER_ID_PREFIX + _netID;
			players.Add(_playerID, _player);
			_player.transform.name = _playerID;
			Debug.Log("Add : "+ _playerID);
			yield return null;
		}

		public static void UnRegisterPlayer(string _playerID)
		{
			players.Remove(_playerID);
		}

		public static Player GetPlayer(string _playerID)
		{
			return players[_playerID];
		}

		public static Player[] GetAllPlayers()
		{
			return players.Values.ToArray();
		}

		private IEnumerator Respawn(Transform player)
		{
			yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

			player.gameObject.SetActive(true);

			Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
			player.position = _spawnPoint.position;
			player.rotation = _spawnPoint.rotation;

			yield return new WaitForSeconds(0.1f);

		
			player.GetComponent<Player>().SetupPlayer();

			Debug.Log(transform.name + " respawned.");
		}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror.PlanetaryCombat;

	public class GameManager : MonoBehaviour
	{

		public static GameManager instance;

	    public MatchSettings matchSettings;

		public delegate void OnPlayerKilledCallback(string player, string source);
		public OnPlayerKilledCallback onPlayerKilledCallback;

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

		public static void RegisterPlayer(string _netID, Player _player)
		{
			string _playerID = PLAYER_ID_PREFIX + _netID;
			players.Add(_playerID, _player);
			_player.transform.name = _playerID;
			Debug.Log(_playerID);
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
	}
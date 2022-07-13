using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using Mirror.PlanetaryCombat;

public class GameManager : NetworkBehaviour
{

	public static GameManager instance;

	public MatchSettings matchSettings;

	private const string PLAYER_ID_PREFIX = "Player ";

	public readonly SyncDictionary<string, Player> players = new SyncDictionary<string, Player>();

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

    public override void OnStartClient()
    {
        base.OnStartClient();

    }

    public void OnPlayerKilledCallback(string playerID, string sourceID)
	{
		StartCoroutine(Respawn(GetPlayer(playerID).transform));
		GetPlayer(playerID).gameObject.SetActive(false);

	}

	public void CmdRegisterPlayer(string _netID, Player _player)
	{
		string _playerID = PLAYER_ID_PREFIX + _netID;
		players.Add(_playerID, _player);
		_player.transform.name = _playerID;

	}

	public void CmdUnRegisterPlayer(string _playerID)
	{
		players.Remove(_playerID);
	}

	public Player GetPlayer(string _playerID)
	{
		return players[_playerID];
	}



	public Player[] GetAllPlayers()
	{
		return players.Values.ToArray();
	}

	private IEnumerator Respawn(Transform player)
	{
		yield return new WaitForSeconds(instance.matchSettings.respawnTime);

		player.gameObject.SetActive(true);

		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		player.position = _spawnPoint.position;
		player.rotation = _spawnPoint.rotation;

		yield return new WaitForSeconds(0.1f);

		
		player.GetComponent<Player>().SetupPlayer();
	}
}
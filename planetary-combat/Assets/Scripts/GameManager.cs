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

    public static Dictionary<string, Player> players = new Dictionary<string, Player>();

    

    [SyncVar] public Player cPlayer;

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

    public override void OnStartServer()
    {
        base.OnStartServer();
    }


    public void RegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID_PREFIX + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;

    }

    [Command]
    public void CmdRegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID_PREFIX + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;
    }

    [Command]
    public void CmdUnRegisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    [Command]
    public void CmdGetPlayer(string playerID)
    {
        cPlayer = players[playerID];
    }

    [ClientRpc]
    public void RpcGetPlayer(string playerID)
    {
        cPlayer = players[playerID];
    }

    [Server]
    public Player SvrGetPlayer(string playerID)
    {
        return players[playerID];
    }

    public static Player GetPlayer(string playerID)
    {
        return players[playerID];
    }

    public Player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

    public void OnPlayerKilledCallback(string playerID, string sourceID)
    {
        StartCoroutine(Respawn(GetPlayer(playerID).transform));
        GetPlayer(playerID).gameObject.SetActive(false);

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
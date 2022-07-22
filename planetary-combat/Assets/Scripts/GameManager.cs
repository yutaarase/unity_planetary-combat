using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.PlanetaryCombat;

public class GameManager : NetworkBehaviour
{

	public static GameManager instance;

    public MatchSettings matchSettings;


    [SyncVar] public Player cPlayer;

    void Start()
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
}
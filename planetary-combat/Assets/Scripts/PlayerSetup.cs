using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
	internal class PlayerSetup : NetworkBehaviour
	{
		[SerializeField]
		GameObject playerUIPrefab;
		[HideInInspector]
		public GameObject playerUIInstance;

		public PlayersManager playerManager;

		void Start()
		{
			// Disable components that should only be
			// active on the player that we control
			if (!isLocalPlayer) return;
			
			// Create PlayerUI
			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

		}

		[Command]
		void CmdSetUsername(string playerID, string username)
		{
			Player player = playerManager.GetPlayer(playerID);
			if (player != null)
			{
				Debug.Log(username + " has joined!");
				player.username = username;
			}

			Debug.Log(player);
		}


		public override void OnStartServer()
		{
			base.OnStartServer();

		}



		public override void OnStartClient()
		{
			base.OnStartClient();


		}

		public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
			
		}
/*
		bool a = true;
		bool b = true;*/
        private void Update()
        {
			/*if (b)
			{
				if (GameManager.instance == null) return;
				string netID = GetComponent<NetworkIdentity>().netId.ToString();
				Player player = GetComponent<Player>();

				playerManager.RegisterPlayer(netID, player);
				b = false;
			}

			if (a)
            {

				if (playerManager.GetPlayer(transform.name) != null){

					Lug();
					a = false;
				}
            }*/
        }


        void Lug()
        {

			string username = "Loading...";

			username = transform.name;
			Debug.Log(transform.name);
			CmdSetUsername(transform.name, username);
		}



        public override void OnStopClient()
        {
            base.OnStopClient();
			Destroy(playerUIInstance);

		}

		internal static void InitialSpawn()
		{
			if (!NetworkServer.active) return;

			//for (int i = 0; i < 10; i++)
				//SpawnReward();
		}

		internal static void SpawnReward()
		{
			if (!NetworkServer.active) return;

			Vector3 spawnPosition = new Vector3(Random.Range(-19, 20), 1, Random.Range(-19, 20));
			NetworkServer.Spawn(Object.Instantiate(((NetworkRoomManagerPyCt)NetworkManager.singleton).rewardPrefab, spawnPosition, Quaternion.identity));
		}
	}

}
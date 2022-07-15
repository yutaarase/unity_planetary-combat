using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
	public class PlayerSetup : NetworkBehaviour
	{
		[SerializeField]
		GameObject playerUIPrefab;
		[HideInInspector]
		public GameObject playerUIInstance;

		void Start()
		{
			// Disable components that should only be
			// active on the player that we control
			if (!isLocalPlayer) return;
			
			// Create PlayerUI
			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

			GetComponent<Player>().SetupPlayer();

			

		}

		[Command]
		void CmdSetUsername(string playerID, string username)
		{
			Player player = GameManager.instance.GetPlayer(playerID);
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
			string netID = GetComponent<NetworkIdentity>().netId.ToString();
			Player player = GetComponent<Player>();

			GameManager.instance.RegisterPlayer(netID, player);
		}

		bool a = true;
		bool b = true;
        private void Update()
        {
			if (b)
			{
				if (GameManager.instance == null) return;
				
				b = false;
			}

			if (a)
            {

				if (GameManager.instance.GetPlayer(transform.name) != null){

					Lug();
					a = false;
				}
            }
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
			GameManager.instance.UnRegisterPlayer(transform.name);
			Destroy(playerUIInstance);

		}
    }

}
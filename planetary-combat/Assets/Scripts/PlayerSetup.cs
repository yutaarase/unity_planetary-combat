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
			GameManager.instance.RpcGetPlayer(playerID);

			Player player = GameManager.instance.cPlayer;
			if (player != null)
			{
				Debug.Log(username + " has joined!");
				player.username = username;
			}
		}

        public override void OnStartServer()
        {
            base.OnStartServer();

			if (!isLocalPlayer) return;
			string _netID = GetComponent<NetworkIdentity>().netId.ToString();
			Player _player = GetComponent<Player>();

			GameManager.instance.RegisterPlayer(_netID, _player);
		}

        public override void OnStartClient()
        {
            base.OnStartClient();

            
        }



        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            if (!isLocalPlayer) return;

            string _username = "Loading...";

            _username = transform.name;
            CmdSetUsername(transform.name, _username);
        }


        public override void OnStopClient()
        {
            base.OnStopClient();
			GameManager.instance.CmdUnRegisterPlayer(transform.name);
			Destroy(playerUIInstance);

		}
    }

}
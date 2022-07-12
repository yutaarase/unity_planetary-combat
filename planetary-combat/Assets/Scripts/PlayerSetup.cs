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
			Player player = GameManager.GetPlayer(playerID);
			if (player != null)
			{
				Debug.Log(username + " has joined!");
				player.username = username;
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();

			string _netID = GetComponent<NetworkIdentity>().netId.ToString();
			Player _player = GetComponent<Player>();

			yield return StartCoroutine(GameManager.RegisterPlayer(_netID, _player));
			


			string _username = "Loading...";

			_username = transform.name;
			//CmdSetUsername(transform.name, _username);
		}


        public override void OnStopClient()
        {
            base.OnStopClient();
			GameManager.UnRegisterPlayer(transform.name);
			Destroy(playerUIInstance);

		}
	}

}
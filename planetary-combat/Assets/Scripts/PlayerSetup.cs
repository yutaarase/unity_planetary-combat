using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
	public class PlayerSetup : NetworkBehaviour
	{

		[SerializeField]
		Behaviour[] componentsToDisable;
		 
		[SerializeField]
		GameObject playerUIPrefab;
		[HideInInspector]
		public GameObject playerUIInstance;

		void Start()
		{
			// Disable components that should only be
			// active on the player that we control
			if (!isLocalPlayer)
			{
				DisableComponents();
				AssignRemoteLayer();
			}
			else
			{

				// Create PlayerUI
				playerUIInstance = Instantiate(playerUIPrefab);
				playerUIInstance.name = playerUIPrefab.name;

				GetComponent<Player>().SetupPlayer();

				string _username = "Loading...";
				//if (UserAccountManager.IsLoggedIn)
				//	_username = UserAccountManager.LoggedIn_Username;
				//else
					_username = transform.name;

				CmdSetUsername(transform.name, _username);
			}
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

		void SetLayerRecursively(GameObject obj, int newLayer)
		{
			obj.layer = newLayer;

			foreach (Transform child in obj.transform)
			{
				SetLayerRecursively(child.gameObject, newLayer);
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();

			string _netID = GetComponent<NetworkIdentity>().netId.ToString();
			Player _player = GetComponent<Player>();

			GameManager.RegisterPlayer(_netID, _player);
		}

		void AssignRemoteLayer()
		{
			gameObject.layer = LayerMask.NameToLayer("Player");
		}

		void DisableComponents()
		{
			for (int i = 0; i < componentsToDisable.Length; i++)
			{
				componentsToDisable[i].enabled = false;
			}
		}

		// When we are destroyed
		void OnDisable()
		{
			Destroy(playerUIInstance);

			GameManager.UnRegisterPlayer(transform.name);
		}

	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    public class Player : NetworkBehaviour
    {
		public string username;

		public bool isDead;
		public bool isSetup;

		[SerializeField]
		private int maxHealth = 100;

		[SyncVar]
		public int currentHealth;

		public float GetHealthPct()
		{
			return (float)currentHealth / maxHealth;
		}


		public int kills;
		public int deaths;


		[SerializeField]
		private GameObject deathEffect;

		[SerializeField]
		private GameObject spawnEffect;

		public PlayersManager playerManager;

        private void Awake()
        {

		}

        private void Start()
        {

		}

        public void SetupPlayer()
		{
			CmdBroadCastNewPlayerSetup();
		}

		[Command]
		private void CmdBroadCastNewPlayerSetup()
		{
			RpcSetupPlayerOnAllClients();
		}

		[ClientRpc]
		private void RpcSetupPlayerOnAllClients()
		{
			Init();
		}

		[ClientRpc]
		public void RpcTakeDamage(int amount, string sourceID)
		{
			if (isDead) return;

			currentHealth -= amount;

			if (currentHealth <= 0)
			{
				Die(sourceID);
			}
		}

		[Command]
		private void Die(string sourceID)
		{
			isDead = true;


			/*Debug.Log("Player : "+ playerManager.GetPlayer(sourceID).username);
			Player sourcePlayer = playerManager.GetPlayer(sourceID);
			if (sourcePlayer != null)
			{
				sourcePlayer.kills++;
				playerManager.OnPlayerKilledCallback(transform.name,sourceID);
			}*/

			deaths++;

			//Switch cameras
			if (isLocalPlayer)
			{
				GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
			}

			Debug.Log(transform.name + " is DEAD!");

			
		}

		public void Init()
		{
			isDead = false;

			currentHealth = maxHealth;


			//Switch cameras
			if (isLocalPlayer)
			{
				GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
			}
		}
	}
}

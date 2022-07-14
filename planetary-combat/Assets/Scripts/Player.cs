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

		private void Die(string sourceID)
		{
			isDead = true;

			GameManager.instance.CmdGetPlayer(sourceID);
			Player sourcePlayer = GameManager.instance.cPlayer;
			if (sourcePlayer != null)
			{
				sourcePlayer.kills++;
				GameManager.instance.OnPlayerKilledCallback(transform.name,sourceID);
			}

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

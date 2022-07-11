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
		private Behaviour[] disableOnDeath;
		private bool[] wasEnabled;

		[SerializeField]
		private GameObject[] disableGameObjectsOnDeath;

		[SerializeField]
		private GameObject deathEffect;

		[SerializeField]
		private GameObject spawnEffect;


        private void OnCollisionStay(Collision collision)
        {
			RpcTakeDamage(1, transform.name);
        }

        private void Start()
        {
			//GameManager.RegisterPlayer(netId.ToString(),GetComponent<Player>());
        }

        public void SetupPlayer()
		{
			if (isLocalPlayer)
			{
				GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
			}

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
			if (isSetup)
			{
				wasEnabled = new bool[disableOnDeath.Length];
				for (int i = 0; i < wasEnabled.Length; i++)
				{
					wasEnabled[i] = disableOnDeath[i].enabled;
				}

				isSetup = false;
			}

			Init();
		}

		[ClientRpc]
		public void RpcTakeDamage(int _amount, string _sourceID)
		{
			if (isDead)
				return;

			currentHealth -= _amount;

			if (currentHealth <= 0)
			{
				Die(_sourceID);
			}
		}

		private void Die(string sourceID)
		{
			isDead = true;

			Player sourcePlayer = GameManager.GetPlayer(sourceID);
			if (sourcePlayer != null)
			{
				sourcePlayer.kills++;
				GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
			}

			deaths++;

			//Disable components
			for (int i = 0; i < disableOnDeath.Length; i++)
			{
				disableOnDeath[i].enabled = false;
			}

			//Disable GameObjects
			for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
			{
				disableGameObjectsOnDeath[i].SetActive(false);
			}

			//Disable the collider
			Collider _col = GetComponent<Collider>();
			if (_col != null)
				_col.enabled = false;

			//Switch cameras
			if (isLocalPlayer)
			{
				GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
			}

			Debug.Log(transform.name + " is DEAD!");

			StartCoroutine(Respawn());
		}

		private IEnumerator Respawn()
		{
			yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

			Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
			transform.position = _spawnPoint.position;
			transform.rotation = _spawnPoint.rotation;

			yield return new WaitForSeconds(0.1f);

			SetupPlayer();

			Debug.Log(transform.name + " respawned.");
		}

		public void Init()
		{
			isDead = false;

			currentHealth = maxHealth;

			//Enable the components
			for (int i = 0; i < disableOnDeath.Length; i++)
			{
				disableOnDeath[i].enabled = wasEnabled[i];
			}

			//Enable the gameobjects
			for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
			{
				disableGameObjectsOnDeath[i].SetActive(true);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Networking.Match;

namespace Mirror.PlanetaryCombat
{
	public class PauseMenu : MonoBehaviour
	{

		public static bool IsOn = false;

		private NetworkManager networkManager;

		void Start()
		{
			networkManager = NetworkManager.singleton;
		}

		public void LeaveRoom()
		{
			networkManager.StopHost();
		}

	}
}
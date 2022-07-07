using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
	[System.Serializable]
	public class WeaponStatus
	{
		public int damage = 10;

		public float range = 100f;

		public float fireRate = 0f;

		public int maxBullets = 20;

		public int bullets;

		public float reloadTime = 1f;

		public GameObject graphics;

		public WeaponStatus()
		{
			bullets = maxBullets;
		}

	}
}
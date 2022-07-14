using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
	public class WeaponManager : NetworkBehaviour
	{

		[SerializeField]
		private Transform weaponHolder;

		private WeaponStatus currentWeapon;

		public bool isReloading = false;

		private AnimationManager animation;

		void Start()
		{
			animation = GetComponent<AnimationManager>();
		}

		public WeaponStatus GetCurrentWeapon()
		{
			return currentWeapon;
		}

		public void Reload()
		{
			if (isReloading)
				return;

			StartCoroutine(Reload_Coroutine());
		}

		private IEnumerator Reload_Coroutine()
		{
			Debug.Log("Reloading...");

			isReloading = true;

			CmdOnReload();

			yield return new WaitForSeconds(currentWeapon.reloadTime);

			currentWeapon.bullets = currentWeapon.maxBullets;

			animation.SetPlaySpeed(1f);

			isReloading = false;
		}

		[Command]
		void CmdOnReload()
		{
			RpcOnReload();
		}

		[ClientRpc]
		void RpcOnReload()
		{
			AnimationManager animation = GetComponent<AnimationManager>();
			animation.Fire(AnimationManager.Shot.Reload);
			animation.SetPlaySpeed(2.667f / currentWeapon.reloadTime);
		}

	}

}

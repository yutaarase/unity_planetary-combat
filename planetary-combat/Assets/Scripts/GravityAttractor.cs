using UnityEngine;
using System.Collections;

namespace Mirror.PlanetaryCombat
{
	public class GravityAttractor : MonoBehaviour
	{

		public float gravity = -9.8f;

		[ServerCallback]
		public void Attract(Rigidbody rb)
		{
			Vector3 gravityUp = (rb.position - transform.position).normalized;
			Vector3 localUp = rb.transform.up;

			// Apply downwards gravity to body
			rb.AddForce(gravityUp * gravity);
			// Allign bodies up axis with the centre of planet
			rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.FromToRotation(localUp, gravityUp) * rb.rotation, 5);

		}
	}
}
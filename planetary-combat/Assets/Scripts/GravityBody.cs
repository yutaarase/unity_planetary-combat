using UnityEngine;
using System.Collections;

namespace Mirror.PlanetaryCombat
{
	[RequireComponent(typeof(Rigidbody))]
	public class GravityBody : MonoBehaviour
	{

		GravityAttractor planet;
		Rigidbody rigidbody;

		void Awake()
		{
			planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();
			rigidbody = GetComponent<Rigidbody>();

			// Disable rigidbody gravity and rotation as this is simulated in GravityAttractor script
			rigidbody.useGravity = false;
			rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		}

		void FixedUpdate()
		{
			// Allow this body to be influenced by planet's gravity
			planet.Attract(rigidbody);
		}

		[ServerCallback]
		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Planet")
			{
				planet = other.gameObject.GetComponent<GravityAttractor>();
			}
		}
	}
}
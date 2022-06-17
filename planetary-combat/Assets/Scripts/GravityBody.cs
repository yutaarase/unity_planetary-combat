using UnityEngine;
using System.Collections;

namespace Mirror.PlanetaryCombat
{
	[RequireComponent(typeof(Rigidbody))]
	public class GravityBody : MonoBehaviour
	{

		GravityAttractor planet;
		Rigidbody rb;

		void Awake()
		{
			planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();
			rb = GetComponent<Rigidbody>();

			// Disable rigidbody gravity and rotation as this is simulated in GravityAttractor script
			rb.useGravity = false;
			rb.constraints = RigidbodyConstraints.FreezeRotation;
		}

		void FixedUpdate()
		{
			// Allow this body to be influenced by planet's gravity
			test();
		}

        [ServerCallback]
		private void OnTriggerEnter(Collider other)
		{
			if (other.transform.parent.tag == "Planet")
			{
				planet = other.transform.parent.gameObject.GetComponent<GravityAttractor>();
			}
		}

		[ServerCallback]
		void test()
        {
			planet.Attract(rb);
		}
	}
}
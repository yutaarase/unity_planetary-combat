using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    [RequireComponent(typeof(GravityBody))]
    public class Player : NetworkBehaviour
    {

		[SerializeField] private float mouseSensitivityX = 1;
        [SerializeField] private float mouseSensitivityY = 1;
		[SerializeField] private float walkSpeed = 6;
		[SerializeField] private float jumpForce = 220;
		[SerializeField] private LayerMask groundedMask;

		[SerializeField] private float offsetX = 0;
		[SerializeField] private float offsetY = 3;
		[SerializeField] private float offsetZ = -3;

		[SerializeField] private float rotateX = 6.5f;
		[SerializeField] private float rotateY = 2f;
		[SerializeField] private float rotateZ = 0f;

		[SerializeField] private float rotateSpeed = 1;

		[SerializeField] private new GameObject camera;

		bool grounded;

        Rigidbody rb;




        private void Start()
        {
			rb = GetComponent<Rigidbody>();
		}


        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
			camera = Instantiate(camera);
			camera.transform.rotation = transform.rotation;
			camera.transform.position = transform.position + new Vector3(offsetX, offsetY, offsetZ);
			camera.transform.SetParent(transform);
			camera.transform.localEulerAngles = new Vector3(rotateX, rotateY, rotateZ);
			Cursor.visible = false;
		}

        // Update is called once per frame
        void Update()
        {
			if (!isLocalPlayer) return;

			var hori = Input.GetAxis("Horizontal");
			var vert = Input.GetAxis("Vertical");

			Vector3 moveVect = transform.right * hori + transform.forward * vert;

			if (moveVect != Vector3.zero)
			{
				PlayerMove(moveVect * 0.1f);
			}

			// Grounded check
			Ray ray = new Ray(transform.position, -transform.up);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask))
			{
				grounded = true;
			}
			else
			{
				grounded = false;
			}

			// Jump
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Jump();	
			}

			Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }


		[Command]
		void Jump()
        {
			if (grounded)
			{
				rb.AddForce(transform.up * jumpForce);
			}
		}

		[Command]
		void Rotate(float x, float y)
        {
			transform.Rotate(Vector3.up * x * mouseSensitivityX);
			camera.transform.Rotate(Vector3.left * y * mouseSensitivityY);
			TestRay();

		}

		[Command]
		void PlayerMove(Vector3 vect)
		{
			rb.MovePosition(vect + transform.position);
		}

		[ServerCallback]
        private void OnCollisionStay(Collision collision)
        {
            
        }

		void TestRay()
		{
			float distance = 100; // 飛ばす&表示するRayの長さ
			float duration = 1;   // 表示期間（秒）

			Ray ray = new Ray(camera.transform.position, camera.transform.forward);
			Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue, duration, false);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    [RequireComponent(typeof(GravityBody))]
    public class Player : NetworkBehaviour
    {

        public float mouseSensitivityX = 1;
        public float mouseSensitivityY = 1;
        public float walkSpeed = 6;
        public float jumpForce = 220;
        public LayerMask groundedMask;


        bool grounded;
        Vector3 moveAmount;
        Vector3 smoothMoveVelocity;
        float verticalLookRotation;
        Transform cameraTransform;
        Rigidbody rigidbody;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // movement for local player
            if (isLocalPlayer)
            {
				// Look rotation:
				transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
				verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
				verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
				cameraTransform.localEulerAngles = new Vector3(6.5f, 0, 0);

				// Calculate movement:
				float inputX = Input.GetAxisRaw("Horizontal");
				float inputY = Input.GetAxisRaw("Vertical");

				Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
				Vector3 targetMoveAmount = moveDir * walkSpeed;
				moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

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
				if (Input.GetButtonDown("Jump"))
				{
					if (grounded)
					{
						rigidbody.AddForce(transform.up * jumpForce);
					}
				}
			}
        }

		void FixedUpdate()
		{
			// movement for local player
			if (isLocalPlayer)
            {
				// Apply movement to rigidbody
				Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
				rigidbody.MovePosition(rigidbody.position + localMove);
			}
		}
	}
}

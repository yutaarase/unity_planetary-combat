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
		[SerializeField] private float jumpForce = 330;
		[SerializeField] private float flyForce = 5;

		[SerializeField] private float offsetX = 0;
		[SerializeField] private float offsetY = 3;
		[SerializeField] private float offsetZ = -3;

		[SerializeField] private float rotateX = 6.5f;
		[SerializeField] private float rotateY = 2f;
		[SerializeField] private float rotateZ = 0f;

		[SerializeField] private new GameObject camera;

		AnimationManager animation;

		bool grounded;

        Rigidbody rb;




        private void Start()
        {
			rb = GetComponent<Rigidbody>();
			animation = GetComponent<AnimationManager>();
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
                if (grounded)
                {
					animation.Action(AnimationManager.ActionID.Walk);
					animation.Move(hori, vert);

					if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
					{
						animation.Action(AnimationManager.ActionID.Dush);
						PlayerMove(transform.forward * vert * 0.2f);
					}
					else if (Input.GetMouseButton(0))
					{
						PlayerMove(moveVect * 0.05f);
					}
					else
					{
						PlayerMove(moveVect * 0.1f);
					}
                }
                else
                {
					PlayerMove(moveVect * 0.1f);
				}
			}

            if (Input.GetMouseButton(0))
            {
				animation.Action(AnimationManager.ActionID.Walk);
				animation.Fire(AnimationManager.Shot.Fire);
            }
            else
            {
				animation.Fire(AnimationManager.Shot.Cease);
			}


			if (Input.GetKey(KeyCode.Space))
			{
				// Jump
				if (Input.GetKeyDown(KeyCode.Space))
				{
					Jump();

				}

				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
					Fly();
				}
			}

			

			


            if (!Input.anyKey)
            {
				animation.Action(AnimationManager.ActionID.Idle);
				animation.Fire(AnimationManager.Shot.Cease);
            }

			var rotX = Input.GetAxis("Mouse X");
			var rotY = Input.GetAxis("Mouse Y");

			CharaRotate(rotX,rotY);
			CameraRotate(rotY);
        }


		[Command]
		void Jump()
        {
			if (grounded)
			{
				animation.Action(AnimationManager.ActionID.Jump);
				rb.AddForce(transform.up * jumpForce);
				grounded = false;
			}
		}

		void Fly()
        {
            if (!grounded)
            {
				animation.Action(AnimationManager.ActionID.Fly);
				rb.AddForce(transform.up * flyForce);
			}
        }

		[Command]
		void CharaRotate(float x, float y)
        {
			transform.Rotate(Vector3.up * x * mouseSensitivityX);
			if(!grounded) transform.Rotate(Vector3.left * y * mouseSensitivityY);

			TestRay();

		}

		[Client]
		void CameraRotate(float y)
        {
			camera.transform.Rotate(Vector3.left * y * mouseSensitivityY);
		}

		[Command]
		void PlayerMove(Vector3 vect)
		{
			rb.MovePosition(vect + transform.position);
		}

		[ServerCallback]
        private void OnCollisionStay(Collision collision)
        {
			if(collision.collider.gameObject.tag == "Planet")
            {
				grounded = true;
			}
			else
			{
				grounded = false;
			}
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

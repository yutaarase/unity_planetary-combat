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


		[SyncVar]bool grounded;

		[SyncVar] public ActionID actionID;

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
			grounded = false;
			actionID = ActionID.Fly;
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
					actionID = ActionID.Walk;

					if (Input.GetMouseButton(0))
					{
						PlayerMove(moveVect * 0.05f);
						animation.Move(hori /1.8f, vert/1.8f);
					}
					else
					{
						if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
						{
							actionID = ActionID.Dush;
							PlayerMove(transform.forward * vert * 0.2f);
                        }
                        else
                        {
							PlayerMove(moveVect * 0.1f);
						}
						animation.Move(hori, vert);
					}
                }
                else
                {
					PlayerMove(moveVect * 0.2f);
				}
            }

            if (Input.GetMouseButton(0))
			{
				Fire(AnimationManager.Shot.Fire);
			}
            else
            {
				Fire(AnimationManager.Shot.Cease);
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
					GetComponent<FlyEffectManager>().EffectActive(!grounded);
				}
			}



			if (!Input.anyKey)
			{
				if (grounded) actionID = ActionID.Idle;
				Fire(AnimationManager.Shot.Cease);
				animation.Action(actionID);
			}

			var rotX = Input.GetAxis("Mouse X");
			var rotY = Input.GetAxis("Mouse Y");

			CharaRotate(rotX,rotY);
			CameraRotate(rotY);

			animation.Action(actionID);
		}


		[Command]
		void Jump()
        {
			if (grounded)
			{
				actionID = ActionID.Jump;
				rb.AddForce(transform.up * jumpForce);
				grounded = false;
			}
		}

		[Command]
		void Fly()
        {
            if (!grounded)
            {
				actionID = ActionID.Fly;
				animation.Fly(1f);
				rb.AddForce(transform.up * flyForce);
			}
        }

        [Command]
		void Fire(AnimationManager.Shot shot)
        {
			if (!grounded)animation.Fly(0.5f);
			animation.Fire(shot);
		}


		[Command]
		void PlayerMove(Vector3 vect)
		{
			rb.MovePosition(vect + transform.position);
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

		[ServerCallback]
        private void OnCollisionEnter(Collision collision)
        {
			if (collision.collider.gameObject.tag == "Planet" && collision.collider.gameObject.tag == "Player")
			{
				actionID = ActionID.Idle;
			}
		}

        [ServerCallback]
        private void OnCollisionStay(Collision collision)
        {

			if (collision.collider.gameObject.tag == "Planet")
            {
				grounded = true;
			}
			else
			{
				grounded = false;
				if(collision.collider.gameObject.tag == "Player")
				actionID = ActionID.Fly;
			}
		}

		void TestRay()
		{
			float distance = 100; // 飛ばす&表示するRayの長さ
			float duration = 1;   // 表示期間（秒）

			Ray ray = new Ray(camera.transform.position, camera.transform.forward);
			Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue, duration, false);
		}

		public enum ActionID
		{
			Idle,
			Walk,
			Dush,
			Jump,
			Fly,
			Die
		}
	}
}

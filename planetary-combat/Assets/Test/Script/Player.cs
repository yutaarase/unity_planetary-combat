using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Test
{
    public class Player : NetworkBehaviour
    {
        Rigidbody rb;
        private new Camera camera;

        [SerializeField] float offsetX;
        [SerializeField] float offsetY;
        [SerializeField] float offsetZ;
        [SerializeField] float sensitiveRotate = 5.0f;
        [SerializeField] float jumpPower = 10.0f;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            camera = Instantiate(Camera.main);
            camera.transform.rotation = transform.rotation;
            camera.transform.position = transform.position + new Vector3(offsetX, offsetY, offsetZ);
            camera.transform.SetParent(transform);
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
                PlayerMove(moveVect* 0.1f);
            }

            if (Input.GetMouseButton(1))
            {
                float rotateX = Input.GetAxis("Mouse X") * sensitiveRotate;
                float rotateY = Input.GetAxis("Mouse Y") * sensitiveRotate;
                camera.transform.Rotate(rotateY, 0.0f, 0.0f);
                Rotation(rotateX);
                
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                Hopper();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }


        }


        [Command]
        void PlayerMove(Vector3 vect)
        {
            rb.MovePosition(vect + transform.position);
        }


        [Command]
        void Hopper()
        {
            transform.position = transform.position + new Vector3(0f,2f,0f);
            transform.rotation = Quaternion.Euler(0f,transform.eulerAngles.y,0f);
        }

        [Command]
        void Jump()
        {
            rb.AddRelativeForce(new Vector3(0f,jumpPower,0f));
        }

        [Command]
        void Rotation(float x)
        {
            transform.Rotate(0f, x, 0f);
        }
    }
}
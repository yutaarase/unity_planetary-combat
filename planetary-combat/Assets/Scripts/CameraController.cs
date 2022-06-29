using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    public class CameraController : MonoBehaviour
    {
        private GameObject parent;
        private Player player;
        private Camera camera;

        // Start is called before the first frame update
        void Start()
        {
            parent = transform.parent.gameObject;
            player = parent.GetComponent<Player>();
            camera = transform.GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (player.isADS) camera.fieldOfView = 25;
            else camera.fieldOfView = 60;

            var rotX = Input.GetAxis("Mouse X");
            var rotY = Input.GetAxis("Mouse Y");
            CameraRotate(rotX,rotY);
        }

        public void CameraRotate(float x,float y)
        {
            transform.transform.Rotate(Vector3.left * y * player.mouseSensitivityY);
            if (player.isADS) transform.transform.Rotate(Vector3.up * x * player.mouseSensitivityX);
        }
    }
}
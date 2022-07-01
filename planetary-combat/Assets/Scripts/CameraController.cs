using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Mirror.PlanetaryCombat
{
    public class CameraController : MonoBehaviour
    {

        [SerializeField] private float offsetX = 0;
        [SerializeField] private float offsetY = 3;
        [SerializeField] private float offsetZ = -3;

        [SerializeField] private float rotateX = 6.5f;
        [SerializeField] private float rotateY = 2f;
        [SerializeField] private float rotateZ = 0f;

        private Vector3 adsOffset;


        private GameObject parent;
        private Player player;
        private Camera camera;


        // Start is called before the first frame update 
        void Start()
        {
            parent = transform.parent.gameObject;
            player = parent.GetComponent<Player>();
            camera = transform.GetComponent<Camera>();

            transform.rotation = transform.parent.rotation;
            transform.localPosition = new Vector3(offsetX, offsetY, offsetZ);
            transform.localEulerAngles = new Vector3(rotateX, rotateY, rotateZ);
        }
        // Update is called once per frame
        void Update()
        {

            var rotX = Input.GetAxis("Mouse X");
            var rotY = Input.GetAxis("Mouse Y");
            //CameraRotate(rotX,rotY);

            if (player.isADS)
            {
                transform.position = parent.transform.position + adsOffset;
                transform.RotateAround(parent.transform.position + parent.transform.up * 2 + parent.transform.forward * 10, parent.transform.up, rotX);
            }
        }

        public void CameraRotate(float x,float y)
        {
            transform.transform.Rotate(Vector3.left * y * player.mouseSensitivityY);

        }

        private void CameraInit()
        {
            transform.localEulerAngles = new Vector3(rotateX, rotateY, rotateZ);
            transform.localPosition = new Vector3(offsetX, offsetY, offsetZ);
        }

        private async void SetFOV(float fov)
        {
            float diff = fov - camera.fieldOfView;
            float before = camera.fieldOfView;

            for (float i = 0; i <= Mathf.Abs(diff) ; i+=10f)
            {
                camera.fieldOfView += Mathf.Sign(diff)*10f;

                await Task.Delay(1);

            }

            if (camera.fieldOfView != before + diff) camera.fieldOfView = before + diff;
        }

        public void ADS(bool b)
        {
            if (b)
            {
                SetFOV(25f);
                transform.parent = null;
                transform.position = parent.transform.position + parent.transform.up * 2 + parent.transform.forward * -10;
                adsOffset = transform.position - parent.transform.position;
            }
            else
            {
                SetFOV(60f);
                transform.SetParent(parent.transform);
                CameraInit();
            }

        }
    }
}
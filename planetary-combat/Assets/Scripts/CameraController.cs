using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Cinemachine;

namespace Mirror.PlanetaryCombat
{
    public class CameraController : NetworkBehaviour
    {

        [SerializeField] private float offsetX = 0;
        [SerializeField] private float offsetY = 3;
        [SerializeField] private float offsetZ = -3;

        [SerializeField] private float rotateX = 6.5f;
        [SerializeField] private float rotateY = 2f;
        [SerializeField] private float rotateZ = 0f;

        private Vector3 adsOffset;


        [SyncVar]private GameObject parent;
        private Player player;
        private Camera camera;
        private CinemachineBrain brain;

        CinemachineVirtualCamera vcamera;


        // Start is called before the first frame update 
        void Start()
        {
            //各オブジェクトコンポーネント取得
            parent = transform.parent.gameObject;
            player = parent.GetComponent<Player>();
            camera = transform.GetComponent<Camera>();
            vcamera = player.vcamera.transform.GetComponent<CinemachineVirtualCamera>();
            brain = transform.GetComponent<CinemachineBrain>();

            transform.rotation = transform.parent.rotation;
            transform.localPosition = new Vector3(offsetX, offsetY, offsetZ);
            transform.localEulerAngles = new Vector3(rotateX, rotateY, rotateZ);

            vcamera.Follow = parent.transform;
            vcamera.LookAt = parent.transform;
        }

        // Update is called once per frame
        void Update()
        {

            var rotX = Input.GetAxis("Mouse X");
            var rotY = Input.GetAxis("Mouse Y");
            
            if (!player.isADS) CameraRotate(rotX, rotY);

            vcamera.enabled = player.isADS;
            brain.enabled = player.isADS;
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
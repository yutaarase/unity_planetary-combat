using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine.UI;

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


        [SyncVar]private GameObject parent;
        private PlayerController controller;
        private Camera camera;
        private CinemachineBrain brain;

        [SerializeField] private Sprite [] reticle;

        public GameObject image;

        //CinemachineVirtualCamera vcamera;


        // Start is called before the first frame update 
        void Start()
        {
            image = GameObject.Find("reticle");

            //各オブジェクトコンポーネント取得
            parent = transform.parent.gameObject;
            controller = parent.GetComponent<PlayerController>();
            camera = transform.GetComponent<Camera>();
            //vcamera = controller.vcamera.transform.GetComponent<CinemachineVirtualCamera>();
            brain = transform.GetComponent<CinemachineBrain>();

            transform.rotation = transform.parent.rotation;
            transform.localPosition = new Vector3(offsetX, offsetY, offsetZ);
            transform.localEulerAngles = new Vector3(rotateX, rotateY, rotateZ);

            //vcamera.Follow = parent.transform;
            //vcamera.LookAt = parent.transform;
        }

        // Update is called once per frame
        void Update()
        {

            var rotX = Input.GetAxis("Mouse X");
            var rotY = Input.GetAxis("Mouse Y");
            
            CameraRotate(rotX, rotY);

            //vcamera.enabled = controller.isADS;
            brain.enabled = controller.isADS;
        }

        public void CameraRotate(float x,float y)
        {
            transform.transform.Rotate(Vector3.left * y * controller.mouseSensitivityY);

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
            var img = image.GetComponent<Image>();
            if (b)
            {
                SetFOV(25f);
                transform.localPosition = new Vector3(0f,2f, 2f);
                img.sprite = reticle[1];
                image.transform.localScale = new Vector3(32.5f,19.5f,0);
                img.color = SetAlpha(img.color,0.2f);
            }
            else
            {
                SetFOV(60f);
                transform.SetParent(parent.transform);
                CameraInit();
                img.sprite = reticle[0];
                image.transform.localScale = new Vector3(1, 1, 0);
                img.color = SetAlpha(img.color, 1);
            }

        }

        Color SetAlpha(Color color, float value)
        {
            color.a = value;
            return color;
        }
    }
}
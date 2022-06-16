using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{

    public class CameraRotate : NetworkBehaviour
    {

        [SerializeField] private float mouseSensitivityY = 1;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!isLocalPlayer) return;

            Rotate(Input.GetAxis("Mouse Y"));
        }

        [Command]
        void Rotate(float y)
        {
            transform.Rotate(Vector3.left * y * mouseSensitivityY);
        }

    }
}
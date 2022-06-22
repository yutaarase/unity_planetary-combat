using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    public class FlyEffectManager : NetworkBehaviour
    {
        [SerializeField] private GameObject effect;

        [SerializeField] private int effectNum = 2;

        [SerializeField] private GameObject[] flyeffect = new GameObject[2];

        [SerializeField] private Vector3 [] effectPosition = {new Vector3(0.3f,1.05f,-0.3f),new Vector3(-0.3f,1.05f,-0.3f)};

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            for (int i = 0; i < effectNum; ++i)
            {
                flyeffect[i] = Instantiate(effect);
                flyeffect[i].transform.SetParent(transform);
                flyeffect[i].transform.position = effectPosition[i];
                flyeffect[i].transform.Rotate(new Vector3(180f, 0f, 0f));
            }

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
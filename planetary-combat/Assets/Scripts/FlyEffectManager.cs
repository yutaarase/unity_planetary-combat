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

        [SerializeField] private Vector3[] effectPosition = { new Vector3(0.3f, 1.05f, -0.3f), new Vector3(-0.3f, 1.05f, -0.3f) };

        private AnimationManager manager;

        // Start is called before the first frame update
        void Start()
        {
            manager = GetComponent<AnimationManager>();
        }


        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            for (int i = 0; i < effectNum; ++i)
            {
                flyeffect[i] = Instantiate(effect);
                flyeffect[i].transform.SetParent(transform);
                flyeffect[i].transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                flyeffect[i].transform.localPosition = effectPosition[i];
                flyeffect[i].transform.Rotate(new Vector3(180f, 0f, 0f));
            }

        }


        
        // Update is called once per frame
        void Update()
        {
            if (!isLocalPlayer) return;
            FlyMotion(manager.actionID == AnimationManager.ActionID.Fly);
            
        }

        [Command]
        void FlyMotion(bool isFly)
        {
            for (int i = 0; i < effectNum; ++i)
            {
                flyeffect[i].SetActive(isFly);
            }
        }
    }
}
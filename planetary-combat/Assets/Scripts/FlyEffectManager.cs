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

        [SyncVar(hook = nameof(FlyMotion))] private bool isFly = false;

        private Player player;

        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<Player>();
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
            if (isServer) ServerTask();
            if (!isLocalPlayer) return;
        }

        [Server]
        void ServerTask()
        {
            if (player.actionID == Player.ActionID.Fly) isFly = true;
            else isFly = false;
        }

        private void FlyMotion(bool OldBool, bool NewBool)
        {
            for (int i = 0; i < effectNum; ++i)
            {
                flyeffect[i].SetActive(isFly);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    public class FlyEffectManager : NetworkBehaviour
    {
        [SerializeField] private GameObject[] flyeffect = new GameObject[2];

        [Command]
        public void CmdOnActive(bool b)
        {
            RpcOnActive(b);
        }

        [ClientRpc]
        private void RpcOnActive(bool b)
        {
            for (int i = 0; i < flyeffect.Length; ++i)
            {
                if (flyeffect[i] == null) return;
                flyeffect[i].SetActive(b);
            }
        }
    }
}
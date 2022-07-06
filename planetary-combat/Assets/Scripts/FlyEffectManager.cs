using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    public class FlyEffectManager : NetworkBehaviour
    {
        [SerializeField] private GameObject[] flyeffect = new GameObject[2];

        [Command]
        public void EffectActive(bool b)
        {
            setActive(b);
        }

        [ClientRpc]
        private void setActive(bool b)
        {
            for (int i = 0; i < flyeffect.Length; ++i)
            {
                if (flyeffect[i] == null) return;
                flyeffect[i].SetActive(b);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    public class Particle : NetworkBehaviour
    {
        public GameObject[] particle;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ParticlesPlay(bool isPlay)
        {
            for (int i = 0; i < particle.Length; ++i)
            {
                if (isPlay) particle[i].GetComponent<ParticleSystem>().Play();
                else particle[i].GetComponent<ParticleSystem>().Stop();
            }
        }

    }
}
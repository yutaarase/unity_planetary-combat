using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    public class AnimationManager : NetworkBehaviour
    {
        [SerializeField] private GunID gun;

        public Shot shot;

        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            animator.SetFloat("GunID",(float)gun);
        }

        // Update is called once per frame
        void Update()
        {

        }

        [Command]
        public void Move(float x, float y)
        {
            animator.SetFloat("Blend_x", x);
            animator.SetFloat("Blend_y", y);
            animator.SetFloat("Move",Mathf.Abs(x)+ Mathf.Abs(y)/2);
        }

        [Server]
        public void Fire(Shot shot)
        {
            animator.SetFloat("Fire", (int)shot);
        }

        [Command]
        public void Action(Player.ActionID id)
        {
            animator.SetInteger("ActionID", (int)id);
        }

        [Server]
        public void Fly(float p)
        {
            animator.SetFloat("Fly",p);
        }

        public enum GunID
        {
            Burst,
            Single
        }

        public enum Shot
        {
            Cease,
            Fire
        }
    }
}
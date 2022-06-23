using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    public class AnimationManager : NetworkBehaviour
    {
        [SerializeField] private GunID gun;

        [SyncVar] public ActionID actionID;

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


        public void Move(float x, float y)
        {
            animator.SetFloat("Blend_x", x);
            animator.SetFloat("Blend_y", y);
            animator.SetFloat("Move",Mathf.Abs(x)+ Mathf.Abs(y)/2);
        }

        public void Fire(Shot shot)
        {
            animator.SetFloat("Fire", (int)shot);
        }

        public void Action(ActionID id)
        {
            animator.SetInteger("ActionID", (int)id);
            actionID = id;
        }

        public void Fly(float p)
        {
            animator.SetFloat("Fly",p);
        }

        public enum ActionID
        {
            Idle,
            Walk,
            Dush,
            Jump,
            Fly,
            Die
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
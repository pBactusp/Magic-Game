using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class AnimationManager : MonoBehaviour
    {
        public float AnimationDampTime;

        public Animator Animator { get; private set; }

        // Parameter hashes
        private int RunningHash;
        private int dieHash;
        private int jumpHash;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            RunningHash = Animator.StringToHash("Running");
            dieHash = Animator.StringToHash("Dead");
            jumpHash = Animator.StringToHash("Jump");
        }


        public void Jump()
        {
            Animator.SetTrigger(jumpHash);
        }


        public void StartRunning()
        {
            Animator.SetBool(RunningHash, true);
        }
        public void StopRunning()
        {
            Animator.SetBool(RunningHash, false);
        }




        public void Die()
        {
            Animator.SetTrigger(dieHash);
        }
    }


}
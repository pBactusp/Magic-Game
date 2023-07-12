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
        private int movementAxisX;
        private int movementAxisY;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            RunningHash = Animator.StringToHash("Running");
            dieHash = Animator.StringToHash("Dead");
            jumpHash = Animator.StringToHash("Jump");
            movementAxisX = Animator.StringToHash("XAxisMovement");
            movementAxisY = Animator.StringToHash("YAxisMovement");
        }





        public void Jump()
        {
            Animator.SetTrigger(jumpHash);
        }

        public void SetRunning(bool running)
        {
            Animator.SetBool(RunningHash, running);
        }



        public void SetMovementDirection(Vector2 direction)
        {
            Animator.SetFloat(movementAxisX, direction.y);
            Animator.SetFloat(movementAxisY, direction.x);
        }

        public void Die()
        {
            Animator.SetTrigger(dieHash);
        }
    }


}
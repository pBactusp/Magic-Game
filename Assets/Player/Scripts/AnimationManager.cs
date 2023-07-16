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
        private int sprintingHash;
        private int dieHash;
        private int jumpHash;
        private int fallHash;
        private int movementAxisX;
        private int movementAxisY;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            sprintingHash = Animator.StringToHash("Sprinting");
            dieHash = Animator.StringToHash("Dead");
            jumpHash = Animator.StringToHash("Jump");
            fallHash = Animator.StringToHash("Falling");
            movementAxisX = Animator.StringToHash("XAxisMovement");
            movementAxisY = Animator.StringToHash("YAxisMovement");
        }





        public void Jump()
        {
            Animator.SetTrigger(jumpHash);
        }
        public void Fall(bool falling)
        {
            Animator.SetBool(fallHash, falling);
        }

        public void SetSprinting(bool sprinting)
        {
            Animator.SetBool(sprintingHash, sprinting);
        }



        public void SetMovementDirection(Vector2 direction)
        {
            Animator.SetFloat(movementAxisX, direction.x, AnimationDampTime, Time.deltaTime);
            Animator.SetFloat(movementAxisY, direction.y, AnimationDampTime, Time.deltaTime);
        }

        public void Die()
        {
            Animator.SetTrigger(dieHash);
        }
    }


}
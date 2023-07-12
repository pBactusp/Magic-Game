using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Animator))]
    public class AnimationManager : MonoBehaviour
    {
        public float AnimationDampTime;

        public Animator Animator { get; private set; }

        private int movementHash;
        private int runningHash;
        private int attackHash;


        private void Awake()
        {
            Animator = GetComponent<Animator>();
            movementHash = Animator.StringToHash("MovementSpeed");
            runningHash = Animator.StringToHash("Running");
            attackHash = Animator.StringToHash("Attack");
        }


        public void SetMovementSpeed(Vector3 velocity)
        {
            float speed = transform.InverseTransformDirection(velocity).z;

            if (speed > 0)
                speed = 1;
            else if (speed < 0)
                speed = -1;

            Animator.SetFloat(movementHash, speed, AnimationDampTime, Time.deltaTime);
        }
        public void StopMoving()
        {
            Animator.SetFloat(movementHash, 0);
        }

        public void Attack()
        {
            Animator.SetTrigger(attackHash);
        }

    }


}
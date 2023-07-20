using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class AnimationManager : MonoBehaviour
    {
        public float AnimationDampTime;

        public Action OnSpawnSpell;
        public Action OnLaunchSpell;

        public Animator Animator { get; private set; }

        // Parameter hashes
        private int sprintingHash;
        private int dieHash;
        private int jumpHash;
        private int fallHash;
        private int movementAxisXHash;
        private int movementAxisYHash;
        private int movementSpeedHash;
        private int castingHash;
        private int castingSpeedHash;

        // Layer Indices
        private int actionsLayer;



        private void Awake()
        {
            // Parameters
            Animator = GetComponent<Animator>();
            sprintingHash = Animator.StringToHash("Sprinting");
            dieHash = Animator.StringToHash("Dead");
            jumpHash = Animator.StringToHash("Jump");
            fallHash = Animator.StringToHash("Falling");
            movementAxisXHash = Animator.StringToHash("XAxisMovement");
            movementAxisYHash = Animator.StringToHash("YAxisMovement");
            movementSpeedHash = Animator.StringToHash("MovementSpeed");
            castingHash = Animator.StringToHash("Cast");
            castingSpeedHash = Animator.StringToHash("CastingSpeed");
            

            // Layers
            actionsLayer = Animator.GetLayerIndex("Actions");
        }



        public void SetOverride(AnimatorOverrideController controller)
        {
            Animator.runtimeAnimatorController = controller;
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
            Animator.SetFloat(movementAxisXHash, direction.x, AnimationDampTime, Time.deltaTime);
            Animator.SetFloat(movementAxisYHash, direction.y, AnimationDampTime, Time.deltaTime);
        }
        public void SetMovementSpeed(float speed)
        {
            Animator.SetFloat(movementSpeedHash, speed);
        }


        public void CastSpell(float castingTime)
        {
            Animator.SetFloat(castingSpeedHash, 1f / castingTime);
            Animator.SetTrigger(castingHash);
        }

        public void Die()
        {
            Animator.SetTrigger(dieHash);
        }



        // For animation events
        public void _SpawnSpell()
        {
            OnSpawnSpell?.Invoke();
        }
        public void _LaunchSpell()
        {
            OnLaunchSpell?.Invoke();
        }
    }


}
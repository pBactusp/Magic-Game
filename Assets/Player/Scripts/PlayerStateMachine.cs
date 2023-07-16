// Ignore Spelling: Nav

using System;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    [RequireComponent(typeof(InputListener))]
    [RequireComponent(typeof(AnimationManager))]
    [RequireComponent(typeof(HealthManager))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerStateMachine : StateMachine
    {

        [field: Header("Handling")]
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float LookRotationDampFactor { get; private set; }
        [field: SerializeField] public float WalkSpeed { get; private set; }
        [field: SerializeField] public float SprintSpeed { get; private set; }


        [Header("Other")]
        public Transform TargetForEnemies;
        public Vector3 Velocity;


        public bool IsAlive { get; private set; }
        public GameManager GameManager { get; private set; }
        public InputListener Input { get; private set; }
        public AnimationManager Animator { get; private set; }
        public HealthManager HealthManager { get; private set; }
        public CharacterController Controller { get; private set; }




        private void Awake()
        {
            GameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            Input = GetComponent<InputListener>();
            Animator = GetComponent<AnimationManager>();
            HealthManager = GetComponent<HealthManager>();
            Controller = GetComponent<CharacterController>();
        }


        void Start()
        {
            IsAlive = true;

            HealthManager.OnDamaged += OnDamaged;
            HealthManager.OnHealed+= OnHealed;
            HealthManager.OnDeath += Die;

            SwitchState(new PlayerMoveState(this));
        }

        new private void Update()
        {
            // Code goes here
            base.Update();
        }



        void OnDamaged()
        {

        }

        void OnHealed()
        {

        }

        void Die()
        {
            if (!IsAlive)
                return;

            IsAlive = false;
            Debug.Log("Player Died");
            SwitchState(new PlayerDeathState(this));
        }

    }


}
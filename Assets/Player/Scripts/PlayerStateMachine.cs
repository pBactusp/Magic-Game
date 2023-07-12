// Ignore Spelling: Nav

using System;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    [RequireComponent(typeof(AnimationManager))]
    [RequireComponent(typeof(HealthManager))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerStateMachine : StateMachine
    {
        [Header("Attack")]
        public float RotationSpeedWhileAttacking;

        [Header("Performance")]
        public float NavMeshAgentUpdateTime;

        [Header("Other")]
        public Transform TargetForEnemies;

        public bool IsAlive { get; private set; }
        public GameManager GameManager { get; private set; }
        public AnimationManager Animator { get; private set; }
        public HealthManager HealthManager { get; private set; }
        public NavMeshAgent Agent { get; private set; }



        [HideInInspector] public float timeSinceLastAttack;



        private void Awake()
        {
            GameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            Animator = GetComponent<AnimationManager>();
            HealthManager = GetComponent<HealthManager>();
            Agent = GetComponent<NavMeshAgent>();
        }


        void Start()
        {
            IsAlive = true;
            timeSinceLastAttack = 0;

            HealthManager.OnDamaged += OnDamaged;
            HealthManager.OnHealed+= OnHealed;
            HealthManager.OnDeath += Die;

            SwitchState(new PlayerIdleState(this));
        }

        new private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [Serializable]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(FOVManager))]
    [RequireComponent(typeof(AnimationManager))]
    [RequireComponent(typeof(HealthManager))]
    public class EnemyStateMachine : StateMachine
    {
        [Header("Movement")]
        public float RotationSpeed;
        public float DistanceFromTarget;

        [Header("When in agro state")]
        public float MinDistanceFromTarget;
        public float MaxDistanceFromTarget;
        public float AttackCooldown;

        [Header("When in searching state")]
        public float TimeToCalmDown;

        [Header("Gizmo")]
        public bool ShowGizmo;

        [Header("Performance")]
        public float NavMeshAgentUpdateTime;

        [Header("Other")]
        public float Radius;
        public Transform EyesPosition;
        public Transform TargetForPlayer;

        public bool IsAlive { get; protected set; }


        // Actions
        public Action OnPlayerEnteredFOV;
        public Action<Vector3, Vector3> OnPlayerLeftFOV;
        public Action OnStartedAttacking;
        public Action OnFinishedAttacking;


        // Time counters
        [HideInInspector] public float timeSinceNavPathUpdate;
        [HideInInspector] public float timeSinceLastAttack;

        public FOVManager FieldOfView { get; private set; }
        public AnimationManager AnimationManager { get; private set; }
        public HealthManager HealthManager { get; private set; }
        public NavMeshAgent Agent { get; protected set; }
        public Rigidbody Rigidbody { get; protected set; }
        public Vector3 SpawnPosition { get; private set; }



        public void Awake()
        {
            FieldOfView = GetComponent<FOVManager>();
            AnimationManager = GetComponent<AnimationManager>();
            HealthManager = GetComponent<HealthManager>();
            Agent = GetComponent<NavMeshAgent>();
            Rigidbody = GetComponent<Rigidbody>();
            FieldOfView.EyesPosition = EyesPosition;
        }

        public void Start()
        {
            IsAlive = true;

            timeSinceNavPathUpdate = 0;
            timeSinceLastAttack = 0;

            SpawnPosition = transform.position;


            HealthManager.OnDamaged += OnDamaged;
            HealthManager.OnHealed += OnHealed;
            HealthManager.OnDeath += Die;

            FieldOfView.OnPlayerEnteredFOV += PlayerEnteredFOV;
            FieldOfView.OnPlayerLeftFOV += PlayerLeftFOV;
        }


        new public void Update()
        {
            timeSinceNavPathUpdate += Time.deltaTime;
            timeSinceLastAttack += Time.deltaTime;
            base.Update();
        }

        protected void PlayerEnteredFOV()
        {
            OnPlayerEnteredFOV?.Invoke();
        }
        protected void PlayerLeftFOV()
        {
            var lastSeenPosition = PlayerData.TargetForEnemies.position;
            var lastSeenVelocity = PlayerData.PlayerObject.GetComponent<Rigidbody>().velocity;

            OnPlayerLeftFOV?.Invoke(lastSeenPosition, lastSeenVelocity);
        }



        protected virtual void OnDamaged()
        {

        }

        protected virtual void OnHealed()
        {

        }

        protected virtual void Die()
        {
            Debug.Log(gameObject.name + " Died");
            IsAlive = false;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            if (!ShowGizmo)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }


}
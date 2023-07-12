using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemies.Goose
{
    public class GooseAgroState : GooseBaseState
    {
        private Transform player;

        public GooseAgroState(GooseStateMachine stateMachine) : base(stateMachine)
        {
            player = PlayerData.TargetForEnemies;
        }

        public override void Enter()
        {
            Debug.Log("Attacking");
            SetDestinationImmediate(transform.position);
            stateMachine.IsHonking = true;
            stateMachine.OnPlayerLeftFOV += OnPlayerLeftFOV;
            stateMachine.OnStartedAttacking += StartAttack;
        }

        public override void Tick()
        {
            MaintainDistanceFromTarget();
            RotateTowards(player.position);

            if (IsFacingPlayer(50f))
                Attack();

            stateMachine.AnimationManager.SetMovementSpeed(stateMachine.Agent.velocity);
        }

        public override void Exit()
        {
            stateMachine.OnPlayerLeftFOV -= OnPlayerLeftFOV;
            stateMachine.OnStartedAttacking -= StartAttack;
        }



        void MaintainDistanceFromTarget()
        {
            float minDistSqrd = stateMachine.MinDistanceFromTarget * stateMachine.MinDistanceFromTarget;
            float distSqrd = (player.position - transform.position).sqrMagnitude;

            if (distSqrd < minDistSqrd)
            {
                stateMachine.SwitchState(new GooseMaintainDistanceFromTargetState(stateMachine));
                return;
            }

            float maxDistSqrd = stateMachine.MaxDistanceFromTarget * stateMachine.MaxDistanceFromTarget;
            if (distSqrd > maxDistSqrd)
            {
                stateMachine.SwitchState(new GooseMoveTowardsTargetState(stateMachine));
                return;
            }
        }


        void OnPlayerLeftFOV(Vector3 lastSeenPosition, Vector3 lastSeenForward)
        {
            stateMachine.SwitchState(new GooseSearchingForTargetState(stateMachine, lastSeenPosition, lastSeenForward));
        }
        void StartAttack()
        {
            stateMachine.SwitchState(new GooseBeakAttackState(stateMachine));
        }
    }

    public class GooseMaintainDistanceFromTargetState : GooseBaseState
    {
        private Transform player;

        public GooseMaintainDistanceFromTargetState(GooseStateMachine stateMachine) : base(stateMachine)
        {
            player = PlayerData.TargetForEnemies;
        }

        public override void Enter()
        {
            stateMachine.Agent.updateRotation = false;
            stateMachine.OnPlayerLeftFOV += OnPlayerLeftFOV;
        }

        public override void Tick()
        {
            float minDistSqrd = stateMachine.MinDistanceFromTarget * stateMachine.MinDistanceFromTarget;
            float distSqrd = (player.position - transform.position).sqrMagnitude;

            if (distSqrd < minDistSqrd)
            {
                MoveBackward();
                RotateTowards(player.position);

                stateMachine.AnimationManager.SetMovementSpeed(stateMachine.Agent.velocity);

                return;
            }

            stateMachine.SwitchState(new GooseAgroState(stateMachine));
        }

        public override void Exit()
        {
            stateMachine.Agent.updateRotation = true;
            stateMachine.OnPlayerLeftFOV -= OnPlayerLeftFOV;
        }

        void OnPlayerLeftFOV(Vector3 lastSeenPosition, Vector3 lastSeenForward)
        {
            stateMachine.SwitchState(new GooseSearchingForTargetState(stateMachine, lastSeenPosition, lastSeenForward));
        }
    }
    public class GooseMoveTowardsTargetState : GooseBaseState
    {
        private Transform player;

        public GooseMoveTowardsTargetState(GooseStateMachine stateMachine) : base(stateMachine)
        {
            player = PlayerData.TargetForEnemies;
        }

        public override void Enter()
        {
            stateMachine.OnPlayerLeftFOV += OnPlayerLeftFOV;
        }

        public override void Tick()
        {
            float maxDistSqrd = stateMachine.MaxDistanceFromTarget * stateMachine.MaxDistanceFromTarget;
            float distSqrd = (player.position - transform.position).sqrMagnitude;

            if (distSqrd > maxDistSqrd)
            {
                stateMachine.AnimationManager.SetMovementSpeed(stateMachine.Agent.velocity);
                SetDestination(player.position);
                return;
            }

            stateMachine.SwitchState(new GooseAgroState(stateMachine));
        }

        public override void Exit()
        {
            stateMachine.OnPlayerLeftFOV -= OnPlayerLeftFOV;
        }

        void OnPlayerLeftFOV(Vector3 lastSeenPosition, Vector3 lastSeenForward)
        {
            stateMachine.SwitchState(new GooseSearchingForTargetState(stateMachine, lastSeenPosition, lastSeenForward));
        }
    }

    public class GooseBeakAttackState : GooseBaseState
    {
        private float previousMovementSpeed;
        private Vector3 lastSeenPosition;
        private Vector3 lastSeenForward;

        public GooseBeakAttackState(GooseStateMachine stateMachine) : base(stateMachine) { }


        public override void Enter()
        {
            Debug.Log("Executing Attack");

            stateMachine.IsHonking = false;
            previousMovementSpeed = stateMachine.Agent.speed;
            stateMachine.Agent.speed = 0f;
            stateMachine.AnimationManager.StopMoving();
            stateMachine.AnimationManager.Attack();

            stateMachine.Beak.SetActive(true);

            stateMachine.Beak.OnHit += OnHit;
            stateMachine.OnFinishedAttacking += OnFinishedAttacking;
            stateMachine.OnPlayerLeftFOV += OnPlayerLeftFOV;
        }

        public override void Tick()
        {
            RotateTowards(PlayerData.TargetForEnemies.position);
            //stateMachine.SetAnimatorMovementSpeed();
        }

        public override void Exit()
        {
            stateMachine.Beak.SetActive(false);

            stateMachine.Agent.speed = previousMovementSpeed;

            stateMachine.Beak.OnHit -= OnHit;
            stateMachine.OnFinishedAttacking -= OnFinishedAttacking;
            stateMachine.OnPlayerLeftFOV -= OnPlayerLeftFOV;
        }

        void OnFinishedAttacking()
        {
            if (stateMachine.FieldOfView.CanSeePlayer)
            {
                stateMachine.SwitchState(new GooseAgroState(stateMachine));
                return;
            }

            stateMachine.SwitchState(new GooseSearchingForTargetState(stateMachine, lastSeenPosition, lastSeenForward));
        }

        void OnHit(HealthManager target, DamageData damage)
        {
            stateMachine.Beak.SetActive(false);
            target.RecieveDamage(damage);
        }

        void OnPlayerLeftFOV(Vector3 lastSeenPosition, Vector3 lastSeenForward)
        {
            this.lastSeenPosition = lastSeenPosition;
            this.lastSeenForward = lastSeenForward;
        }
    }


    public class GooseSearchingForTargetState : GooseBaseState
    {
        private float elapsedSearchTime;
        private Vector3 lastSeenPosition;
        private Vector3 lastSeenVelocity;

        private bool movingTowardsLastSeenPosition;
        private bool movingForward;

        public GooseSearchingForTargetState(GooseStateMachine stateMachine, Vector3 lastSeenPosition, Vector3 lastSeenVelocity) : base(stateMachine)
        {
            this.lastSeenPosition = lastSeenPosition;
            this.lastSeenVelocity = new Vector3(lastSeenVelocity.x, 0f, lastSeenVelocity.z).normalized;
        }


        public override void Enter()
        {
            Debug.Log("Searching");

            stateMachine.IsHonking = false;
            elapsedSearchTime = 0;
            movingTowardsLastSeenPosition = true;
            movingForward = false;

            SetDestinationImmediate(lastSeenPosition);

            stateMachine.OnPlayerEnteredFOV += OnPlayerEnteredFOV;
            //Debug.DrawRay(lastSeenPosition, lastSeenVelocity, Color.black, 5f);
        }
        public override void Tick()
        {
            if (movingTowardsLastSeenPosition)
            {
                if (NavMeshAgentReachedDestination())
                {
                    movingTowardsLastSeenPosition = false;
                    movingForward = true;
                }
            }
            if (movingForward)
            {
                WanderTowardsGeneralDirection(lastSeenVelocity.normalized);
            }



            if (elapsedSearchTime >= stateMachine.TimeToCalmDown)
            {
                stateMachine.SwitchState(new GooseReturnToSpawnState(stateMachine));
            }

            stateMachine.AnimationManager.SetMovementSpeed(stateMachine.Agent.velocity);

            elapsedSearchTime += Time.deltaTime;
        }

        public override void Exit()
        {
            stateMachine.OnPlayerEnteredFOV -= OnPlayerEnteredFOV;
        }

        void OnPlayerEnteredFOV()
        {
            stateMachine.SwitchState(new GooseAgroState(stateMachine));
        }
    }


    public class GooseIdleState : GooseBaseState
    {
        public GooseIdleState(GooseStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Idle");

            stateMachine.AnimationManager.StopMoving();
            stateMachine.OnPlayerEnteredFOV += OnPlayerEnteredFOV;
        }
        public override void Tick() { }

        public override void Exit()
        {
            stateMachine.OnPlayerEnteredFOV -= OnPlayerEnteredFOV;
        }


        void OnPlayerEnteredFOV()
        {
            stateMachine.SwitchState(new GooseAgroState(stateMachine));
        }


    }

    public class GooseReturnToSpawnState : GooseBaseState
    {
        public GooseReturnToSpawnState(GooseStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Returning To Spawn");

            stateMachine.OnPlayerEnteredFOV += OnPlayerEnteredFOV;
        }
        public override void Tick()
        {
            GoBackToSpawn();
            stateMachine.AnimationManager.SetMovementSpeed(stateMachine.Agent.velocity);

            if (NavMeshAgentReachedDestination())
                stateMachine.SwitchState(new GooseIdleState(stateMachine));
        }

        public override void Exit()
        {
            stateMachine.OnPlayerEnteredFOV -= OnPlayerEnteredFOV;
        }


        void OnPlayerEnteredFOV()
        {
            stateMachine.SwitchState(new GooseAgroState(stateMachine));
        }


    }

    public class GooseRagdollState : GooseBaseState
    {
        private Vector3 collisionForce;

        public GooseRagdollState(GooseStateMachine stateMachine, Vector3 collisionForce) : base(stateMachine)
        {
            this.collisionForce = collisionForce;

            if (this.collisionForce.y < 0)
                this.collisionForce.Scale(new Vector3(1, 0, 1));
        }

        public override void Enter()
        {
            Debug.Log("Ragdoll");

            stateMachine.RagdollManager.SetRagdoll(true);

            stateMachine.RagdollManager.Addforce(collisionForce, ForceMode.Impulse);
            //stateMachine.Rigidbody.AddForce()
        }
        public override void Tick() { }

        public override void Exit()
        {
            stateMachine.RagdollManager.SetRagdoll(false);
        }


    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{

    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine stateMachine;
        protected Transform transform;

        protected EnemyBaseState(EnemyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            transform = stateMachine.transform;
        }


        protected void MoveForward()
        {
            stateMachine.Agent.Move(transform.forward * Time.deltaTime);
            //SetDestination(transform.position + transform.forward * 50f);
        }

        protected void MoveBackward()
        {
            float minRetreatDistance = 1f;
            float maxRetreatDistance = 5f;

            Vector3 direction = -transform.forward;

            if (MoveUsingRaycast(direction, minRetreatDistance, maxRetreatDistance))
                return;

            // Fan out
            for (int i = 1; i < 10; i++)
            {
                // Try first direction
                Vector3 newDir = Quaternion.AngleAxis(10 * i, transform.up) * direction;

                if (MoveUsingRaycast(newDir, minRetreatDistance, maxRetreatDistance))
                    return;

                // Try second direction
                newDir = Quaternion.AngleAxis(-10 * i, transform.up) * direction;

                if (MoveUsingRaycast(newDir, minRetreatDistance, maxRetreatDistance))
                    return;
            }
        }


        protected void WanderTowardsGeneralDirection(Vector3 direction)
        {
            float minCollisiontDistance = 3f;
            float maxStepDistance = 5f;


            if (MoveUsingRaycast(direction, minCollisiontDistance, maxStepDistance))
                return;

            // Fan out first direction
            for (int i = 1; i < 10; i++)
            {
                // Try first direction
                Vector3 newDir = Quaternion.AngleAxis(10 * i, transform.up) * direction;

                if (MoveUsingRaycast(newDir, minCollisiontDistance, maxStepDistance))
                    return;
            }
            for (int i = 1; i < 10; i++)
            {
                Vector3 newDir = Quaternion.AngleAxis(-10 * i, transform.up) * direction;

                if (MoveUsingRaycast(newDir, minCollisiontDistance, maxStepDistance))
                    return;
            }
        }

        private bool MoveUsingRaycast(Vector3 direction, float minDistance, float maxDistance)
        {
            float sqrMinDistance = minDistance * minDistance;
            Ray ray = new Ray(transform.position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, LayerMasks.Obstruction))
            {
                if ((hit.point - transform.position).sqrMagnitude > sqrMinDistance)
                {
                    SetDestination(hit.point);
                    return true;
                }
            }
            else
            {
                SetDestination(transform.position + direction * maxDistance);
                return true;
            }

            return false;
        }

        protected void SetDestination(Vector3 target)
        {
            if (stateMachine.timeSinceNavPathUpdate >= stateMachine.NavMeshAgentUpdateTime)
            {
                stateMachine.Agent.SetDestination(target);
                stateMachine.timeSinceNavPathUpdate = 0;
            }
        }
        protected void SetDestinationImmediate(Vector3 target)
        {
            stateMachine.Agent.SetDestination(target);
        }

        protected void RotateTowards(Vector3 target)
        {
            Vector3 targetVec = (target - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(targetVec, transform.up);
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, stateMachine.RotationSpeed * Time.deltaTime);

            transform.eulerAngles = new Vector3(0f, targetRotation.eulerAngles.y, 0f);
        }

        protected void GoBackToSpawn()
        {
            SetDestination(stateMachine.SpawnPosition);

            //var direction = stateMachine.SpawnPosition - transform.position;
            //// Return if is in span position
            //if (Vector3.SqrMagnitude(direction) < 5f)
            //    return;

            //direction = Vector3.Scale(direction.normalized, new Vector3(1f, 0f, 1f));

            //// If not facing spawn point
            //if (Vector3.Dot(direction, transform.forward) < 0.95f)
            //{
            //    RotateTowards(stateMachine.SpawnPosition);
            //}
            //else
            //{
            //    CalculateMoveForward();
            //    Move();
            //}
        }

        //protected void MaintainDistanceFromTarget(Vector3 target)
        //{
        //    float minDistSqrd = stateMachine.MinDistanceFromTarget * stateMachine.MinDistanceFromTarget;
        //    float distSqrd = (target - transform.position).sqrMagnitude;

        //    if (distSqrd < minDistSqrd)
        //    {
        //        MoveBackward();
        //        return;
        //    }

        //    float maxDistSqrd = stateMachine.MaxDistanceFromTarget * stateMachine.MaxDistanceFromTarget;
        //    if (distSqrd > maxDistSqrd)
        //    {
        //        SetDestination(target);
        //        return;
        //    }

        //    SetDestination(transform.position);
        //}

        protected void Stop()
        {
            stateMachine.Agent.isStopped = true; ;
        }

        protected bool NavMeshAgentReachedDestination()
        {
            float dist = stateMachine.Agent.remainingDistance;
            return dist != Mathf.Infinity && stateMachine.Agent.pathStatus == NavMeshPathStatus.PathComplete && stateMachine.Agent.remainingDistance == 0;
        }

        protected bool IsFacingDirection(Vector3 direction, float maxAngle = 10)
        {
            var a = Vector3.SignedAngle(transform.forward, direction, transform.up);
            return Mathf.Abs(Vector3.SignedAngle(transform.forward, direction, transform.up)) <= maxAngle;
        }
        protected bool IsFacingPlayer(float MaxAngle = 10)
        {
            Vector3 direction = (PlayerData.TargetForEnemies.position - transform.position).normalized;
            return IsFacingDirection(direction, MaxAngle);
        }

        protected void Attack()
        {
            if (stateMachine.timeSinceLastAttack >= stateMachine.AttackCooldown)
            {
                stateMachine.timeSinceLastAttack = 0;
                stateMachine.OnStartedAttacking?.Invoke();
            }
        }
    }

}
// Ignore Spelling: Nav

using UnityEngine;
using UnityEngine.AI;

namespace Player
{

    public abstract class PlayerBaseState : State
    {
        protected readonly PlayerStateMachine player;
        protected readonly Transform transform;

        protected PlayerBaseState(PlayerStateMachine stateMachine)
        {
            player = stateMachine;
            transform = player.transform;
        }



        protected void RotateTowards(Vector3 target)
        {
            Vector3 targetVec = (target - player.transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(targetVec, player.transform.up);
            targetRotation = Quaternion.RotateTowards(player.transform.rotation, targetRotation, player.RotationSpeedWhileAttacking * Time.deltaTime);

            player.transform.eulerAngles = new Vector3(0f, targetRotation.eulerAngles.y, 0f);
        }

        
    }

}
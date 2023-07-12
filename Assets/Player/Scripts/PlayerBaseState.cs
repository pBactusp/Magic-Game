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

        protected void CalculateMoveDirection()
        {
            var camera = GameManager.Instance.MainCamera.transform;
            var moveComposite = player.Input.MoveComposite;

            Vector3 cameraForward = new(camera.forward.x, 0, camera.forward.z);
            Vector3 cameraRight = new(camera.right.x, 0, camera.right.z);

            Vector3 moveDirection = cameraForward.normalized * moveComposite.y + cameraRight.normalized * moveComposite.x;

            player.Velocity.x = moveDirection.x * player.MovementSpeed;
            player.Velocity.z = moveDirection.z * player.MovementSpeed;
        }

        //protected void FaceMoveDirection()
        //{
        //    Vector3 faceDirection = new(player.Velocity.x, 0f, player.Velocity.z);

        //    if (faceDirection == Vector3.zero)
        //        return;

        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(faceDirection), player.LookRotationDampFactor * Time.deltaTime);
        //}

        protected void ApplyGravity()
        {
            if (player.Velocity.y > Physics.gravity.y)
            {
                player.Velocity.y += Physics.gravity.y * Time.deltaTime;
            }
        }

        protected void Move()
        {
            player.Controller.Move(player.Velocity * Time.deltaTime);
        }

        //protected void RotateTowards(Vector3 target)
        //{
        //    Vector3 targetVec = (target - player.transform.position).normalized;

        //    Quaternion targetRotation = Quaternion.LookRotation(targetVec, player.transform.up);
        //    targetRotation = Quaternion.RotateTowards(player.transform.rotation, targetRotation, player.RotationSpeedWhileAttacking * Time.deltaTime);

        //    player.transform.eulerAngles = new Vector3(0f, targetRotation.eulerAngles.y, 0f);
        //}


    }

}
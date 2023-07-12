// Ignore Spelling: Nav

using System;
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



        protected void SetWalkingAnimationAnimationDirections_InTesting()
        {

            float moveY = player.Velocity.z / player.MovementSpeed;
            float moveX = player.Velocity.x / player.MovementSpeed;

            Debug.Log(moveX + ", " + moveY);
            player.Animator.SetMovementDirection(new Vector2(moveX, moveY));
        }

        protected void SetWalkingAnimationAnimationDirections()
        {
            float moveY = 0;
            float moveX = 0;

            if (player.Input.MoveComposite.y > 0)
                moveY = 1f;
            else if (player.Input.MoveComposite.y < 0)
                moveY = -1f;

            if (player.Input.MoveComposite.x > 0)
                moveX = 1f;
            else if (player.Input.MoveComposite.x < 0)
                moveX = -1f;

            Debug.Log(moveX + ", " + moveY);
            player.Animator.SetMovementDirection(new Vector2(moveX, moveY));
        }

    }

}
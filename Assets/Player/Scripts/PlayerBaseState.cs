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

        protected void CalculateMoveDirection(float movementSpeed)
        {
            var camera = GameManager.Instance.MainCamera.transform;
            var moveComposite = player.Input.MoveComposite;

            var cameraForward = new Vector3(camera.forward.x, 0, camera.forward.z);
            var cameraRight = new Vector3(camera.right.x, 0, camera.right.z);

            Vector3 moveDirection = cameraForward.normalized * moveComposite.y + cameraRight.normalized * moveComposite.x;

            player.Velocity.x = moveDirection.x * movementSpeed;
            player.Velocity.z = moveDirection.z * movementSpeed;
        }

        protected void FaceMoveDirection()
        {
            Vector3 faceDirection = new(player.Velocity.x, 0f, player.Velocity.z);

            if (faceDirection == Vector3.zero)
                return;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(faceDirection), player.LookRotationDampFactor * Time.deltaTime);
        }

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

        protected void HandleMovement(float movementSpeed, bool faceMoveDirection = true)
        {
            CalculateMoveDirection(movementSpeed);
            Move();
            if (faceMoveDirection) FaceMoveDirection();
            SetWalkingAnimationAnimationDirections();
        }

        protected void SetWalkingAnimationAnimationDirections()
        {

            Vector3 velocity = player.Velocity;

            float moveY = Vector3.Dot(velocity, transform.forward);
            float moveX = Vector3.Dot(velocity, transform.right);

            //Debug.Log(moveX + ", " + moveY);
            player.Animator.SetMovementDirection(new Vector2(moveX, moveY));
        }

        protected void RotateTowards(Vector3 target, float turnSpeed)
        {
            var targetRotation = Quaternion.LookRotation(target, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed);
        }
    }

}
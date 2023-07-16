using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class PlayerMoveState : PlayerBaseState
    {
        float currentSpeed;

        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }


        public override void Enter()
        {
            player.Velocity.y = Physics.gravity.y;

            if (player.Input.IsSprinting)
                StartedSprinting();
            else
                StoppedSprinting();

            player.Input.OnJumpPerformed += SwitchToJumpState;
            player.Input.OnStartedSprinting += StartedSprinting;
            player.Input.OnStoppedSprinting += StoppedSprinting;
        }

        public override void Tick()
        {
            if (!player.Controller.isGrounded)
            {
                player.SwitchState(new PlayerFallState(player));
            }

            CalculateMoveDirection(currentSpeed);
            Move();
            FaceMoveDirection();
            SetWalkingAnimationAnimationDirections();
        }

        public override void Exit()
        {
            StoppedSprinting();

            player.Input.OnJumpPerformed -= SwitchToJumpState;
            player.Input.OnStartedSprinting -= StartedSprinting;
            player.Input.OnStoppedSprinting -= StoppedSprinting;
        }


        private void SwitchToJumpState()
        {
            player.SwitchState(new PlayerJumpState(player));
        }

        private void StartedSprinting()
        {
            currentSpeed = player.SprintSpeed;
            player.Animator.SetSprinting(true);
        }
        private void StoppedSprinting()
        {
            currentSpeed = player.WalkSpeed;
            player.Animator.SetSprinting(false);
        }
    }

    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter() { }
        public override void Tick() { }
        public override void Exit() { }
    }


    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            player.Velocity = new Vector3(player.Velocity.x, player.JumpForce, player.Velocity.z);

            player.Animator.Jump();
        }

        public override void Tick()
        {
            ApplyGravity();

            if (player.Velocity.y <= 0f)
            {
                player.SwitchState(new PlayerFallState(player));
            }

            Move();
        }

        public override void Exit() { }
    }

    public class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            player.Velocity.y = 0f;

            player.Animator.Fall(true);
        }

        public override void Tick()
        {
            ApplyGravity();
            Move();

            if (player.Controller.isGrounded)
            {
                player.SwitchState(new PlayerMoveState(player));
            }
        }

        public override void Exit()
        {
            player.Animator.Fall(false);
        }
    }



    public class PlayerDeathState : PlayerBaseState
    {
        public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            player.Animator.Die();
        }

        public override void Tick()
        {

        }

        public override void Exit()
        {

        }
    }
    

}
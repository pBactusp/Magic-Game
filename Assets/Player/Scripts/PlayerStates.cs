using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }


        public override void Enter()
        {
            player.Velocity.y = Physics.gravity.y;

            player.Input.OnJumpPerformed += SwitchToJumpState;
        }

        public override void Tick()
        {
            if (!player.Controller.isGrounded)
            {
                player.SwitchState(new PlayerFallState(player));
            }

            CalculateMoveDirection();
            //FaceMoveDirection();
            Move();
            SetWalkingAnimationAnimationDirections();
        }

        public override void Exit()
        {
            player.Input.OnJumpPerformed -= SwitchToJumpState;
        }


        private void SwitchToJumpState()
        {
            player.SwitchState(new PlayerJumpState(player));
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
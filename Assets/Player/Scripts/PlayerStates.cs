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
            player.Animator.StartRunning();
        }

        public override void Tick() { }

        public override void Exit()
        {
            player.Animator.StopRunning();
        }

    }

    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter() { }
        public override void Tick() { }
        public override void Exit() { }
    }
   

    public class PlayerDeathState : PlayerBaseState
    {
        public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            player.Agent.SetDestination(player.transform.position);
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
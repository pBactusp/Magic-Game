using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Goose
{
    public abstract class GooseBaseState : EnemyBaseState
    {
        protected new readonly GooseStateMachine stateMachine;
        protected GooseBaseState(GooseStateMachine stateMachine) : base(stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    }
}


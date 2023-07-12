using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State currentState;

    public void SwitchState(State state)
    {
        currentState?.Exit();
        currentState = state;
        currentState.Enter();
    }

    protected void Update()
    {
        currentState?.Tick();
    }
}

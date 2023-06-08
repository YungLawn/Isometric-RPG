using UnityEngine;

public class IdleState : MovementState
{
    public override void EnterState(StateManager incomingState)
    {
        // Debug.Log("Entering Idle");
        moveSpeed = 0.0f;
        framerate = 0.125f;
        action = IDLE;
        state = incomingState;
        body = state.body;
        anim = state.animator;
    }

    public override void OnCollisionEnter(StateManager state)
    {

    }
}
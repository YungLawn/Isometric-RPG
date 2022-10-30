using UnityEngine;

public class IdleState : MovementState
{
    public override void EnterState(StateManager incomingState)
    {
        // Debug.Log("Entering Idle");
        moveSpeed = 0.0f;
        idleIntervalMultiplier = 2;
        framerate  = 0.125f;
        action = IDLE;
        state = incomingState;
        body = state.body;
        anim = state.animator;
    }

    public override void UpdateState()
    {
        // Debug.Log("Idling");
        move();
    }

    public override void FixedUpdateState()
    {
        Animate();
    }

    public override void OnCollisionEnter(StateManager state)
    {

    }
}
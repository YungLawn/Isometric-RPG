using UnityEngine;

public class IdleState : MovementState
{
    public override void EnterState(StateManager incomingState)
    {
        moveSpeed = 0.0f;
        // totalFrames = 8;
        // totalFrames = totalFrames * (int)idleIntervalMultiplier;
        framerate  = 0.125f;
        action = IDLE;
        // Debug.Log("Entering Idle");
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
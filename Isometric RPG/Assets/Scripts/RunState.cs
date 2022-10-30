using UnityEngine;

public class RunState : MovementState
{
    public override void EnterState(StateManager incomingState)
    {
        // Debug.Log("Entering Run");
        moveSpeed = 1.0f;
        idleIntervalMultiplier = 1;
        framerate  = 0.125f;
        action = WALK;
        state = incomingState;
        body = state.body;
        anim = state.animator;
    }

    public override void UpdateState()
    {
        // Debug.Log("Running");
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
using UnityEngine;

public class RunState : MovementState
{
    public override void EnterState(StateManager incomingState)
    {
        moveSpeed = 1.0f;
        // totalFrames = 8;
        framerate  = 0.125f;
        action = WALK;
        // Debug.Log("Entering Run");
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
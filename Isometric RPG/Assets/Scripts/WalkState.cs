using UnityEngine;

public class WalkState : MovementState
{
    public override void EnterState(StateManager incomingState)
    {
        moveSpeed = 0.5f;
        // totalFrames = 8;
        framerate  = 0.125f;
        action = WALK;
        // Debug.Log("Entering Walk");
        state = incomingState;
        body = state.body;
        anim = state.animator;
    }

    public override void UpdateState()
    {
        // Debug.Log("Walking");
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
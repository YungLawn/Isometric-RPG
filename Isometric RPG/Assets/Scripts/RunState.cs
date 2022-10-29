using UnityEngine;

public class RunState : MovementState
{
    public override void EnterState(StateManager state)
    {
        moveSpeed = 1.0f;
        totalFrames = 8;
        // Debug.Log("Entering Run");
        body = state.body;
        anim = state.animator;
    }

    public override void UpdateState(StateManager state)
    {
        // Debug.Log("Running");
        move(state, state.direction);
        // Animate(state, WALK);
    }

    public override void FixedUpdateState(StateManager state)
    {
        Animate(state, WALK);
    }

    public override void OnCollisionEnter(StateManager state)
    {

    }
}
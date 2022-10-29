using UnityEngine;

public class WalkState : MovementState
{
    public override void EnterState(StateManager state)
    {
        moveSpeed = 0.5f;
        totalFrames = 8;
        // Debug.Log("Entering Walk");
        body = state.body;
        anim = state.animator;
    }

    public override void UpdateState(StateManager state)
    {
        // Debug.Log("Walking");
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
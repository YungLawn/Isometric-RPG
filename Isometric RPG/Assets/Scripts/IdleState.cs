using UnityEngine;

public class IdleState : MovementState
{
    public override void EnterState(StateManager state)
    {
        moveSpeed = 0.0f;
        totalFrames = 8;
        // Debug.Log("Entering Idle");
        body = state.body;
        anim = state.animator;
    }

    public override void UpdateState(StateManager state)
    {
        // Debug.Log("Idling");
        move(state, state.direction);
        // Animate(state, IDLE);
    }

    public override void FixedUpdateState(StateManager state)
    {
        Animate(state, IDLE);
    }

    public override void OnCollisionEnter(StateManager state)
    {

    }
}
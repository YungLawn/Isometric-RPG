using UnityEngine;

public class IdleState : MovementState
{
    public override void EnterState(StateManager incomingState)
    {
        // Debug.Log("Entering Idle");
        moveSpeed = 0.0f;
        action = IDLE;
        state = incomingState;
        body = state.body;
        anim = state.animator;
        state.direction = lastMoveDirection;
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
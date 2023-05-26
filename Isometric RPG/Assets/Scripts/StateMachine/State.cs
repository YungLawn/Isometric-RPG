using UnityEngine;

public abstract class State
{
    public Rigidbody2D body;
    public Animator anim;
    public StateManager state;

    public abstract void EnterState(StateManager IncomingState);

    public abstract void UpdateState();

    public abstract void FixedUpdateState();

    public abstract void OnCollisionEnter(StateManager state);
}
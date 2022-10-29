using UnityEngine;

public abstract class State
{
    public Rigidbody2D body;
    public Animator anim;
    public int totalFrames;


    public abstract void EnterState(StateManager state);

    public abstract void UpdateState(StateManager state);

    public abstract void FixedUpdateState(StateManager state);

    public abstract void OnCollisionEnter(StateManager state);
}
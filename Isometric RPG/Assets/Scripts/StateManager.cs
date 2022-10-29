using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateManager : MonoBehaviour
{
    public State currentState;
    public IdleState idleState = new IdleState();
    public WalkState walkState = new WalkState();
    public RunState runState = new RunState();

    public Rigidbody2D body;
    public Animator animator;

    private PlayerInput playerInput;
    private InputAction directionalInput;
    private InputAction sprint;
    private InputAction dontsprint;

    public Vector2 direction;
    public bool runToggle;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        directionalInput = playerInput.actions["Move"];
        sprint = playerInput.actions["Sprint"];
        dontsprint = playerInput.actions["dontSprint"];
    }

    void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
    }

    void Update()
    {
        getDirectionalInput();
        isRunning();
        currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SwitchState(State state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void getDirectionalInput()
    {
        direction = directionalInput.ReadValue<Vector2>();
    }

    public void isRunning()
    {   
        if(sprint.triggered)
            runToggle = true;
        else if(dontsprint.triggered)
            runToggle = false;
    }
}
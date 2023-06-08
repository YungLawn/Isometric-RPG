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

    private new Collider2D collider;
    [SerializeField]
    private ContactFilter2D filter;
    private List<Collider2D> collidedObjects = new List<Collider2D>(1);

    private PlayerInput playerInput;
    private InputAction directionalInput;
    private InputAction sprint;
    private InputAction dontSprint;
    private InputAction interact;
    private InputAction dontInteract;

    public Vector2 direction;
    public bool runToggle;
    public bool interactionToggle;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        directionalInput = playerInput.actions["Move"];
        sprint = playerInput.actions["Sprint"];
        dontSprint = playerInput.actions["dontSprint"];
        interact = playerInput.actions["Interact"];
        dontInteract = playerInput.actions["dontInteract"];
    }

    void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
    }

    void Update()
    {
        getDirectionalInput();
        checkCollisions();
        isRunning();
        isInteracting();
        currentState.UpdateState();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState();
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
        else if(dontSprint.triggered)
            runToggle = false;
    }
    public void isInteracting()
    {   
        if(interact.triggered)
            interactionToggle = true;
        else if(dontInteract.triggered)
            interactionToggle = false;
    }

    public void checkCollisions() {
        collider.OverlapCollider(filter, collidedObjects);
        if(interactionToggle) {
            foreach(var o in collidedObjects) {
                Debug.Log(body.position.x + " , " + body.position.y);
            }
        }
    }
}
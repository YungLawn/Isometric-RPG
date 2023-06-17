using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float runMultiplier = 1.5f;
    [SerializeField]
    bool diagonal = false;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Rigidbody2D body;
    private Animator animator;

    const string BASE = "Human_";
    const string WALK = "Walk_";
    const string IDLE =  "Idle_";
    const string RUN = "Run_";
    const string NORTH = "North";
    const string SOUTH = "South";
    const string EAST = "East";
    const string WEST = "West";

    [SerializeField]
    bool isRunning;

    float framerate = 0.125f;
    int totalFrames = 8;
    int idleIntervalMultiplier = 1;
    [SerializeField]
    [Range (1,5)]
    int idleIntervalFloor = 3;
    [Range (1,10)]
    public int idleIntervalCeiling = 7;
    int currentFrame;
    int idleCycleFrame;
    float timer;

    string currentAction = IDLE;
    string currentDirection = SOUTH;
    string currentAnimation = BASE + IDLE + SOUTH;


    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Animate();
    }

    void FixedUpdate() {

        Vector2 diagonalFix = diagonal ? new Vector2(1f,0.5f) : new Vector2(1f,1f);
        float diagonalSpeedFix = diagonal ? 1.5f : 1f;
        // Vector2 diagonalFix = new Vector2(1f,1f);


        if(isRunning)
            body.velocity = (moveInput * diagonalFix) * ((moveSpeed * diagonalSpeedFix) * runMultiplier) * Time.fixedDeltaTime;
        else
            body.velocity = (moveInput * diagonalFix) * (moveSpeed * diagonalSpeedFix) * Time.fixedDeltaTime;

        // Debug.Log(body.velocity);

    }

    void OnMove(InputValue value) {

        moveInput = value.Get<Vector2>();

        if(moveInput.x == 0 && moveInput.y == 0) {
            currentAction = IDLE;
            diagonal = false;
        }
        else{
            currentAction = WALK;

            // if(isRunning) {
            //     currentAction = RUN;
            // }
            // else
            //     currentAction = WALK;
        }
    }

    void OnSprint(){
        isRunning = true;
    }
    void OnDontSprint(){
        isRunning = false;
    }

    void OnLook(InputValue value){
        lookInput = value.Get<Vector2>();
    }

    void Animate() {

        determineDirection();

        currentAnimation = currentAnimation = BASE + currentAction + currentDirection;

        timer += Time.deltaTime;
        if(timer >= framerate)
        {
            timer -= framerate;
            currentFrame = (currentFrame + 1) % totalFrames; //cycling through animation frames
            idleCycleFrame = (idleCycleFrame + 1) % (totalFrames * idleIntervalMultiplier); // cycling through idle interval
        }

        if(idleCycleFrame == 0)
        {
            idleIntervalMultiplier = Random.Range(idleIntervalFloor,idleIntervalCeiling);
        }

        float normalizedTime = currentFrame / (float)(totalFrames + 1f);//calculate percentage of animation based on current frame

        // if idling, restrict animation for X cycles
        if(idleCycleFrame < ((totalFrames * idleIntervalMultiplier) - totalFrames) && currentAction == IDLE)
            animator.PlayInFixedTime(currentAnimation, 0, 0);
        else    //play animation as normal
            animator.PlayInFixedTime(currentAnimation, 0, normalizedTime);

    }

    void determineDirection() {
        if(moveInput.x == 0 && moveInput.y > 0) //North
        {
            currentDirection = NORTH;
            diagonal = false;
        }
        else if(moveInput.x == 0 && moveInput.y < 0) //South
        {
            currentDirection = SOUTH;
            diagonal = false;
        }
        else if(moveInput.x > 0 && moveInput.y == 0) //East
        {
            currentDirection = EAST;
            diagonal = false;
        }
        else if(moveInput.x < 0 && moveInput.y == 0) //West
        {
            currentDirection = WEST;
            diagonal = false;
        }
        else if(moveInput.x > 0 && moveInput.y > 0) //NorthEast
        {
            currentDirection = NORTH + EAST;
            diagonal = true;
        }
        else if(moveInput.x < 0 && moveInput.y > 0) //NorthWest
        {
            currentDirection = NORTH + WEST;
            diagonal = true;
        }
        else if(moveInput.x > 0 && moveInput.y < 0) //SouthEast
        {
            currentDirection = SOUTH + EAST;
            diagonal = true;
        }
        else if(moveInput.x < 0 && moveInput.y < 0) //SouthWest
        {
            currentDirection = SOUTH + WEST;
            diagonal = true;
        }
    }


}

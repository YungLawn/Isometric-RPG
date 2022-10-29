using UnityEngine;

public abstract class MovementState : State
{
    public float moveSpeed;
    Vector2 moveDirection;
    Vector2 lastMoveDirection;
    public string currentAnimation;
    public int currentFrame;
    string action;
    public float idleCount;

    const string BASE = "Human_";
    public const string WALK = "Walk_";
    public const string IDLE =  "Idle_";
    public const string RUN = "Run_";
    const string NORTH = "North";
    const string SOUTH = "South";
    const string EAST = "East";
    const string WEST = "West";

    const float idleIntervalMult = 3;
    
    // public override void EnterState(StateManager state)
    // {
    //     Debug.Log("Entering MovementState");
    // }

    // public override void UpdateState(StateManager state)
    // {
    //     Debug.Log("Moving");
    //     move(state);
    // }

    // public override void OnCollisionEnter(StateManager state)
    // {

    // }

    public void move(StateManager state, Vector2 direction)
    {
        moveDirection = direction;

        if(moveDirection.magnitude > 0 && !state.runToggle)
        {
            state.SwitchState(state.walkState);
        }
        else if(moveDirection.magnitude > 0 && state.runToggle)
        {
            state.SwitchState(state.runState);
        }
        else if(moveDirection.magnitude <= 0)   
        {
            state.SwitchState(state.idleState);
            moveDirection = lastMoveDirection;
        }

        // Debug.Log("CurrentState: " + state.currentState);
        // Debug.Log(currentAnimation);
        Debug.Log("Frame: " + currentFrame + "\nidleCnt: " + idleCount);
        // Debug.Log(idleCount);

        // if(moveDirection.magnitude == 0)
        //     Debug.Log("lastdir: " + lastMoveDirection);
        body.velocity = moveDirection * moveSpeed;

        if((body.velocity.magnitude <= 0.0) && moveDirection.x != 0 || moveDirection.y != 0)
            lastMoveDirection = moveDirection;
    }

    public void Animate(StateManager state, string doing)
    {
        action = doing;
        getAnimation(state);

        float playTime = Time.time;//time since last movement
        int Frames = (int)((playTime) * state.currentState.totalFrames);//total frame since movement began
        // Debug.Log(Frames);
        currentFrame = Frames % state.currentState.totalFrames;//current frame
        // Debug.Log(totalFrames);
        float normalizedTime = currentFrame / (float)(state.currentState.totalFrames) + 0.001f;
        // Debug.Log(normalizedTime);
        
        anim.Play(currentAnimation, 0, normalizedTime);
    }

    void getAnimation(StateManager state)
    {
        string direction = SOUTH;

        if(moveDirection.x == 0 && moveDirection.y > 0) //North
        {
            direction = NORTH;
        }
        else if(moveDirection.x == 0 && moveDirection.y < 0) //South
        {
            direction = SOUTH;
        }
        else if(moveDirection.x > 0 && moveDirection.y == 0) //East
        {
            direction = EAST;
        }
        else if(moveDirection.x < 0 && moveDirection.y == 0) //West
        {
            direction = WEST;
        }
        else if(moveDirection.x > 0 && moveDirection.y > 0) //NorthEast
        {
            direction = NORTH + EAST;
        }
        else if(moveDirection.x < 0 && moveDirection.y > 0) //NorthWest
        {
            direction = NORTH + WEST;
        }
        else if(moveDirection.x > 0 && moveDirection.y < 0) //SouthEast
        {
            direction = SOUTH + EAST;
        }
        else if(moveDirection.x < 0 && moveDirection.y < 0) //SouthWest
        {
            direction = SOUTH + WEST;
        }

        currentAnimation = BASE + action + direction;
    }
    

}
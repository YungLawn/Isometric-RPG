using UnityEngine;

public abstract class MovementState : State
{
    public float moveSpeed;
    public Vector2 lastMoveDirection;
    public Vector2 moveDirection;

    float framerate = 0.125f;
    int totalFrames = 8;
    int idleIntervalMultiplier = 1;
    int currentFrame;
    int idleCycleFrame;
    float timer;

    string currentAnimation;
    public string action;

    const string BASE = "Human_";
    public const string WALK = "Walk_";
    public const string IDLE =  "Idle_";
    public const string RUN = "Run_";
    const string NORTH = "North";
    const string SOUTH = "South";
    const string EAST = "East";
    const string WEST = "West";    

    public void move()
    {
        moveDirection = state.direction;

        if(moveDirection.magnitude == 0) //Idle Condition
        {
            state.SwitchState(state.idleState);
        }
        else if(moveDirection.magnitude > 0 && state.runToggle) //Run Condition
        {
            state.SwitchState(state.runState);
        }
        if(moveDirection.magnitude > 0 && !state.runToggle) //Walk Condition
        {
            state.SwitchState(state.walkState);
        }

        if(state.direction.magnitude != 0)
            lastMoveDirection = state.direction;

        // Debug.Log("Frame: " + currentFrame);
        // Debug.Log(idleIntervalMultiplier);
        Debug.Log(lastMoveDirection);

        body.velocity = moveDirection * moveSpeed; 
    }

    public void Animate()
    {

        if(action == IDLE)
            currentAnimation = BASE + action + translateDirection(lastMoveDirection);
        else
            currentAnimation = BASE + action + translateDirection(moveDirection);

        timer += Time.deltaTime;
        if(timer >= framerate)
        {
            timer -= framerate;
            currentFrame = (currentFrame + 1) % totalFrames; //cycling through animation frames
            idleCycleFrame = (idleCycleFrame + 1) % (totalFrames * idleIntervalMultiplier); // cycling through idle interval
        }

        if(idleCycleFrame == 0)
        {
            idleIntervalMultiplier = Random.Range(3,7);
        }

        float normalizedTime = currentFrame / (float)(totalFrames + 1f);//calculate percentage of animation based on current frame

        //if idling, restrict animation for X cycles
        if(idleCycleFrame < ((totalFrames * idleIntervalMultiplier) - totalFrames) && action == IDLE)
            anim.PlayInFixedTime(currentAnimation, 0, 0);
        else    //play animation as normal
            anim.PlayInFixedTime(currentAnimation, 0, normalizedTime);
    }

    string translateDirection(Vector2 incoming)
    {
        string output = SOUTH;

        if(incoming.x == 0 && incoming.y > 0) //North
        {
            output = NORTH;
        }
        else if(incoming.x == 0 && incoming.y < 0) //South
        {
            output = SOUTH;
        }
        else if(incoming.x > 0 && incoming.y == 0) //East
        {
            output = EAST;
        }
        else if(incoming.x < 0 && incoming.y == 0) //West
        {
            output = WEST;
        }
        else if(incoming.x > 0 && incoming.y > 0) //NorthEast
        {
            output = NORTH + EAST;
        }
        else if(incoming.x < 0 && incoming.y > 0) //NorthWest
        {
            output = NORTH + WEST;
        }
        else if(incoming.x > 0 && incoming.y < 0) //SouthEast
        {
            output = SOUTH + EAST;
        }
        else if(incoming.x < 0 && incoming.y < 0) //SouthWest
        {
            output = SOUTH + WEST;
        }
        // else 
        //     output = SOUTH;

        return output;
    }

    // void getAnimation()
    // {
    //     currentAnimation = BASE + action + translateDirection(state.direction);
    // }
}
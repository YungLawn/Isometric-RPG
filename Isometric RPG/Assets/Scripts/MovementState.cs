using UnityEngine;

public abstract class MovementState : State
{
    public float moveSpeed;
    public Vector2 lastMoveDirection;

    float framerate = 0.125f;
    int totalFrames = 8;
    int idleIntervalMultiplier = 1;
    int currentFrame;
    int idleCycleFrame;
    float timer;

    string currentAnimation;
    public string action;
    string direction;

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
        if(state.direction.magnitude > 0 && !state.runToggle) //Walk Condition
        {
            state.SwitchState(state.walkState);
            lastMoveDirection = state.direction;
        }
        else if(state.direction.magnitude > 0 && state.runToggle) //Run Condition
        {
            state.SwitchState(state.runState);
            lastMoveDirection = state.direction;
        }
        else if(state.direction.magnitude <= 0) //Idle Condition
        {
            state.SwitchState(state.idleState);
        }

        // Debug.Log("Frame: " + currentFrame);
        // Debug.Log(idleIntervalMultiplier);
        Debug.Log(lastMoveDirection);

        body.velocity = state.direction * moveSpeed; 
    }

    public void Animate()
    {
        timer += Time.deltaTime;
        if(timer >= framerate)
        {
            timer -= framerate;
            currentFrame = (currentFrame + 1) % totalFrames; //cycling through animation frames
            idleCycleFrame = (idleCycleFrame + 1) % (totalFrames * idleIntervalMultiplier); // cycling through idle interval
        }

        float normalizedTime = currentFrame / (float)(totalFrames + 1f);//calculate percentage of animation based on current frame

        getAnimation(); //determine current movement

        if(idleCycleFrame == 0)
        {
            idleIntervalMultiplier = Random.Range(2,7);
        }

        //play idle anim every X animation cycles, if idling
        if(idleCycleFrame < ((totalFrames * idleIntervalMultiplier) - totalFrames) && action == IDLE)
            anim.PlayInFixedTime(currentAnimation, 0, 0);
        else
            anim.PlayInFixedTime(currentAnimation, 0, normalizedTime);
    }

    void getAnimation()
    {
        direction = SOUTH;

        if(state.direction.x == 0 && state.direction.y > 0) //North
        {
            direction = NORTH;
        }
        else if(state.direction.x == 0 && state.direction.y < 0) //South
        {
            direction = SOUTH;
        }
        else if(state.direction.x > 0 && state.direction.y == 0) //East
        {
            direction = EAST;
        }
        else if(state.direction.x < 0 && state.direction.y == 0) //West
        {
            direction = WEST;
        }
        else if(state.direction.x > 0 && state.direction.y > 0) //NorthEast
        {
            direction = NORTH + EAST;
        }
        else if(state.direction.x < 0 && state.direction.y > 0) //NorthWest
        {
            direction = NORTH + WEST;
        }
        else if(state.direction.x > 0 && state.direction.y < 0) //SouthEast
        {
            direction = SOUTH + EAST;
        }
        else if(state.direction.x < 0 && state.direction.y < 0) //SouthWest
        {
            direction = SOUTH + WEST;
        }

        currentAnimation = BASE + action + direction;
    }
}
using UnityEngine;

public abstract class MovementState : State
{
    public float moveSpeed;
    Vector2 lastMoveDirection;

    public string currentAnimation;
    public int currentFrame;
    public int totalFrames = 8;
    public string action;
    public float framerate;
    public int idleIntervalMultiplier;
    string direction;
    float timer;

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
        }
        else if(state.direction.magnitude > 0 && state.runToggle) //Run Condition
        {
            state.SwitchState(state.runState);
        }
        else if(state.direction.magnitude <= 0) //Idle Condition
        {
            state.SwitchState(state.idleState);
            state.direction = lastMoveDirection;
        }

        // Debug.Log("Frame: " + currentFrame);

        body.velocity = state.direction * moveSpeed; //

        if((body.velocity.magnitude == 0))
            lastMoveDirection = state.direction;
    }

    public void Animate()
    {
        getAnimation();

        timer += Time.deltaTime;
        if(timer >= framerate)
        {
            // Debug.Log(totalFrames);
            timer -= framerate;
            currentFrame = (currentFrame + 1) % totalFrames;
        }

        float normalizedTime = currentFrame / (float)(totalFrames + 1);

        Debug.Log(totalFrames * idleIntervalMultiplier);
        
        if(action == IDLE)
        {
            anim.PlayInFixedTime(currentAnimation, 0, normalizedTime);
        }
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
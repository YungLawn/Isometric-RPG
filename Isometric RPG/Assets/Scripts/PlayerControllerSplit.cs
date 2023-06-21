using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementSplit : MonoBehaviour
{
    public float moveSpeed;
    public float runMultiplier;
    [SerializeField]
    bool diagonal = false;

    private Vector2 moveInput;
    private Vector2 lookInput;
    // private float lookAngle;
    private Rigidbody2D body;
    private SpriteRenderer rendererTop;
    private SpriteRenderer rendererBottom;

    public Sprite[] idleSpritesTop;
    public Sprite[] idleSpritesBottom;



    const string BASE = "IsoHuman";
    const string WALK = "Walk";
    const string IDLE =  "Idle";
    const string RUN = "Run";
    const string NORTH = "N";
    const string SOUTH = "S";
    const string EAST = "E";
    const string WEST = "W";
    const string TOP = "Top-";
    const string BOTTOM = "Bottom-";

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
    string currentLookDirection = SOUTH;
    string currentSpriteTop = BASE + IDLE + TOP + SOUTH;
    string currentSpriteBottom = BASE + IDLE + BOTTOM + SOUTH;


    void OnGUI() {
        GUIStyle headStyle = new GUIStyle();
        headStyle.fontSize = 30;
        GUI.Label(new Rect(0, 0, 500, 50), currentFrame.ToString(), headStyle);
        GUI.Label(new Rect(0, 30, 500, 50), "Top: " + currentSpriteTop, headStyle);
        GUI.Label(new Rect(0, 60, 500, 50), "Bottom: " + currentSpriteBottom, headStyle);
    }

    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D>();
        rendererTop = transform.Find("Top").GetComponent<SpriteRenderer>();
        rendererBottom = transform.Find("Bottom").GetComponent<SpriteRenderer>();
        // foreach (Sprite sprite in idleSprites)
        // {
        //     Debug.Log(sprite.name);
        // }
    }

    // Update is called once per frame
    void Update() {
        Animate();
    }

    void FixedUpdate() {
        Move();
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
        lookInput = Camera.main.ScreenToWorldPoint(value.Get<Vector2>()) - body.transform.position;
        // lookAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
    }

    void Move(){
        Vector2 diagonalFix = diagonal ? new Vector2(1f,0.5f) : new Vector2(1f,1f);
        float diagonalSpeedFix = diagonal ? 1.5f : 1f;

        if(isRunning)
            body.velocity = (moveInput * diagonalFix) * ((moveSpeed * diagonalSpeedFix) * runMultiplier) * Time.fixedDeltaTime;
        else
            body.velocity = (moveInput * diagonalFix) * (moveSpeed * diagonalSpeedFix) * Time.fixedDeltaTime;

        // Debug.Log(body.velocity);
    }

    void Animate() {

        determineDirection();
        determineLookDirection();

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

        currentSpriteTop = BASE + currentAction + TOP + currentLookDirection + "_" + currentFrame;
        currentSpriteBottom = BASE + currentAction + BOTTOM + currentDirection + "_" + currentFrame;

        foreach (Sprite sprite in idleSpritesTop)
        {
            if(sprite.name == currentSpriteTop) {
                rendererTop.sprite = sprite;
            }

        }
        foreach (Sprite sprite in idleSpritesBottom)
        {
            if(sprite.name == currentSpriteBottom) {
                rendererBottom.sprite = sprite;
            }

        }
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

    void determineLookDirection() {
        if(lookInput.x < 0 && Mathf.Abs(lookInput.y) < 0.5){ //North
            currentLookDirection = WEST;
        }
        else if(lookInput.x > 0 && Mathf.Abs(lookInput.y) < 0.5){ //South
            currentLookDirection = EAST;
        }
        else if(lookInput.y > 0 && Mathf.Abs(lookInput.x) < 0.5){ //East
            currentLookDirection = NORTH;
        }
        else if(lookInput.y < 0 && Mathf.Abs(lookInput.x) < 0.5){ //West
            currentLookDirection = SOUTH;
        }
        else if(lookInput.x < 0 && lookInput.y > 0.5){ //NorthWest
            currentLookDirection = NORTH + WEST;
        }
        else if(lookInput.x < 0 && lookInput.y < 0.5){ //SouthWest
            currentLookDirection = SOUTH + WEST;
        }
        else if(lookInput.x > 0 && lookInput.y > 0.5){ //NorthEast
            currentLookDirection = NORTH + EAST;
        }
        else if(lookInput.x > 0 && lookInput.y < 0.5){ //SouthEast
            currentLookDirection = SOUTH + EAST;
        }

    }


}

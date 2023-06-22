using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementSplit : MonoBehaviour
{
    public float moveSpeed;
    public float runMultiplier;
    bool diagonal;
    bool isRunning;
    bool isInteracting;

    private Vector2 moveInput;
    private Vector2 lookInput;
    // private float lookAngle;
    private Rigidbody2D body;
    private SpriteRenderer rendererTop;
    private SpriteRenderer rendererBottom;

    public Sprite[] idleSpritesTop;
    public Sprite[] idleSpritesBottom;
    public Sprite[] walkSpritesTop;
    public Sprite[] walkSpritesBottom;

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


    float framerate = 0.125f;
    int totalFrames = 8;
    int idleIntervalMultiplier = 1;
    [SerializeField] [Range (1,5)]
    int idleIntervalFloor = 3;
    [SerializeField] [Range (1,10)]
    int idleIntervalCeiling = 7;
    int currentFrame;
    int idleCycleFrame;
    float timer;

    string currentAction = IDLE;
    string currentDirection = SOUTH;
    string currentLookDirection = SOUTH;
    string currentSpriteTop = BASE + IDLE + TOP + SOUTH;
    string currentSpriteBottom = BASE + IDLE + BOTTOM + SOUTH;

    string logString1;
    string logString2;

    void OnGUI() {
        GUIStyle headStyle = new GUIStyle();
        headStyle.fontSize = 30;
        // GUI.Label(new Rect(0, 0, 500, 50), currentFrame.ToString(), headStyle);
        // GUI.Label(new Rect(0, 30, 500, 50), idleCycleFrame.ToString(), headStyle);
        // GUI.Label(new Rect(0, 60, 500, 50), idleIntervalMultiplier.ToString(), headStyle);
        GUI.Label(new Rect(0, 00, 500, 50), isRunning.ToString(), headStyle);
        GUI.Label(new Rect(0, 30, 500, 50), isInteracting.ToString(), headStyle);
    }

    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D>();
        rendererTop = transform.Find("Top").GetComponent<SpriteRenderer>();
        rendererBottom = transform.Find("Bottom").GetComponent<SpriteRenderer>();
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

    void OnSprint() {
        isRunning = !isRunning;
    }

    void OnInteract() {
        isInteracting = !isInteracting;
    }

    void OnLook(InputValue value){
        lookInput = Camera.main.ScreenToWorldPoint(value.Get<Vector2>()) - body.transform.position;
        // lookAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
    }

    void Move(){
        Vector2 diagonalFix = diagonal ? new Vector2(1f,0.5f) : new Vector2(1f,1f);
        float diagonalSpeedFix = diagonal ? 1.5f : 1f;

        if(isRunning){
            body.velocity = (moveInput * diagonalFix) * ((moveSpeed * diagonalSpeedFix) * runMultiplier) * Time.fixedDeltaTime;
            framerate = 0.0625f;
        }
        else {
            body.velocity = (moveInput * diagonalFix) * (moveSpeed * diagonalSpeedFix) * Time.fixedDeltaTime;
            framerate = 0.125f;
        }

        // Debug.Log(body.velocity);
    }

    void Animate() {

        determineLookDirection();
        determineDirection();
        
        timer += Time.deltaTime;
        if(timer >= framerate) {
            timer -= framerate;
            currentFrame = (currentFrame + 1) % totalFrames; //cycling through animation frames
            idleCycleFrame = (idleCycleFrame + 1) % (totalFrames * idleIntervalMultiplier); // cycling through idle interval
        }

        if(idleCycleFrame == 0) {
            idleIntervalMultiplier = Random.Range(idleIntervalFloor,idleIntervalCeiling);
        }

        if(idleCycleFrame < ((totalFrames * idleIntervalMultiplier) - totalFrames) && currentAction == IDLE){
            currentFrame = 0;
            currentDirection = currentLookDirection;
        }
        currentSpriteTop = BASE + currentAction + TOP + currentLookDirection + "_" + currentFrame;
        currentSpriteBottom = BASE + currentAction + BOTTOM + currentDirection + "_" + currentFrame;
        
        
        for(int i = 0;i < idleSpritesTop.Length; i++) {
            if(idleSpritesTop[i].name == currentSpriteTop){
                rendererTop.sprite = idleSpritesTop[i];
            }
            if(idleSpritesBottom[i].name == currentSpriteBottom){
                rendererBottom.sprite = idleSpritesBottom[i];
            }

            if(walkSpritesTop[i].name == currentSpriteTop){
                rendererTop.sprite = walkSpritesTop[i];
            }
            if(walkSpritesBottom[i].name == currentSpriteBottom){
                rendererBottom.sprite = walkSpritesBottom[i];
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
        else if((moveInput.x > 0 && moveInput.y == 0)) //East
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
        if(lookInput.x < 0 && Mathf.Abs(lookInput.y) < 0.4){ //North
            currentLookDirection = WEST;
        }
        else if(lookInput.x > 0 && Mathf.Abs(lookInput.y) < 0.4){ //South
            currentLookDirection = EAST;
        }
        else if(lookInput.y > 0 && Mathf.Abs(lookInput.x) < 0.4){ //East
            currentLookDirection = NORTH;
        }
        else if(lookInput.y < 0 && Mathf.Abs(lookInput.x) < 0.4){ //West
            currentLookDirection = SOUTH;
        }
        else if(lookInput.x < 0 && lookInput.y > 0.4){ //NorthWest
            currentLookDirection = NORTH + WEST;
        }
        else if(lookInput.x < 0 && lookInput.y < 0.4){ //SouthWest
            currentLookDirection = SOUTH + WEST;
        }
        else if(lookInput.x > 0 && lookInput.y > 0.4){ //NorthEast
            currentLookDirection = NORTH + EAST;
        }
        else if(lookInput.x > 0 && lookInput.y < 0.4){ //SouthEast
            currentLookDirection = SOUTH + EAST;
        }

    }


}

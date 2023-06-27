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
    bool weaponDrawn;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float lookAngle;
    private Rigidbody2D body;
    private SpriteRenderer rendererTop;
    private SpriteRenderer rendererBottom;
    private Transform Weapon;
    private SpriteRenderer rendererWeapon;

    public Sprite[] idleSpritesTop;
    public Sprite[] idleSpritesBottom;
    public Sprite[] walkSpritesTop;
    public Sprite[] walkSpritesBottom;
    public Sprite[] drawnSpritesTop;

    const string BASE = "Human";
    const string WALK = "Walk";
    const string IDLE =  "Idle";
    const string DRAWN = "WeaponDrawn-";
    const string RUN = "Run";
    const string NORTH = "N";
    const string SOUTH = "S";
    const string EAST = "E";
    const string WEST = "W";
    const string TOP = "Top-";
    const string BOTTOM = "Bottom-";
    public float turnLimit = 0.3f;


    [SerializeField] float framerate;
    int totalFrames = 6;
    int idleIntervalMultiplier = 1;
    [SerializeField] [Range (1,5)] int idleIntervalFloor = 3;
    [SerializeField] [Range (1,10)] int idleIntervalCeiling = 7;
    int currentFrame;
    int idleCycleFrame;
    float timer;

    string currentAction = IDLE;
    string currentDirection = SOUTH;
    string currentLookDirection = SOUTH;
    string currentSpriteTop = BASE + IDLE + TOP + SOUTH;
    string currentSpriteBottom = BASE + IDLE + BOTTOM + SOUTH;

    string logString1 = "--";
    string logString2 = "--";

    void OnGUI() {
        GUIStyle headStyle = new GUIStyle();
        headStyle.fontSize = 30;
        // GUI.Label(new Rect(0, 0, 500, 50), currentFrame.ToString(), headStyle);
        // GUI.Label(new Rect(0, 30, 500, 50), idleCycleFrame.ToString(), headStyle);
        // GUI.Label(new Rect(0, 60, 500, 50), idleIntervalMultiplier.ToString(), headStyle);
        GUI.Label(new Rect(0, 00, 500, 50), logString1, headStyle);
        GUI.Label(new Rect(0, 30, 500, 50), logString2, headStyle);
    }

    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D>();
        rendererTop = transform.Find("Top").GetComponent<SpriteRenderer>();
        rendererBottom = transform.Find("Bottom").GetComponent<SpriteRenderer>();
        Weapon = transform.Find("Weapon");
        rendererWeapon = Weapon.GetComponent<SpriteRenderer>();
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
        // logString1  = moveInput.ToString();

        if(moveInput.x == 0 && moveInput.y == 0) {
            currentAction = IDLE;
        }
        else{
            currentAction = WALK;
        }
    }

    void OnSprint() {
        isRunning = !isRunning;
    }

    void OnInteract() {
        isInteracting = !isInteracting;
    }

    void OnDrawWeapon() {
        weaponDrawn = !weaponDrawn;
        logString2 = "Drawn: " +weaponDrawn.ToString();
    }

    void OnLook(InputValue value){
        lookInput = Camera.main.ScreenToWorldPoint(value.Get<Vector2>()) - body.transform.position;
        // logString1 = lookInput.ToString();
        lookAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
    }

    void Move(){
        Vector2 diagonalFix = diagonal ? new Vector2(1f,0.5f) : new Vector2(1f,1f);
        float diagonalSpeedFix = diagonal ? 1.5f : 1f;

        if(isRunning){
            body.velocity = (moveInput * diagonalFix) * ((moveSpeed * diagonalSpeedFix) * runMultiplier) * Time.fixedDeltaTime;
        }
        else {
            body.velocity = (moveInput * diagonalFix) * (moveSpeed * diagonalSpeedFix) * Time.fixedDeltaTime;
        }
    }

    void Animate() {
        determineLookDirection();
        determineDirection();
        handleWeapon();

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
        }
        currentSpriteTop = BASE + (weaponDrawn ? DRAWN : currentAction + TOP) + currentLookDirection + "_" + currentFrame;
        currentSpriteBottom = BASE + currentAction + BOTTOM + currentDirection + "_" + currentFrame;

        logString1 = currentSpriteTop;


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

            if(drawnSpritesTop[i].name == currentSpriteTop){
                rendererTop.sprite = drawnSpritesTop[i];
            }
        }
    }

    void handleWeapon() {
        rendererWeapon.enabled = weaponDrawn;
        Weapon.eulerAngles = new Vector3(0,0,lookAngle);
        if(lookInput.x < 0) {
             rendererWeapon.flipY = true;
        }
        else{
            rendererWeapon.flipY = false;
        }

        if(lookInput.y > turnLimit){
            rendererWeapon.sortingOrder = 0;
        }
        else{
            rendererWeapon.sortingOrder = 1;
        }
    }

    void determineDirection() {
        rendererBottom.flipX = false;

        if(currentAction != IDLE) {
            if(moveInput.y > 0) { //north
                if(Mathf.Abs(moveInput.x) > 0){
                    currentDirection = NORTH + EAST;
                    diagonal = true;
                    if(moveInput.x < 0) {
                        rendererBottom.flipX = true;
                    }
                }
                else {
                    currentDirection = NORTH;
                    diagonal = false;
                }
            }
            else if (moveInput.y < 0) { //South
                if(Mathf.Abs(moveInput.x) > 0){
                    currentDirection = SOUTH + EAST;
                    diagonal = true;
                    if(moveInput.x < 0) {
                        rendererBottom.flipX = true;
                    }
                }
                else {
                    currentDirection = SOUTH;
                    diagonal = false;
                }
            }
            else{
                if(moveInput.x > 0) {
                    currentDirection = EAST;
                    diagonal = false;
                }
                else if(moveInput.x < 0) {
                    currentDirection = EAST;
                    rendererBottom.flipX = true;
                    diagonal = false;
                }

            }
        }
        else {
            currentDirection = currentLookDirection;
            if(lookInput.x < 0){
                rendererBottom.flipX = true;
            }
        }
    }

    void determineLookDirection() {
        rendererTop.flipX = false;
        if(lookInput.y > turnLimit) { //north
            if(Mathf.Abs(lookInput.x) > turnLimit){
                currentLookDirection = NORTH + EAST;
                if(lookInput.x < -turnLimit){
                    rendererTop.flipX = true;
                }
            }
            else {
                currentLookDirection = NORTH;
            }
        }
        else if (lookInput.y < -turnLimit) { //South
            if(Mathf.Abs(lookInput.x) > turnLimit){
                currentLookDirection = SOUTH + EAST;
                if(lookInput.x < -turnLimit){
                 rendererTop.flipX = true;
                }
            }
            else {
                currentLookDirection = SOUTH;
            }
        }
        else {
            currentLookDirection = EAST;
            if(lookInput.x < 0) {
                rendererTop.flipX = true;
            }
        }
    }

}

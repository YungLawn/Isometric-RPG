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
    private Vector2 lookInputNormalized;
    private float lookAngle;
    private Rigidbody2D body;
    private SpriteRenderer rendererTop;
    private SpriteRenderer rendererLegs;
    private SpriteRenderer rendererArms;
    private Transform Weapon;
    private SpriteRenderer rendererWeapon;

    public Sprite[] idleSpritesTop;
    public Sprite[] idleSpritesLegs;
    public Sprite[] idleSpritesArms;

    public Sprite[] walkSpritesTop;
    public Sprite[] walkSpritesLegs;
    public Sprite[] walkSpritesArms;

    public Sprite[] drawnSpritesTop;
    public Sprite[] drawnSpritesArms;

    const string BASE = "Human";
    const string WALK = "Walk";
    const string IDLE =  "Idle";
    const string DRAWN = "WeaponDrawn";
    const string RUN = "Run";
    const string NORTH = "N";
    const string SOUTH = "S";
    const string EAST = "E";
    const string WEST = "W";
    const string TOP = "Top-";
    const string LEGS = "Legs-";
    const string ARMS = "Arms-";
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
    string currentSpriteLegs = BASE + IDLE + LEGS + SOUTH;
    string currentSpriteArms = BASE + IDLE + ARMS + SOUTH;

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
        rendererLegs = transform.Find("Legs").GetComponent<SpriteRenderer>();
        rendererArms = transform.Find("Arms").GetComponent<SpriteRenderer>();
        Weapon = transform.Find("Weapon");
        rendererWeapon = Weapon.GetComponent<SpriteRenderer>();

        Cursor.visible = false;
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
        lookInputNormalized = lookInput.normalized;
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
        currentSpriteTop = BASE + currentAction + TOP + currentLookDirection + "_" + currentFrame;
        currentSpriteArms = BASE + (weaponDrawn ? DRAWN : currentAction) + ARMS + currentLookDirection + "_" + currentFrame;
        currentSpriteLegs = BASE + currentAction + LEGS + currentDirection + "_" + currentFrame;

        // logString1 = currentSpriteArms;

        for(int i = 0;i < idleSpritesTop.Length; i++) {
            if(idleSpritesTop[i].name == currentSpriteTop){
                rendererTop.sprite = idleSpritesTop[i];
            }
            if(idleSpritesLegs[i].name == currentSpriteLegs){
                rendererLegs.sprite = idleSpritesLegs[i];
            }
            if(idleSpritesArms[i].name == currentSpriteArms){
                rendererArms.sprite = idleSpritesArms[i];
            }

            if(walkSpritesTop[i].name == currentSpriteTop){
                rendererTop.sprite = walkSpritesTop[i];
            }
            if(walkSpritesLegs[i].name == currentSpriteLegs){
                rendererLegs.sprite = walkSpritesLegs[i];
            }
            if(walkSpritesArms[i].name == currentSpriteArms){
                rendererArms.sprite = walkSpritesArms[i];
            }

            if(drawnSpritesTop[i].name == currentSpriteTop){
                rendererTop.sprite = drawnSpritesTop[i];
            }
            if(drawnSpritesArms[i].name == currentSpriteArms){
                rendererArms.sprite = drawnSpritesArms[i];
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

        if(lookInput.y > 0){
            rendererWeapon.sortingOrder = 0;
        }
        else {
            rendererWeapon.sortingOrder = 1;
        }
    }

    void determineDirection() {
        rendererLegs.flipX = false;

        if(currentAction != IDLE) {
            if(moveInput.y > 0) { //north
                if(Mathf.Abs(moveInput.x) > 0){
                    currentDirection = NORTH + EAST;
                    diagonal = true;
                    if(moveInput.x < 0) {
                        rendererLegs.flipX = true;
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
                        rendererLegs.flipX = true;
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
                    rendererLegs.flipX = true;
                    diagonal = false;
                }

            }
        }
        else {
            currentDirection = currentLookDirection;
            if(lookInput.x < 0){
                rendererLegs.flipX = true;
            }
        }
    }

    void determineLookDirection() {
        rendererTop.flipX = false;
        rendererArms.flipX = false;
        if(lookInputNormalized.y > turnLimit) { //north
            if(Mathf.Abs(lookInputNormalized.x) > turnLimit){
                currentLookDirection = NORTH + EAST;
                if(lookInputNormalized.x < -turnLimit){
                    rendererTop.flipX = true;
                    rendererArms.flipX = true;
                }
            }
            else {
                currentLookDirection = NORTH;
            }
        }
        else if (lookInputNormalized.y < -turnLimit) { //South
            if(Mathf.Abs(lookInputNormalized.x) > turnLimit){
                currentLookDirection = SOUTH + EAST;
                if(lookInputNormalized.x < -turnLimit){
                 rendererTop.flipX = true;
                 rendererArms.flipX = true;
                }
            }
            else {
                currentLookDirection = SOUTH;
            }
        }
        else {
            currentLookDirection = EAST;
            if(lookInputNormalized.x < 0) {
                rendererTop.flipX = true;
                rendererArms.flipX = true;
            }
        }
    }

}

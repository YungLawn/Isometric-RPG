using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    private bool weaponDrawn;

    private Dictionary<string, Sprite> topSpritesDIC = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> legSpritesDIC = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> armSpritesDIC = new Dictionary<string, Sprite>();

    public Sprite[] topSprites;
    public Sprite[] legSprites;
    public Sprite[] armSprites;

    private SpriteRenderer rendererTop;
    private SpriteRenderer rendererLegs;
    private SpriteRenderer rendererArms;

    string currentAction = IDLE;
    string currentDirection = SOUTH;
    string currentLookDirection = SOUTH;
    string currentSpriteTop = BASE + IDLE + TOP + SOUTH;
    string currentSpriteLegs = BASE + IDLE + LEGS + SOUTH;
    string currentSpriteArms = BASE + IDLE + ARMS + SOUTH;

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

    GUIStyle headStyle = new GUIStyle();
    string logString1 = "--";
    string logString2 = "--";
    string logString3 = "--";

    void OnGUI() {
        headStyle.fontSize = 30;
        headStyle.normal.textColor = Color.yellow;
        GUI.Label(new Rect(300, 0, 500, 50), "--SpriteAnimator--", headStyle);
        GUI.Label(new Rect(300, 30, 500, 50), logString1, headStyle);
        GUI.Label(new Rect(300, 60, 500, 50), logString2, headStyle);
        GUI.Label(new Rect(300, 90, 500, 50), logString3, headStyle);
    }

    void Start() {
        rendererTop = transform.Find("Top").GetComponent<SpriteRenderer>();
        rendererLegs = transform.Find("Legs").GetComponent<SpriteRenderer>();
        rendererArms = transform.Find("Arms").GetComponent<SpriteRenderer>();

        foreach(Sprite sprite in topSprites) {
            topSpritesDIC.Add(sprite.name,sprite);
        }
        foreach(Sprite sprite in armSprites) {
            armSpritesDIC.Add(sprite.name,sprite);
        }
        foreach(Sprite sprite in legSprites) {
            legSpritesDIC.Add(sprite.name,sprite);
        }

    }

    public void Animate(bool weaponDrawn, Vector2 moveInput, bool diagonal, Vector2 lookInput) {

        if(moveInput.x == 0 && moveInput.y == 0) {
            currentAction = IDLE;
        }
        else{
            currentAction = WALK;
        }

        determineDirection(moveInput, diagonal, lookInput);
        determineLookDirection(lookInput.normalized);

        timer += Time.deltaTime;
        if(timer >= framerate) {
            timer -= framerate;
            currentFrame = (currentFrame + 1) % totalFrames; //cycling through animation frames
            idleCycleFrame = (idleCycleFrame + 1) % (totalFrames * idleIntervalMultiplier); // cycling through idle interval
        }

        idleIntervalMultiplier = idleCycleFrame == 0 ? Random.Range(idleIntervalFloor,idleIntervalCeiling) : idleIntervalMultiplier;

        if(idleCycleFrame < ((totalFrames * idleIntervalMultiplier) - totalFrames) && currentAction == IDLE){
            currentFrame = 0;
        }
        currentSpriteTop = BASE + currentAction + TOP + currentLookDirection + "_" + currentFrame;
        currentSpriteArms = BASE + (weaponDrawn ? DRAWN : currentAction) + ARMS + currentLookDirection + "_" + currentFrame;
        currentSpriteLegs = BASE + currentAction + LEGS + currentDirection + "_" + currentFrame;

        // logString1 = currentSpriteTop;
        // logString2 = currentSpriteArms;
        // logString3 = currentSpriteLegs;

        rendererTop.sprite = topSpritesDIC[currentSpriteTop];
        rendererLegs.sprite = legSpritesDIC[currentSpriteLegs];
        rendererArms.sprite = armSpritesDIC[currentSpriteArms];

    }

    void determineDirection(Vector2 moveInput, bool diagonal, Vector2 lookInput) {
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

    void determineLookDirection(Vector2 lookInputNormalized) {
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

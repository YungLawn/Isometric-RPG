using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    private bool weaponDrawn;

    public Sprite[] idleSpritesTop;
    public Sprite[] idleSpritesLegs;
    public Sprite[] idleSpritesArms;

    public Sprite[] walkSpritesTop;
    public Sprite[] walkSpritesLegs;
    public Sprite[] walkSpritesArms;

    public Sprite[] drawnSpritesTop;
    public Sprite[] drawnSpritesArms;

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

    void Start() {
        rendererTop = transform.Find("Top").GetComponent<SpriteRenderer>();
        rendererLegs = transform.Find("Legs").GetComponent<SpriteRenderer>();
        rendererArms = transform.Find("Arms").GetComponent<SpriteRenderer>();
    }

    public void Animate(bool weaponDrawn, Vector2 moveInput, bool diagonal, Vector2 lookInput, Vector2 lookInputNormalized) {

        if(moveInput.x == 0 && moveInput.y == 0) {
            currentAction = IDLE;
        }
        else{
            currentAction = WALK;
        }

        determineDirection(moveInput, diagonal, lookInput);
        determineLookDirection(lookInputNormalized);

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

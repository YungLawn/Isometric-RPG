using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float runMultiplier;
    bool diagonal;
    bool isRunning;
    bool isInteracting;
    public bool weaponDrawn;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector2 lookInputNormalized;
    private float lookAngle;
    private Rigidbody2D body;
    private SpriteRenderer rendererTop;
    private SpriteRenderer rendererLegs;
    private SpriteRenderer rendererArms;

    public GameObject bulletPF;
    public float bulletForce = 20f;
    public Transform Crosshair;
    private Transform Gun;
    public GameObject muzzleFlashPF;
    Vector3 recoil = new Vector3(-0.02f, 0,0);

    string logString1 = "--";
    string logString2 = "--";

    private SpriteAnimator animator;

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
        Gun = transform.Find("Gun");
        animator = GetComponent<SpriteAnimator>();
    }

    // Update is called once per frame
    void Update() {
        animator.Animate(weaponDrawn, moveInput, diagonal, lookInput, lookInputNormalized);
        handleGun();
    }

    void FixedUpdate() {
        Move();
    }

    void OnMove(InputValue value) {

        moveInput = value.Get<Vector2>();
        // logString1  = moveInput.ToString();
    }

    void OnSprint() {
        isRunning = !isRunning;
    }

    void OnInteract() {
        isInteracting = !isInteracting;
    }

    void OnDrawWeapon() {
        weaponDrawn = !weaponDrawn;
        // logString2 = "Drawn: " +weaponDrawn.ToString();
    }

    void OnFire() {
        if(weaponDrawn) {
            handleShootProjectile();
        }
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



    void handleGun() {
        Gun.GetComponent<SpriteRenderer>().enabled = weaponDrawn;
        Gun.eulerAngles = new Vector3(0,0,lookAngle);
        if(lookInput.x < 0) {
             Gun.GetComponent<SpriteRenderer>().flipY = true;
        }
        else{
            Gun.GetComponent<SpriteRenderer>().flipY = false;
        }

        if(lookInput.y > 0){
            Gun.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else {
            Gun.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    void handleShootProjectile() {
        GameObject bullet = Instantiate(bulletPF, Gun.transform.Find("ShootPoint").position, Gun.rotation);
        GameObject muzzleFlash = Instantiate(muzzleFlashPF, Gun.transform.Find("ShootPoint").position, Gun.rotation);
        Destroy(muzzleFlash, 0.05f);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(Gun.transform.Find("ShootPoint").right * bulletForce, ForceMode2D.Impulse);
    }

}

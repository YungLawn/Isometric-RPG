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
    private float lookAngle;
    private Rigidbody2D body;

    private SpriteAnimator animator;
    private GunController gunController;
    private InventoryManager InventoryManager;

    GUIStyle headStyle = new GUIStyle();
    string logString1 = "--";
    string logString2 = "--";

    void OnGUI() {
        headStyle.fontSize = 30;
        headStyle.normal.textColor = Color.yellow;
        GUI.Label(new Rect(0, 0, 500, 50), "--PlayerContoller--", headStyle);
        GUI.Label(new Rect(0, 30, 500, 50), logString1, headStyle);
        GUI.Label(new Rect(0, 60, 500, 50), logString2, headStyle);
    }

    void Start() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<SpriteAnimator>();
        if(transform.Find("Inventory").transform.Find("Gun") != null)
            gunController = transform.Find("Inventory").transform.Find("Gun").GetComponent<GunController>();
        InventoryManager = transform.Find("Inventory").GetComponent<InventoryManager>();
    }

    void Update() {
        animator.Animate(weaponDrawn, moveInput, diagonal, lookInput);
        if(gunController != null) gunController.handleGun(weaponDrawn, lookAngle, lookInput);
    }

    void FixedUpdate() {
        Move();
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value){
        lookInput = Camera.main.ScreenToWorldPoint(value.Get<Vector2>()) - body.transform.position;
        lookAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
    }

    void OnSprint() {
        isRunning = !isRunning;
    }

    void OnInteract() {
        isInteracting = !isInteracting;

        // foreach(string item in InventoryManager.Inventory)
        // {
        //     print(item);
        // }
    }

    void OnDrawWeapon() {
        if(gunController != null)
            weaponDrawn = !weaponDrawn;
    }

    void OnFire() {
        if(weaponDrawn) {
            if(gunController != null) gunController.Shoot();
        }
    }

    void Move(){
        Vector2 diagonalFix = diagonal ? new Vector2(1f,0.5f) : new Vector2(1f,1f);
        float diagonalSpeedFix = diagonal ? 1.5f : 1f;

        body.velocity = isRunning
            ? (moveInput * diagonalFix) * ((moveSpeed * diagonalSpeedFix) * runMultiplier) * Time.fixedDeltaTime
            : (moveInput * diagonalFix) * (moveSpeed * diagonalSpeedFix) * Time.fixedDeltaTime;
    }

}

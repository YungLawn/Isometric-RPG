using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private PlayerInput playerInput;

    private InputAction directionalInput;
    private InputAction sprint;
    private InputAction dontsprint;

    public Vector2 direction;
    private bool runToggle;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        directionalInput = playerInput.actions["Move"];
        sprint = playerInput.actions["Sprint"];
        dontsprint = playerInput.actions["dontSprint"];
    }

    public Vector2 getDirectionalInput()
    {
        direction = directionalInput.ReadValue<Vector2>();
        return direction;
    }

    public bool isRunning()
    {   
        if(sprint.triggered)
            runToggle = true;
        else if(dontsprint.triggered)
            runToggle = false;
        return runToggle;
    }

    private void Update()
    {
        isRunning();
    }

}

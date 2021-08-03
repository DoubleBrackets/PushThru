using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public ForceMovementScript movementScript;

    [HideInInspector] public Vector2 inputVector;
    [HideInInspector] public Vector2 inputVectorSoftened;
    private Vector2 inputVectorPreviousFrame;
    private float inputSoftenedTimer = 0f;
    [HideInInspector]public float inputVectorLastChanged = 0;
    //Mouse
    public Vector2 mouseDirNormalized;
    //Events
    public event System.Action AttackKeyDown;
    public event System.Action AttackKeyUp;

    public event System.Action DashKeyDown;
    public event System.Action DashKeyUp;

    private void Update()
    {
        inputVectorLastChanged += Time.deltaTime;
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        movementScript.inputVector = inputVector;
        if (inputVector != inputVectorPreviousFrame)
        {
            inputSoftenedTimer = 0.04f;
            inputVectorLastChanged = 0f;
        }
        if (inputSoftenedTimer <= 0f)
        {
            inputVectorSoftened = inputVector;
        }
        else
            inputSoftenedTimer -= Time.deltaTime;
        inputVectorPreviousFrame = inputVector;

        //Mouse
        Vector2 screenCenter = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height)/2f;
        Vector2 mouseScreenPos = Input.mousePosition;
        mouseDirNormalized = (mouseScreenPos - screenCenter).normalized;
        //Keybinds
        if(Input.GetMouseButtonDown(0))
        {
            AttackKeyDown?.Invoke();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            AttackKeyUp?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DashKeyDown?.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            DashKeyUp?.Invoke();
        }
    }
}

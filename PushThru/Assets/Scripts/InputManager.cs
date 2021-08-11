using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public ForceMovementScript movementScript;
    public FacingScript facingScript;

    [HideInInspector] public Vector2 inputVector;
    [HideInInspector] public Vector2 inputVectorSoftened;
    private Vector2 inputVectorPreviousFrame;
    private float inputSoftenedTimer = 0f;
    [HideInInspector]public float inputVectorLastChanged = 0;
    //Mouse
    [HideInInspector] public Vector2 mouseDirNormalized;
    public Transform mouseCenterTarget;

    //Locks
    public int movementInputEnabled = 0;
    public int dashInputEnabled = 0;
    public int actionInputEnabled = 0;

    //Events
    public event System.Action<Vector2> AttackKeyDown;
    public event System.Action<Vector2> AttackKeyUp;

    public event System.Action<Vector2> BlockKeyDown;

    public event System.Action DashKeyDown;
    public event System.Action DashKeyUp;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        DirectionKeyInput();
        

        //Mouse
        Vector2 screenCenter = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height) / 4f;
        if(mouseCenterTarget)
            screenCenter = CameraSystem.Main.WorldToScreenPoint(mouseCenterTarget.position);
        
             
        Vector2 mouseScreenPos = Input.mousePosition;
        mouseDirNormalized = (mouseScreenPos - screenCenter).normalized;
        //Keybinds
        ActionKeys();
        DashKey();
        //Updates values in other scripts
        movementScript.inputVector = inputVector;
        facingScript.sourceInputVector = inputVector;
    }

    private void ActionKeys()
    {
        if (actionInputEnabled != 0)
            return;
        if (Input.GetMouseButton(0))
        {
            AttackKeyDown?.Invoke(mouseDirNormalized);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            AttackKeyUp?.Invoke(mouseDirNormalized);
        }

        if (Input.GetMouseButton(1))
        {
            BlockKeyDown?.Invoke(mouseDirNormalized);
        }
    }

    private void DashKey()
    {
        if (dashInputEnabled != 0)
            return;
        if (Input.GetKey(KeyCode.Space))
        {
            DashKeyDown?.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            DashKeyUp?.Invoke();
        }
    }

    private void DirectionKeyInput()
    {
        inputVectorLastChanged += Time.deltaTime;
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (movementInputEnabled != 0)
            inputVector = Vector2.zero;
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
    }

    public void IncrementAllInputEnabled()
    {
        dashInputEnabled++;
        movementInputEnabled++;
        actionInputEnabled++;
    }
    public void DecrementAllInputEnabled()
    {
        dashInputEnabled--;
        movementInputEnabled--;
        actionInputEnabled--;
    }
}

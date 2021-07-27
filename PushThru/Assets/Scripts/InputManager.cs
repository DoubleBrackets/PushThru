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

    private void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        movementScript.inputVector = inputVector;
        if (inputVector != inputVectorPreviousFrame)
        {
            inputSoftenedTimer = 0.04f;
        }
        if (inputSoftenedTimer <= 0f)
        {
            inputVectorSoftened = inputVector;
        }
        else
            inputSoftenedTimer -= Time.deltaTime;
        inputVectorPreviousFrame = inputVector;
    }
}

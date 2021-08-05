using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMoveCamera : MonoBehaviour
{
    private Quaternion targetCameraRotation;
    private Quaternion currentCameraRotation;

    public float sensitivity = 1;
    public float speed;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = GetInput();
        MoveCamera();
        Movement(input);

    }

    private void Movement(Vector3 input)
    {
        transform.rotation = currentCameraRotation;
        transform.position += currentCameraRotation * input * speed * Time.deltaTime;
    }

    private void MoveCamera()
    {
        Vector3 deltaAngle = GetMouseDeltaRotation();
        targetCameraRotation.eulerAngles = ClampCamera(targetCameraRotation.eulerAngles + deltaAngle);
        currentCameraRotation = targetCameraRotation;
    }

    private Vector3 GetInput()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        float verticalMovement = 0;
        if (Input.GetKey(KeyCode.E))
        {
            verticalMovement = 1;
        }
        else if (Input.GetKey(KeyCode.Q))
            verticalMovement = -1;
        return new Vector3(xInput, verticalMovement, yInput).normalized;
    }

    private Vector3 GetMouseDeltaRotation()
    {
        float xDelta = Input.GetAxisRaw("Mouse X");
        float yDelta = -Input.GetAxisRaw("Mouse Y");

        return new Vector3(yDelta, xDelta, 0) * sensitivity;
    }

    private Vector3 ClampCamera(Vector3 rot)
    {
        if (rot.x > 89f && rot.x < 180f)
            rot.x = 89f;
        else if (rot.x < 271f && rot.x > 180f)
            rot.x = 271f;
        return rot;
    }
}

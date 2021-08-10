using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationScript : MonoBehaviour
{
    private float targetRotation;
    public float lerpTime;

    private float timer = 0f;

    private void Awake()
    {
        targetRotation = transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetRotation += 45;
            timer = lerpTime;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            targetRotation -= 45;
            timer = lerpTime;
        }
        if(timer >= 0)
        {
            timer -= Time.deltaTime;
            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y = Mathf.SmoothStep(currentRotation.y, currentRotation.y + Mathf.DeltaAngle(currentRotation.y, targetRotation), lerpTime - timer);
            transform.rotation = Quaternion.Euler(currentRotation);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0,targetRotation,0);
        }

    }
}

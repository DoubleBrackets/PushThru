using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoPixelMoveCamera : MonoBehaviour
{
    public static OrthoPixelMoveCamera orthoCam;

    public float xspeed;
    public float zspeed;
    public float lockStep;
    public Camera cam;

    public bool locked = false;

    private Vector3 offset;

    public Transform target;

    private Vector3 currentPosition;
    public Vector3 currentPixelOffset;

    public float lerpFactor;
    private void Awake()
    {
        orthoCam = this;
        lockStep *= 4;
        offset = transform.localPosition;
        offset.y = 0;
        currentPosition = transform.position;
    }

    void LateUpdate()
    {
        float edgeWidth = 100;
        Vector2 mousePos = Input.mousePosition;
        Vector2 size = new Vector2(Screen.width,Screen.height);
        float z = 0; //Input.GetAxisRaw("Vertical");
        float x = 0;// Input.GetAxisRaw("Horizontal");
        if (mousePos.x < edgeWidth)
        {
            x = -1;
        }
        else if (mousePos.x > size.x - edgeWidth)
            x = 1;
        if (mousePos.y < edgeWidth)
        {
            z = -1;
        }
        else if (mousePos.y > size.y - edgeWidth)
            z = 1;

        if (Input.GetKeyDown(KeyCode.R))
        {
            locked = !locked;
        }
        float modifier = 1;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            modifier = 0.2f;
        }
        Vector3 movement = new Vector3(x, 0, z).normalized;
        movement.x *= xspeed * modifier;
        movement.z *= zspeed * modifier;
        Quaternion rot = Quaternion.Euler(0, transform.rotation.eulerAngles.y,0);

        if (!locked)
        {
            currentPosition += rot * movement * Time.deltaTime;
        }
        else if (locked)
        {
            float time = lerpFactor * Time.deltaTime;
            LerpToTarget(transform, time);
        }
        //Lock
        transform.position = new Vector3(Mathf.FloorToInt(currentPosition.x / lockStep) * lockStep, currentPosition.y, Mathf.FloorToInt(currentPosition.z / lockStep*0.5f) * lockStep*2);
        currentPixelOffset = currentPosition - transform.position;
    }
    private void LerpToTarget(Transform toLerp, float time)
    {
        Vector3 targetPos = target.transform.position;
        Vector3 cPos = transform.position;
        Vector3 newPos = new Vector3(Mathf.Lerp(cPos.x, targetPos.x, time), transform.position.y, Mathf.Lerp(cPos.z, targetPos.z, time));
        currentPosition = newPos;
    }



}

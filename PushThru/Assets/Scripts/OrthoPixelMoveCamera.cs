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
    public float snapDistance;
    public float maxDistance;
    private void Awake()
    {
        orthoCam = this;
        lockStep *= 4;
        offset = transform.localPosition;
        offset.y = 0;
        currentPosition = transform.position;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            locked = !locked;
        }
    }
    void FixedUpdate()
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
            currentPosition += rot * movement * Time.fixedDeltaTime;
        }
        else if (locked)
        {
            float time = lerpFactor;
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
        targetPos.y = cPos.y;
        Vector3 cVel = Vector3.zero;
        Vector3 newPos = Vector3.SmoothDamp(cPos, targetPos, ref cVel, time);
        Vector3 newPosConst = newPos;
        if ((targetPos - cPos).magnitude < snapDistance)
        {
            newPos = cPos;
        }
        if(Mathf.Abs((targetPos - newPosConst).x) > maxDistance)
        {
            float diff = Mathf.Sign((targetPos - cPos).x) * maxDistance;
            newPos.x = targetPos.x - diff;
        }
        if (Mathf.Abs((targetPos - newPosConst).z) > maxDistance)
        {
            float diff = Mathf.Sign((targetPos - cPos).z) * maxDistance;
            newPos.z = targetPos.z - diff;
        }
        print((cPos - targetPos).magnitude);
        currentPosition += newPos - cPos;
    }



}

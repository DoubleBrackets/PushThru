using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoPixelMoveCamera : MonoBehaviour
{
    public static OrthoPixelMoveCamera orthoCam;

    public InputManager inputManager;

    public float xspeed;
    public float zspeed;
    public float lockStep;
    public int pixelSize;
    public Camera cam;

    public bool locked = false;
    public bool hardLock = false;
    public bool stuckToTarget = false;

    private Vector3 offset;

    public Transform target;

    private Vector3 currentPosition;
    private Vector3 prevPosition;

    public Vector3 currentPixelOffset;

    public float lerpFactor;
    public float maxDistance;
    private Vector3 targetPosition;
    public bool isCurrentlyTracking = false;
    public Rigidbody predictionRb;
    public float predictionFactor;
    private void Awake()
    {
        orthoCam = this;
        lockStep *= pixelSize;
        offset = transform.localPosition;
        offset.y = 0;
        currentPosition = transform.position;
        prevPosition = transform.position;
    }

    public event System.Action<Vector3> OrthoCamMove;

    void FixedUpdate()
    {
        if(!stuckToTarget)
        {
            float edgeWidth = 100;
            Vector2 mousePos = Input.mousePosition;
            Vector2 size = new Vector2(Screen.width, Screen.height);
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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                modifier = 0.2f;
            }
            Vector3 movement = new Vector3(x, 0, z).normalized;
            movement.x *= xspeed * modifier;
            movement.z *= zspeed * modifier;
            Quaternion rot = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            stuckToTarget = false;
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
            transform.position = new Vector3(Mathf.FloorToInt(currentPosition.x / lockStep) * lockStep, currentPosition.y, Mathf.FloorToInt(currentPosition.z / lockStep * 0.5f) * lockStep * 2);
            currentPixelOffset = currentPosition - transform.position;
            OrthoCamMove?.Invoke(transform.position - prevPosition);
            prevPosition = transform.position;
        }     
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            targetPosition = currentPosition;
            locked = !locked;
        }
        if (stuckToTarget)
        {
            stuckToTarget = false;
            if (locked)
            {
                float time = lerpFactor;
                LerpToTarget(transform, time);
            }

            //Lock
            transform.position = new Vector3(Mathf.FloorToInt(currentPosition.x / lockStep) * lockStep, currentPosition.y, Mathf.FloorToInt(currentPosition.z / lockStep * 0.5f) * lockStep * 2);
            currentPixelOffset = currentPosition - transform.position;
            OrthoCamMove?.Invoke(transform.position - prevPosition);
            prevPosition = transform.position;
        }
    }
    public void UpdateTarget()
    {
        UpdateTarget(predictionFactor);
    }
    public void UpdateTarget(float _predictionFactor)
    {
        Vector3 targetPos = target.transform.position;
        targetPos.y = currentPosition.y;
        Vector3 prediction = predictionRb.velocity.normalized;
        prediction.y = 0;
        prediction *= _predictionFactor;
        prediction.z *= 2;
        targetPosition = targetPos + prediction;
        isCurrentlyTracking = true;
    }

    private void LerpToTarget(Transform toLerp, float time)
    {

        Vector3 targetPos = target.transform.position;
        targetPos.y = currentPosition.y;
        if (hardLock)
        {
            currentPosition = targetPos;
            stuckToTarget = true;
            return;
        }

        if(!isCurrentlyTracking && (targetPosition - targetPos).magnitude > maxDistance)
        {
            UpdateTarget(predictionFactor);
        }
        if(isCurrentlyTracking)
        {
            targetPos = targetPosition;
            Vector3 cPos = transform.position;
            targetPos.y = cPos.y;
  
            Vector3 cVel = Vector3.zero;
            Vector3 newPos = Vector3.SmoothDamp(cPos, targetPos, ref cVel, time);

            if ((currentPosition - targetPos).magnitude <= 1.5f)
            {
                isCurrentlyTracking = false;
                targetPosition = currentPosition;
            }
            currentPosition += newPos - cPos;
        }


/*
        Vector3 vectorToTarget = targetPos - currentPosition;
        
        if ((vectorToTarget).magnitude > maxDistance - margin * 
             (stuckToTarget ? 3f : 1) && inputManager.inputVector != Vector2.zero)
        {
            Vector3 diff = (vectorToTarget).normalized * maxDistance;
            //angle snapping
            float angle = Mathf.Rad2Deg * Mathf.Atan2(diff.z, diff.x);
            if (inputManager.inputVector.x != 0 && inputManager.inputVector.y != 0)
            {
                stuckToTarget = true;
                angle *= Mathf.Deg2Rad;
                diff = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * maxDistance;
                diff.x = Mathf.RoundToInt(diff.x/0.05f)*0.05f;
                diff.z = Mathf.RoundToInt(diff.z/0.1f)*0.1f;
            }
            else
            {
                if(stuckToTarget)
                {
                    stuckToTarget = false;
                    frameFreeze = 3;
                }
                else
                {
                    angle *= Mathf.Deg2Rad;
                    diff = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * maxDistance;
                }
            }
            print(diff.x + " " + diff.z);


            currentPosition = targetPos - diff;
        }
        else if(stuckToTarget)
        {
            stuckToTarget = false;
            frameFreeze = 3;
        }*/
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScript : MonoBehaviour
{
    //Dependencies
    public InputManager inputManager;
    public ForceMovementScript movementScript;
    public BoxCollider colliderCast;

    //Dash Values
    public float maxDistance;

    public float windupTime;
    private float windupTimer = 0f;
    public float recoverTime;
    private float recoverTimer = 0f;
    public float cooldown;
    private float cooldownTimer = 0f;

    private Vector2 targetDir;

    public bool isInWindup
    {
        get => windupTimer > 0;
    }

    public bool isInRecover
    {
        get => recoverTimer > 0;
    }

    private void Awake()
    {
        inputManager.DashKeyDown += StartDash;
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if(recoverTimer > 0)
        {
            recoverTimer -= Time.deltaTime;
            if(recoverTimer <= 0)
            {
                movementScript.DecrementMovementActive();
            }
        }
        if(windupTimer > 0)
        {
            windupTimer -= Time.deltaTime;
            if(windupTimer <= 0)
            {
                PerformDash();
                recoverTimer = recoverTime;
            }
        }
    }

    private void StartDash()
    {
        if (cooldownTimer > 0)
            return;
        Vector2 dir = inputManager.inputVector.normalized;
        if(dir == Vector2.zero)
        {
            dir = movementScript.facing;
        }
        cooldownTimer = cooldown;
        targetDir = dir;
        movementScript.IncrementMovementActive();
        windupTimer = windupTime;      
    }

    public LayerMask terrainMask;

    private void PerformDash()
    {
        targetDir.y *= 2;
        bool hit = Physics.BoxCast(colliderCast.center, 
            colliderCast.bounds.extents-new Vector3(0,0.1f,0), 
            targetDir.Vector2To3TopDown(), 
            out RaycastHit hitInfo, 
            Quaternion.identity,
            maxDistance,
            terrainMask);
        float finalDistance = maxDistance;
        if(hit)
        {
            print("e");
            finalDistance = hitInfo.distance - 0.2f;
        }
        transform.position += new Vector3(targetDir.x, 0, targetDir.y) * finalDistance;
    }
}

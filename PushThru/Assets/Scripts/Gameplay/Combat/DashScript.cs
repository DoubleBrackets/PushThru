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
        Vector3 size = colliderCast.bounds.extents;
        bool hit = Physics.BoxCast(transform.position + colliderCast.center,
            size - new Vector3(0.1f, 0.1f, 0.1f),
            targetDir.Vector2To3TopDown(),
            out RaycastHit hitInfo,
            Quaternion.identity,
            targetDir.magnitude * maxDistance,
            terrainMask);
/*        bool hit = Physics.Raycast(transform.position + colliderCast.center, 
            targetDir.Vector2To3TopDown(),
            out RaycastHit hitInfo,
            targetDir.magnitude * maxDistance,
            terrainMask);*/
        float finalDistance = maxDistance;
        Vector3 finalVector = new Vector3(targetDir.x, 0, targetDir.y) * finalDistance;
        if (hit)
        {
            finalDistance = hitInfo.distance - 0.1f;
            finalVector = new Vector3(targetDir.x, 0, targetDir.y).normalized * finalDistance;
        }

        transform.position += finalVector;
    }
}

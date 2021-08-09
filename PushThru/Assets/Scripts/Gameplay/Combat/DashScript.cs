using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScript : MonoBehaviour
{
    //Dependencies
    public InputManager inputManager;
    public ForceMovementScript movementScript;
    public BoxCollider colliderCast;
    public PlayerCombatActionManager actionManager;
    public Rigidbody rb;
    public DashVFXScript vFXScript;
    //Dash Values
    public float maxDistance;

    public float recoverTime;
    private float recoverTimer = 0f;
    public float cooldown;
    private float cooldownTimer = 0f;

    private Vector2 targetDir;

    public event System.Action DashPerformed;

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
    }

    private void StartDash()
    {
        if (cooldownTimer > 0 || !actionManager.isActionInterruptable(0.05f))
        {
            return;
        }
        actionManager.EndCurrentActionCheckInterruptable();
        actionManager.ExtendComboResetTimer(recoverTime/2f);
        Vector2 dir = inputManager.inputVector.normalized;
        if(dir == Vector2.zero)
        {
            dir = movementScript.facing;
        }
        cooldownTimer = cooldown;
        recoverTimer = recoverTime;
        targetDir = dir;
        movementScript.IncrementMovementActive();
        DashPerformed?.Invoke();
        PerformDash();

        actionManager.StartNullAction(recoverTime, targetDir);
        OrthoPixelMoveCamera.orthoCam.UpdateTarget(4);
    }

    public LayerMask terrainMask;

    private void PerformDash()
    {
        Vector3 originalPosition = transform.position;
        targetDir.y *= 1.5f;
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
        Vector3 dirVectorFlat = new Vector3(targetDir.x, 0, targetDir.y).normalized;
        if (hit)
        {
            finalDistance = hitInfo.distance - 0.1f;
            finalVector = dirVectorFlat * finalDistance;
        }
        
        transform.position += finalVector;

        Vector3 overshootVelocity = dirVectorFlat;
        overshootVelocity *= movementScript.baseMaxMoveSpeed * 4f;
        rb.velocity = overshootVelocity;
        vFXScript.CreateDashVFX(originalPosition, originalPosition + finalVector, dirVectorFlat);

    }
}

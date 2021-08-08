using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacingScript : MonoBehaviour
{
    public Rigidbody rb;
    public ForceMovementScript moveScript;
    public PlayerAttackManager attackManager;
    public InputManager inputManager;

    private Vector3 _facingNormalized;
    public Vector3 facingNormalized
    {
        get => _facingNormalized;
    }

    private void Update()
    {
        UpdateFacing();
    }

    public void UpdateFacing()
    {
        Vector2 rbVel = new Vector2(rb.velocity.x, rb.velocity.z / moveScript.zAxisMultiplier);
        float rawVelocityAngle = (Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.z / moveScript.zAxisMultiplier, rb.velocity.x));
        float rawInputAngle = (Mathf.Rad2Deg * Mathf.Atan2(inputManager.inputVectorSoftened.y, inputManager.inputVectorSoftened.x));
        float angle = 0;
        if (attackManager.isAttacking)
        {
            angle = Mathf.Rad2Deg * Mathf.Atan2(attackManager.currentAttackDirection.y, attackManager.currentAttackDirection.x);
            angle = angle.RoundToIntMultiple(45);
            rb.transform.rotation = Quaternion.Euler(0, 90 - angle, 0);
        }
        else if (rbVel.magnitude > 0.2f)
        {
            if (inputManager.inputVectorSoftened != Vector2.zero)
            {
                angle = inputManager.inputVectorSoftened.Angle();
            }
            else
            {
                angle = rawVelocityAngle.RoundToIntMultiple(45);
            }
            rb.transform.rotation = Quaternion.Euler(0, 90 - angle, 0);
        }
        _facingNormalized = Quaternion.Euler(0,-angle,0) * Vector3.right;
    }
}

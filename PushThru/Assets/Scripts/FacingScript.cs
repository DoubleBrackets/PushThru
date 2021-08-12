using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingScript : MonoBehaviour
{
    public Rigidbody rb;
    public ForceMovementScript moveScript;
    public CombatActionManager actionManager;
    //Vector that facing is determined by
    public Vector2 sourceInputVector;

    private Vector3 _facingVectorNormalized;
    public Vector3 facingVectorNormalized
    {
        get => _facingVectorNormalized;
    }

    private void Update()
    {
        UpdateFacing();
    }

    public void SetFacing(Vector2 dir)
    {
        float angle = dir.Angle().RoundToIntMultiple(45);
        rb.transform.rotation = Quaternion.Euler(0, 90 - angle, 0);
    }

    public void UpdateFacing()
    {
        Vector2 rbVel = new Vector2(rb.velocity.x, rb.velocity.z / moveScript.zAxisMultiplier);
        float rawVelocityAngle = (Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.z / moveScript.zAxisMultiplier, rb.velocity.x));
        float rawInputAngle = (Mathf.Rad2Deg * Mathf.Atan2(sourceInputVector.y, sourceInputVector.x));
        float angle = 90-transform.rotation.eulerAngles.y;
        if (actionManager.IsPerformingAction())
        {
            angle = Mathf.Rad2Deg * Mathf.Atan2(actionManager.currentActionDirection.y, actionManager.currentActionDirection.x);
            angle = angle.RoundToIntMultiple(45);
            rb.transform.rotation = Quaternion.Euler(0, 90 - angle, 0);
        }
        else if (rbVel.Vector2To3TopDown().magnitude > 0.2f && moveScript.movementActive)
        {
            if (sourceInputVector != Vector2.zero)
            {
                angle = sourceInputVector.Angle();
            }
            else
            {
                angle = rawVelocityAngle.RoundToIntMultiple(45);
            }
            rb.transform.rotation = Quaternion.Euler(0, 90 - angle, 0);
        }
        _facingVectorNormalized = Quaternion.Euler(0,-angle,0) * Vector3.right;
    }
}

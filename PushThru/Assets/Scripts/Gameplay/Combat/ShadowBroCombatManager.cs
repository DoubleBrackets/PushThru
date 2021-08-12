using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShadowBroEntity))]
public class ShadowBroCombatManager : EntityCombatManager
{
    //Dependencies
    private ShadowBroEntity shadowBroEntity;
    public ForceMovementScript movementScript;
    public Rigidbody rb;

    public bool isBlocking = false;
    private Vector2 blockingDir;

    private void Awake()
    {
        shadowBroEntity = GetComponent<ShadowBroEntity>();
        
    }

    private void StartBlocking(Vector2 dir)
    {
        blockingDir = dir;
        isBlocking = true;
    }
    private void StopBlocking(Vector2 dir)
    {
        isBlocking = false;
    }

    public override void ReceiveAttack(Attack attack)
    {
        Vector3 knockback = attack.direction * attack.kbVel;
        knockback.z *= 1.5f;
        movementScript.DisableControlAndSlowdown(attack.disableDuration, attack.disableDuration);
        float dotProd = Vector2.Dot(blockingDir, attack.direction.Vector3To2TopDown().normalized);
        if (isBlocking && dotProd <= 0)
        {
            rb.velocity += knockback;
        }
        else
        {
            rb.velocity += knockback;
            shadowBroEntity.TakeDamage(attack);
        }
    }
}

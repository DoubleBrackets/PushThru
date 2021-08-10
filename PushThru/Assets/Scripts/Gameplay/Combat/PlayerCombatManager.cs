using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
public class PlayerCombatManager : EntityCombatManager
{
    //Dependencies
    private PlayerEntity playerEntity;

    public PlayerCombatActionManager actionManager;
    public ForceMovementScript movementScript;
    public Rigidbody rb;

    public bool isBlocking = false;
    private Vector2 blockingDir;

    private void Awake()
    {
        playerEntity = GetComponent<PlayerEntity>();
        actionManager.BlockStartedEvent += StartBlocking;
        actionManager.BlockEndedEvent += StopBlocking;
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
        knockback.y *= 1.5f;
        movementScript.DisableControlAndSlowdown(attack.disableDuration, attack.disableDuration);
        if (!isBlocking)
        {
            rb.velocity += knockback;
            playerEntity.TakeDamage(attack);
        }
        else
        {
            ParticleManager.particleManager.PlayParticle("PlayerSuccessfulBlockParticles");
            rb.velocity += knockback;
        }
    }

}

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

    private float immunityTime = 0.35f;
    private float immunityTimer = 0f;

    private void Awake()
    {
        playerEntity = GetComponent<PlayerEntity>();
        actionManager.BlockStartedEvent += StartBlocking;
        actionManager.BlockEndedEvent += StopBlocking;
    }

    private void Update()
    {
        immunityTimer -= Time.deltaTime;
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
        if (immunityTimer > 0)
            return;
        immunityTimer = immunityTime;
        Vector3 knockback = attack.direction * attack.kbVel;
        knockback.y *= 1.5f;
        movementScript.DisableControlAndSlowdown(attack.disableDuration, attack.disableDuration);
        float dotProd = Vector2.Dot(blockingDir, attack.direction.Vector3To2TopDown().normalized);
        if (isBlocking && dotProd <= 0)
        {
            TimeUtils.instance.FreezeTime(0.01f, 0.15f);
            ParticleManager.particleManager.PlayParticle("PlayerSuccessfulBlockParticles");
            ParticleManager.particleManager.PlayParticle("BlockDustParticles");
            rb.velocity += knockback;
        }
        else
        {
            rb.velocity += knockback;
            playerEntity.TakeDamage(attack);
        }
    }

}

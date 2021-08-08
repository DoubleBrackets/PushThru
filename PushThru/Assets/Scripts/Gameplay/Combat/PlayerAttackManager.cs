using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackManager : MonoBehaviour
{
    public InputManager inputManager;
    public ForceMovementScript movementScript;
    public Rigidbody rb;
    public PlayerFacingScript facing;

    public bool isAttacking
    { 
        get=> attackDurationTimer > 0;
    }

    [HideInInspector]public Vector2 currentAttackDirection;


    [System.Serializable]
    public struct BasicAttackData
    {
        public float basicAttackDuration;
        public SwordSmearEffect basicAttackSmear;
        public float basicAttackForwardVelocity;
    }

    public BasicAttackData[] basicAttacks;
 
    private float attackDurationTimer = 0;

    private int comboCounter = 0;
    private float basicAttackComboResetTimer = 0f;

    public float basicAttackComboCooldown;
    private float basicAttackComboCooldownTimer;

    //Events
    public event System.Action<int> BasicAttackStartedEvent;
    public event System.Action<int> BasicAttackEndedEvent;

    private void Update()
    {
        if(attackDurationTimer > 0)
        {
            attackDurationTimer -= Time.deltaTime;
            if(attackDurationTimer <= 0)
            {
                FinishAttack();
            }
        }
        if(basicAttackComboResetTimer > 0)
        {
            basicAttackComboResetTimer -= Time.deltaTime;
            if(basicAttackComboResetTimer <= 0)
            {
                comboCounter = 0;
            }
        }
        basicAttackComboCooldownTimer -= Time.deltaTime;
    }

    private void Awake()
    {
        inputManager.AttackKeyDown += PerformAttack;
    }

    private void FinishAttack()
    {
        movementScript.DecrementMovementActive();
        BasicAttackEndedEvent?.Invoke(0);
    }

    private void PerformAttack()
    {
        if (attackDurationTimer > 0.11f)
            return;
        else if(attackDurationTimer > 0)
        {
            attackDurationTimer = 0;
            FinishAttack();
        }
        if(basicAttackComboCooldownTimer <= 0)
        {
            PerformBasicAttack();
        }
       
    }

    private void PerformBasicAttack()
    {
        BasicAttackData basicAttack = basicAttacks[comboCounter];
        attackDurationTimer = basicAttack.basicAttackDuration;
        currentAttackDirection = inputManager.mouseDirNormalized;
        movementScript.IncrementMovementActive();
        BasicAttackStartedEvent?.Invoke(comboCounter);
        basicAttack.basicAttackSmear.PerformSmear();

        basicAttackComboResetTimer = basicAttack.basicAttackDuration + 0.1f;
        comboCounter++;
        if (comboCounter == basicAttacks.Length)
        {
            comboCounter = 0;
            basicAttackComboCooldownTimer = basicAttackComboCooldown;
        }
        facing.UpdateFacing();
        rb.velocity += facing.facingNormalized * basicAttack.basicAttackForwardVelocity;
    }
}

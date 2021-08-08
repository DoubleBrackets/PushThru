using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackManager : MonoBehaviour
{
    public InputManager inputManager;
    public ForceMovementScript movementScript;

    public bool isAttacking
    { 
        get=> attackDurationTimer > 0;
    }

    [HideInInspector]public Vector2 currentAttackDirection;

    public float basicAttackDuration;
    private float attackDurationTimer = 0;

    //Smears
    public SwordSmearEffect[] basicAttackSmears;

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
        if (attackDurationTimer > 0)
            return;
        attackDurationTimer = basicAttackDuration;
        currentAttackDirection = inputManager.mouseDirNormalized;
        movementScript.IncrementMovementActive();
        BasicAttackStartedEvent?.Invoke(0);
        basicAttackSmears[0].PerformSmear();
    }
}

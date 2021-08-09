using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCombatActionManager : MonoBehaviour
{
    [System.Serializable]
    public struct BasicAttackData
    {
        public float basicAttackDuration;
        public SwordSmearEffect basicAttackSmear;
        public float basicAttackForwardVelocity;
    }

    public enum ActionType
    { 
        NullAction,
        BasicAttack,
        Block
    }


    public InputManager inputManager;
    public ForceMovementScript movementScript;
    public Rigidbody rb;
    public PlayerFacingScript facing;

    public bool isPerfomingAction
    { 
        get=> actionDurationTimer > 0;
    }


    [Space(10)]

    private float actionDurationTimer = 0;
    [HideInInspector]public Vector2 currentActionDirection;
    private ActionType currentActionType;

    private float actionInterruptMargin = 0.1f;

    [Space(10)]

    public BasicAttackData[] basicAttacks;

    private int comboCounter = 0;
    private float basicAttackComboResetTimer = 0f;

    public float basicAttackComboCooldown;
    private float basicAttackComboCooldownTimer;

    [Space(10)]

    public float blockDuration;
    public float blockCooldown;
    private float blockCooldownTimer;

    //Events

    public event System.Action<int> BasicAttackStartedEvent;
    public event System.Action<int> BasicAttackEndedEvent;

    public event System.Action<Vector2> BlockStartedEvent;
    public event System.Action<Vector2> BlockEndedEvent;
    private void Awake()
    {
        inputManager.AttackKeyDown += PerformAttack;
        inputManager.BlockKeyDown += PerformBlock;
    }


    private void Update()
    {
        if(actionDurationTimer > 0)
        {
            actionDurationTimer -= Time.deltaTime;
            if(actionDurationTimer <= 0)
            {
                EndCurrentAction();
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
        blockCooldownTimer -= Time.deltaTime;
    }

    private void EndCurrentAction()
    {
        actionDurationTimer = 0;
        if (currentActionType == ActionType.BasicAttack)
            FinishAttack();
        else if (currentActionType == ActionType.Block)
            FinishBlock();
    }

    //Checks if theres an action to interrupt, otherwise do not end action(EndCurrentAction() but intended for public use)
    public void EndCurrentActionCheckInterruptable()
    {
        if (!isPerfomingAction)
            return;
        EndCurrentAction();
    }


    private void PerformBlock(Vector2 dirNormalized)
    {
        if (actionDurationTimer > actionInterruptMargin || blockCooldownTimer > 0)
            return;
        else if (actionDurationTimer > 0)
        {
            EndCurrentAction();
        }
        blockCooldownTimer = blockCooldown;

        StartAction(ActionType.Block, blockDuration, dirNormalized);

        facing.UpdateFacing();

        movementScript.IncrementMovementActive();
        ParticleManager.particleManager.PlayParticle("StarParticles");
        ParticleManager.particleManager.PlayParticle("AttackDustParticles");
        BlockStartedEvent?.Invoke(dirNormalized);
    }

    private void FinishBlock()
    {
        movementScript.DecrementMovementActive();
        BlockEndedEvent?.Invoke(currentActionDirection);
    }

    private void PerformAttack(Vector2 dirNormalized)
    {
        if (actionDurationTimer > actionInterruptMargin)
            return;
        else if(actionDurationTimer > 0)
        {
            EndCurrentAction();
        }
        if(basicAttackComboCooldownTimer <= 0)
        {
            PerformBasicAttack(dirNormalized);
        }
        OrthoPixelMoveCamera.orthoCam.UpdateTarget(2f);
    }
    private void FinishAttack()
    {
        movementScript.DecrementMovementActive();
        BasicAttackEndedEvent?.Invoke(comboCounter-1);
    }

    private void PerformBasicAttack(Vector2 dirNormalized)
    {
        BasicAttackData basicAttack = basicAttacks[comboCounter];
        StartAction(ActionType.BasicAttack, basicAttack.basicAttackDuration, dirNormalized);

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
        Vector3 dashForward = facing.facingNormalized * basicAttack.basicAttackForwardVelocity;
        dashForward.z *= 1.5f;
        rb.velocity += dashForward;
        ParticleManager.particleManager.PlayParticle("AttackDustParticles");
    }

    public void StartAction(ActionType type, float time, Vector2 dir)
    {
        currentActionType = type;
        actionDurationTimer = time;
        currentActionDirection = dir;
    }

    //placeholder for external actions, like dash, that are handled by a separate script
    public void StartNullAction(float time, Vector2 dir)
    {
        actionDurationTimer = time;
        currentActionDirection = dir;
        currentActionType = ActionType.NullAction;
    }

    public void ExtendComboResetTimer(float val)
    {
        if(basicAttackComboResetTimer > 0)
        {
            basicAttackComboResetTimer += val;
        }
    }

    public bool isActionInterruptable()
    {
        return isActionInterruptable(0);
    }

    public bool isActionInterruptable(float offset)
    {
        return actionDurationTimer <= actionInterruptMargin + offset;
    }
}

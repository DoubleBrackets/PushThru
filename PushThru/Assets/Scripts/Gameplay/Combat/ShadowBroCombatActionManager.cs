using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBroCombatActionManager : CombatActionManager
{
    [System.Serializable]
    public struct BasicAttackData
    {
        public float basicAttackDuration;
        public SwordSmearEffect basicAttackSmear;
        public float basicAttackForwardVelocity;
        public AttackCaster caster;
    }

    public enum ActionType
    {
        NullAction,
        BasicAttack,
        RingsOfDeth
    }

    public ShadowBroEnemyAI ai;
    public ForceMovementScript movementScript;
    public Rigidbody rb;
    public FacingScript facing;

    public GameObject ringsPrefab;

    public override bool IsPerformingAction()
    {
        return actionDurationTimer > 0;
    }


    [Space(10)]

    private float actionDurationTimer = 0;
    private ActionType currentActionType;

    private float actionInterruptMargin = 0.1f;

    [Space(10)]

    public BasicAttackData[] basicAttacks;

    //Events

    public event System.Action<int> BasicAttackStartedEvent;
    public event System.Action<int> BasicAttackEndedEvent;



    private void Update()
    {
        if (actionDurationTimer > 0)
        {
            actionDurationTimer -= Time.deltaTime;
            if (actionDurationTimer <= 0)
            {
                EndCurrentAction();
            }
        }
    }

    private void EndCurrentAction()
    {
        actionDurationTimer = 0;
        if (currentActionType == ActionType.BasicAttack)
            FinishAttack();
        else if (currentActionType == ActionType.RingsOfDeth)
            movementScript.DecrementMovementActive();
    }

    //Checks if theres an action to interrupt, otherwise do not end action(EndCurrentAction() but intended for public use)
    public void EndCurrentActionCheckInterruptable()
    {
        if (!IsPerformingAction())
            return;
        EndCurrentAction();
    }

    public void TryBasicAttack(Vector2 dirNormalized,int attackIndex)
    {
        if (actionDurationTimer > actionInterruptMargin)
            return;
        else if (actionDurationTimer > 0)
        {
            EndCurrentAction();
        }
        PerformBasicAttack(dirNormalized,attackIndex);
    }
    private void FinishAttack()
    {
        movementScript.DecrementMovementActive();
    }

    private void PerformBasicAttack(Vector2 dirNormalized,int attackIndex)
    {
        BasicAttackData basicAttack = basicAttacks[attackIndex];
        StartAction(ActionType.BasicAttack, basicAttack.basicAttackDuration, dirNormalized);

        movementScript.IncrementMovementActive();
        BasicAttackStartedEvent?.Invoke(attackIndex);
        basicAttack.basicAttackSmear.PerformSmear();

        //Damage&physics logic
        Attack attack = new Attack(1, dirNormalized.Vector2To3TopDown(), 25, 0.25f);
        StartCoroutine(Corout_DoubleCast(basicAttack, attack, basicAttack.basicAttackDuration / 1.9f, dirNormalized));

        facing.UpdateFacing();
        Vector3 dashForward = facing.facingVectorNormalized * basicAttack.basicAttackForwardVelocity;
        ParticleManager.particleManager.PlayParticle("ShadowBroAttackDustParticles");
        dashForward.z *= 1.5f;
        rb.velocity += dashForward;
    }

    private IEnumerator Corout_DoubleCast(BasicAttackData basicAttack, Attack attack, float delay, Vector2 dirNormalized)
    {
        yield return new WaitForSeconds(delay);
        HashSet<EntityCombatManager> targets = basicAttack.caster.CastForCombatManagers(dirNormalized);
        foreach (EntityCombatManager target in targets)
        {
            target.ReceiveAttack(attack);
        }
    }

    public void TryRingsAttack()
    {
        if (actionDurationTimer > actionInterruptMargin)
            return;
        else if (actionDurationTimer > 0)
        {
            EndCurrentAction();
        }
        PerformRingsAttack();
    }

    private void PerformRingsAttack()
    {
        movementScript.IncrementMovementActive();
        StartAction(ActionType.RingsOfDeth, 0.25f, Vector3.up);
        GameObject newRing = Instantiate(ringsPrefab, transform.position + Vector3.up * 0.2f, ringsPrefab.transform.rotation);
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

    public bool isActionInterruptable()
    {
        return isActionInterruptable(0);
    }

    public bool isActionInterruptable(float offset)
    {
        return actionDurationTimer <= actionInterruptMargin + offset;
    }
}

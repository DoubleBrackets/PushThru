using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBroEnemyAI : MonoBehaviour
{
    public ShadowBroCombatActionManager actionManager;
    public ForceMovementScript moveScript;
    //Tracking
    private Transform target;
    private Vector3 vectorToTarget;
    private Vector2 vectorToTargetNormalized;
    private float distanceToTarget;
    //Values
    public float maxDistanceFromTarget;
    public Vector2 teleportToTargetRange;
    public float minWalkDistance;
    //Attacks
    public float basicAttackRange;

    //States
    private bool isInAction = false;
    public int actionRoll = 0;
    private int prevActionRoll = 0;

    public float actionInterval;
    private float actionIntervalTimer = 0f;

    private void Start()
    {
        target = PlayerEntity.player.transform;
        actionRoll = 400;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ParticleManager.particleManager.StopParticle("RingAttackParticles");
        ParticleManager.particleManager.StopParticle("ShadowBroEyeTrailParticles");
    }

    private void RerollAction()
    {
        prevActionRoll = actionRoll;
        actionRoll = Random.Range(0, 1000);
    }

    private void Update()
    {
        vectorToTarget = target.position - transform.position;
        vectorToTarget.y = 0;
        vectorToTargetNormalized = vectorToTarget.normalized.Vector3To2TopDown();
        Vector2 multipliers = vectorToTargetNormalized.GetOrthoAxisMultipliers(0.5f);
        distanceToTarget = vectorToTarget.magnitude;

        moveScript.inputVector = Vector2.zero;
        actionIntervalTimer -= Time.deltaTime;

        if (isInAction)
            return;

        if (distanceToTarget > minWalkDistance * multipliers.y)
        {
            Vector2 vectorToTargetMove = vectorToTargetNormalized;
            vectorToTargetMove.y /= 2;
            vectorToTargetMove = vectorToTargetMove.normalized;
            moveScript.inputVector = vectorToTargetMove;
        }

        if (actionIntervalTimer > 0)
            return;
        if (distanceToTarget  > maxDistanceFromTarget * multipliers.y || actionRoll <= 300)
        {
            RerollAction();
            Teleport();
        }
        if(actionManager.isActionInterruptable())
        {
            if(actionRoll > 300 && actionRoll < 750)
            {
                if(prevActionRoll != actionRoll)
                    ParticleManager.particleManager.PlayParticle("ShadowBroEyeTrailParticles");
                if (distanceToTarget < basicAttackRange * multipliers.y)
                {
                    isInAction = true;
                    StartCoroutine(Corout_BasicAttackFlurry());
                }
            }
            else if(actionRoll >= 750)
            {
                isInAction = true;
                StartCoroutine(Corout_RingsOfDeth());
            }

        }
    }

    private IEnumerator Corout_RingsOfDeth()
    {
        ParticleManager.particleManager.PlayParticle("RingAttackParticles");
        yield return new WaitForSeconds(0.5f);
        actionManager.TryRingsAttack();
        yield return new WaitForSeconds(1.5f);
        actionManager.TryRingsAttack();
        yield return new WaitForSeconds(1f);
        isInAction = false;
        ParticleManager.particleManager.StopParticle("RingAttackParticles");
        actionIntervalTimer = actionInterval;
        RerollAction();
    }

    private IEnumerator Corout_BasicAttackFlurry()
    {
        yield return new WaitForSeconds(0.4f);

        Vector2 savedVector = vectorToTargetNormalized;
        yield return new WaitForSeconds(0.05f);
        actionManager.TryBasicAttack(savedVector, 0);

        RerollAction();
        if (actionRoll <= 250)
        {
            Teleport();
            ParticleManager.particleManager.StopParticle("ShadowBroEyeTrailParticles");
            isInAction = false;
            actionIntervalTimer = actionInterval;
            actionRoll = 400;
            yield break;
        }

        yield return new WaitForSeconds(0.6f);
        savedVector = vectorToTargetNormalized;
        yield return new WaitForSeconds(0.05f);
        actionManager.TryBasicAttack(savedVector, 1);

        yield return new WaitForSeconds(0.6f);

        RerollAction();
        if (actionRoll <= 250)
        {
            Teleport();
            ParticleManager.particleManager.StopParticle("ShadowBroEyeTrailParticles");
            isInAction = false;
            actionIntervalTimer = actionInterval;
            actionRoll = 400;
            yield break;
        }

        yield return new WaitForSeconds(0.4f);
        savedVector = vectorToTargetNormalized;
        yield return new WaitForSeconds(0.05f);
        actionManager.TryBasicAttack(savedVector, 2);
        ParticleManager.particleManager.StopParticle("ShadowBroEyeTrailParticles");
        yield return new WaitForSeconds(0.8f);
        isInAction = false;
        actionIntervalTimer = actionInterval;
    }

    private void Teleport()
    {
        //Teleports to a random position near the target
        float radius = Random.Range(teleportToTargetRange.x, teleportToTargetRange.y);
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        Vector3 pos = target.transform.position + offset;
        ParticleManager.particleManager.PlayParticle("ShadowBroTeleportParticlesStart");
        ParticleManager.particleManager.SetParticlePosition("ShadowBroTeleportParticlesStart", transform.position);
        transform.position = pos;
        ParticleManager.particleManager.PlayParticle("ShadowBroTeleportParticles");
    }
}

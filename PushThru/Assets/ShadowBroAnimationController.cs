using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBroAnimationController : MonoBehaviour
{
    public Animator animator;

    public Rigidbody rb;
    public ForceMovementScript moveScript;
    public ShadowBroCombatActionManager attackManager;

    private void Awake()
    {
        attackManager.BasicAttackStartedEvent += PlayBasicAttackAnimation;
        //attackManager.BasicAttackEndedEvent += EndBasicAttackAnimation;
    }
    private void PlayBasicAttackAnimation(int val)
    {
        PlayAnimation("BasicAttack" + val);
    }

    private void Update()
    {
        Vector2 rbVel = new Vector2(rb.velocity.x, rb.velocity.z);
        Vector2 axisScale = rbVel.GetOrthoAxisMultipliers(0.5f);
        SetBool("isPerformingAction", attackManager.IsPerformingAction());
        if (rbVel.magnitude * axisScale.y > 1.5f && !attackManager.IsPerformingAction())
        {
            if (SetBool("isRunning", true))
            {
                ParticleManager.particleManager.PlayParticle("ShadowBroFootstepParticles");
            }
        }
        if (attackManager.IsPerformingAction())
        {
            if (SetBool("isRunning", false))
            {
                ParticleManager.particleManager.StopParticle("ShadowBroFootstepParticles");
            }
        }
    }

    public void PlayAnimation(string anim)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
        {
            animator.Play(anim);
        }
    }
    public bool SetBool(string name, bool value)
    {
        if (animator.GetBool(name) != value)
            animator.SetBool(name, value);
        else
            return false;
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;

    public Rigidbody rb;
    public ForceMovementScript moveScript;
    public InputManager inputManager;
    public PlayerCombatActionManager playerAttackManager;
    public DashScript dashScript;

    public GameObject hipSword;
    public GameObject handSword;

    private void Awake()
    {
        playerAttackManager.BasicAttackStartedEvent += PlayBasicAttackAnimation;
        playerAttackManager.BasicAttackEndedEvent += EndBasicAttackAnimation;

        playerAttackManager.BlockStartedEvent += PlayBlockAnimation;
        playerAttackManager.BlockEndedEvent += EndBlockAnimation;

        dashScript.DashPerformed += PlayDashAnimation;
        handSword.SetActive(false);
    }
    private void PlayDashAnimation()
    {
        PlayAnimation("Dash");
    }

    private void PlayBasicAttackAnimation(int val)
    {
        hipSword.SetActive(false);
        handSword.SetActive(true);
        PlayAnimation("BasicAttack" + val);
    }

    private void EndBasicAttackAnimation(int val)
    {
        hipSword.SetActive(true);
        handSword.SetActive(false);
    }

    private void PlayBlockAnimation(Vector2 dir)
    {
        hipSword.SetActive(false);
        handSword.SetActive(true);
        PlayAnimation("Block");
    }

    private void EndBlockAnimation(Vector2 dir)
    {
        hipSword.SetActive(true);
        handSword.SetActive(false);
    }

    public void SetHipSword(bool val)
    {
        hipSword.SetActive(val);
    }


    private void Update()
    {
        Vector2 rbVel = new Vector2(rb.velocity.x, rb.velocity.z / moveScript.zAxisMultiplier);
        SetBool("isPerformingAction", playerAttackManager.IsPerformingAction());
        if(rbVel.magnitude > 0.5 && !playerAttackManager.IsPerformingAction())
        {
            if(SetBool("isRunning", true))
            {
                ParticleManager.particleManager.PlayParticle("FootstepParticles");
            }
        }
        else if(inputManager.inputVectorLastChanged > 0.2f || playerAttackManager.IsPerformingAction())
        {
            if(SetBool("isRunning", false))
            {               
                ParticleManager.particleManager.StopParticle("FootstepParticles");               
            }
        }
    }

    public void PlayAnimation(string anim)
    { 
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
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

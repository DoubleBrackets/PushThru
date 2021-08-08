using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;

    public Rigidbody rb;
    public ForceMovementScript moveScript;
    public InputManager inputManager;
    public PlayerAttackManager playerAttackManager;

    public GameObject hipSword;
    public GameObject handSword;

    private void Awake()
    {
        playerAttackManager.BasicAttackStartedEvent += PlayBasicAttackAnimation;
        playerAttackManager.BasicAttackEndedEvent += EndBasicAttackAnimation;
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

    private void Update()
    {
        Vector2 rbVel = new Vector2(rb.velocity.x, rb.velocity.z / moveScript.zAxisMultiplier);
        
        if(rbVel.magnitude > 0.2)
        {
            if(SetBool("isRunning", true))
            {
                ParticleManager.particleManager.PlayParticle("FootstepParticles");
            }
        }
        else if(inputManager.inputVectorLastChanged > 0.2f)
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

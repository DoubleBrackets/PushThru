using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;

    public Rigidbody rb;
    public ForceMovementScript moveScript;
    public InputManager inputManager;

    private void Update()
    {
        Vector2 rbVel = new Vector2(rb.velocity.x, rb.velocity.z / moveScript.zAxisMultiplier);
        float rawAngle = (Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.z/moveScript.zAxisMultiplier, rb.velocity.x));
        float rawInputAngle = (Mathf.Rad2Deg * Mathf.Atan2(inputManager.inputVectorSoftened.y,inputManager.inputVectorSoftened.x));
        float dirSign = Mathf.Sign(Mathf.DeltaAngle(rawInputAngle,rawAngle));
        
        if(rbVel.magnitude > 0.2f)
        {
            float angle;
            if (inputManager.inputVectorSoftened != Vector2.zero)
            {
                angle = inputManager.inputVectorSoftened.Angle();
            }
            else
            {
                angle = Mathf.RoundToInt(rawAngle / 45) * 45;
            }
            rb.transform.rotation = Quaternion.Euler(0, 90 - angle, 0);
        }
        
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

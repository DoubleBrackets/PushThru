
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBroEntity : Entity
{

    public override void TakeDamage(Attack attack)
    {
        if (currentHealth <= 0)
            return;
        currentHealth -= attack.damage;
        InvokeHealthChanged(currentHealth);
        if(currentHealth <= 0)
        {
            ShadowBroDeath();
        }
        if(attack.damage > 0)
        {
            ParticleManager.particleManager.PlayParticle("ShadowBroHurtParticles");
        }
    }

    private void ShadowBroDeath()
    {
        if (PlayerEntity.player.currentHealth <= 0)
            return;
        GetComponent<ShadowBroEnemyAI>().enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<ForceMovementScript>().IncrementMovementActive();
        GetComponent<FacingScript>().SetFacing((PlayerEntity.player.transform.position - transform.position).Vector3To2TopDown());
        Level2Director.instance.EnemyFinishingAttack();
    }
}

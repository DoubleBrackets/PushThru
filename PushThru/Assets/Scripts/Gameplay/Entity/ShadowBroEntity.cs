
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
        Level2Director.instance.EnemyFinishingAttack();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}

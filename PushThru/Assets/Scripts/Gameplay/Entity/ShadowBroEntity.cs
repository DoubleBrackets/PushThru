using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBroEntity : Entity
{

    public override void TakeDamage(Attack attack)
    {
        currentHealth -= attack.damage;
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
        print("shadow bro died");
    }
}

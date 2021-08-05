using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{

    public override void TakeDamage(Attack attack)
    {
        currentHealth -= attack.damage;
        if(currentHealth <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        print("player ded");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DestroyableObjectEntity : Entity
{
    public UnityEvent OnKilledEvent;
    public override void TakeDamage(Attack attack)
    {
        if (currentHealth <= 0)
            return;
        currentHealth -= attack.damage;
        if (currentHealth <= 0)
        {
            ObjectDeath();
        }   
    }

    private void ObjectDeath()
    {
        OnKilledEvent?.Invoke();
    }

}

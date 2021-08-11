using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public event System.Action<float> HealthChanged;
    public abstract void TakeDamage(Attack attack);

    protected void InvokeHealthChanged(float val)
    {
        HealthChanged?.Invoke(val);
    }

}

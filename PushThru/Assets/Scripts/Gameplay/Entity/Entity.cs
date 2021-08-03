using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public abstract void TakeDamage(Attack attack);

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack 
{
    public Attack(int damage, Vector3 direction, float kbVel, float disableDuration)
    {
        this.damage = damage;
        this.direction = direction;
        this.kbVel = kbVel;
        this.disableDuration = disableDuration;
    }

    public Attack(int damage, Vector3 direction, float kbVel)
    {
        this.damage = damage;
        this.direction = direction;
        this.kbVel = kbVel;
    }

    public Attack(int damage, Vector3 direction)
    {
        this.damage = damage;
        this.direction = direction;
    }

    public int damage = 0;
    public Vector3 direction = Vector2.zero;
    public float kbVel = 0;
    public float disableDuration = 0;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack 
{
    public Attack(int damage, Vector2 direction, float kbVel)
    {
        this.damage = damage;
        this.direction = direction;
        this.kbVel = kbVel;
    }

    public Attack(int damage, Vector2 direction)
    {
        this.damage = damage;
        this.direction = direction;
        this.kbVel = 0;
    }

    public int damage;
    public Vector2 direction;
    public float kbVel;
}

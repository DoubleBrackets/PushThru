using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnTouch : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.GetComponent<EntityCombatManager>())
        {
            Attack attack = new Attack(100000, Vector3.zero);
            collision.collider.GetComponent<EntityCombatManager>().ReceiveAttack(attack);
        }
    }
}

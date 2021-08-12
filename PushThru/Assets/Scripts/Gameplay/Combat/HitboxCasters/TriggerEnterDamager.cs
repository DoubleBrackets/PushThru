using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterDamager : MonoBehaviour
{
    public LayerMask targetMask;
    public int damage;
    public float kbVel;
    public float disableTime;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<EntityCombatManager>() && ((1 << collider.gameObject.layer) & targetMask) != 0)
        {
            Vector3 diff = collider.transform.position - transform.position;
            diff.y = 0;

            Attack attack = new Attack(damage, diff.normalized , kbVel,disableTime);
            collider.GetComponent<EntityCombatManager>().ReceiveAttack(attack);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<EntityCombatManager>() && ((1 << collision.collider.gameObject.layer) & targetMask) != 0)
        {
            Vector3 diff = collision.collider.transform.position - transform.position;
            diff.y = 0;

            Attack attack = new Attack(damage, diff.normalized, kbVel, disableTime);
            collision.collider.GetComponent<EntityCombatManager>().ReceiveAttack(attack);
        }
    }
}

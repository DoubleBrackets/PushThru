using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcCaster : AttackCaster
{
    public float radius;
    public int raycastCount;
    public float arcAngle;
    public override HashSet<EntityCombatManager> CastForCombatManagers(Vector2 direction)
    {
        HashSet<EntityCombatManager> found = new HashSet<EntityCombatManager>();
        float targetAngle = direction.Angle();
        float startAngle = targetAngle - arcAngle / 2;
        float interval = arcAngle / (raycastCount-1);
        float lengthMultiplier = 1 + 0.5f * Mathf.Abs(Mathf.Sin(targetAngle * Mathf.Deg2Rad));
        for (int x = 0;x < raycastCount;x++)
        {
            float cAngle = x * interval + startAngle;
            cAngle *= Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(cAngle), 0, Mathf.Sin(cAngle)) * radius;
            dir.z *= lengthMultiplier;
            float tempRadius = dir.magnitude;
            RaycastHit[] hits = Physics.RaycastAll(transform.position,dir, tempRadius, targetLayerMask);
            foreach (RaycastHit hit in hits)
            {
                EntityCombatManager combatManager = hit.collider.GetComponent<EntityCombatManager>();
                if (combatManager)
                {
                     found.Add(combatManager);
                }
            }
        }
        return found;
    }
}
